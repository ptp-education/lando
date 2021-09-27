using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrefabEpisodeNodeObject : EpisodeNodeObject
{
    [SerializeField] RawImage cameraImage_;

    private RenderTexture renderTexture_;
    private PrefabContent content_;
    private Camera camera_;

    public override void Init(EpisodeNode node, ReadyToStartLoop callback)
    {
        base.Init(node, callback);
    }

    public override void Hide()
    {
        base.Hide();

        cameraImage_.texture = null;
    }

    public override void Play()
    {
        base.Play();

        if (renderTexture_ == null) {
            renderTexture_ = new RenderTexture(1920, 1080, 0);
        }
        camera_.targetTexture = renderTexture_;
        cameraImage_.texture = renderTexture_;
    }

    public override void Loop()
    {
        base.Loop();
    }

    public override void Preload(EpisodeNode node)
    {
        base.Preload(node);

        PrefabContent o = Resources.Load<PrefabContent>(ShareManager.PREFAB_PATH + node.PrefabPath);
        content_ = GameObject.Instantiate<PrefabContent>(o);
        camera_ = content_.Camera;
    }

    public override void ReceiveAction(string action)
    {
        base.ReceiveAction(action);

        content_.ReceiveAction(action);
    }
}
