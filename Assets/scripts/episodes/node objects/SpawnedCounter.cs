using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SpawnedCounter : SpawnedObject
{
    private GoTweenFlow flow_;

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
            AudioPlayer.PlayAudio("episodes/icebreakers/ding03");
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
        counterText_.transform.localScale = Vector3.one;

        if (flow_ != null)
        {
            flow_.destroy();
        }

        flow_ = new GoTweenFlow();

        flow_.insert(0f, new GoTween(counterText_.transform, 0.15f, new GoTweenConfig().scale(1.25f)));
        flow_.insert(0.15f, new GoTween(counterText_.transform, 0.15f, new GoTweenConfig().scale(1f)));
        flow_.insert(0.15f, new GoTween(counterText_.transform, 0.15f, new GoTweenConfig().onComplete(t =>
        {
            if (counter_ != null)
            {
                counterText_.text = counter_.value.ToString();
            }
        })));

        flow_.play();

        if (counter_ == null)
        {
            counterText_.text = "";
        }
    }
}
