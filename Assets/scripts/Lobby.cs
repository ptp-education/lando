using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Realtime;
using Photon.Pun;
using Photon;
using ExitGames.Client.Photon;

public class Lobby : MonoBehaviourPunCallbacks
{
    [SerializeField] Text status_;
    [SerializeField] ButtonToggle buttonToggle_;

    private string selectedButton_;

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        status_.text = "Connecting";

        buttonToggle_.Init(ButtonPressed);
        buttonToggle_.DisableAllButtons();
    }

    private void ButtonPressed(string buttonName)
    {
        selectedButton_ = buttonName;
        PhotonNetwork.NickName = buttonName + "-" + System.Guid.NewGuid().ToString();

        RoomOptions roomOptions = new RoomOptions
        {
            PublishUserId = true,
            CleanupCacheOnLeave = false
        };

        TypedLobby typedLobby = new TypedLobby(null, LobbyType.Default);

        //PhotonNetwork.JoinOrCreateRoom(Application.isEditor ? "editor" : "prod", roomOptions, typedLobby);
        PhotonNetwork.JoinOrCreateRoom("prod", roomOptions, typedLobby);
        status_.text = "Joining room...";
    }

    #region callbacks

    public override void OnConnectedToMaster()
    {
        status_.text = "Connected to server!";

        buttonToggle_.EnableAllButtons();

        Debug.Log(string.Format("Connected to server. Region: {0}, AppVersion: {1}", PhotonNetwork.CloudRegion, PhotonNetwork.AppVersion));
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        status_.text = "Successfully joined room!";

        UnityEngine.SceneManagement.SceneManager.LoadScene(selectedButton_);

        Debug.Log(string.Format("Joined room {0}, there are {1} players", PhotonNetwork.CurrentRoom.Name, PhotonNetwork.CurrentRoom.PlayerCount));
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        status_.text = "Joining room failed with reason: " + message;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        status_.text = "Disconnected from server with reason: " + cause.ToString();
    }

    #endregion
}
