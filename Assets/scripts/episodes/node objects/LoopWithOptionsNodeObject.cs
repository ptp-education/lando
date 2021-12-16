using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class LoopWithOptionsNodeObject : EpisodeNodeObject
{
    [SerializeField] private VideoPlayer videoPlayerLoop_;

    private List<VideoPlayer> listOfVideos_ = new List<VideoPlayer>();
    private Dictionary<string, int> lastRandomIndex_ = new Dictionary<string, int>();

    public override void Init(GameManager gameManager, EpisodeNode node, ReadyToStartLoop callback)
    {
        base.Init(gameManager, node, callback);

        videoPlayerLoop_.isLooping = true;
    }

    private void VideoFinished(VideoPlayer vp)
    {
        StartCoroutine(PlayVideo(videoPlayerLoop_));
    }

    public override void Hide()
    {
        base.Hide();

        videoPlayerLoop_.Pause();

        foreach(VideoPlayer vp in listOfVideos_)
        {
            vp.Pause();
        }
    }

    public override void Play()
    {
        base.Play();

        Loop();

        //Play is handled separately through actions. We go immediately to loop.
    }

    public override void Loop()
    {
        base.Loop();

        videoPlayerLoop_.Play();
        StartCoroutine(PlayVideo(videoPlayerLoop_));
    }

    private void PlayVideo(string path)
    {
        List<VideoPlayer> players = new List<VideoPlayer>(listOfVideos_);
        players.Add(videoPlayerLoop_);

        foreach(VideoPlayer vp in players)
        {
            if (vp.url.Contains(path))
            {
                StartCoroutine(PlayVideo(vp));
                return;
            }
        }
    }

    private IEnumerator PlayVideo(VideoPlayer play)
    {
        play.Play();

        for (int i = 0; i < 12; i++)
        {
            yield return 0;
        }

        List<VideoPlayer> videos = new List<VideoPlayer>(listOfVideos_);
        videos.Add(videoPlayerLoop_);

        foreach (VideoPlayer vp in videos)
        {
            if (vp != play)
            {
                vp.Stop();
                vp.transform.localScale = Vector3.zero;
            }
        }

        play.transform.localScale = Vector3.one;
    }

    public override void Preload(EpisodeNode node)
    {
        base.Preload(node);

        VideoEpisodeNodeObject.PreloadVideo(videoPlayerLoop_, node.VideoLoopFilePath);

        foreach (EpisodeNode.VideoOption vo in node.VideoOptions)
        {
            foreach(EpisodeNode.VideoOption.Video v in vo.Videos)
            {
                VideoPlayer vp = Instantiate<VideoPlayer>(videoPlayerLoop_, videoPlayerLoop_.transform.parent);
                vp.transform.localPosition = videoPlayerLoop_.transform.localPosition;
                vp.transform.localScale = Vector3.zero;
                vp.isLooping = false;

                vp.loopPointReached += VideoFinished;

                VideoEpisodeNodeObject.PreloadVideo(vp, v.VideoPath);

                listOfVideos_.Add(vp);
            }
        }
    }

    public override void ReceiveAction(string action)
    {
        base.ReceiveAction(action);

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
            foreach(EpisodeNode.VideoOption n in episodeNode_.VideoOptions)
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
                    while (previousIndex != index && vo.Videos.Count > 1)
                    {
                        index = Random.Range(0, vo.Videos.Count);
                    }
                }
                
            }
            videoPath = vo.Videos[index].VideoPath;

            if (pop)
            {
                vo.Videos.RemoveAt(index);
            }
        }

        if (videoPath != null)
        {
            PlayVideo(videoPath);
        }
    }
}
