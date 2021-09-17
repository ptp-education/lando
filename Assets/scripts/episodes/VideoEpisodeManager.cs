using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Linq;

public class VideoEpisodeManager : GameManager
{
    [SerializeField] private VideoPlayer videoPlayer_;

    private string internalState_ = "";
    private Dictionary<string, VideoPlayer> videoPlayers_ = new Dictionary<string, VideoPlayer>();

    protected override void NewNodeEventInternal(EpisodeNode node)
    {
        base.NewNodeEventInternal(node);

        internalState_ = NodeState.Playing;
        UpdateNodeState(NodeState.Playing);

        StartCoroutine(UpdateVideoPlayer(internalState_, currentNode_));
    }

    private IEnumerator UpdateVideoPlayer(string nodeState, EpisodeNode currentNode)
    {
        List<string> videoPaths = new List<string>();
        videoPaths.Add(currentNode.VideoFilePath);
        videoPaths.Add(currentNode.VideoLoopFilePath);
        videoPaths.Add(currentNode.NextNode.VideoFilePath);
        foreach (EpisodeNode.Option o in currentNode_.Options)
        {
            videoPaths.Add(o.Node.VideoFilePath);
        }

        foreach(string v in videoPaths)
        {
            AddVideoPlayer(videoPlayers_, v);
        }

        yield return 0;

        bool isLooping = string.Equals(nodeState, GameManager.NodeState.Looping);
        string videoToPlay = isLooping ? currentNode.VideoLoopFilePath : currentNode.VideoFilePath;

        VideoPlayer currentPlayer = videoPlayers_[videoToPlay];
        currentPlayer.isLooping = isLooping;
        currentPlayer.loopPointReached += VideoFinished;
        currentPlayer.Play();

        for (int i = 0; i < 12; i++)
        {
            yield return 0;
        }

        currentPlayer.transform.localScale = videoPlayer_.transform.localScale;
        currentPlayer.transform.SetAsLastSibling();

        yield return 0;

        List<string> allKeys = new List<string>(videoPlayers_.Keys);
        foreach (string k in allKeys)
        {
            if (!videoPaths.Any(p => string.Equals(p, k)))
            {
                GameObject.Destroy(videoPlayers_[k].gameObject);
                videoPlayers_.Remove(k);
            }
        }
    }

    private void AddVideoPlayer(Dictionary<string, VideoPlayer> dict, string videoPath)
    {
        if (dict.ContainsKey(videoPath))
        {
            return;
        }

        VideoPlayer vp = GameObject.Instantiate<VideoPlayer>(videoPlayer_);
        vp.playOnAwake = false;
        vp.transform.SetParent(videoPlayer_.transform.parent);
        vp.transform.position = videoPlayer_.transform.position;
        vp.transform.localScale = Vector3.zero;

        RenderTexture rt = new RenderTexture(1920, 1080, 0);
        vp.GetComponent<VideoPlayer>().targetTexture = rt;

        RawImage ri = vp.GetComponentInChildren<RawImage>();
        ri.texture = rt;

        string[] split = videoPath.Split('/');
        vp.gameObject.name = split[split.Length - 1];
        vp.url = System.IO.Path.Combine(Application.streamingAssetsPath, videoPath);

        dict[videoPath] = vp;
    }

    private void VideoFinished(VideoPlayer vp)
    {
        if (string.Equals(internalState_, NodeState.Playing))
        {
            internalState_ = NodeState.Looping;
            UpdateNodeState(NodeState.Looping);

            StartCoroutine(UpdateVideoPlayer(internalState_, currentNode_));
        }
    }
}
