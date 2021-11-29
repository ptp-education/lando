using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;

public class BeforePlay
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static private void OnBeforeSceneLoadRuntimeMethod()
    {
        SaveEpisodes();
        SaveSongs();
    }

    private static void SaveEpisodes()
    {
#if UNITY_EDITOR
        string prefix = "prefabs/episodes/";
        string path = Application.dataPath + "/Resources/" + prefix;

        string[] fileNames = Directory.GetFiles(path)
            .Where(x => Path.GetExtension(x) != ".meta").ToArray();

        for (int i = 0; i < fileNames.Length; i++)
        {
            fileNames[i] = prefix + fileNames[i].StripExtensions();
        }
        StringsFile fileInfo = new StringsFile(fileNames);
        string fileInfoJson = JsonUtility.ToJson(fileInfo);

        File.WriteAllText(Application.dataPath + "/Resources/all_episodes.txt", fileInfoJson);

        AssetDatabase.Refresh();
#endif
    }

    private static void SaveSongs()
    {
#if UNITY_EDITOR
        string prefix = "audio/songs/";
        string path = Application.dataPath + "/Resources/" + prefix;

        string[] fileNames = Directory.GetFiles(path)
            .Where(x => Path.GetExtension(x) != ".meta").ToArray();

        for (int i = 0; i < fileNames.Length; i++)
        {
            fileNames[i] = prefix + fileNames[i].StripExtensions();
        }

        StringsFile fileInfo = new StringsFile(fileNames);
        string fileInfoJson = JsonUtility.ToJson(fileInfo);

        File.WriteAllText(Application.dataPath + "/Resources/all_songs.txt", fileInfoJson);

        AssetDatabase.Refresh();
#endif
    }
}
