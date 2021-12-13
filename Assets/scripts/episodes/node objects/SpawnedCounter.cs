using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnedCounter : SpawnedObject
{
    [SerializeField] private Text counterText_;

    private int counter_ = 0;
    
    public override void ReceivedAction(string action)
    {
        base.ReceivedAction(action);

        if (string.Equals(action, "Increase Counter"))
        {
            counter_++;
            counterText_.text = counter_.ToString();
        }
    }

    public override void Reset()
    {
        base.Reset();

        counter_ = 0;
        RefreshCounter();
    }

    private void RefreshCounter()
    {
        counterText_.text = counter_.ToString();
    }
}
