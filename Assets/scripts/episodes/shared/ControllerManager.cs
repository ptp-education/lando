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
    [SerializeField] private Dropdown episodesDropdown_;
    [SerializeField] Image muteButton_;
    [SerializeField] Transform uiHolder_;

    [SerializeField] Transform stationHolder_;
    [SerializeField] List<StationManager> stationManagerPrefabs_;

    private List<string> episodePaths = new List<string>();
    private CommandDispatch dispatch_ = new CommandDispatch();
    private int testNfcId_ = 9999;

    private List<StationManager> loadedStationManagers_ = new List<StationManager>();

    private void Start()
    {
        if (Application.isEditor)
        {
            shareManager_.localScale = Application.isEditor ? new Vector3(1f, 1f, 1f) : new Vector3(-1f, 1f, 1f);
        }

        RefreshEpisodeList();
        RefreshMuteButton();

        //GetConnectors();
    }

    public override void Init(NetworkManager nm)
    {
        base.Init(nm);

        LoadStationManagers();
    }

    private async void GetConnectors()
    {
        SmartObjectManager manager = SmartObjectManager.Instance;
        if (manager != null)
        {
            SmartObjectConnector legoStoreConnector = await manager.GetSmartConnector(SmartObjectType.ResourceStation);
            SmartObjectConnector magicPadConnector = await manager.GetSmartConnector(SmartObjectType.HintStation);
            SmartObjectConnector magicPrinterConnector = await manager.GetSmartConnector(SmartObjectType.TestingStation);

            legoStoreConnector.Connect(this.NewNfcScan);
            magicPadConnector.Connect(this.NewNfcScan);
            magicPrinterConnector.Connect(this.NewNfcScan);
        }
    }

    private void NewNfcScan(string nfcId, SmartObjectType stationType)
    {
        dispatch_.NewNfcScan(nfcId, stationType);
    }

    private void LoadStationManagers()
    {
        //debug only - should not need station managers in controller in production
        if (!Application.isEditor) return;

        foreach(StationManager p in stationManagerPrefabs_)
        {
            StationManager station = Instantiate(p);
            station.transform.SetParent(stationHolder_);
            station.transform.localPosition = Vector3.zero;
            station.transform.localScale = Vector3.zero;
            loadedStationManagers_.Add(station);

            networkManager_.AddNewGameManager(station);
        }
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

        dispatch_.Init(this);
    }

    protected override void NewActionInternal(string a)
    {
        base.NewActionInternal(a);

        List<string> validatorArgs = ArgumentHelper.ArgumentsFromCommand("-validator", a);

        if (validatorArgs.Count > 1)
        {
            List<string> additionalArgs = new List<string>(validatorArgs.GetRange(2, validatorArgs.Count - 2));
            dispatch_.NewValidatorAction(validatorArgs[0], validatorArgs[1], additionalArgs);
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
            RefreshStation(0);
        }
        else if (Input.GetKeyUp("2"))
        {
            RefreshStation(1);
        }
        else if (Input.GetKeyUp("3"))
        {
            RefreshStation(2);
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

    private void RefreshStation(int counter)
    {
        if (counter >= loadedStationManagers_.Count) return;

        StationManager m = loadedStationManagers_[counter];
        if (m.transform.localScale == Vector3.one)
        {
            m.transform.localScale = Vector3.zero;
        } else
        {
            foreach(StationManager sm in loadedStationManagers_)
            {
                sm.transform.localScale = sm == m ? Vector3.one : Vector3.zero;
            }
        }
    }

    private void SendActionFromButton(string buttonCommand)
    {
        switch(buttonCommand)
        {
            case "scan-store":
                NewNfcScan(testNfcId_.ToString(), SmartObjectType.ResourceStation);
                break;
            case "scan-hint":
                NewNfcScan(testNfcId_.ToString(), SmartObjectType.HintStation);
                break;
            case "scan-test":
                NewNfcScan(testNfcId_.ToString(), SmartObjectType.TestingStation);
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