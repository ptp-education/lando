using UnityEngine;
using UnityEngine.UI;

using Photon.Realtime;
using Photon.Pun;
using Lando.Networking;

public class Lobby : MonoBehaviourPunCallbacks
{
    public const string kShareMode = "share";
    public const string kPromptMode= "prompt";

    [SerializeField] Text status_;
    [SerializeField] InputField inputField_;
    [SerializeField] ButtonToggle buttonToggle_;

    private void Start()
    {
		NetworkingMediator.Instance.OnNetworkingBackendStatusChanged += OnNetworkingBackendStatusChanged;
		OnNetworkingBackendStatusChanged(NetworkingMediator.Instance.CurrentConnectionStatus);
	}
	private void OnDestroy()
	{
		NetworkingMediator.Instance.OnNetworkingBackendStatusChanged -= OnNetworkingBackendStatusChanged;
	}

	private void OnNetworkingBackendStatusChanged(NetworkingMediator.eCurrentConnectionStatus status)
	{
		switch (status)
		{
			case NetworkingMediator.eCurrentConnectionStatus.None:
				UpdatePlaceholderText("Nothing happening");
				break;
			case NetworkingMediator.eCurrentConnectionStatus.Connecting:
			case NetworkingMediator.eCurrentConnectionStatus.Disconnecting:
				inputField_.interactable = false;
				UpdatePlaceholderText("Waiting for server");
				status_.text = "Connecting";
				break;
			case NetworkingMediator.eCurrentConnectionStatus.Connected:
				inputField_.interactable = true;
				UpdatePlaceholderText("Enter code");
				break;

		}
	}

	private void Update()
    {
        if (inputField_.text.Length > 0 && Input.GetKeyUp(KeyCode.Return))
        {
            PhotonNetwork.NickName = buttonToggle_.Selected + "-" + System.Guid.NewGuid().ToString();

            RoomOptions roomOptions = new RoomOptions
            {
                PublishUserId = true,
                CleanupCacheOnLeave = false
            };

            TypedLobby typedLobby = new TypedLobby(null, LobbyType.Default);

            PhotonNetwork.JoinOrCreateRoom(inputField_.text, roomOptions, typedLobby);
            status_.text = "Joining room...";
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

        Debug.Log(string.Format("Connected to server. Region: {0}, AppVersion: {1}", PhotonNetwork.CloudRegion, PhotonNetwork.AppVersion));
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
