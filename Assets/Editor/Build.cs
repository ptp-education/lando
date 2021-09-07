using System;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using System.IO;
using System.Linq;

public class Build : IPreprocessBuildWithReport
{
    public int callbackOrder { get { return 0; } }

    public void OnPreprocessBuild(UnityEditor.Build.Reporting.BuildReport report)
    {
        SaveEpisodes();
    }

    private void SaveEpisodes()
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