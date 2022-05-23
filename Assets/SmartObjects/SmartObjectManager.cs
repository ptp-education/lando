using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Lando.SmartObjects
{
	[RequireComponent(typeof(uFrUnity.uFrUnityPlugin))]
	public class SmartObjectManager : MonoBehaviour
	{

		[System.Serializable]
		public class SaveData
		{
			public List<SmartObjectConnector> Connectors;
		}
		private uFrUnity.uFrUnityPlugin m_ufrPlugin = default;
		/// <summary>
		/// Collection of "smart object type" -> "SmartConnector" pairs. Each smart object has a dedicated SmartConnector, which is the entry point to this API.
		/// The assignment of a smart object to a SmartConnector is handled in this class
		/// </summary>
		private Dictionary<string, SmartObjectConnector> m_smartObjectReaders = new Dictionary<string, SmartObjectConnector>();

		private int m_configuringSmartObjectIndex = 0;// Which smart object are we currently configuring (if any)
		private bool m_isConfigured = false; // Have all the requested smart objects been linked to a specific reader?

		/// <summary>
		/// String-based unique name for a Physical object which needs to be assigned to a specific NFC/RFID reader.
		/// </summary>
		[SerializeField]
		private SmartObjectType[] m_smartObjectsToConfigure = default;

		/// <summary>
		/// Reference to the UI which shows the current configuration state. (if any)
		/// </summary>
		[SerializeField]
		private CanvasGroup m_smartObjectConfigurator = default;

		[SerializeField]
		private bool m_configureOnAwake = true;
		const string KEY = "SmartObjectReaders";

		private SaveData m_saveData;

		private void Awake()
		{
			m_ufrPlugin = GetComponent<uFrUnity.uFrUnityPlugin>();
			if (m_configureOnAwake)
			{
				Configure();
			}

		}

		private void OnDestroy()
		{
			//m_ufrPlugin.OnReadData -= M_ufrPlugin_OnReadData;
			m_ufrPlugin.OnScanCard -= M_ufrPlugin_OnReadData;
		}

		#region API
		public async Task<SmartObjectConnector> GetSmartConnector(SmartObjectType smartObjectType)
		{
			while (!m_isConfigured)
			{
				await Task.Yield();
			}

			foreach (var pair in m_smartObjectReaders)
			{
				if (pair.Value != null && pair.Value.IsConnectorFor(smartObjectType))
				{
					return pair.Value;
				}
			}

			return null;
		}

		public async void ReConfigure()
		{
			if (PlayerPrefs.HasKey(KEY))
			{
				PlayerPrefs.DeleteKey(KEY);
			}

			try
			{
				await RunConfiguration();
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}

		}

		public async void Configure()
		{
			try
			{
				await RunConfiguration();
			} catch(Exception ex)
			{
				Debug.LogException(ex);
			}
			
		}
		#endregion

		#region IMPLEMENTATION

		private void M_ufrPlugin_OnReadData(uFrUnity.SuccessfulRead obj)
		{
			if (m_isConfigured && m_smartObjectReaders.TryGetValue(obj.ReaderId, out SmartObjectConnector value))
			{
				value.Trigger(obj.ReaderData);
			}
			else // Unexpected reader, let's remove data and re-configure
			{
				if (PlayerPrefs.HasKey(KEY))
				{
					PlayerPrefs.DeleteKey(KEY);
				}

				m_ufrPlugin.OnReadData -= M_ufrPlugin_OnReadData;

				m_isConfigured = false;
				Configure();
			}
		}

		private async Task RunConfiguration()
		{

			void ReadData(uFrUnity.SuccessfulRead obj)
			{
				if (!m_smartObjectReaders.ContainsKey(obj.ReaderId))
				{
					m_smartObjectReaders.Add(obj.ReaderId, new SmartObjectConnector(obj.ReaderId, m_smartObjectsToConfigure[m_configuringSmartObjectIndex]));
					m_configuringSmartObjectIndex++;
				}
			}

			try
			{
				if (PlayerPrefs.HasKey(KEY))
				{
					SaveData data = JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString(KEY));
					var readers = data.Connectors;
					if (readers.Count == m_smartObjectsToConfigure.Length)
					{
						m_smartObjectReaders.Clear();
						foreach (var reader in readers)
						{
							m_smartObjectReaders.Add(reader.UID, reader);
						}
						m_isConfigured = true;
						m_smartObjectConfigurator.gameObject.SetActive(false);
					}

				}

				if (!m_isConfigured)
				{
					m_smartObjectConfigurator.gameObject.SetActive(true);
					//m_ufrPlugin.OnReadData += ReadData;
					m_ufrPlugin.OnScanCard += ReadData;

					Text message = m_smartObjectConfigurator.GetComponentInChildren<Text>();
					while (m_configuringSmartObjectIndex < m_smartObjectsToConfigure.Length)
					{
						message.text = $"Please scan any NFC tag on the {m_smartObjectsToConfigure[m_configuringSmartObjectIndex]} smart object reader";
						await Task.Yield();
					}

					List<SmartObjectConnector> connectorsToSave = new List<SmartObjectConnector>();
					foreach (var pair in m_smartObjectReaders)
					{
						connectorsToSave.Add(pair.Value);
					}

					PlayerPrefs.SetString(KEY, JsonUtility.ToJson(new SaveData() { Connectors = connectorsToSave }));

					m_smartObjectConfigurator.gameObject.SetActive(false);
					m_isConfigured = true;
				}
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
			finally
			{
				//m_ufrPlugin.OnReadData -= ReadData;
				m_ufrPlugin.OnScanCard -= ReadData;
			}

			if (m_isConfigured)
			{
				//m_ufrPlugin.OnReadData += M_ufrPlugin_OnReadData;
				m_ufrPlugin.OnScanCard += M_ufrPlugin_OnReadData;
			}
		}
		#endregion // IMPLEMENTATION
	}
}

