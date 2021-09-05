using System;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using System.IO;
using System.Linq;

[Serializable]
public class EpisodesFileInfo
{
    public string[] episodeNames;

    public EpisodesFileInfo(string[] fileNames)
    {
        this.episodeNames = fileNames;
    }
}

public class Build : IPreprocessBuildWithReport
{
    public int callbackOrder { get { return 0; } }

    public void OnPreprocessBuild(UnityEditor.Build.Reporting.BuildReport report)
    {
        string resourcsPath = Application.dataPath + "/Resources/prefabs/episodes/";

        string[] fileNames = Directory.GetFiles(resourcsPath)
            .Where(x => Path.GetExtension(x) != ".meta").ToArray();

        EpisodesFileInfo fileInfo = new EpisodesFileInfo(fileNames);
        string fileInfoJson = JsonUtility.ToJson(fileInfo);

        File.WriteAllText(Application.dataPath + "/Resources/all_episodes.txt", fileInfoJson);

        AssetDatabase.Refresh();
    }
}