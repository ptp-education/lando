#if UNITY_WEBGL
using System.Runtime.InteropServices;
#endif
using Lando.Logger;
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
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Lando.Networking
{
	public class LanNetworkDiscovery : MonoBehaviour
	{
		private readonly object _lock = new object();
		private UdpClient m_udpClient = default;
		private readonly int[] m_possiblePorts = new int[] { 3000, 5000, 8000, 8080 };
		private int m_checkPortIndex = 0;
		private List<IPAddress> m_localIpAddresses = new List<IPAddress>();
		private CancellationTokenSource m_cts = new CancellationTokenSource();
		[SerializeField]
		private Text m_discoveryStatusText = default;

		public string ServerAddress { get { return m_serverAddress; } }
		private string m_serverAddress = null;
		public event UnityAction OnNetworkDiscovered = default;

		private void Awake()
		{
			m_udpClient = new UdpClient();
			SearchForServer();
		}

		private void LateUpdate()
		{
			lock (_lock)
			{
				if (OnNetworkDiscovered != null && !string.IsNullOrEmpty(ServerAddress))
				{
					Debug.Log("Found");
					SetServerAddress(ServerAddress);
				}
			}
		}

		private void SetServerAddress(string address)
		{
			m_serverAddress = address;
			m_discoveryStatusText.text = $"Server: {address}";
			m_discoveryStatusText.Rebuild(CanvasUpdate.PostLayout);
			OnNetworkDiscovered?.Invoke();
			OnNetworkDiscovered = null; 
		}

		private async void SearchForServer()
		{
			m_discoveryStatusText.text = "Searching for local photon server";
			Debug.Log("Search for server");
			Task send = SendDiscoveryBroadcast(m_udpClient);
			Task receive = Task.Run(() => ReceiveResponse(m_udpClient), m_cts.Token);
			await Task.WhenAll(send, receive);
		}

		private async Task SendDiscoveryBroadcast(UdpClient Client)
		{
			var RequestData = Encoding.ASCII.GetBytes("lando_client");
			Client.EnableBroadcast = true;
			while (!m_cts.IsCancellationRequested)
			{
				try
				{
					Client.Send(RequestData, RequestData.Length, new IPEndPoint(IPAddress.Broadcast, 8888));
					await Task.Delay(2000);
				}
				catch(Exception e)
				{
					Debug.Log(e.Message);
				}
				
			}
		}

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