using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnscreenCharacter : MonoBehaviour
{
    [SerializeField] private GameObject voiceBubble_;
    [SerializeField] private GameObject thoughtBubble_;
    [SerializeField] private Animator anim_;
    [SerializeField] private float speed_ = 250f;

    private static string kSharedVoRoot = "audio/shared_vo/";

    private bool canReceiveAction_ = true;
    private bool firstWave_ = true;
    private GoTweenFlow walkFlow_;

    public float Talk(List<string> audio, string root)
    {
        if (!canReceiveAction_) return -1f;

        canReceiveAction_ = false;

        string vo = audio[Random.Range(0, audio.Count)];

        float duration = AudioPlayer.PlayAudio(root + vo, expectFailure: true);

        if (duration == -1f)
        {
            duration = AudioPlayer.PlayAudio(kSharedVoRoot + vo);
        }

        voiceBubble_.gameObject.SetActive(true);
        Go.to(transform, duration, new GoTweenConfig().onComplete(t =>
        {
            voiceBubble_.gameObject.SetActive(false);
            canReceiveAction_ = true;
        }));

        return duration;
    }

    public void SuggestPrinter()
    {
        float duration = Talk(new List<string>()
        {
            "testing-offer-hint1",
            "testing-offer-hint2",
            "testing-offer-hint3",
            "testing-offer-hint4",
            "testing-offer-hint5",
            "testing-offer-hint6",
            "testing-offer-hint7",
            "testing-offer-hint8",
            "testing-offer-hint9",
            "testing-offer-hint10"
        }, kSharedVoRoot);

        thoughtBubble_.SetActive(true);

        Go.to(transform, duration, new GoTweenConfig().onComplete(t =>
        {
            thoughtBubble_.gameObject.SetActive(false);
        }));
    }

    public void HideMagicPrinter()
    {
        thoughtBubble_.SetActive(false);
    }

    public void DelayedTalk(string delay, List<string> audio, string root)
    {
        float d = float.Parse(delay);
        Go.to(transform, d, new GoTweenConfig().onComplete(t =>
        {
            Talk(audio, root);
        }));
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

        GoTweenFlow flow = new GoTweenFlow();
        flow.insert(duration, new GoTween(transform, 0.2f, new GoTweenConfig().onComplete(t =>
        {
            Talk(new List<string>()
            {
                "hints-print1",
                "hints-print2",
                "hints-print3",
                "hints-print4",
                "hints-print5",
                "hints-print6",
                "hints-print7",
                "hints-print8",
                "hints-print9",
                "hints-print10",
            }, kSharedVoRoot);
        })));
        flow.insert(duration + 2.5f, new GoTween(transform, 0.4f, new GoTweenConfig().onComplete(t =>
        {
            CommandLineHelper.PrintPdf(print);
        })));
        flow.insert(duration + 5f, new GoTween(transform, 0.4f, new GoTweenConfig().onComplete(t =>
        {
            Debug.Log("finish printing");
        })));
        flow.play();

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

    public void Walk(int xPosition)
    {
        if (walkFlow_ != null)
        {
            walkFlow_.destroy();
            walkFlow_ = null;
        }

        walkFlow_ = new GoTweenFlow();

        anim_.Play("didi-walk");
        float duration = Mathf.Abs(transform.localPosition.x - xPosition) / speed_;

        walkFlow_.insert(0f, new GoTween(transform, duration, new GoTweenConfig().localPosition(new Vector3(xPosition, transform.localPosition.y, transform.localPosition.z)).onComplete(t =>
        {
            anim_.Play("didi-idle");
        })));

        walkFlow_.play();
    }

    public void Phone()
    {
        anim_.Play("didi-onphone");
    }

    public void Idle()
    {
        anim_.Play("didi-idle");

    }
}
