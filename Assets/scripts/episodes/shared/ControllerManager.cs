using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private Dropdown episodesDropdown_;
    [SerializeField] Image muteButton_;
    [SerializeField] Transform uiHolder_;

    private List<string> episodePaths = new List<string>();

    private void Start()
    {
        GameManager.PromptActive = true;

        TextAsset fileNamesAsset = Resources.Load<TextAsset>("all_episodes");
        StringsFile sf = JsonUtility.FromJson<StringsFile>(fileNamesAsset.text);
        foreach (string fileName in sf.FileNames)
        {
            Dropdown.OptionData od = new Dropdown.OptionData();
            od.text = fileName.StripExtensions();
            episodesDropdown_.options.Add(od);
        }

        episodePaths = new List<string>(sf.FileNames);

        RefreshMuteButton();
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
        else if (Input.GetKeyUp("a"))
        {
            SendActionFromButton("scan-store");
        }
        else if (Input.GetKeyUp("b"))
        {
            SendActionFromButton("scan-hint");
        }
        else if (Input.GetKeyUp("c"))
        {
            SendActionFromButton("scan-test");
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
                SendNewAction("-nfc store scan 9999");
                break;
            case "scan-hint":
                SendNewAction("-nfc hint scan 9999");
                break;
            case "scan-test":
                SendNewAction("-nfc test scan 9999");
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