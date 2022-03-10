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
}