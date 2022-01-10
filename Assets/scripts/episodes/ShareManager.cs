using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class ShareManager : GameManager
{
    public const string PREFAB_PATH = "prefabs/episode_objects/";

    [SerializeField] private Transform nodeObjectParent_;

    private EpisodeNodeObject activeNode_;

    private Image fadeOverlay_;
    private GoTweenFlow fadeFlow_;

    private void Start()
    {
        if (GameManager.PromptActive)
        {
            nodeObjectParent_.transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        GameObject overlay = new GameObject("Fade Overlay");
        fadeOverlay_ = overlay.AddComponent<Image>();
        fadeOverlay_.color = Color.black;
        fadeOverlay_.color = new Color(0, 0, 0, 0);
        fadeOverlay_.transform.SetParent(transform, false);
        fadeOverlay_.transform.localPosition = Vector3.zero;
        fadeOverlay_.rectTransform.sizeDelta = new Vector2(1920, 1080);
    }

    private void Update()
    {
        if (fadeOverlay_ != null)
        {
            fadeOverlay_.transform.SetAsLastSibling();
        }
    }

    protected override void NewNodeEventInternal(EpisodeNode node)
    {
        base.NewNodeEventInternal(node);

        StartCoroutine(UpdateEpisodeNode(currentNode_));
        HandleBackgroundLoop(currentNode_);
    }

    protected override void NewEpisodeEventInternal(Episode e)
    {
        base.NewEpisodeEventInternal(e);
    }

    protected override void NewActionInternal(string a)
    {
        base.NewActionInternal(a);

        if (string.Equals(RADIO_COMMAND, a))
        {
            AudioPlayer.StartRadio();
        }

        if (activeNode_ != null)
        {
            activeNode_.ReceiveAction(a);
        }
    }

    private void HandleBackgroundLoop(EpisodeNode node)
    {
        if (node.BgLoopPath != null && node.BgLoopPath.Length > 0)
        {
            AudioPlayer.LoopAudio(node.BgLoopPath, AudioPlayer.kMain);
        }
    }

    private IEnumerator UpdateEpisodeNode(EpisodeNode currentNode)
    {
        if (currentNode.FadeInFromPreviousScene)
        {
            if (fadeFlow_ != null)
            {
                fadeFlow_.complete();
                fadeFlow_ = null;
            }
            fadeFlow_ = new GoTweenFlow();
            fadeFlow_.insert(0f, new GoTween(fadeOverlay_, 0.3f, new GoTweenConfig().colorProp("color", new Color(0, 0, 0, 1f))));
            fadeFlow_.insert(1.3f, new GoTween(fadeOverlay_, 0.7f, new GoTweenConfig().colorProp("color", new Color(0, 0, 0, 0f))));
            fadeFlow_.play();

            yield return new WaitForSeconds(0.8f);
        }

        EpisodeNodeObject newObject = LoadEpisodeNodeObject(currentNode);
        newObject.Play();

        while (!newObject.IsPlaying)
        {
            yield return 0;
        }

        for (int i = 0; i < 8; i++)
        {
            yield return 0;
        }

        if (activeNode_ != null)
        {
            Destroy(activeNode_.gameObject);
        }

        activeNode_ = newObject;
    }

    private EpisodeNodeObject LoadEpisodeNodeObject(EpisodeNode node)
    {
        EpisodeNodeObject nodeObject = null;
        string prefabPath = PREFAB_PATH;
        switch (node.Type)
        {
            case EpisodeNode.EpisodeType.Video:
                prefabPath += "video_player";
                break;
            case EpisodeNode.EpisodeType.Image:
                prefabPath += "image_player";
                break;
            case EpisodeNode.EpisodeType.LoopWithOptions:
                prefabPath += "loopwithoptions_player";
                break;
            case EpisodeNode.EpisodeType.PREFAB_DEPRECATED:
                Debug.LogWarning("Prefab objects have been deprecated");
                return null;
        }
        EpisodeNodeObject o = Resources.Load<EpisodeNodeObject>(prefabPath);
        nodeObject = GameObject.Instantiate<EpisodeNodeObject>(o);

        nodeObject.gameObject.name = node.gameObject.name;
        nodeObject.transform.SetParent(nodeObjectParent_);
        nodeObject.transform.localPosition = Vector3.zero;
        nodeObject.transform.localScale = Vector3.one;
        nodeObject.transform.SetAsFirstSibling();

        nodeObject.Init(this, node);

        return nodeObject;
    }

    private string Key(EpisodeNode node)
    {
        return node.ToString();
    }

    public float ProgressPercentage
    {
        get
        {
            return activeNode_ == null ? 0f : activeNode_.ProgressPercentage;
        }
    }
}
