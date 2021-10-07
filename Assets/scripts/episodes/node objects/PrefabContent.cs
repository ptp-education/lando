using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabContent : MonoBehaviour
{
    [SerializeField] public Camera Camera;

    protected GameManager gameManager_;

    public void Init(GameManager gameManager)
    {
        gameManager_ = gameManager;
    }

    public virtual void ReceiveAction(string action)
    {
        //stub
    }

    public virtual void Play()
    {
        //stub
    }

    public virtual void Loop()
    {
        //stub
    }

    public virtual void OnFirstHide()
    {
        //stub
    }
}
