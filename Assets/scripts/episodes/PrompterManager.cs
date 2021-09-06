using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private void Start()
    {
        TextAsset fileNamesAsset = Resources.Load<TextAsset>("all_episodes");
        EpisodesFileInfo efi = JsonUtility.FromJson<EpisodesFileInfo>(fileNamesAsset.text);
        //Use data?
        foreach (string fileName in efi.episodeNames)
        {
            Dropdown.OptionData od = new Dropdown.OptionData();
            od.text = fileName;
            episodesDropdown_.options.Add(od);
        }
    }

    public void OnEpisodeLoadClick()
    {


        string selected = episodesDropdown_.options[episodesDropdown_.value].text;
        object[] content = new object[] { selected };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions {
            Receivers = ReceiverGroup.All,
            CachingOption = EventCaching.AddToRoomCacheGlobal
        };

        PhotonNetwork.RaiseEvent(NetworkManager.kNewEpisodeCode, content, raiseEventOptions, SendOptions.SendReliable);
    }
}
