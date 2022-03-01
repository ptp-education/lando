using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class StringsFile
{
    public string[] FileNames;

    public StringsFile(string[] fileNames)
    {
        this.FileNames = fileNames;
    }
}

public class PrompterManager : GameManager
{
    [SerializeField] private Dropdown episodesDropdown_;
    [SerializeField] private Dropdown characterDropdown_;
    [SerializeField] private TextMeshProUGUI teleprompter_;
    [SerializeField] private GameObject buttonsPanel_;
    [SerializeField] private GameObject prompterPanel_;

    [SerializeField] private Image muteAllBg_;
    [SerializeField] private Image masterBg_;
    [SerializeField] private Image zoneBg_;

    [SerializeField] private PromptButton commandsButtonPrefab_;
    [SerializeField] private PromptButtonHolder commandButtonsHolderPrefab_;

    private List<string> previousNodes_ = new List<string>();
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

        if (characterDropdown_ != null)
        {
            foreach(string c in System.Enum.GetNames(typeof(EpisodeNode.Character.Option)))
            {
                Dropdown.OptionData od = new Dropdown.OptionData();
                od.text = c;
                characterDropdown_.options.Add(od);
            }
        }

        episodePaths = new List<string>(sf.FileNames);

        RefreshMuteMode();
        RefreshMasterMode();
        OnCharacterChange();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            AdjustPanelPosition(100);
        } else if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            AdjustPanelPosition(-100);
        }
        if (Input.GetKeyUp("z"))
        {
            SendNewAction(TOGGLE_LIGHT);
        }
        if (Input.GetKeyUp("h"))
        {
            SendNewAction(HIDE_ALL);
        }
        if (Input.GetKeyUp("s"))
        {
            SendNewAction(SILENCE_COUNTER);
        }
        if (Input.GetKeyUp("0"))
        {
            SendNewAction(RESET_SILENCE_COUNTER);
        }
    }

    private void AdjustPanelPosition(int position)
    {
        prompterPanel_.transform.position = new Vector3(prompterPanel_.transform.position.x, prompterPanel_.transform.position.y + position, prompterPanel_.transform.position.z);
    }

    private void ResetTaPanelPosition()
    {
        prompterPanel_.transform.localPosition = new Vector3(0, -100, prompterPanel_.transform.position.z);
    }

    private void SpawnButtons()
    {
        if (currentNode_ == null)
        {
            return;
        }

        HideButtons();

        PromptButtonHolder defaultHolder = Instantiate<PromptButtonHolder>(commandButtonsHolderPrefab_);
        defaultHolder.Header.text = "Default";

        if (currentNode_.NextNode != null)
        {
            PromptButton b = Instantiate<PromptButton>(commandsButtonPrefab_);
            b.transform.SetParent(defaultHolder.transform, true);
            b.Init("Next", GameManager.NODE_PREFIX + currentNode_.NextNode.name, "n", CommandButtonPressed);
        } else if (currentNode_.NextNode == null && currentNode_.OptionHolders.Count == 0)
        {
            PromptButton b = Instantiate<PromptButton>(commandsButtonPrefab_);
            b.transform.SetParent(defaultHolder.transform, true);
            b.Init("End of class", "", "-", CommandButtonPressed);
            b.Interactable = false;

            return;
        }
        foreach(EpisodeNode.OptionHolder holder in currentNode_.OptionHolders)
        {
            PromptButtonHolder h = Instantiate<PromptButtonHolder>(commandButtonsHolderPrefab_);
            h.transform.SetParent(buttonsPanel_.transform, true);

            h.Header.text = holder.Name;

            for (int i = 0; i < holder.Options.Count; i++)
            {
                PromptButton b = Instantiate<PromptButton>(commandsButtonPrefab_);
                b.transform.SetParent(h.transform, true);

                string action = "";
                if (holder.Options[i].Node != null)
                {
                    action = GameManager.NODE_PREFIX + holder.Options[i].Node.name;
                } else
                {
                    action = GameManager.ACTION_PREFIX + holder.Options[i].Action;
                }

                string name = holder.Options[i].Name;
                if (name == null || name.Length == 0)
                {
                    name = holder.Options[i].Action;
                }

                b.Init(name, action, "", CommandButtonPressed);
            }
        }

        if (previousNodes_.Count >= 2)
        {
            PromptButton b = Instantiate<PromptButton>(commandsButtonPrefab_);
            b.transform.SetParent(defaultHolder.transform, true);
            b.Init("Undo", "", "u", UndoButtonPressed);
        }

        defaultHolder.transform.SetParent(buttonsPanel_.transform, true);
    }

    private void HideButtons()
    {
        foreach(PromptButton b in buttonsPanel_.GetComponentsInChildren<PromptButton>())
        {
            GameObject.Destroy(b.gameObject);
        }
        foreach (PromptButton b in prompterPanel_.GetComponentsInChildren<PromptButton>())
        {
            GameObject.Destroy(b.gameObject);
        }
        foreach(PromptButtonHolder h in buttonsPanel_.GetComponentsInChildren<PromptButtonHolder>())
        {
            GameObject.Destroy(h.gameObject);
        }
    }

    private void CommandButtonPressed(string linkedEpisode)
    {
        UpdateEpisodeNode(linkedEpisode);
    }

    private void UndoButtonPressed(string notNeeded)
    {
        previousNodes_.RemoveAt(previousNodes_.Count - 1);
        UpdateEpisodeNode(previousNodes_[previousNodes_.Count - 1]);
    }

    public void OnEpisodeLoadClick()
    {
        UpdateEpisode(episodePaths[episodesDropdown_.value]);
    }

    public void OnCharacterChange()
    {
        //GameManager.SelectedCharacter = System.Enum.GetNames(typeof(EpisodeNode.Character.Option))[characterDropdown_.value];
    }

    public void OnMuteAllClick()
    {
        GameManager.MuteAll = !GameManager.MuteAll;
        RefreshMuteMode();
    }

    public void OnMasterClick()
    {
        GameManager.Master = !GameManager.Master;
        RefreshMasterMode();
    }

    private void RefreshMuteMode()
    {
        if (muteAllBg_ != null)
        {
            muteAllBg_.color = !GameManager.MuteAll ? Color.green : Color.red;
        }

        if (GameManager.MuteAll)
        {
            AudioListener.volume = 0;
        }
    }

    private void RefreshMasterMode()
    {
        if (masterBg_ != null)
        {
            masterBg_.color = GameManager.Master ? Color.green : Color.red;
        }
    }

    private string FormatText(string text)
    {
        string r = text.Replace("TA", "<b><color=\"yellow\">TA</color></b>");
        r = r.Replace("[", "<i>");
        r = r.Replace("]", "</i>");

        while (r.EndsWith("\n"))
        {
            r = r.Remove(r.Length - 1);
        }

        return r;
    }

    protected override void NewEpisodeEventInternal(Episode e)
    {
        base.NewEpisodeEventInternal(e);

        teleprompter_.text = "";
        previousNodes_ = new List<string>();
        HideButtons();
    }

    protected override void NewNodeEventInternal(EpisodeNode n)
    {
        base.NewNodeEventInternal(n);

        teleprompter_.text = FormatText(currentNode_.Prompt);
        ResetTaPanelPosition();

        string currentAction = GameManager.NODE_PREFIX + currentNode_.name;
        if (previousNodes_.Count == 0 || !string.Equals(previousNodes_[previousNodes_.Count - 1], currentAction))
        {
            previousNodes_.Add(GameManager.NODE_PREFIX + currentNode_.name);
        }

        SpawnButtons();
    }
}