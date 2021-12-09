using Lando.Logger;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Lando.Networking
{
	public class NetworkingMediator : MonoBehaviour
	{
		public enum eCurrentNetworkingBackend
		{
			NotSet,
			CloudHosted,
			SelfHosted
		}

		public enum eCurrentConnectionStatus
		{
			None,
			Connecting,
			Disconnecting,
			Connected
		}
	
		#region PRIVATE_API
		private static NetworkingMediator s_instance = default;

		private const int HEART_BEAT_DELAY = 5;
		private AppSettings m_localServerSettings = default;
		private eCurrentConnectionStatus m_currentConnectionStatus = eCurrentConnectionStatus.None;
		private eCurrentNetworkingBackend m_currentNetworkingBackend = eCurrentNetworkingBackend.NotSet;
		[SerializeField]
		private Toggle m_toggleManualBackend = default;
		#endregion

		#region PUBLIC_API
		public event UnityAction<eCurrentNetworkingBackend> OnNetworkingBackendChanging = default;
		public event UnityAction<eCurrentConnectionStatus> OnNetworkingBackendStatusChanged = default;
		public eCurrentConnectionStatus CurrentConnectionStatus
		{
			private set
			{
				if (m_currentConnectionStatus != value)
				{
					m_currentConnectionStatus = value;
					OnNetworkingBackendStatusChanged?.Invoke(m_currentConnectionStatus);
				}
			}
			get
			{
				return m_currentConnectionStatus;
			}
		}

		public eCurrentNetworkingBackend CurrentNetworkingBackend
		{
			private set
			{
				if (m_currentNetworkingBackend != value)
				{
					m_currentNetworkingBackend = value;
					OnNetworkingBackendChanging?.Invoke(m_currentNetworkingBackend);
				}
			}
			get
			{
				return m_currentNetworkingBackend;
			}
		}

		public static NetworkingMediator Instance
		{
			get
			{
				return s_instance;
			}
			private set
			{
				s_instance = value;
			}
		}
		#endregion

		private eCurrentNetworkingBackend m_backend = eCurrentNetworkingBackend.NotSet;

		public void Awake()
		{
			if (Instance != null)
			{
				Destroy(gameObject);
			}
			Instance = this;
			m_localServerSettings = new AppSettings
			{
				Server = "ws://192.168.0.18",
				Port = 9090,
				Protocol = ExitGames.Client.Photon.ConnectionProtocol.WebSocket,
				UseNameServer = false,
				AuthMode = AuthModeOption.Auth,
				FixedRegion = null,
				AppVersion = "local"
			};

			SwitchConnection(m_toggleManualBackend.isOn);
			m_toggleManualBackend.onValueChanged.AddListener(SwitchConnection);
			StartCoroutine(HeartBeat());

			DontDestroyOnLoad(gameObject);
		}

		public void SwitchConnection(bool isOn)
		{
			m_backend = m_toggleManualBackend.isOn ? eCurrentNetworkingBackend.CloudHosted : eCurrentNetworkingBackend.SelfHosted;
			m_toggleManualBackend.GetComponentInChildren<Text>().text = m_backend == eCurrentNetworkingBackend.SelfHosted ? "LAN" : "WAN";
			CurrentConnectionStatus = eCurrentConnectionStatus.Disconnecting;
		}


		private IEnumerator HeartBeat()
		{
			CurrentConnectionStatus = eCurrentConnectionStatus.Connecting;
			while (true)
			{
				yield return new WaitForSeconds(HEART_BEAT_DELAY);

				eCurrentNetworkingBackend previousBackend = m_currentNetworkingBackend;

				CurrentNetworkingBackend = m_backend;

				if (previousBackend != m_currentNetworkingBackend)
				{
					// Connected back to Cloud service..let the app know
					Log($"Networking backend changed to {m_currentNetworkingBackend}");
					// Let clients know that the backend will be changing, so any current state (rooms, lobby names, etc.) can be saved
					OnNetworkingBackendChanging?.Invoke(m_currentNetworkingBackend);
					CurrentConnectionStatus = eCurrentConnectionStatus.Disconnecting;
					yield return null;

					if (PhotonNetwork.IsConnected)
					{
						PhotonNetwork.Disconnect();
						while (PhotonNetwork.NetworkClientState != ClientState.Disconnected)
						{
							yield return null;
						}
					}

					CurrentConnectionStatus = eCurrentConnectionStatus.Connecting;

					switch (m_currentNetworkingBackend)
					{
						case eCurrentNetworkingBackend.CloudHosted:
							if (!PhotonNetwork.ConnectUsingSettings())
							{
								CurrentNetworkingBackend = eCurrentNetworkingBackend.SelfHosted;
								goto case eCurrentNetworkingBackend.SelfHosted;
							} else
							{
								PhotonNetwork.NetworkingClient.AppVersion = PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion;
								CurrentConnectionStatus = eCurrentConnectionStatus.Connected;
							}
							break;
						case eCurrentNetworkingBackend.SelfHosted:
							if (!PhotonNetwork.ConnectToMaster(m_localServerSettings.Server, m_localServerSettings.Port, m_localServerSettings.AppIdRealtime))
							{
								LogAssert($"Cloud hosting was not accessible and we failed to connect to the local server");
							} else
							{
								PhotonNetwork.NetworkingClient.AppVersion = m_localServerSettings.AppVersion;
								CurrentConnectionStatus = eCurrentConnectionStatus.Connected;
							}
							break;
					}
				}
			}
		}

		private void Log(string message)
		{
			NativeLogger.Log($"NetworkingMediator:: {message}");
		}
		private void LogError(string message)
		{
			NativeLogger.Log($"NetworkingMediator Error:: {message}");
		}

		private void LogAssert(string message)
		{
			NativeLogger.Log($"NetworkingMediator Assert:: {message}");
		}
	}
}