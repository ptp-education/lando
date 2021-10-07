using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EpisodeNodeObject : MonoBehaviour
{
    public delegate void ReadyToStartLoop();

    protected ReadyToStartLoop startLoopCallback_;
    protected EpisodeNode episodeNode_;
    protected GameManager gameManager_;

    private bool hidden_ = false;

    public virtual void Init(GameManager manager, EpisodeNode node, ReadyToStartLoop callback)
    {
        gameManager_ = manager;
        startLoopCallback_ = callback;
        episodeNode_ = node;
    }

    public virtual void Preload(EpisodeNode node)
    {
        Hide();
    }

    public virtual void Hide()
    {
        if (!hidden_) 
        {
            OnFirstHide();
        }
        hidden_ = true;
        transform.localScale = Vector3.zero;
    }

    public virtual void Play()
    {
        hidden_ = false;
        transform.localScale = Vector3.one;
    }

    public virtual void Loop()
    {
        hidden_ = false;
        transform.localScale = Vector3.one;
    }

    public virtual void ReceiveAction(string action)
    {

    }

    protected virtual void OnFirstHide() 
    {

    }
}
