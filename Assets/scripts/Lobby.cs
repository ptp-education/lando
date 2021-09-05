using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;

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
            PhotonNetwork.CreateRoom(inputField_.text);
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

        if (string.Equals(buttonToggle_.Selected, "share"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("share");
        } else if (string.Equals(buttonToggle_.Selected, "prompt"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("prompt");
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
