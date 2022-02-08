using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnscreenCharacter : MonoBehaviour
{
    [SerializeField] private GameObject voiceBubble_;
    [SerializeField] private Animator anim_;

    private bool canReceiveAction_ = true;

    private bool firstWave_ = true;

    public float Talk(List<string> audio, string root)
    {
        if (!canReceiveAction_) return -1f;

        canReceiveAction_ = false;

        float duration = AudioPlayer.PlayAudio(root + audio[Random.Range(0, audio.Count)]);
        voiceBubble_.gameObject.SetActive(true);
        Go.to(transform, duration, new GoTweenConfig().onComplete(t =>
        {
            voiceBubble_.gameObject.SetActive(false);
            canReceiveAction_ = true;
        }));

        return duration;
    }

    public void Cheer(List<string> audio, string root)
    {
        if (Talk(audio, root) > 0f)
        {
            anim_.Play("didi-cheer");
        }
    }

    public float TalkAndPrint(List<string> audio, string print, string root)
    {
        float duration = Talk(audio, root);
        if (duration == -1f) return -1f;

        Go.to(transform, 0.01f, new GoTweenConfig().onComplete(t =>
        {
            anim_.Play("didi-phone");
        }));
        Go.to(transform, 1.8f, new GoTweenConfig().onComplete(t =>
        {
            AudioPlayer.PlayAudio("audio/sfx/beep");
        }));
        Go.to(transform, 2.0f, new GoTweenConfig().onComplete(t =>
        {
            CommandLineHelper.PrintPdf(print);
        }));

        return duration;
    }

    public void Wave()
    {
        anim_.Play("didi-wave");
        AudioPlayer.PlayAudio("audio/sfx/wave");

        if (firstWave_)
        {
            firstWave_ = false;
            return;
        }

        if (Random.Range(1, 100) > 40)
        {
            Go.to(transform, 0.3f, new GoTweenConfig().onComplete(t =>
            {
                List<string> hiFiles = new List<string> { "hi-a", "hi-b", "hi-b", "hi-e", "hi-e,", "hi-h", "hi-h", "hi-h", "hi-i", "hi-j", "hi-k", "hi-l", "hi-m", "hi-n", "hi-o", "hi-p", "hi-p", "hi-n", "hi-j", "hi-k" };
                AudioPlayer.PlayAudio("audio/didi_hi/" + hiFiles[Random.Range(0, hiFiles.Count)]);
            }));
        }
    }
}
