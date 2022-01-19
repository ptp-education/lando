using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnscreenCharacter : MonoBehaviour
{
    [SerializeField] private GameObject voiceBubble_;
    [SerializeField] private Animator anim_;

    private GoTweenFlow flow_;
    private Vector3 talkPosition_;
    private ShareManager shareManager_;

    public void Init(Vector3 talkPosition, ShareManager sm)
    {
        talkPosition_ = talkPosition;
        shareManager_ = sm;
    }

    private void Start()
    {
        MoveOffscreen(null, false);
    }

    public void ToggleVoiceBubble(bool show)
    {
        if (show)
        {
            if (!anim_.GetCurrentAnimatorStateInfo(0).IsName("didi-idle"))
            {
                MoveOnscreen(playSound: false);
                Go.to(transform, 1.0f, new GoTweenConfig().onComplete(t =>
                {
                    voiceBubble_.gameObject.SetActive(true);
                }));
            } else
            {
                voiceBubble_.gameObject.SetActive(true);
            }
        }
        else
        {
            if (!GameManager.ZoneActive)
            {
                MoveOffscreen(null, true);
            }
            voiceBubble_.gameObject.SetActive(false);
        }
        
    }

    public void MoveOnscreen(bool playSound = true)
    {
        if (!anim_.GetCurrentAnimatorStateInfo(0).IsName("didi-idle") && !anim_.GetCurrentAnimatorStateInfo(0).IsName("didi-enter"))
        {
            anim_.Play("didi-enter");
        }

        if (transform.localPosition != talkPosition_)
        {
            transform.localPosition = talkPosition_;
        }

        if (playSound)
        {
            AudioPlayer.PlayAudio("audio/sfx/whoosh");
        }
    }

    public void MoveOffscreen(System.Action callback, bool sound)
    {
        anim_.Play("didi-exits");

        if (sound)
        {
            //AudioPlayer.PlayAudio("audio/sfx/whoosh");
        }

        Go.to(transform, 1.25f, new GoTweenConfig().onComplete(t =>
        {
            transform.localPosition = new Vector3(-3000f, 750);

            if (callback != null)
            {
                callback();
            }
        }));
    }
}
