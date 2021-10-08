using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DanceCharacter : MonoBehaviour
{
    private Animator anim_;
    
    void Start()
    {
        anim_ = GetComponent<Animator>();
    }

    public void Selected()
    {
        if (anim_ != null)
        {
            anim_.Play("Victory");
        }
    }

    public void Idle()
    {
        if (anim_ != null)
        {
            anim_.Play("Idle");
        }
    }

    public float PlayAnimation(string animation)
    {
        float animTime = -1f;
        if (anim_ != null)
        {
            animTime = TimeForAnimation(animation);
            if (animTime >= 0f)
            {
                anim_.Play(animation);
            }
        }
        return animTime;
    }

    public float TimeForAnimation(string animation)
    {
        foreach(AnimationClip c in anim_.runtimeAnimatorController.animationClips)
        {
            if (string.Equals(c.name, animation))
            {
                return c.length;
            }
        }
        return -1f;
    }
}
