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
    private Dictionary<string, OnscreenCharacter> characters_ = new Dictionary<string, OnscreenCharacter> ();

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

        if (a.Contains(FADE_COMMAND))
        {
            HandleFade(a);
        }

        if (a.Contains(FADEOUT_COMMAND))
        {
            HandleFadeOut(a);
        }

        if (activeNode_ != null)
        {
            activeNode_.ReceiveAction(a);
        }

        if (a.Contains(ZONE_ACTIVE))
        {
            HandleZoneUpdated();
        }

        if (a.Contains(TERMINAL_COMMAND))
        {
            HandleTerminalCommand(a);
        }

        if (a.Contains(PRINT_COMMAND))
        {
            HandlePrintCommand(a);
        }

        if (a.Contains(CHARACTER_COMMAND))
        {
            HandleCharacterCommand(a);
        }
    }

    private void RefreshCharacters(EpisodeNode nextNode)
    {
        foreach(EpisodeNode.Character nextCharacter in nextNode.Characters)
        {
            if (!characters_.ContainsKey(nextCharacter.Name))
            {
                OnscreenCharacter characterPrefab = Resources.Load<OnscreenCharacter>("characters/" + nextCharacter.Name);
                OnscreenCharacter newCharacter = Instantiate<OnscreenCharacter>(characterPrefab, transform);
                characters_[nextCharacter.Name] = newCharacter;
            }

            characters_[nextCharacter.Name].transform.localPosition = nextCharacter.TalkingPosition;
            characters_[nextCharacter.Name].transform.localScale = nextCharacter.Scale;
            characters_[nextCharacter.Name].transform.SetSiblingIndex(fadeOverlay_.transform.GetSiblingIndex() - 1);
        }

        for (int i = 0; i < characters_.Keys.Count; i++)
        {
            string characterKey = characters_.Keys.ToList<string>()[i];
            if (!nextNode.Characters.Exists(c => string.Equals(characterKey, c.Name)))
            {
                Destroy(characters_[characterKey].gameObject);
                characters_.Remove(characterKey);
            }
        }
    }

    private IEnumerator UpdateEpisodeNode(EpisodeNode currentNode)
    {
        EpisodeNodeObject previousNode = activeNode_;
        activeNode_ = LoadEpisodeNodeObject(currentNode);

        activeNode_.Reset();

        if (fadeFlow_ != null)
        {
            fadeFlow_.complete();
            fadeFlow_ = null;
        }

        if (currentNode.FadeInFromPreviousScene)
        {
            fadeFlow_ = new GoTweenFlow();
            fadeFlow_.insert(0f, new GoTween(fadeOverlay_, 0.3f, new GoTweenConfig().colorProp("color", new Color(0, 0, 0, 1f))));
            fadeFlow_.insert(0.9f, new GoTween(fadeOverlay_, 0.7f, new GoTweenConfig().colorProp("color", new Color(0, 0, 0, 0f))));
            fadeFlow_.play();

            yield return new WaitForSeconds(0.8f);
        }


        while (!activeNode_.IsPlaying)
        {
            yield return 0;
        }

        HandleChoices(currentNode);
        RefreshCharacters(currentNode);

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

    #region HANDLERS

    private void HandleFade(string action)
    {
        string[] split = action.Split(' ');

        string lengthText = null;
        for (int i = 0; i < split.Length; i++)
        {
            if (string.Equals(FADE_COMMAND, split[i]))
            {
                if (i < split.Length - 1)
                {
                    lengthText = split[i + 1];
                    break;
                }
            }
        }

        if (lengthText != null)
        {
            float length = -1f;
            float.TryParse(lengthText, out length);
            if (length != -1f)
            {
                if (fadeFlow_ != null)
                {
                    fadeFlow_.complete();
                    fadeFlow_ = null;
                }

                fadeOverlay_.color = Color.black;

                fadeFlow_ = new GoTweenFlow();
                fadeFlow_.insert(0f, new GoTween(fadeOverlay_, 0.01f, new GoTweenConfig().colorProp("color", new Color(0, 0, 0, 1f))));
                fadeFlow_.insert(length - 0.3f, new GoTween(fadeOverlay_, 0.3f, new GoTweenConfig().colorProp("color", new Color(0, 0, 0, 0f))));
                fadeFlow_.play();
            }
        }
    }

    private void HandleFadeOut(string action)
    {
        string[] split = action.Split(' ');

        string lengthText = null;
        for (int i = 0; i < split.Length; i++)
        {
            if (string.Equals(FADEOUT_COMMAND, split[i]))
            {
                if (i < split.Length - 1)
                {
                    lengthText = split[i + 1];
                    break;
                }
            }
        }

        if (lengthText != null)
        {
            float length = -1f;
            float.TryParse(lengthText, out length);
            if (length != -1f)
            {
                if (fadeFlow_ != null)
                {
                    fadeFlow_.complete();
                    fadeFlow_ = null;
                }

                fadeFlow_ = new GoTweenFlow();
                fadeFlow_.insert(0f, new GoTween(fadeOverlay_, length, new GoTweenConfig().colorProp("color", new Color(0, 0, 0, 1f))));
                fadeFlow_.play();
            }
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

        bool containsChoice = false;

        foreach (EpisodeNode.Option o in n.Options)
        {
            if (o.Action.Contains("-spawnoption"))
            {
                choicesHolder_.AddOption(o.Name);
                containsChoice = true;
            }
        }

        if (containsChoice)
        {
            AudioPlayer.PlayAudio("audio/sfx/new-option");
        }
    }

    private void HandleTerminalCommand(string a)
    {
        string[] split = a.Split(' ');

        int start = a.IndexOf(TERMINAL_COMMAND);
        int firstQuote = a.IndexOf('\"', start);
        int secondQuote = a.IndexOf('\"', firstQuote + 1);
        if (firstQuote == -1 || secondQuote == -1)
        {
            Debug.LogWarning("Could not find matching quotes for argument " + TERMINAL_COMMAND + ": " + a);
            return;
        }

        string cmd = a.Substring(firstQuote + 1, secondQuote - firstQuote - 1);

        CommandLineHelper.ExecuteProcessTerminal(a.Substring(firstQuote + 1, secondQuote - firstQuote - 1));
    }

    private void HandlePrintCommand(string a)
    {
        string[] split = a.Split(' ');

        string print = null;
        for (int i = 0; i < split.Length; i++)
        {
            if (string.Equals(split[i], PRINT_COMMAND))
            {
                if (i + 1 < split.Length)
                {
                    print = split[i + 1];
                    break;
                }
            }
        }
        if (print != null)
        {
            CommandLineHelper.PrintPdf(print);
        }
        else
        {
            Debug.LogWarning("Could not find enough arguments to print. " + a);
        }
    }

    private void HandleZoneUpdated()
    {
        foreach (OnscreenCharacter c in characters_.Values)
        {
            c.Wave();
        }
        CommandLineHelper.ExecuteProcessTerminal("osascript \"~/Desktop/legov5/press-12-lightkey.scpt\"");
    }

    private void HandleCharacterCommand(string a)
    {
        List<string> args = ArgumentsFromCommand(CHARACTER_COMMAND, a.Split(' '));

        List<OnscreenCharacter> characters = characters_.Values.ToList<OnscreenCharacter>();

        switch (args[0])
        {
            case "print":
                characters.ForEach(c => c.TalkAndPrint(args.GetRange(2, args.Count - 2), args[1], episode_.VORoot));
                break;
            case "talk":
                characters.ForEach(c => c.Talk(args.GetRange(1, args.Count - 1), episode_.VORoot));
                break;
            case "cheer":
                characters.ForEach(c => c.Cheer(args.GetRange(1, args.Count - 1), episode_.VORoot));
                break;
            case "wave":
                characters.ForEach(c => c.Wave());
                break;

        }
    }

    private List<string> ArgumentsFromCommand(string command, string[] commands)
    {
        List<string> args = new List<string>();

        for (int i = 0; i < commands.Length; i++)
        {
            if (string.Equals(commands[i], command))
            {
                for (int ii = 0; ii < commands.Length - i; ii++)
                {
                    int indexToCheck = i + 1 + ii;

                    if (indexToCheck < commands.Length && !commands[indexToCheck].StartsWith("-"))
                    {
                        args.Add(commands[indexToCheck]);
                    }
                    else
                    {
                        return args;
                    }
                }
            }
        }

        return args;
    }

    #endregion
}
