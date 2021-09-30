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
        anim_.Play("Victory");
    }
}
