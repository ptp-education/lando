using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

[RequireComponent(typeof(SkeletonGraphic))]
public class Character : MonoBehaviour
{
    public const int kMainTrack = 0;
    public const int kBlinkTrack = 10;
    public const string kIdleAnimation = "idle";

    private SkeletonGraphic skeletonGraphic
    {
        get
        {
            return GetComponent<SkeletonGraphic>();
        }
    }

    public void StartLooping()
    {
        skeletonGraphic.AnimationState.SetAnimation(kMainTrack, kIdleAnimation, true);
    }

    private void Start()
    {
        SkeletonGraphic sg = GetComponent<SkeletonGraphic>();
        //blink
    }
}
