using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SpawnedZoneIndicator : SpawnedObject
{
    private GoTweenFlow flow_;
    public override void ReceivedAction(string action)
    {
        base.ReceivedAction(action);

        if (string.Equals(action, "-showzoneindicator"))
        {
            Reset();
            AudioPlayer.PlayAudio("audio/sfx/small-whoosh");
            flow_ = new GoTweenFlow();
            flow_.insert(0f, new GoTween(transform, 0.8f, new GoTweenConfig().localPosition(new Vector3(0, -525, 0)).setEaseType(GoEaseType.SineOut)));
            flow_.play();
        }
    }

    public override void Reset()
    {
        base.Reset();

        if (flow_ != null)
        {
            flow_.complete();
        }
        transform.localPosition = new Vector3(0, -925, 0);
    }
}
