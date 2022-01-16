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
    private ChoicesHolder choicesHolder_;
    private Dictionary<string, KeyValuePair<Image, Image>> characterBubbles_ = new Dictionary<string, KeyValuePair<Image, Image>> ();

    private void Start()
    {
        nodeObjectParent_.transform.localScale = new Vector3(-1f, 1f, 1f);

        GameObject overlay = new GameObject("Fade Overlay");
        fadeOverlay_ = overlay.AddComponent<Image>();
        fadeOverlay_.color = Color.black;
        fadeOverlay_.color = new Color(0, 0, 0, 0);
        fadeOverlay_.transform.SetParent(transform, false);
        fadeOverlay_.transform.localPosition = Vector3.zero;
        fadeOverlay_.rectTransform.sizeDelta = new Vector2(1920, 1080);

        ChoicesHolder choicesPrefab = Resources.Load<ChoicesHolder>("prefabs/episode_objects/choices_parent");
        choicesHolder_ = GameObject.Instantiate<ChoicesHolder>(choicesPrefab, transform);
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

        AudioPlayer.StopLoop(AudioPlayer.kMain);
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

    private void HandleChoices(EpisodeNode n)
    {
        choicesHolder_.DeleteOptions();
        foreach(EpisodeNode.Option o in n.Options)
        {
            if (o.Action.Contains("-spawnoption"))
            {
                choicesHolder_.AddOption(o.Name);
            }
        }
    }

    private void HandleCharacterBubbles(EpisodeNode n)
    {
        foreach(KeyValuePair <Image, Image> i in characterBubbles_.Values)
        {
            Destroy(i.Key.gameObject);
            Destroy(i.Value.gameObject);
        }

        characterBubbles_ = new Dictionary<string, KeyValuePair<Image, Image>> ();
        foreach(EpisodeNode.CharacterVoiceBubble b in n.CharacterBubbles)
        {
            Sprite notTalkingSprite = null; Sprite talkingSprite = null;

            if (string.Equals(b.Character, "didi")) {
                if (b.Type == EpisodeNode.CharacterVoiceBubble.BubbleType.CharacterOnScreen)
                {
                    talkingSprite = Resources.Load<Sprite>("sprites/character_bubbles/didi-speaking");
                }
                else if (b.Type == EpisodeNode.CharacterVoiceBubble.BubbleType.CharacterOffScreen)
                {
                    notTalkingSprite = Resources.Load<Sprite>("sprites/character_bubbles/didi");
                    talkingSprite = Resources.Load<Sprite>("sprites/character_bubbles/didi-speaking");
                }
            }

            Image talkingImage = new GameObject(b.Character + " talking").AddComponent<Image>();
            talkingImage.sprite = talkingSprite;
            talkingImage.transform.SetParent(transform);
            talkingImage.transform.localPosition = b.BubblePosition;
            talkingImage.gameObject.SetActive(false);

            Image notTalkingImage = new GameObject("talking").AddComponent<Image>();
            notTalkingImage.sprite = notTalkingSprite;
            notTalkingImage.transform.SetParent(transform);
            notTalkingImage.transform.localPosition = b.BubblePosition;

            characterBubbles_.Add(b.Character, new KeyValuePair<Image, Image>(notTalkingImage, talkingImage));
        }
    }

    private IEnumerator UpdateEpisodeNode(EpisodeNode currentNode)
    {
        EpisodeNodeObject previousNode = activeNode_;
        activeNode_ = LoadEpisodeNodeObject(currentNode);

        if (currentNode.FadeInFromPreviousScene)
        {
            if (fadeFlow_ != null)
            {
                fadeFlow_.complete();
                fadeFlow_ = null;
            }
            fadeFlow_ = new GoTweenFlow();
            fadeFlow_.insert(0f, new GoTween(fadeOverlay_, 0.3f, new GoTweenConfig().colorProp("color", new Color(0, 0, 0, 1f))));
            fadeFlow_.insert(0.9f, new GoTween(fadeOverlay_, 0.7f, new GoTweenConfig().colorProp("color", new Color(0, 0, 0, 0f))));
            fadeFlow_.play();

            yield return new WaitForSeconds(0.8f);
        }

        activeNode_.Play();

        while (!activeNode_.IsPlaying)
        {
            yield return 0;
        }

        HandleChoices(currentNode);
        //HandleCharacterBubbles(currentNode);

        for (int i = 0; i < 8; i++)
        {
            yield return 0;
        }

        if (previousNode != null)
        {
            Destroy(previousNode.gameObject);
        }
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

    public float ProgressPercentage
    {
        get
        {
            return activeNode_ == null ? 0f : activeNode_.ProgressPercentage;
        }
    }
}
