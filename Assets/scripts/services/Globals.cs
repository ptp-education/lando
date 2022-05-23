using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Lando
{
	public class Globals : MonoBehaviour
	{
		private static Globals s_instance = default;
		private void Awake()
		{
			if (s_instance != null)
			{
				Destroy(gameObject);
			} else
			{
				DontDestroyOnLoad(gameObject);
				s_instance = this;
			}
			
		}

		private void Start()
		{
			SceneManager.LoadScene("main");
		}

		private static SmartObjects.SmartObjectManager s_smartObjectManager = default;

		public static SmartObjects.SmartObjectManager SmartManager
		{
			get
			{
				if (s_smartObjectManager == null)
				{
					s_smartObjectManager = s_instance.GetComponentInChildren<SmartObjects.SmartObjectManager>();
				}

				return s_smartObjectManager;
			}
		}
	}
}

