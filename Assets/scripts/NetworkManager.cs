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
    public const byte kPrompterActionCode = 3;
    public const byte kNewEpisodeNodeStateCode= 4;
    public const byte kNewPlayerCode= 5;

    // Start is called before the first frame update
    void Start()
    {
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