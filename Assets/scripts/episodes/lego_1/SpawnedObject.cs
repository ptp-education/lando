using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedObject : MonoBehaviour
{
    protected GameManager gameManager_;

    public void Init(GameManager gameManager)
    {
        gameManager_ = gameManager;

        Reset();
    }

    public virtual void ReceivedAction(string action)
    {

    }

    public virtual void Reset()
    {

    }

    public virtual void Hide()
    {
        //implement this if you want this SpawnedObject to listen to "-hideall")
    }
}
