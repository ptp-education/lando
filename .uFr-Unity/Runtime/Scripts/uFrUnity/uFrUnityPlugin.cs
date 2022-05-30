using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static uFrUnity.uFApi;
using System.Threading;
using System.Collections.Concurrent;
using System.Linq;

namespace uFrUnity
{

	public class uFrUnityPlugin : MonoBehaviour
	{
		[System.Serializable]
		public class ReaderConnection
		{
			public uFReader Reader = default;
			public ConnectionInfo Data = new ConnectionInfo();
			public string ReaderSN = default;

			public string LastReadCardUID = default;

			public bool Connected => Reader != null && Reader.opened;
			public bool IsReady => Data != null && Data.CardUID != null;
			public bool CardConnected = false;

			public bool HaveReadCard => IsReady && LastReadCardUID != null && LastReadCardUID == Data.CardUID;
			public bool isReading = false;

			public void ResetCardReads()
			{
				Data = null;
				LastReadCardUID = null;
			}

			public void Disconnected()
			{
				Reader?.close();
				Reader = null;
			}
		}

		public event System.Action<SuccessfulRead> OnReadData = default;
		public event System.Action<SuccessfulRead> OnScanCard = default;
		private ConcurrentDictionary<int, ReaderConnection> m_activeReaders = new ConcurrentDictionary<int, ReaderConnection>();
		private ConcurrentQueue<string> Errors = new ConcurrentQueue<string>();
		private ConcurrentQueue<string> Info = new ConcurrentQueue<string>();
		private ConcurrentQueue<SuccessfulRead> SuccessulReads = new ConcurrentQueue<SuccessfulRead>();
		private ConcurrentQueue<SuccessfulRead> SuccessulScans = new ConcurrentQueue<SuccessfulRead>();
		public List<ReaderConnection> m_readers = new List<ReaderConnection>();

		private CancellationTokenSource m_cancellationTokenSource = new CancellationTokenSource();



		private void Awake()
		{
			Task.Run(Discover, m_cancellationTokenSource.Token);
		}

		private async void Discover()
		{
			while (!m_cancellationTokenSource.IsCancellationRequested)
			{
				try
				{
					int currentReadersDetected = ReaderCount();

					if (currentReadersDetected != m_activeReaders.Count)
					{
						if (currentReadersDetected > m_activeReaders.Count)
						{
							// A reader was connected
							for (int i = 0; i < currentReadersDetected; i++)
							{
								if (!m_activeReaders.ContainsKey(i))
								{
										uFReader reader = new uFReader(i);
										if (Ok(reader.open(), out var status))
										{
											lock (m_activeReaders)
											{
												var newConn = new ReaderConnection() { Reader = reader, Data = null, ReaderSN = reader.reader_sn };
												if (!m_activeReaders.TryAdd(i, newConn))
												{
													//Errors.Enqueue($"Failed to add new connected reader {i} to active readers");
												}
												else
												{
													_ = Task.Run(() => UpdateCardAndConnectionInfo(newConn));
												}
											}
										}
										else
										{
										}
									
								}
							}
						}
					}
				}
				 catch(Exception ex)
				{
					Errors.Enqueue(ex.Message);
				}


				await Task.Delay(500);
			}
		}

		private async void UpdateCardAndConnectionInfo(ReaderConnection conn)
		{
			while (!m_cancellationTokenSource.IsCancellationRequested && conn.Connected)
			{
				lock (conn)
				{
					try
					{

						{
							if (!Ok(GetCardConnectionInfo(conn), out var status))
							{
								if (!UpdateConnectionStatus(status, conn))
								{
									Errors.Enqueue($"Failed to GetCardConnectionInfo {conn.ReaderSN} {status}");
								}

							}
							else if (!conn.HaveReadCard)
							{
								conn.CardConnected = true;
								conn.LastReadCardUID = conn.Data.CardUID;
								SuccessulScans.Enqueue(new SuccessfulRead() { ReaderId = conn.ReaderSN, ReaderData = conn.Data.CardUID });
							}
						}

					}
					catch (Exception ex)
					{
						Errors.Enqueue(ex.Message);
					}
				}

				await Task.Delay(50);
			}

		}

		private bool UpdateConnectionStatus(DL_STATUS status, ReaderConnection conn)
		{
			if (status == DL_STATUS.UFR_NO_CARD || status == DL_STATUS.UFR_FT_STATUS_ERROR_5 || status == DL_STATUS.UFR_PARAMETERS_ERROR)
			{
				conn.CardConnected = false;
				conn.ResetCardReads();
				return true;
			}

			return false;
		}

		private void Update()
		{
			while(SuccessulReads.TryDequeue(out SuccessfulRead readResult))
			{
				OnReadData?.Invoke(readResult);
			}

			while (SuccessulScans.TryDequeue(out SuccessfulRead scanResult))
			{
				OnScanCard?.Invoke(scanResult);
			}

			while (Errors.TryDequeue(out string error))
			{
				Debug.LogWarning(error);
			}

			while(Info.TryDequeue(out string info))
			{
				Debug.Log(info);
			}

			m_readers = m_activeReaders.Values.ToList();
		}

		private void OnDestroy()
		{
			m_cancellationTokenSource.Cancel();
			foreach (var reader in m_activeReaders)
			{
				Close(reader.Value);
			}
		}
	}
}
