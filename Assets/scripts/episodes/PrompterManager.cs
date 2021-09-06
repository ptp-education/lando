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

    private void SpawnButtons()
    {
        int buttonCounter = 1;
        if (currentNode_.NextNode != null)
        {
            PromptButton b = Instantiate<PromptButton>(commandsButtonPrefab_);
            b.transform.SetParent(buttonsHome_.transform, true);
            b.Init("Next", currentNode_.NextNode.name, buttonCounter, CommandButtonPressed);

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
            b.Init(currentNode_.Options[i].Prompt, currentNode_.Options[i].Node.name, buttonCounter, CommandButtonPressed);

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

    private void CommandButtonPressed(string linkedEpisode)
    {
        if (currentNodeState_.Equals(NodeState.Looping))
        {
            UpdateEpisodeNode(linkedEpisode);
            HideButtons();
        } else
        {
            Debug.LogWarning("Command button was pressed while video is not ready for command...somehow");
        }
    }

    public void OnEpisodeLoadClick()
    {
        UpdateEpisode(episodesDropdown_.options[episodesDropdown_.value].text);
    }

    public override void NewEpisodeEvent(string e)
    {
        base.NewEpisodeEvent(e);

        teleprompter_.text = "";
        HideButtons();
    }

    public override void NewStateEvent(string s)
    {
        base.NewStateEvent(s);

        if (s.Equals(NodeState.Looping))
        {
            SpawnButtons();
        } else if (s.Equals(NodeState.Playing))
        {
            teleprompter_.text = currentNode_.Prompt;
            HideButtons();
        }
    }

}
