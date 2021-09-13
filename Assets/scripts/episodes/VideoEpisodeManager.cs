using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoEpisodeManager : GameManager
{
    [SerializeField] private VideoPlayer videoPlayer_;

    private VideoPlayer videoPlayerA_;
    private VideoPlayer videoPlayerB_;
    private VideoPlayer mainVideoPlayer_;

    private string internalState_ = "";

    // Start is called before the first frame update
    void Start()
    {
        videoPlayerA_ = videoPlayer_;

        videoPlayerB_ = GameObject.Instantiate<VideoPlayer>(videoPlayer_);
        videoPlayerB_.transform.SetParent(videoPlayerA_.transform.parent);
        videoPlayerB_.transform.position = videoPlayerA_.transform.position;

        videoPlayerA_.loopPointReached += VideoFinished;
        videoPlayerB_.loopPointReached += VideoFinished;

        mainVideoPlayer_ = videoPlayerA_;
    }

    protected override void NewNodeEventInternal(EpisodeNode node)
    {
        base.NewNodeEventInternal(node);

        StartCoroutine(UpdateVideoPlayer(node.VideoFilePath, false));

        internalState_ = NodeState.Playing;
        UpdateNodeState(NodeState.Playing);
    }

    private IEnumerator UpdateVideoPlayer(string videoPath, bool loop)
    {
        VideoPlayer previousVideoPlayer = mainVideoPlayer_;
        if (mainVideoPlayer_ == videoPlayerA_)
        {
            mainVideoPlayer_ = videoPlayerB_;
        } else
        {
            mainVideoPlayer_ = videoPlayerA_;
        }

        mainVideoPlayer_.url = System.IO.Path.Combine(Application.streamingAssetsPath, videoPath);
        mainVideoPlayer_.isLooping = loop;
        mainVideoPlayer_.Play();

        yield return 0;

        mainVideoPlayer_.transform.SetAsLastSibling();
        previousVideoPlayer.Stop();
    }

    private void VideoFinished(VideoPlayer vp)
    {
        if (string.Equals(internalState_, NodeState.Playing))
        {
            StartCoroutine(UpdateVideoPlayer(currentNode_.VideoLoopFilePath, true));

            internalState_ = NodeState.Looping;
            UpdateNodeState(NodeState.Looping);
        }
    }
}
