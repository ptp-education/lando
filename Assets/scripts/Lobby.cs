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
    public const string kShareMode = "share";
    public const string kPromptMode= "prompt";

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
            PhotonNetwork.NickName = buttonToggle_.Selected + "-" + System.Guid.NewGuid().ToString();

            RoomOptions roomOptions = new RoomOptions
            {
                PublishUserId = true
            };

            PhotonNetwork.CreateRoom(inputField_.text, roomOptions);
            status_.text = "Creating room...";
        }
    }

    private void UpdatePlaceholderText(string s)
    {
        Text placeholderText = inputField_.placeholder.GetComponent<Text>();
        if (placeholderText)
        {
            placeholderText.text = s;
        }
    }

    public void OnClick()
    {
        Episode o = Resources.Load<Episode>("prefabs/episodes/" + "test_episode");
        Instantiate<Episode>(o);
    }

    #region callbacks

    public override void OnConnectedToMaster()
    {
        status_.text = "Connected to server!";
        UpdatePlaceholderText("Enter code");
        inputField_.interactable = true;
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        status_.text = "Successfully joined room!";

        inputField_.interactable = false;

        if (string.Equals(buttonToggle_.Selected, kShareMode))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(kShareMode);
        } else if (string.Equals(buttonToggle_.Selected, kPromptMode))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(kPromptMode);
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        status_.text = "Creating room failed. Trying to join room instead.";

        PhotonNetwork.JoinRoom(inputField_.text);
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
