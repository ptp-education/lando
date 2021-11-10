using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System;
using Newtonsoft.Json;

public class EpisodeNode : MonoBehaviour
{
    public enum EpisodeType
    {
        Video,
        Prefab,
        Image,
        Sequence
    }

    [Serializable]
    public class Option
    {
        [SerializeField] public string Prompt;
        [SerializeField] public EpisodeNode Node;
        [SerializeField] public string Test;
        [SerializeField] public Option NewOption;
    }

    public EpisodeType Type;
    public UnityEngine.Object Video;
    public string VideoFilePath;
    public UnityEngine.Object VideoLoop;
    public string VideoLoopFilePath;

    public UnityEngine.Object Image;
    public string ImageFilePath;
    public UnityEngine.Object ImageLoop;
    public string ImageLoopFilePath;

    public Episode Episode
    {
        get
        {
            return GetComponentInParent<Episode>();
        }
    }

    public GameObject Prefab;
    public string PrefabPath;

    public string Prompt;

    public EpisodeNode NextNode;

    public List<Option> Options = new List<Option>();

    public override string ToString()
    {
        string contentName = "";
        switch(Type)
        {
            case EpisodeType.Video:
                contentName = VideoFilePath;
                break;
            case EpisodeType.Prefab:
                contentName = PrefabPath;
                break;
        }

        return string.Format("{0} - {1} - {2}", gameObject.name, Type.ToString(), contentName);
    }

    public NodeVisualizer VisualNode;
}