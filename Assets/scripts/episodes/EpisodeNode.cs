using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System;

public class EpisodeNode : MonoBehaviour
{
    public enum EpisodeType
    {
        Video,
        Prefab
    }

    [Serializable]
    public class Option
    {
        [SerializeField] public string Prompt;
        [SerializeField] public EpisodeNode Node;
    }

    public EpisodeType Type;
    public UnityEngine.Object Video;
    public string VideoFilePath;
    public UnityEngine.Object VideoLoop;
    public string VideoLoopFilePath;

    public GameObject Prefab;
    public string PrefabPath;

    public string Prompt;

    public EpisodeNode NextNode;

    public List<Option> Options = new List<Option>();
}