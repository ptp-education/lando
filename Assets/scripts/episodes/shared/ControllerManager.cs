using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Lando.SmartObjects;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using Lando;

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
    [SerializeField] ShareManager shareManager_;
    [SerializeField] private Dropdown episodesDropdown_;
    [SerializeField] Image muteButton_;
    [SerializeField] Transform uiHolder_;

    [SerializeField] Transform stationHolder_;
    [SerializeField] List<StationManager> stationManagerPrefabs_;

    private List<string> episodePaths = new List<string>();
    private CommandDispatch dispatch_ = new CommandDispatch();
    private string testNfcId_ = "047F5E2AD86D80";

    private string kTeacherNfcId = "047F5E2AD86D80";

    private KeyValuePair<SmartObjectType, string> lastScannedNfcId_;

    private List<StationManager> loadedStationManagers_ = new List<StationManager>();

    private void Start()
    {
        shareManager_.transform.localScale = Application.isEditor ? new Vector3(1f, 1f, 1f) : new Vector3(-1f, 1f, 1f);

        RefreshEpisodeList();
        RefreshMuteButton();

#if !UNITY_IOS
        GetConnectors();
#endif
    }

    public override void Init(NetworkManager nm)
    {
        base.Init(nm);

#if UNITY_EDITOR
        LoadStationManagers();
#endif
    }

#if !UNITY_IOS
    private async void GetConnectors()
    {
        SmartObjectManager manager = Globals.SmartManager;

        if (manager != null)
        {
            SmartObjectConnector resourceStationConnector = await manager.GetSmartConnector(SmartObjectType.ResourceStation);
            SmartObjectConnector hintStationConnector = await manager.GetSmartConnector(SmartObjectType.HintStation);
            SmartObjectConnector testingStationConnector = await manager.GetSmartConnector(SmartObjectType.TestingStation);
            SmartObjectConnector option1Connector = await manager.GetSmartConnector(SmartObjectType.Option1);
            SmartObjectConnector option2Connector = await manager.GetSmartConnector(SmartObjectType.Option2);
            SmartObjectConnector option3Connector = await manager.GetSmartConnector(SmartObjectType.Option3);

            resourceStationConnector?.Connect(this.NewNfcScan);
            hintStationConnector?.Connect(this.NewNfcScan);
            testingStationConnector?.Connect(this.NewNfcScan);
            option1Connector?.Connect(this.NewNfcScan);
            option2Connector?.Connect(this.NewNfcScan);
            option3Connector?.Connect(this.NewNfcScan);
        }
    }
#endif

    private void NewNfcScan(string nfcId, SmartObjectType stationType)
    {
        if (lastScannedNfcId_.Key == stationType && lastScannedNfcId_.Value.Equals(nfcId))
        {
            if (lastScannedNfcId_.Key != SmartObjectType.Option1 && lastScannedNfcId_.Key != SmartObjectType.Option2 && lastScannedNfcId_.Key != SmartObjectType.Option3)
                return;
        }

        lastScannedNfcId_ = new KeyValuePair<SmartObjectType, string>(stationType, nfcId);

        dispatch_.NewNfcScan(nfcId, stationType);

        int optionSelected = -1;
        switch(stationType)
        {
            case SmartObjectType.Option1:
                optionSelected = 1;
                break;
            case SmartObjectType.Option2:
                optionSelected = 2;
                break;
            case SmartObjectType.Option3:
                optionSelected = 3;
                break;
        }

        if (optionSelected != -1)
        {
            shareManager_.NewOptionSelected(optionSelected, string.Equals(nfcId, kTeacherNfcId), nfcId);
            AudioPlayer.PlaySfx("beep");
        }
    }

    public override void SendNewActionInternal(string a)
    {
        base.SendNewActionInternal(a);

        shareManager_.NewNodeAction(ACTION_PREFIX + a);
    }

    private void LoadStationManagers()
    {
        //debug only - should not need station managers in controller in production
        //if (true) return;

        foreach (StationManager p in stationManagerPrefabs_)
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

    protected override void NewNodeEventInternal(EpisodeNode n)
    {
        base.NewNodeEventInternal(n);

        lastScannedNfcId_ = new KeyValuePair<SmartObjectType, string>();
    }

    protected override void NewActionInternal(string a)
    {
        base.NewActionInternal(a);

        List<string> validatorArgs = ArgumentHelper.ArgumentsFromCommand("-validator", a);
        List<string> hintUsedArgs = ArgumentHelper.ArgumentsFromCommand("-hint-used", a);
        List<string> refreshArgs = ArgumentHelper.ArgumentsFromCommand("-refresh-station", a);
        List<string> claimReward = ArgumentHelper.ArgumentsFromCommand(CLAIM_REWARD, a);

        if (validatorArgs.Count > 1)
        {
            List<string> additionalArgs = new List<string>(validatorArgs.GetRange(2, validatorArgs.Count - 2));
            dispatch_.NewValidatorAction(validatorArgs[0], validatorArgs[1], additionalArgs);
        }

        if (hintUsedArgs.Count > 1)
        {
            dispatch_.OnUsedHint(hintUsedArgs[0], hintUsedArgs[1]);
        }

        if (refreshArgs.Count > 0)
        {
            dispatch_.OnRefresh(refreshArgs[0], refreshArgs[1]);
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
        testNfcId_ = Random.Range(0, 10000).ToString();
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
                SendNewActionNetworked(NODE_COMMAND + " back");
                break;
            case "next":
                SendNewActionNetworked(NODE_COMMAND + " next");
                break;
            case "select-1":
                NewNfcScan(testNfcId_.ToString(), SmartObjectType.Option1);
                break;
            case "select-2":
                NewNfcScan(testNfcId_.ToString(), SmartObjectType.Option2);
                break;
            case "select-3":
                NewNfcScan(testNfcId_.ToString(), SmartObjectType.Option3);
                break;
        }
    }
}