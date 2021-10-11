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
    }

    static private void SaveEpisodes()
    {
        string path = Application.dataPath + "/Resources/prefabs/episodes/";

        string[] fileNames = Directory.GetFiles(path)
            .Where(x => Path.GetExtension(x) != ".meta").ToArray();

        for (int i = 0; i < fileNames.Length; i++)
        {
            string[] split = fileNames[i].Split('/');
            string fn = split[split.Length - 1];

            string[] split2 = fn.Split('.');
            fileNames[i] = split2[0];
        }

        EpisodesFileInfo fileInfo = new EpisodesFileInfo(fileNames);
        string fileInfoJson = JsonUtility.ToJson(fileInfo);

        File.WriteAllText(Application.dataPath + "/Resources/all_episodes.txt", fileInfoJson);

        AssetDatabase.Refresh();
    }
}
