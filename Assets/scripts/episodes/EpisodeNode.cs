using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System;
using System.Linq;
using Newtonsoft.Json;

public class EpisodeNode : MonoBehaviour
{
    public enum EpisodeType
    {
        Video,
        Prefab,
        Image,
        LoopWithOptions
    }

    [Serializable]
    public class Option
    {
        [SerializeField] public string Prompt;
        [SerializeField] public EpisodeNode Node;
    }

    [Serializable]
    public class PrefabSpawnObject
    {
        [SerializeField] public float TimeStamp;
        [SerializeField] public Vector3 Position;
        [SerializeField] public UnityEngine.Object Object;
        [SerializeField] public string Path;
        public bool Spawned = false;
    }

    [Serializable]
    public class VideoOption
    {
        [SerializeField] public string Key;
        [SerializeField] public List<Video> Videos = new List<Video>();

        [Serializable]
        public class Video
        {
            [SerializeField] public UnityEngine.Object VideoObject;
            [SerializeField] public string VideoPath;
        }
    }

    //BG AUDIO OPTIONS
    public string BgLoopPath;
    public UnityEngine.Object BgLoop;
    public bool StopBgLoopAtSequence = false;   //generally expecting a new Bg Loop to appear and play after loop
    public bool StartBgLoopAfterSequence = false; //used when we want to change a loop after sequence ends

    public EpisodeType Type;

    //VIDEO OPTIONS
    public UnityEngine.Object Video;
    public string VideoFilePath;
    public UnityEngine.Object VideoLoop;
    public string VideoLoopFilePath;

    //IMAGE OPTIONS
    public UnityEngine.Object Image;
    public string ImageFilePath;
    public UnityEngine.Object ImageLoop;
    public string ImageLoopFilePath;

    //PREFAB OPTIONS
    public GameObject Prefab;
    public string PrefabPath;

    //LOOPWITHOPTIONS OPTIONS
    //uses VideoLoop and VideoLoopFilePath
    public List<VideoOption> VideoOptions = new List<VideoOption>();

    //ALL OPTIONS
    public string Prompt;
    public List<PrefabSpawnObject> PrefabSpawnObjects = new List<PrefabSpawnObject>();
    public EpisodeNode NextNode;
    public List<Option> Options = new List<Option>();

    public Episode Episode
    {
        get
        {
            return GetComponentInParent<Episode>();
        }
    }

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
            case EpisodeType.Image:
                contentName = ImageFilePath;
                break;
            case EpisodeType.LoopWithOptions:
                contentName = VideoLoopFilePath;
                break;
        }

        return string.Format("{0} - {1} - {2}", gameObject.name, Type.ToString(), contentName);
    }

    public NodeVisualizer VisualNode;

    public static EpisodeNode CreateNewNode(Transform parent, string videoFile, string loopFile, string script, List<EpisodeSpawnData.NodeOption> options)
    {
        GameObject obj = new GameObject();
        obj.AddComponent<EpisodeNode>();
        EpisodeNode newNode = obj.GetComponent<EpisodeNode>();

        newNode.transform.SetParent(parent);

        string[] split = videoFile.Split('/');
        newNode.name = split[split.Length - 1];

        newNode.VideoFilePath = videoFile;
        newNode.VideoLoopFilePath = loopFile;
        newNode.Prompt = script;

        if (options != null)
        {
            foreach(EpisodeSpawnData.NodeOption o in options) {
                Option newOption = new Option();
                newOption.Prompt = o.Name;
                newOption.Node = CreateNewNode(parent, o.Node.VideoFile, o.Node.LoopVideoFile, o.Node.Script, null);
                newNode.Options.Add(newOption);
            }
        }
        return newNode;
    }
}