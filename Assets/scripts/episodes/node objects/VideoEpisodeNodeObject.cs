using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoEpisodeNodeObject : EpisodeNodeObject
{
    [SerializeField] private VideoPlayer videoPlayerMain_;
    [SerializeField] private VideoPlayer videoPlayerLoop_;

    private bool started_ = false;

    public override void Init(GameManager gameManager, EpisodeNode node, ReadyToStartLoop callback)
    {
        base.Init(gameManager, node, callback);

        videoPlayerMain_.loopPointReached += VideoFinished;

        videoPlayerMain_.isLooping = false;
        videoPlayerLoop_.isLooping = true;
    }

    private void VideoFinished(VideoPlayer vp)
    {
        startLoopCallback_.Invoke();
    }

    public override void Hide()
    {
        base.Hide();

        videoPlayerMain_.Pause();
        videoPlayerLoop_.Pause();
    }

    public override void Play()
    {
        base.Play();

        started_ = true;

        videoPlayerMain_.Stop();
        StartCoroutine(SwapPlayer(videoPlayerMain_, videoPlayerLoop_));
    }

    public override void Loop()
    {
        base.Loop();

        videoPlayerLoop_.Stop();
        StartCoroutine(SwapPlayer(videoPlayerLoop_, videoPlayerMain_));
    }

    private IEnumerator SwapPlayer(VideoPlayer play, VideoPlayer stop)
    {
        play.Play();

        for (int i = 0; i < 12; i++)
        {
            yield return 0;
        }

        stop.Pause();

        play.transform.localScale = Vector3.one;
        stop.transform.localScale = Vector3.zero;
    }

    public override void Preload(EpisodeNode node)
    {
        base.Preload(node);

        PreloadVideo(videoPlayerMain_, node.VideoFilePath);
        PreloadVideo(videoPlayerLoop_, node.VideoLoopFilePath);
    }

    public override float ProgressPercentage
    {
        get
        {
            if (!started_) return 0f;

            double percentage = (float)videoPlayerMain_.time / (float)videoPlayerMain_.length;
            if (percentage > 0.97)
            {
                return 1f;
            }
            else
            {
                return (float)videoPlayerMain_.time / (float)videoPlayerMain_.length;
            }
        }
    }

    static public void PreloadVideo(VideoPlayer player, string path)
    {
        RenderTexture rt1 = new RenderTexture(1920, 1080, 0);
        player.GetComponent<VideoPlayer>().targetTexture = rt1;

        player.SetDirectAudioMute(0, GameManager.MuteAll);

        RawImage ri = player.GetComponentInChildren<RawImage>();
        ri.texture = rt1;

        if(GameManager.PromptActive)
        {
            ri.transform.localScale = new Vector3(-1, 1, 1);
        }

        string[] split = path.Split('/');
        player.gameObject.name = split[split.Length - 1];
        player.playOnAwake = false;
        player.url = System.IO.Path.Combine(Application.streamingAssetsPath, path);
    }

    public override void ReceiveAction(string action)
    {
        base.ReceiveAction(action);
    }
}
