using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Lando.Networking
{
	public class LanNetworkDiscovery : MonoBehaviour
	{
		private const string DISCOVERY_BROADCAST_MESSAGE = "lando_client";
		private readonly object _lock = new object();
		private readonly int[] m_possiblePorts = new int[] { 3000, 5000, 8000, 8080 };
		private int m_checkPortIndex = 0;
		private List<IPAddress> m_localIpAddresses = new List<IPAddress>();
		private CancellationTokenSource m_cts = new CancellationTokenSource();
		[SerializeField]
		private Text m_discoveryStatusText = default;

		public string ServerAddress { get { return m_serverAddress; } }
		private string m_serverAddress = null;
		public event UnityAction OnNetworkDiscovered = default;

		/// <summary>
		/// Starts the discovery process
		/// </summary>
		private void Awake()
		{
			SearchForServer();
		}

		/// <summary>
		/// Aquires a mutual-exclusion lock, which allows to safely check our ServerAddress variable to see if it has been set or not
		/// </summary>
		private void LateUpdate()
		{
			lock (_lock)
			{
				if (OnNetworkDiscovered != null && !string.IsNullOrEmpty(ServerAddress))
				{
					SetServerAddress(ServerAddress);
				}
			}
		}

		/// <summary>
		/// Sets the discovered address of the Photon server, updates text and triggers an event.
		/// It also triggers all running threads to cancel, due to the <see cref="CancellationToken.ca"/>
		/// </summary>
		/// <param name="address"></param>
		private void SetServerAddress(string address)
		{
			m_serverAddress = address;
			m_discoveryStatusText.text = $"Server: {address}";
			m_cts.Cancel();
			OnNetworkDiscovered?.Invoke();
			OnNetworkDiscovered = null; 
		}

		/// <summary>
		/// Bind each Network Interface to an <see cref="UdpClient"/>, configure the socket for Broadcasting, and FOR EACH bound interface
		/// Send the discovery message and listen for a response on that same sock (<see cref="UdpClient"/>).
		/// This triggers multi-threaded code, once a response is received (and it contains the expected format <see cref="ReceiveResponse(UdpClient)"/>) 
		/// Or a response fails due to some other network error, OR the app is closed, all threads will be terminated.
		/// </summary>
		private async void SearchForServer()
		{
			List<Task> tasks = new List<Task>();
			foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
			{
				if (ni.OperationalStatus == OperationalStatus.Up && ni.SupportsMulticast && ni.GetIPProperties().GetIPv4Properties() != null)
				{
					int id = ni.GetIPProperties().GetIPv4Properties().Index;
					if (NetworkInterface.LoopbackInterfaceIndex != id)
					{
						foreach (UnicastIPAddressInformation uip in ni.GetIPProperties().UnicastAddresses)
						{
							if (uip.Address.AddressFamily == AddressFamily.InterNetwork)
							{
								IPEndPoint local = new IPEndPoint(uip.Address, 0);
								UdpClient udpc = new UdpClient(local);
								udpc.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
								udpc.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontRoute, 1);

								IPEndPoint target = new IPEndPoint(IPAddress.Broadcast, 8888);

								tasks.Add(Task.Run(() => SendDiscoveryBroadcast(udpc, target), m_cts.Token));
								tasks.Add(Task.Run(() => ReceiveResponse(udpc), m_cts.Token));
							}
						}
					}
				}
			}

			await Task.WhenAll(tasks);

			if (string.IsNullOrEmpty(m_serverAddress))
			{
				m_discoveryStatusText.text = "Failed to find discovery server";
			}
			
		}

		/// <summary>
		/// Broadcast the <see cref="DISCOVERY_BROADCAST_MESSAGE"/> to the supplied endpoint
		/// </summary>
		/// <param name="Client"></param>
		/// <param name="endPoint"></param>
		/// <returns></returns>
		private async Task SendDiscoveryBroadcast(UdpClient Client, IPEndPoint endPoint)
		{
			var RequestData = Encoding.ASCII.GetBytes(DISCOVERY_BROADCAST_MESSAGE);
			Client.EnableBroadcast = true;
			while (!m_cts.IsCancellationRequested)
			{
				try
				{
					Client.Send(RequestData, RequestData.Length, endPoint);
					await Task.Delay(2000);
				}
				catch (Exception e)
				{
					Debug.Log(e.Message);
					break;
				}

			}
		}

		/// <summary>
		/// Runs a blocking thread to receive data from a specified <see cref="UdpClient"/>.
		/// If a valid message is received, the <see cref="m_cts"/> is cancelled and ALL other running threads terminate.
		/// If a message is received which is *not* valid, it tries again (this may be the case if the discovery server was down)
		/// </summary>
		/// <param name="Client"></param>
		/// <returns></returns>
		private Task ReceiveResponse(UdpClient Client)
		{
			TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
			void DataReceived(IAsyncResult ar)
			{
				if (m_cts.IsCancellationRequested)
				{
					return;
				}
				UdpClient c = (UdpClient)ar.AsyncState;
				IPEndPoint receivedIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
				Byte[] receivedBytes = c.EndReceive(ar, ref receivedIpEndPoint);
				var ServerResponse = Encoding.ASCII.GetString(receivedBytes);
				dynamic serverMessage = JsonConvert.DeserializeObject<dynamic>(ServerResponse);

				if (serverMessage.server != null)
				{
					c.Close();
					m_cts.Cancel();
					m_serverAddress = serverMessage.server;
					tcs.SetResult(true);
				}
				else
				{
					c.BeginReceive(DataReceived, ar.AsyncState);
				}
			}

			Client.BeginReceive(DataReceived, Client);

			return tcs.Task;
		}

		private void OnDestroy()
		{
			m_cts.Cancel();
		}
	}

}