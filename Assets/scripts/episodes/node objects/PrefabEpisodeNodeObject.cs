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

    private void OnDestroy()
    {
        if (content_ != null)
        {
            GameObject.Destroy(content_.gameObject);
        }
    }

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
            renderTexture_.graphicsFormat = UnityEngine.Experimental.Rendering.GraphicsFormat.R8G8B8A8_UNorm;
            renderTexture_.depth = 24;
        }
        camera_.targetTexture = renderTexture_;
        cameraImage_.texture = renderTexture_;

        if (content_ != null)
        {
            content_.Play();
        }
    }

    public override void Loop()
    {
        base.Loop();

        if (content_ != null)
        {
            content_.Loop();
        }
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

        if (content_ != null)
        {
            content_.ReceiveAction(action);
        }
    }
}
