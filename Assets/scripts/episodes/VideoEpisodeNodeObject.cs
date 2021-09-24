using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoEpisodeNodeObject : EpisodeNodeObject
{
    [SerializeField] private VideoPlayer videoPlayerMain_;
    [SerializeField] private VideoPlayer videoPlayerLoop_;

    public override void Init(ReadyToStartLoop callback)
    {
        base.Init(callback);

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

        videoPlayerMain_.Play();
        videoPlayerMain_.transform.localScale = Vector3.one;
        videoPlayerLoop_.transform.localScale = Vector3.zero;
    }

    public override void Loop()
    {
        base.Loop();

        videoPlayerLoop_.Play();
        videoPlayerLoop_.transform.localScale = Vector3.one;
        videoPlayerMain_.transform.localScale = Vector3.zero;
    }

    public override void Preload(EpisodeNode node)
    {
        base.Preload(node);

        PreloadVideo(videoPlayerMain_, node.VideoFilePath);
        PreloadVideo(videoPlayerLoop_, node.VideoLoopFilePath);
    }

    private void PreloadVideo(VideoPlayer player, string path)
    {
        RenderTexture rt1 = new RenderTexture(1920, 1080, 0);
        player.GetComponent<VideoPlayer>().targetTexture = rt1;

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
