using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

public class Episode : MonoBehaviour
{
    [SerializeField] public EpisodeNode StartingNode;
    [SerializeField] public string VORoot;
    [SerializeField] public ChallengeData ChallengeData;

    public EpisodeNode[] AllNodes
    {
        get
        {
            return GetComponentsInChildren<EpisodeNode>();
        }
    }

    public EpisodeNode JumpFromNode(string jumpFrom, int byAmount)
    {
        for (int i = 0; i < AllNodes.Length; i++)
        {
            if (string.Equals(jumpFrom, AllNodes[i].name))
            {
                int newCounter = i + byAmount;
                if (newCounter >= 0 && newCounter < AllNodes.Length)
                {
                    return AllNodes[newCounter];
                }
            }
        }
        return null;
    }
}