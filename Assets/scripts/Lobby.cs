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
    [SerializeField] InputField inputField_;
    [SerializeField] ButtonToggle buttonToggle_;

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        status_.text = "Connecting";
        inputField_.interactable = false;
        UpdatePlaceholderText("Waiting for server");
    }

    private void Update()
    {
        if (inputField_.text.Length > 0 && Input.GetKeyUp(KeyCode.Return))
        {
            EnterPressed(inputField_.text);
        }
    }

    private void EnterPressed(string roomName)
    {
        PhotonNetwork.NickName = buttonToggle_.Selected + "-" + System.Guid.NewGuid().ToString();

        RoomOptions roomOptions = new RoomOptions
        {
            PublishUserId = true,
            CleanupCacheOnLeave = false
        };

        TypedLobby typedLobby = new TypedLobby(null, LobbyType.Default);

        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, typedLobby);
        status_.text = "Joining room...";
    }

    private void UpdatePlaceholderText(string s)
    {
        Text placeholderText = inputField_.placeholder.GetComponent<Text>();
        if (placeholderText)
        {
            placeholderText.text = s;
        }
    }

    #region callbacks

    public override void OnConnectedToMaster()
    {
        status_.text = "Connected to server!";
        UpdatePlaceholderText("Enter code");
        inputField_.interactable = true;

        Debug.Log(string.Format("Connected to server. Region: {0}, AppVersion: {1}", PhotonNetwork.CloudRegion, PhotonNetwork.AppVersion));

        if (Application.isEditor)
        {
            EnterPressed("editor-test");
        } else
        {
            EnterPressed("prod");
        }
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        status_.text = "Successfully joined room!";

        inputField_.interactable = false;

        UnityEngine.SceneManagement.SceneManager.LoadScene(buttonToggle_.Selected);

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
