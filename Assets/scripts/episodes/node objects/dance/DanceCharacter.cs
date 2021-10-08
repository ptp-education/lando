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

    public void PlayAnimation(string animation)
    {
        if (anim_ != null)
        {
            anim_.Play(animation);
        }
    }
}
