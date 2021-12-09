#if !UNITY_EDITOR
#define USE_NATIVE
#endif
using System.Runtime.InteropServices;
using UnityEngine;

namespace Lando.Logger
{
	public class NativeLogger : MonoBehaviour
	{
#if USE_NATIVE
		[DllImport("__Internal")]
		public static extern void Log(string str);
#else
		public static void Log(string str)
		{
			UnityEngine.Debug.Log(str);
		}
#endif
	}
}
