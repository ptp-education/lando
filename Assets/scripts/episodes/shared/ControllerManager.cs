using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Lando.SmartObjects;

public class StringsFile
{
    public string[] FileNames;

    public StringsFile(string[] fileNames)
    {
        this.FileNames = fileNames;
    }
}

public class ControllerManager : GameManager
{
    [SerializeField] Transform shareManager_;
    [SerializeField] Transform testingButtonHolder_;
    [SerializeField] Button testingButtonPrefab_;
    [SerializeField] private Dropdown episodesDropdown_;
    [SerializeField] Image muteButton_;
    [SerializeField] Transform uiHolder_;

    private List<string> episodePaths = new List<string>();
    private CommandDispatch dispatch_ = new CommandDispatch();
    private int testNfcId_ = 9999;

    private void Start()
    {
        if (Application.isEditor)
        {
            shareManager_.localScale = Application.isEditor ? new Vector3(1f, 1f, 1f) : new Vector3(-1f, 1f, 1f);
        }

        RefreshEpisodeList();
        RefreshMuteButton();
    }

    private void RefreshEpisodeList()
    {
        TextAsset fileNamesAsset = Resources.Load<TextAsset>("all_episodes");
        StringsFile sf = JsonUtility.FromJson<StringsFile>(fileNamesAsset.text);
        foreach (string fileName in sf.FileNames)
        {
            Dropdown.OptionData od = new Dropdown.OptionData();
            od.text = fileName.StripExtensions();
            episodesDropdown_.options.Add(od);
        }

        episodePaths = new List<string>(sf.FileNames);
    }

    protected override void NewEpisodeEventInternal(Episode e)
    {
        base.NewEpisodeEventInternal(e);

        dispatch_.Init(ChallengeData, this);
    }

    protected override void NewNodeEventInternal(EpisodeNode node)
    {
        base.NewNodeEventInternal(node);

        RemoveTestingButtons();

        if (currentNode_.TestingActive)
        {
            AddTestingButtons();
        }
    }

    protected override void NewActionInternal(string a)
    {
        base.NewActionInternal(a);

        List<string> nfcArgs = ArgumentHelper.ArgumentsFromCommand("-nfc", a);
        List<string> validatorArgs = ArgumentHelper.ArgumentsFromCommand("-validator", a);

        if (nfcArgs.Count > 1)
        {
            dispatch_.NewNfc(nfcArgs[0], nfcArgs[1]);
        }
        if (validatorArgs.Count > 1)
        {
            List<string> additionalArgs = new List<string>(validatorArgs.GetRange(2, validatorArgs.Count - 2));
            dispatch_.NewValidatorAction(validatorArgs[0], validatorArgs[1], additionalArgs);
        }
    }

    private void AddTestingButtons()
    {
        if (ChallengeData == null)
        {
            return;
        }

        List<KeyValuePair<string, string>> buttons = new List<KeyValuePair<string, string>>();

        buttons.Add(new KeyValuePair<string, string>(
            "Tested successfully",
            string.Format(
                "-validator {0} {1}",
                SmartObjectType.TestingStation.ToString(),
                CommandDispatch.ValidatorResponse.Success.ToString()
            )
        ));
        buttons.Add(new KeyValuePair<string, string>(
            "Tested and failed",
            string.Format(
                "-validator {0} {1}",
                SmartObjectType.TestingStation.ToString(),
                CommandDispatch.ValidatorResponse.Failure.ToString()
            )
        ));

        foreach(LevelData.BeforeTestFail failOption in ChallengeData.WaysToFail)
        {
            buttons.Add(new KeyValuePair<string, string>(failOption.ButtonName, failOption.Command));
        }

        foreach(KeyValuePair<string, string> b in buttons)
        {
            Button newButton = GameObject.Instantiate<Button>(testingButtonPrefab_);
            newButton.GetComponentInChildren<Text>().text = b.Key;
            newButton.onClick.AddListener(() =>
            {
                SendNewAction(b.Value);
            });
            newButton.transform.SetParent(testingButtonHolder_);
        }
    }

    private void RemoveTestingButtons()
    {
        Button[] buttons = testingButtonHolder_.GetComponentsInChildren<Button>();
        for (int i = 0; i < buttons.Length; i++)
        {
            Destroy(buttons[i].gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp("t"))
        {
            uiHolder_.gameObject.SetActive(!uiHolder_.gameObject.activeSelf);
        }
        else if (Input.GetKeyUp("m"))
        {
            OnMutePress();
        }
        else if (Input.GetKeyUp("1"))
        {
            SendActionFromButton("select-1");
        }
        else if (Input.GetKeyUp("2"))
        {
            SendActionFromButton("select-2");
        }
        else if (Input.GetKeyUp("3"))
        {
            SendActionFromButton("select-3");
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            SendActionFromButton("back");
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            SendActionFromButton("next");
        }
    }

    public void OnEpisodeLoadClick()
    {
        UpdateEpisode(episodePaths[episodesDropdown_.value]);
    }

    public void OnScrambleCodeClick()
    {
        testNfcId_ = Random.Range(0, 10000);
    }

    public void OnButtonPress(string command)
    {
        SendActionFromButton(command);
    }

    public void OnMutePress()
    {
        GameManager.MuteAll = !GameManager.MuteAll;
        RefreshMuteButton();
    }

    private void RefreshMuteButton()
    {
        muteButton_.color = GameManager.MuteAll ? Color.red : Color.green;
    }

    private void SendActionFromButton(string buttonCommand)
    {
        switch(buttonCommand)
        {
            case "scan-store":
                SendNewAction(string.Format("-nfc {0} {1}", SmartObjectType.ResourceStation.ToString(), testNfcId_));
                break;
            case "scan-hint":
                SendNewAction(string.Format("-nfc {0} {1}", SmartObjectType.HintStation.ToString(), testNfcId_));
                break;
            case "scan-test":
                SendNewAction(string.Format("-nfc {0} {1}", SmartObjectType.TestingStation.ToString(), testNfcId_));
                break;
            case "back":
                SendNewAction(NODE_COMMAND + " back");
                break;
            case "next":
                SendNewAction(NODE_COMMAND + " next");
                break;
            case "select-1":
                SendNewAction("-option 1");
                break;
            case "select-2":
                SendNewAction("-option 2");
                break;
            case "select-3":
                SendNewAction("-option 3");
                break;
        }
    }
}