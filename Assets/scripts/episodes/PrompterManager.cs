using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrompterManager : GameManager
{
    private void Start()
    {
        TextAsset fileNamesAsset = Resources.Load<TextAsset>("all_episodes");
        EpisodesFileInfo efi = JsonUtility.FromJson<EpisodesFileInfo>(fileNamesAsset.text);
        //Use data?
        foreach (string fileName in fileInfoLoaded.fileNames)
        {
            Debug.Log(fileName);
        }
    }
}
