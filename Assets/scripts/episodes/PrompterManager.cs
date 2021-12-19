using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    [SerializeField] private TextMeshProUGUI teleprompter_;
    [SerializeField] private GameObject taPanel_;

    [SerializeField] private PromptButton commandsButtonPrefab_;

    private bool endOfClass_ = false;
    private List<string> previousNodes_ = new List<string>();
    private List<string> episodePaths = new List<string>();

    private void Start()
    {
        GameManager.PromptActive = true;

#if !UNITY_EDITOR
        GameManager.MuteAll = true;
#endif

        if (GameManager.MuteAll)
        {
            AudioListener.volume = 0;
        }

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

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            AdjustPanelPosition(100);
        } else if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            AdjustPanelPosition(-100);
        }
    }

    private void AdjustPanelPosition(int position)
    {
        taPanel_.transform.position = new Vector3(taPanel_.transform.position.x, taPanel_.transform.position.y + position, taPanel_.transform.position.z);
    }

    private void ResetTaPanelPosition()
    {
        taPanel_.transform.localPosition = new Vector3(0, -100, taPanel_.transform.position.z);
    }

    private void SpawnButtons()
    {
        if (currentNode_ == null)
        {
            return;
        }

        HideButtons();

        int buttonCounter = 1;
        if (currentNode_.NextNode != null)
        {
            PromptButton b = Instantiate<PromptButton>(commandsButtonPrefab_);
            b.transform.SetParent(taPanel_.transform, true);
            b.Init("Next", GameManager.NODE_PREFIX + currentNode_.NextNode.name, "n", CommandButtonPressed);
        } else if (currentNode_.NextNode == null && currentNode_.Options.Count == 0)
        {
            PromptButton b = Instantiate<PromptButton>(commandsButtonPrefab_);
            b.transform.SetParent(taPanel_.transform, true);
            b.Init("End of class", "", "1", CommandButtonPressed);
            b.GetComponent<Button>().interactable = false;

            endOfClass_ = true;

            return;
        }
        for(int i = 0; i < currentNode_.Options.Count; i++)
        {
            PromptButton b = Instantiate<PromptButton>(commandsButtonPrefab_);
            b.transform.SetParent(taPanel_.transform, true);

            string action = "";
            if (currentNode_.Options[i].Node != null)
            {
                action = GameManager.NODE_PREFIX + currentNode_.Options[i].Node.name;
            } else
            {
                action = GameManager.ACTION_PREFIX + currentNode_.Options[i].Action;
            }

            string name = currentNode_.Options[i].Name;
            if (name == null || name.Length == 0)
            {
                name = currentNode_.Options[i].Action;
            }

            b.Init(name, action, buttonCounter.ToString(), CommandButtonPressed);

            buttonCounter++;
        }
        if (previousNodes_.Count >= 2)
        {
            PromptButton b = Instantiate<PromptButton>(commandsButtonPrefab_);
            b.transform.SetParent(taPanel_.transform, true);
            b.Init("Undo", "", "u", UndoButtonPressed);
        }
    }

    private void HideButtons()
    {
        foreach(PromptButton b in taPanel_.GetComponentsInChildren<PromptButton>())
        {
            GameObject.Destroy(b.gameObject);
        }
    }

    private void DisableButtons()
    {
        foreach(PromptButton b in taPanel_.GetComponentsInChildren<PromptButton>())
        {
            b.Interactable = false;
        }
    }

    private void EnableButtons()
    {
        if (endOfClass_) return;

        foreach (PromptButton b in taPanel_.GetComponentsInChildren<PromptButton>())
        {
            b.Interactable = true;
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

    private string FormatText(string text)
    {
        string r = text.Replace("TA", "<b><color=\"yellow\">TA</color></b>");
        r = r.Replace("[", "<i>");
        r = r.Replace("]", "</i>");
        return r;
    }

    protected override void NewEpisodeEventInternal(Episode e)
    {
        base.NewEpisodeEventInternal(e);

        endOfClass_ = false;
        teleprompter_.text = "";
        previousNodes_ = new List<string>();
        HideButtons();
    }

    protected override void NewStateEventInternal(string s)
    {
        base.NewStateEventInternal(s);

        if (string.Equals(s, NodeState.Playing))
        {
            teleprompter_.text = FormatText(currentNode_.Prompt);
            ResetTaPanelPosition();
        } else if (string.Equals(s, NodeState.Looping)) {
            EnableButtons();
        }
    }

    protected override void NewNodeEventInternal(EpisodeNode n)
    {
        base.NewNodeEventInternal(n);

        string currentAction = GameManager.NODE_PREFIX + currentNode_.name;
        if (previousNodes_.Count == 0 || !string.Equals(previousNodes_[previousNodes_.Count - 1], currentAction))
        {
            previousNodes_.Add(GameManager.NODE_PREFIX + currentNode_.name);
        }

        SpawnButtons();
        //DisableButtons();
    }
}