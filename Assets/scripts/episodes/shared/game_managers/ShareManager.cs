using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class ShareManager : GameManager
{
    public const string PREFAB_PATH = "prefabs/episode_objects/";

    [SerializeField] private Transform nodeObjectParent_;

    [HideInInspector] public Transform OverlayParent;
    [HideInInspector] public Transform CharacterParent;

    private EpisodeNodeObject activeNode_;
    private EpisodeNode.OptionsHolder activeOption_;

    private List<GameObject> spawnedObjects_ = new List<GameObject>();

    private Image fadeOverlay_;
    private GoTweenFlow fadeFlow_;
    private ChoicesHolder choicesHolder_;
    private Dictionary<string, OnscreenCharacter> characters_ = new Dictionary<string, OnscreenCharacter> ();

    private void Start()
    {
        GameObject fadeOverlayObject = new GameObject("Fade Overlay");
        fadeOverlay_ = fadeOverlayObject.AddComponent<Image>();
        fadeOverlay_.color = Color.black;
        fadeOverlay_.color = new Color(0, 0, 0, 0);
        fadeOverlay_.transform.SetParent(transform, false);
        fadeOverlay_.transform.localPosition = Vector3.zero;
        fadeOverlay_.rectTransform.sizeDelta = new Vector2(1920, 1080);

        OverlayParent = new GameObject("Overlay Parent").GetComponent<Transform>();
        OverlayParent.transform.SetParent(transform, false);
        OverlayParent.transform.localPosition = Vector3.zero;

        CharacterParent = new GameObject("Character Parent").GetComponent<Transform>();
        CharacterParent.transform.SetParent(transform, false);
        CharacterParent.transform.localPosition = Vector3.zero;

        ChoicesHolder choicesPrefab = Resources.Load<ChoicesHolder>("prefabs/episode_objects/choices_parent");
        choicesHolder_ = GameObject.Instantiate<ChoicesHolder>(choicesPrefab, transform);
    }

    private void Update()
    {
        if (fadeOverlay_ != null)
        {
            fadeOverlay_.transform.SetAsLastSibling();
        }
        if (choicesHolder_ != null)
        {
            choicesHolder_.transform.SetSiblingIndex(transform.childCount - 2);
        }
        OverlayParent.transform.SetSiblingIndex(transform.childCount - 4);
        CharacterParent.transform.SetSiblingIndex(transform.childCount - 3);
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

        if (ArgumentHelper.ContainsCommand(RADIO_COMMAND, a))
        {
            AudioPlayer.StartRadio();
        }

        if (ArgumentHelper.ContainsCommand(FADEOUT_COMMAND, a))
        {
            HandleFadeOut(a);
        }

        if (ArgumentHelper.ContainsCommand(FADEIN_COMMAND, a))
        {
            HandleFadeIn(a);
        }

        if (activeNode_ != null)
        {
            activeNode_.ReceiveAction(a);
        }

        if (ArgumentHelper.ContainsCommand(HIDE_ALL, a))
        {
            HandleHideAll();
        }

        if (ArgumentHelper.ContainsCommand(TERMINAL_COMMAND, a))
        {
            HandleTerminalCommand(a);
        }

        if (ArgumentHelper.ContainsCommand(PRINT_COMMAND, a))
        {
            HandlePrintCommand(a);
        }

        if (ArgumentHelper.ContainsCommand(HIDE_CHOICES, a))
        {
            choicesHolder_.ToggleVisbility(false);
        }

        if (ArgumentHelper.ContainsCommand(SHOW_CHOICES, a))
        {
            choicesHolder_.ToggleVisbility(true);
        }

        if (ArgumentHelper.ContainsCommand(DIDI_HMMM, a))
        {
            SendNewActionInternal("-character talk hm-1 hm-2 hm-3 hm-4 hm-5 hm-6 hm-7 hm-8 hm-9 hm-10");
        }

        if (ArgumentHelper.ContainsCommand(CHARACTER_COMMAND, a))
        {
            HandleCharacterCommand(a);
        }

        if (ArgumentHelper.ContainsCommand(NODE_COMMAND, a))
        {
            HandleNodeAction(a);
        }

        if (ArgumentHelper.ContainsCommand(CLAIM_REWARD, a))
        {
            HandleClaimReward(a);
        }

        if (ArgumentHelper.ContainsCommand(CHANGE_OPTIONS, a))
        {
            HandleUpdateOptions(a);
        }
    }

    public void NewOptionSelected(int option, bool isTeacher, string userId)
    {
        SendNewActionInternal(string.Format("{0} {1}", OPTION_SELECT, option.ToString()));

        if (choicesHolder_.IsActive)
        {
            option = option - 1;
            if (activeOption_.Options.Count > option)
            {
                EpisodeNode.Option selectedOption = activeOption_.Options[option];

                if (!isTeacher && selectedOption.TeacherOnly)
                {
                    //don't run teacher only commands if the scanner is not a teacher
                    return;
                }

                if (selectedOption.Command != null && selectedOption.Command.Length > 0)
                {
                    SendNewActionInternal(string.Format(selectedOption.Command, userId));
                }

                if (selectedOption.EventObject != null)
                {
                    choicesHolder_.ToggleVisbility(false);

                    EventObject eo = Instantiate(selectedOption.EventObject);
                    eo.transform.SetParent(OverlayParent);
                    eo.transform.localScale = Vector3.one;
                    eo.transform.localPosition = Vector3.zero;
                    eo.Init(EventObject.Type.Projector, this, () =>
                    {
                        choicesHolder_.ToggleVisbility(true);
                    });
                    spawnedObjects_.Add(eo.gameObject);
                }
            }
        }
    }

    private void RefreshCharacters(EpisodeNode nextNode)
    {
        foreach(EpisodeNode.Character nextCharacter in nextNode.Characters)
        {
            if (!characters_.ContainsKey(nextCharacter.Name))
            {
                OnscreenCharacter characterPrefab = Resources.Load<OnscreenCharacter>("characters/" + nextCharacter.Name);
                OnscreenCharacter newCharacter = Instantiate<OnscreenCharacter>(characterPrefab, CharacterParent);
                newCharacter.Init(this);
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

        choicesHolder_.DeleteOptions();

        for (int i = 0; i < spawnedObjects_.Count; i++)
        {
            Destroy(spawnedObjects_[i].gameObject);
        }
        spawnedObjects_ = new List<GameObject>();

        if (fadeFlow_ != null)
        {
            fadeFlow_.complete();
            fadeFlow_ = null;
        }

        if (currentNode.FadeInFromPreviousScene)
        {
            HandleFadeIn(GameManager.FADEIN_COMMAND + " 1.5");

            yield return new WaitForSeconds(0.4f);
        }

        while (!activeNode_.IsPlaying)
        {
            yield return 0;
        }

        RefreshCharacters(currentNode);

        if (currentNode.OptionsToSpawn.Count > 0)
        {
            UpdateOptions(currentNode.OptionsToSpawn[0]);
        }

        for (int i = 0; i < 8; i++)
        {
            yield return 0;
        }

        if (previousNode != null)
        {
            previousNode.Reset();
            Destroy(previousNode.gameObject);
        }
    }

    private void UpdateOptions(EpisodeNode.OptionsHolder optionsHolder)
    {
        activeOption_ = optionsHolder;
        choicesHolder_.UpdateChoices(optionsHolder);
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
            case EpisodeNode.EpisodeType.Simulator:
                prefabPath += "simulator_player";
                break;
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

    #region HANDLERS

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

    public void HandleUpdateOptions(string a)
    {
        List<string> args = ArgumentHelper.ArgumentsFromCommand(CHANGE_OPTIONS, a);

        if (args.Count > 0)
        {
            EpisodeNode.OptionsHolder optionsHolder = activeNode_.Node.OptionsToSpawn.Find(o => string.Equals(o.Name, args[0]));
            if (optionsHolder != null)
            {
                UpdateOptions(optionsHolder);
            }
        }
    }

    public void HandleClaimReward(string a)
    {
        List<string> args = ArgumentHelper.ArgumentsFromCommand(CLAIM_REWARD, a);

        if (args.Count < 1) return;

        string id = args[0];

        GameStorage.UserData userData = UserDataForUserId(id);

        if (userData.RedeemedChallenges.Count < userData.CompletedChallenges.Count)
        {
            List<string> unredeemedChallenges = new List<string>(userData.CompletedChallenges);
            for (int i = unredeemedChallenges.Count - 1; i >= 0; i--)
            {
                if (userData.RedeemedChallenges.Contains(unredeemedChallenges[i]))
                {
                    unredeemedChallenges.RemoveAt(i);
                }
            }

            if (unredeemedChallenges.Count > 0)
            {
                string challengeToRedeem = unredeemedChallenges[0];
                userData.RedeemedChallenges.Add(challengeToRedeem);
                SaveUserData(userData, id);

                LevelData.Challenge c = FindChallenge(challengeToRedeem);

                SendNewActionInternal(string.Format(c.RewardCommand, id));
            }
        }
    }


    private void HandleNodeAction(string a)
    {
        List<string> args = ArgumentHelper.ArgumentsFromCommand(NODE_COMMAND, a);
        if (args.Count > 0)
        {
            switch(args[0])
            {
                case "back":
                    MoveNode(-1);
                    break;
                case "next":
                    MoveNode(1);
                    break;
                case "seek":
                    SeekNode(args[1]);
                    break;
            }
        }
    }

    private void MoveNode(int byAmount)
    {
        EpisodeNode newNode = episode_.JumpFromNode(activeNode_.name, byAmount);

        if (newNode == null)
        {
            Debug.LogWarning(string.Format("Unsuccessfully tried to jump from node \"{0}\" by amount {1}.", activeNode_.name, byAmount));
            return;
        }
        LoadNewNode(newNode.name);
    }

    private void SeekNode(string nodeName)
    {
        EpisodeNode newNode = episode_.FindNode(nodeName);
        if (newNode == null)
        {
            Debug.LogWarning("Couldn't find node with name: " + nodeName);
        }
        LoadNewNode(newNode.name);
    }

    private void HandleFadeIn(string a)
    {
        List<string> args = ArgumentHelper.ArgumentsFromCommand(FADEIN_COMMAND, a);

        float duration = 1f;
        if (args.Count > 0)
        {
            duration = float.Parse(args[0]);
        }

        if (fadeFlow_ != null && fadeFlow_.state == GoTweenState.Running)
        {
            fadeFlow_.destroy();
            fadeOverlay_.color = Color.black;
        }

        fadeFlow_ = new GoTweenFlow();
        fadeFlow_.insert(0f, new GoTween(fadeOverlay_, 0.15f, new GoTweenConfig().colorProp("color", new Color(0, 0, 0, 1f))));
        fadeFlow_.insert(duration - 0.25f, new GoTween(fadeOverlay_, 0.25f, new GoTweenConfig().colorProp("color", new Color(0, 0, 0, 0f))));
        fadeFlow_.play();
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

    private void HandleHideAll()
    {
        SendNewActionInternal("-art-holder hide -guideHideGuides -hidespawnoption");

        if (activeNode_ != null)
        {
            activeNode_.Hide();
        }
    }

    private void HandleCharacterCommand(string a)
    {
        List<string> args = ArgumentHelper.ArgumentsFromCommand(CHARACTER_COMMAND, a);

        List<OnscreenCharacter> characters = characters_.Values.ToList<OnscreenCharacter>();

        switch (args[0])
        {
            case "print":
                characters.ForEach(c => c.TalkAndPrint(args.GetRange(2, args.Count - 2), args[1], episode_.VORoot));
                break;
            case "delayed-talk":
                characters.ForEach(c => c.DelayedTalk(args[1], args.GetRange(2, args.Count - 2), episode_.VORoot));
                break;
            case "talk":
                characters.ForEach(c => c.Talk(args.GetRange(1, args.Count - 1), episode_.VORoot));
                break;
            case "progression-talk":
                characters.ForEach(c => c.ProgressionTalk(args[1], episode_.VORoot));
                break;
        }
    }

    #endregion
}
