using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class LoopWithOptionsNodeObject : EpisodeNodeObject
{
    [SerializeField] private VideoPlayer videoPlayerLoop_;

    private VideoPlayer activePlayer_;
    private Dictionary<string, int> lastRandomIndex_ = new Dictionary<string, int>();

    public override void Init(GameManager gameManager, EpisodeNode node)
    {
        base.Init(gameManager, node);

        videoPlayerLoop_.isLooping = true;
        VideoEpisodeNodeObject.PreloadVideo(videoPlayerLoop_, node.VideoLoopFilePath);
    }

    private void VideoFinished(VideoPlayer vp)
    {
        videoPlayerLoop_.transform.localScale = Vector3.one;
        Destroy(activePlayer_.gameObject);
        activePlayer_ = null;
    }

    public override void Reset()
    {
        base.Reset();

        videoPlayerLoop_.Play();
    }

    public override bool IsPlaying
    {
        get
        {
            return (activePlayer_ != null && activePlayer_.isPlaying) || (videoPlayerLoop_.isPlaying);
        }
    }

    private IEnumerator PlayVideo(string path)
    {
        activePlayer_ = Instantiate<VideoPlayer>(videoPlayerLoop_, videoPlayerLoop_.transform.parent);
        activePlayer_.transform.localPosition = videoPlayerLoop_.transform.localPosition;
        activePlayer_.transform.localScale = Vector3.zero;
        activePlayer_.isLooping = false;

        activePlayer_.loopPointReached += VideoFinished;

        VideoEpisodeNodeObject.PreloadVideo(activePlayer_, path);

        activePlayer_.Prepare();

        while (!activePlayer_.isPrepared)
        {
            yield return 0;
        }

        activePlayer_.Play();

        for (int i = 0; i < 3; i++)
        {
            yield return 0;
        }

        activePlayer_.transform.localScale = Vector3.one;

        videoPlayerLoop_.transform.localScale = Vector3.zero;
    }

    public override void ReceiveAction(string action)
    {
        base.ReceiveAction(action);

        if (activePlayer_ != null)
        {
            return;
        }

        //args: VideoOption.key
        //-pop, pop each video played
        //-random, randomize each time

        string[] args = action.Split(' ');

        string videoKey = null;
        string videoPath = null;
        bool pop = false;
        bool random = false;

        if (args.Length > 0) videoKey = args[0];

        foreach(string a in args)
        {
            if (string.Equals("-pop", a)) pop = true;
            if (string.Equals("-random", a)) random = true;
        }

        if (videoKey != null)
        {
            EpisodeNode.VideoOption vo = null;
            foreach(EpisodeNode.VideoOption n in node_.VideoOptions)
            {
                if (string.Equals(videoKey, n.Key))
                {
                    vo = n;
                    break;
                }
            }

            if (vo == null) return;
            if (vo.Videos.Count == 0) return;

            int index = 0;
            if (random)
            {
                index = Random.Range(0, vo.Videos.Count);

                if (lastRandomIndex_.ContainsKey(videoKey))
                {
                    int previousIndex = lastRandomIndex_[videoKey];
                    while (previousIndex == index && vo.Videos.Count > 1)
                    {
                        index = Random.Range(0, vo.Videos.Count);
                    }
                }
                
            }
            lastRandomIndex_[videoKey] = index;
            videoPath = vo.Videos[index].VideoPath;

            if (pop)
            {
                vo.Videos.RemoveAt(index);
            }
        }

        if (videoPath != null)
        {
            StartCoroutine(PlayVideo(videoPath));
        }
    }
}
