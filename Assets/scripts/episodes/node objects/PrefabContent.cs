using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabContent : MonoBehaviour
{
    [SerializeField] public Camera Camera;

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
}
