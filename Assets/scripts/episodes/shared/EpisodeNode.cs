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
        Image,
        PREFAB_DEPRECATED,
        LoopWithOptions
    }

    [Serializable]
    public class Character
    {
        public enum Option
        {
            Cat
        }
        [SerializeField] public Vector3 TalkingPosition;
        [SerializeField] public Vector3 Scale;
        [SerializeField] public Option SelectedCharacter;

        public string Name
        {
            get
            {
                return SelectedCharacter.ToString();
            }
        }
    }

    [Serializable]
    public class OptionHolder
    {
        [SerializeField] public string Name;
        [SerializeField] public List<Option> Options = new List<Option>();
    }

    [Serializable]
    public class Option
    {
        [SerializeField] public string Name;
        [SerializeField] public string Action;
        [SerializeField] public EpisodeNode Node;
    }

    [Serializable]
    public class PrefabSpawnObject
    {
        [SerializeField] public float TimeStamp;
        [SerializeField] public string SpawnKey;    //set this spawn key to be able to spawn from command line
        [SerializeField] public Vector3 Position;
        [SerializeField] public Vector3 Scale = Vector3.one;
        [SerializeField] public UnityEngine.Object Object;
        [SerializeField] public string Path;
        [HideInInspector] public bool Spawned = false;
    }

    [Serializable]
    public class CommandLine
    {
        [SerializeField] public float TimeStamp;
        [SerializeField] public string Command;
        [HideInInspector] public bool Ran = false;
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

    public EpisodeType Type;

    //CHARACTER ON SCREEN
    public List<Character> Characters = new List<Character>();

    //VIDEO OPTIONS
    public UnityEngine.Object Video;
    public string VideoFilePath;
    public UnityEngine.Object VideoLoop;
    public string VideoLoopFilePath;

    //IMAGE OPTIONS
    public UnityEngine.Object Image;
    public string ImageFilePath;

    //LOOPWITHOPTIONS OPTIONS
    //uses VideoLoop and VideoLoopFilePath
    public List<VideoOption> VideoOptions = new List<VideoOption>();

    //ALL OPTIONS
    public bool FadeInFromPreviousScene;
    public string Prompt;
    public List<PrefabSpawnObject> PrefabSpawnObjects = new List<PrefabSpawnObject>();
    public List<CommandLine> CommandLines = new List<CommandLine>();
    public EpisodeNode NextNode;
    public List<OptionHolder> OptionHolders = new List<OptionHolder>();


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
            case EpisodeType.Image:
                contentName = ImageFilePath;
                break;
            case EpisodeType.LoopWithOptions:
                contentName = VideoLoopFilePath;
                break;
        }

        return string.Format("{0} - {1} - {2}", gameObject.name, Type.ToString(), contentName);
    }
}