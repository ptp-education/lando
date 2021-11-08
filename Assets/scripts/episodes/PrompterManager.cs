using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;
using Photon;

public class EpisodesFileInfo
{
    public string[] episodeNames;

    public EpisodesFileInfo(string[] fileNames)
    {
        this.episodeNames = fileNames;
    }
}

public class PrompterManager : GameManager
{
    [SerializeField] private Dropdown episodesDropdown_;
    [SerializeField] private TextMeshProUGUI teleprompter_;
    [SerializeField] private GameObject buttonsHome_;

    [SerializeField] private PromptButton commandsButtonPrefab_;

    private void Start()
    {
        TextAsset fileNamesAsset = Resources.Load<TextAsset>("all_episodes");
        EpisodesFileInfo efi = JsonUtility.FromJson<EpisodesFileInfo>(fileNamesAsset.text);
        foreach (string fileName in efi.episodeNames)
        {
            Dropdown.OptionData od = new Dropdown.OptionData();
            od.text = fileName;
            episodesDropdown_.options.Add(od);
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            AdjustTeleprompterPosition(100);
        } else if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            AdjustTeleprompterPosition(-100);
        }
    }

    private void AdjustTeleprompterPosition(int position)
    {
        teleprompter_.transform.position = new Vector3(teleprompter_.transform.position.x, teleprompter_.transform.position.y + position, teleprompter_.transform.position.z);
    }

    private void ResetTeleprompterPosition()
    {
        teleprompter_.transform.localPosition = new Vector3(0, -100, teleprompter_.transform.position.z);
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
            b.transform.SetParent(buttonsHome_.transform, true);
            b.Init("Next", GameManager.NODE_PREFIX + currentNode_.NextNode.name, buttonCounter, CommandButtonPressed);

            buttonCounter++;
        } else if (currentNode_.NextNode == null && currentNode_.Options.Count == 0)
        {
            PromptButton b = Instantiate<PromptButton>(commandsButtonPrefab_);
            b.transform.SetParent(buttonsHome_.transform, true);
            b.Init("End of class", "", -1, CommandButtonPressed);
            b.GetComponent<Button>().interactable = false;

            return;
        }
        for(int i = 0; i < currentNode_.Options.Count; i++)
        {
            PromptButton b = Instantiate<PromptButton>(commandsButtonPrefab_);
            b.transform.SetParent(buttonsHome_.transform, true);

            string action = "";
            if (currentNode_.Options[i].Node != null)
            {
                action = GameManager.NODE_PREFIX + currentNode_.Options[i].Node.name;
            } else
            {
                action = GameManager.ACTION_PREFIX + currentNode_.Options[i].Prompt;
            }

            b.Init(currentNode_.Options[i].Prompt, action, buttonCounter, CommandButtonPressed);

            buttonCounter++;
        }
    }

    private void HideButtons()
    {
        foreach(Transform child in buttonsHome_.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    private void DisableButtons()
    {
        foreach(Button b in buttonsHome_.GetComponentsInChildren<Button>())
        {
            b.interactable = false;
        }
    }

    private void EnableButtons()
    {
        foreach (Button b in buttonsHome_.GetComponentsInChildren<Button>())
        {
            b.interactable = true;
        }
    }

    private void CommandButtonPressed(string linkedEpisode)
    {
        UpdateEpisodeNode(linkedEpisode);
    }

    public void OnEpisodeLoadClick()
    {
        UpdateEpisode(episodesDropdown_.options[episodesDropdown_.value].text);
    }

    protected override void NewEpisodeEventInternal(Episode e)
    {
        base.NewEpisodeEventInternal(e);

        teleprompter_.text = "";
        HideButtons();
    }

    protected override void NewStateEventInternal(string s)
    {
        base.NewStateEventInternal(s);

        if (string.Equals(s, NodeState.Playing))
        {
            teleprompter_.text = currentNode_.Prompt;
            ResetTeleprompterPosition();
        } else if (string.Equals(s, NodeState.Looping)) {
            EnableButtons();
        }
    }

    protected override void NewNodeEventInternal(EpisodeNode n)
    {
        base.NewNodeEventInternal(n);

        SpawnButtons();
        DisableButtons();
    }
}