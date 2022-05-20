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
    public const string kEpisodeKey = "episode";
    public const string kEpisodeNodeKey = "node";
    public const string kNodeStateKey = "node-state";

    public const byte kNewEpisodeCode = 1;
    public const byte kNewEpisodeNodeCode = 2;
    public const byte kNewEpisodeNodeStateCode= 3;

    private List<GameManager> gameManagers_ = new List<GameManager>();

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameManager gm in FindObjectsOfType<GameManager>())
        {
            AddNewGameManager(gm);
        }
    }

    public void AddNewGameManager(GameManager gm)
    {
        gm.Init(this);
        gameManagers_.Add(gm);
    }

    public void SendNewEpisodeMessage(string e)
    {
        object[] content = new object[] { e };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(kNewEpisodeCode, content, raiseEventOptions, SendOptions.SendReliable);

        UpdateRoomState(episode: e, node: "");
    }

    public void SendNewEpisodeNodeMessage(string n)
    {
        object[] content = new object[] { n };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(kNewEpisodeNodeCode, content, raiseEventOptions, SendOptions.SendReliable);

        UpdateRoomState(node: n);
    }

    private void UpdateRoomState(string episode = null, string node = null)
    {
        ExitGames.Client.Photon.Hashtable h = PhotonNetwork.CurrentRoom.CustomProperties;
        if (episode != null)
        {
            h[kEpisodeKey] = episode;
        }
        if (node != null)
        {
            h[kEpisodeNodeKey] = node;
        }

        PhotonNetwork.CurrentRoom.SetCustomProperties(h);
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        object[] data = (object[])photonEvent.CustomData;

        if (photonEvent.CustomData != null && eventCode < 200)
        {
            string eventDescriptor = "";
            switch (eventCode)
            {
                case kNewEpisodeCode:
                    eventDescriptor = "new_episode";
                    break;
                case kNewEpisodeNodeCode:
                    eventDescriptor = "new_node";
                    break;
                case kNewEpisodeNodeStateCode:
                    eventDescriptor = "new_state";
                    break;
            }
            Debug.Log(string.Format("Received event: {0} {1}", eventDescriptor, data[0].ToString()));
        }

        if (eventCode == kNewEpisodeCode)
        {
            string episode = (string)data[0];
            foreach(GameManager gm in gameManagers_)
            {
                gm.NewEpisodeEvent(episode);
            }

        } else if (eventCode == kNewEpisodeNodeCode)
        {
            string node = (string)data[0];
            foreach (GameManager gm in gameManagers_)
            {
                gm.NewNodeAction(node);
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.Log(string.Format("Player entered room. Current player count is {0}", PhotonNetwork.CurrentRoom.PlayerCount));
    }

    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.Log(string.Format("Player left room. Current player count is {0}", PhotonNetwork.CurrentRoom.PlayerCount));
    }
}