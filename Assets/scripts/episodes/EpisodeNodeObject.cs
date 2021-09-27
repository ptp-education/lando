using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EpisodeNodeObject : MonoBehaviour
{
    public delegate void ReadyToStartLoop();

    protected ReadyToStartLoop startLoopCallback_;
    protected EpisodeNode episodeNode_;

    public virtual void Init(EpisodeNode node, ReadyToStartLoop callback)
    {
        startLoopCallback_ = callback;
        episodeNode_ = node;
    }

    public virtual void Preload(EpisodeNode node)
    {
        transform.localScale = Vector3.zero;
    }

    public virtual void Hide()
    {
        transform.localScale = Vector3.zero;
    }

    public virtual void Play()
    {
        transform.localScale = Vector3.one;
    }

    public virtual void Loop()
    {
        transform.localScale = Vector3.one;
    }

    public virtual void ReceiveAction(string action)
    {

    }
}
