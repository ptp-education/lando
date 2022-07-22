using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class OnscreenCharacterKiki : OnscreenCharacter
{
    [SerializeField] private SkeletonGraphic model_;

    private bool isSpeaking_ = false;

    protected override void HandleSpeaking(bool isSpeaking)
    {
        base.HandleSpeaking(isSpeaking);

        if (isSpeaking != isSpeaking_)
        {
            if (isSpeaking)
            {
                Debug.LogWarning("setting animation to speak");
                model_.AnimationState.SetAnimation(0, "speak", true);
            }
            else
            {
                Debug.LogWarning("setting animation to idle");
                model_.AnimationState.SetAnimation(0, "idle", true);
            }
        }

        isSpeaking_ = isSpeaking;
    }
}
