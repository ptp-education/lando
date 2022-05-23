using System;
using UnityEngine;

namespace Lando.SmartObjects
{
	/// <summary>
	/// SmartObjectConnector represents a physical object, which is augmented using an NFC/RFID reader
	/// </summary>
	[System.Serializable]
	public class SmartObjectConnector
	{
		
		[SerializeField]
		private string m_readerUid = default; // NFC/RFID Reader Unique Id
		private Action<string, SmartObjectType> m_onTagRead = default;
		[SerializeField]
		private SmartObjectType m_smartObjectType = default;

		public SmartObjectConnector(string readerUid, SmartObjectType smartObjectType)
		{
			m_smartObjectType = smartObjectType;
			m_readerUid = readerUid;
		}

		public void Trigger(string data)
		{
			m_onTagRead?.Invoke(data, m_smartObjectType);
		}

		public void Connect(Action<string, SmartObjectType> callback)
		{
			m_onTagRead = callback;
		}

		public bool IsConnectorFor(SmartObjectType type)
		{
			return m_smartObjectType == type;
		}

		public string UID => m_readerUid;
	}
}
