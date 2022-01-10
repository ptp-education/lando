using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoEpisodeNodeObject : EpisodeNodeObject
{
    [SerializeField] private VideoPlayer videoPlayerMain_;
    [SerializeField] private VideoPlayer videoPlayerLoop_;

    private VideoPlayer activePlayer_;
    private bool started_ = false;
    private bool completed_ = false;

    public override void Init(GameManager gameManager, EpisodeNode node)
    {
        base.Init(gameManager, node);

        videoPlayerMain_.loopPointReached += MainVideoFinished;

        videoPlayerMain_.isLooping = false;
        videoPlayerLoop_.isLooping = true;

        Preload();
    }

    private void MainVideoFinished(VideoPlayer vp)
    {
        completed_ = true;

        videoPlayerLoop_.Stop();
        StartCoroutine(SwapPlayer(videoPlayerLoop_, videoPlayerMain_, pauseBetweenSwitch: true));
    }

    public override void Play()
    {
        base.Play();

        started_ = true;
        completed_ = false;

        videoPlayerMain_.Stop();
        StartCoroutine(SwapPlayer(videoPlayerMain_, videoPlayerLoop_, pauseBetweenSwitch: false));
    }

    private IEnumerator SwapPlayer(VideoPlayer play, VideoPlayer stop, bool pauseBetweenSwitch = false)
    {
        activePlayer_ = play;
        play.Prepare();

        while (!play.isPrepared)
        {
            yield return 0;
        }

        if (pauseBetweenSwitch)
        {
            yield return 0;
            yield return 0;
        }

        play.Play();

        if (pauseBetweenSwitch)
        {
            yield return 0;
            yield return 0;
        }

        stop.Stop();

        play.transform.localScale = Vector3.one;
        stop.transform.localScale = Vector3.zero;
    }

    public void Preload()
    {
        PreloadVideo(videoPlayerMain_, episodeNode_.VideoFilePath);
        PreloadVideo(videoPlayerLoop_, episodeNode_.VideoLoopFilePath);
    }

    public override bool IsPlaying
    {
        get
        {
            return activePlayer_ == null ? false : activePlayer_.isPlaying;
        }
    }

    public override float ProgressPercentage
    {
        get
        {
            if (!started_) return 0f;
            if (completed_) return 1f;

            return (float)videoPlayerMain_.time / (float)videoPlayerMain_.length;
        }
    }

    static public void PreloadVideo(VideoPlayer player, string path)
    {
        RenderTexture rt1 = new RenderTexture(1920, 1080, 0);
        player.GetComponent<VideoPlayer>().targetTexture = rt1;

        player.SetDirectAudioMute(0, GameManager.MuteAll);

        RawImage ri = player.GetComponentInChildren<RawImage>();
        ri.texture = rt1;

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
