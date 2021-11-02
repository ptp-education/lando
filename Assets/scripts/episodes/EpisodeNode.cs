using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System;
using Newtonsoft.Json;

public class SequenceData
{
    public class Object
    {
        public enum Type
        {
            Image,
            Video,
            Prefab,
            Spine
        }
        public string Name;
        public string ObjectType;
        public string ModelPath;
        public Vector3 StartingPosition = Vector3.zero;
        public Vector3 StartingScale = Vector3.one;
    }

    public class Movement
    {
        public enum Type
        {
            Move,
            Scale
        }

        public string MovementType;
        public Vector3 Target;
        public float Duration;
        public float RelativeTimeAfter = 0f;
    }

    public class Animation
    {
        public string AnimationName;
        public bool LoopForever = false;
        public int LoopTimes = 1;
        public float DelayTime = 0.05f;
    }
    public class Accompaniment
    {
        public float RelativeTimeAfter;
        public string ObjectName;
        public string SoundPath;
        public List<Animation> Animations = new List<Animation>();
        public List<Movement> Movements = new List<Movement>();
    }

    public class SequenceStep
    {
        public enum Type
        {
            Wait,
            Voiceover
        }

        public string SequenceType;
        public float RelativeTimeAfter;
        public float Duration;
        public string VoiceoverPath;
        public float VoiceoverSpeed;
        public List<Accompaniment> Accompaniments = new List<Accompaniment>();
    }

    public class LoopInstructions
    {
        public string ObjectName;
        public string Animation;
    }

    public string TemplateId = null;
    public List<Object> Objects = new List<Object>();
    public List<SequenceStep> SequenceSteps = new List<SequenceStep>();
    public List<LoopInstructions> Looping = new List<LoopInstructions>();
}

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

    public string SequenceData;
    public SequenceData ProcessedSequenceData
    {
        get
        {
            SequenceData d = JsonConvert.DeserializeObject<SequenceData>(SequenceData);
            return d;
        }
    }

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