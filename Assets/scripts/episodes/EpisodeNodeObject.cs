using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EpisodeNodeObject : MonoBehaviour
{
    public delegate void ReadyToStartLoop();

    protected ReadyToStartLoop startLoopCallback_;

    public virtual void Init(ReadyToStartLoop callback)
    {
        startLoopCallback_ = callback;
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
