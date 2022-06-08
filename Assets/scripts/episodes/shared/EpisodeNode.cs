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
        Simulator
    }

    [Serializable]
    public class Character
    {
        public enum Option
        {
            Didi
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
    public class OptionsHolder
    {
        [SerializeField] public bool StartShown = true;
        [SerializeField] public List<Option> Options = new List<Option>();
    }

    [Serializable]
    public class Option
    {
        [SerializeField] public string ButtonName;
        [SerializeField] public bool TeacherOnly;
        [SerializeField] public string Command;
        [SerializeField] public EventObject EventObject;
    }

    [Serializable]
    public class CommandLine
    {
        [SerializeField] public float TimeStamp;
        [SerializeField] public string Command;
        [SerializeField] public EventObject EventObject;
        [HideInInspector] public bool Ran = false;
    }

    [Serializable]
    public class CommandContainer
    {
        [SerializeField] public string CommandToCall;
        [SerializeField] public List<CommandLine> StoredCommands = new List<CommandLine>();
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

    [Serializable]
    public class SimulatorStep
    {
        [SerializeField] public EventObject Question;
        [SerializeField] public EventObject Answer;
    }

    [Serializable]
    public class SimulatorHolder
    {
        [SerializeField] public bool ShowAnswer = false;
        [SerializeField] public List<SimulatorStep> Steps = new List<SimulatorStep>();
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

    //LOOPWITHOPTIONS OPTIONS (DEPRECATED)
    //uses VideoLoop and VideoLoopFilePath
    public List<VideoOption> VideoOptions = new List<VideoOption>();

    //Simulator Options
    public SimulatorHolder SimulatorDetails;

    //ALL OPTIONS
    public bool FadeInFromPreviousScene;
    public bool TestingActive;
    public OptionsHolder OptionsToSpawn;
    public List<PrefabSpawnObject> PrefabSpawnObjects = new List<PrefabSpawnObject>();
    public List<CommandLine> CommandLines = new List<CommandLine>();
    public List<CommandContainer> CommandLineContainers = new List<CommandContainer>();
    public EpisodeNode NextNode;

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
        }

        return string.Format("{0} - {1} - {2}", gameObject.name, Type.ToString(), contentName);
    }
}