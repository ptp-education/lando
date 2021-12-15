using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SpawnedCounter : SpawnedObject
{
    public class Integer
    {
        public int value = 0;

        public Integer(int v)
        {
            value = v;
        }
    }

    [SerializeField] private Text counterText_;

    private Integer counter_;
    
    public override void ReceivedAction(string action)
    {
        base.ReceivedAction(action);

        if (string.Equals(action, "increase-counter"))
        {
            counter_.value++;
            gameManager_.Storage.Add<Integer>(GameStorage.Key.Counter, counter_);

            RefreshCounter();
        }
    }

    public override void Reset()
    {
        base.Reset();

        counter_ = gameManager_.Storage.GetValue<Integer>(GameStorage.Key.Counter);

        if (counter_ == null)
        {
            counter_ = new Integer(0);
        }
        RefreshCounter();
    }

    private void RefreshCounter()
    {
        if (counter_ != null)
        {
            counterText_.text = counter_.value.ToString();
        } else
        {
            counterText_.text = "";
        }
    }
}
