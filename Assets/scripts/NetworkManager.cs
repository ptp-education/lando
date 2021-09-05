using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Realtime;
using Photon.Pun;
using Photon;
using ExitGames.Client.Photon;

public class NetworkManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    [SerializeField] GameObject statusObject_;
    [SerializeField] Text statusText_;
    [SerializeField] GameManager gameManager_;
    [SerializeField] GameManager.Type type_;


    public const byte kNewEpisodeCode = 1;
    public const byte kNewEpisodeNodeCode = 2;
    public const byte kPrompterActionCode = 3;  //only changed by prompters, 
    public const byte kNewEpisodeNodeStateCode= 4;    //only changed by sharers

    // Start is called before the first frame update
    void Start()
    {
        CheckForPrompters();
    }

    private int CheckForPrompters()
    {
        string checkFor = type_ == GameManager.Type.Prompter ? Lobby.kShareMode : Lobby.kPromptMode;

        int count = 0;
        foreach (KeyValuePair<int, Player> kvp in PhotonNetwork.CurrentRoom.Players)
        {
            if (kvp.Value.NickName.Contains(Lobby.kPromptMode))
            {
                count++;
            }
        }

        if (count == 0)
        {
            SetStatus(string.Format("You need to connect to the same room, with at least one additional client in \"{0}\" Mode", checkFor));
        }
        else
        {
            if (type_ == GameManager.Type.Sharer)
            {
                SetStatus("Prompt Mode Connected!\n\nSelect an episode to play in the \"Prompt\" Mode client!");
            } else
            {
                SetStatus(null);
            }
        }
        return count;
    }

    private void SetStatus(string s)
    {
        if (s == null || s.Length == 0)
        {
            statusObject_.SetActive(false);
            return;
        }

        statusObject_.SetActive(true);
        statusText_.text = s;
    }

    public override void OnPlayerEnteredRoom(Player other)
    {
        CheckForPrompters();
    }

    public override void OnPlayerLeftRoom(Player other)
    {
        CheckForPrompters();
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        object[] data = (object[])photonEvent.CustomData;

        if (eventCode == kNewEpisodeCode)
        {
            string episode = (string)data[0];
            gameManager_.LoadEpisode(episode);

            SetStatus(null);
        } else if (eventCode == kPrompterActionCode)
        {
            string action = (string)data[0];
            gameManager_.ReceiveAction(action);
        }
    }
}