using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System;

public class EpisodeNode : MonoBehaviour
{
    [Serializable]
    public class Option
    {
        [SerializeField] public string Prompt;
        [SerializeField] public EpisodeNode Node;
    }

    [SerializeField] public UnityEngine.Object Video;
    [SerializeField] public string VideoFilePath;
    [SerializeField] public UnityEngine.Object VideoLoop;
    [SerializeField] public string VideoLoopFilePath;

    [TextArea(15, 20)]
    [SerializeField] public string Prompt;

    [Space]
    [Header("   Optional - include a next video as the first option")]
    [Space]
    [SerializeField] public EpisodeNode NextNode;

    [SerializeField] public List<Option> Options = new List<Option>();
}