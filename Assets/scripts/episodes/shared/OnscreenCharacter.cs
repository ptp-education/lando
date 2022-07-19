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

    private bool firstWave_ = true;
    private GoTweenFlow walkFlow_;

    private GameManager gameManager_;

    public void Init(GameManager gm)
    {
        gameManager_ = gm;
    }

    public float Talk(string audio, string root)
    {
        return Talk(new List<string>() { audio }, root);
    }

    public float Talk(List<string> audio, string root)
    {
        string vo = audio[Random.Range(0, audio.Count)];

        float duration = AudioPlayer.PlayVoiceover(vo, root);

        if (duration == -1f)
        {
            duration = AudioPlayer.PlayVoiceover(vo, kSharedVoRoot);
        }

        if (duration == -1f)
        {
            //did not find audio file in shared or episode root
            return duration;
        }

        voiceBubble_.gameObject.SetActive(true);
        //Go.to(transform, duration, new GoTweenConfig().onComplete(t =>
        //{
        //    voiceBubble_.gameObject.SetActive(false);
        //}));

        return duration;
    }

    private void Update()
    {

        //AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);
        if (AudioPlayer.GetAudioSourcePlaying() != null) 
        {
            float[] spectrum = new float[512];
            AudioPlayer.GetAudioSourcePlaying().GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

                Debug.LogWarning(spectrum[0] - 10);
            if (spectrum[0] - 10 <= -9.999974)
            {
                voiceBubble_.gameObject.SetActive(false);
            }
            else
            {
                voiceBubble_.gameObject.SetActive(true);
            }
            for (int i = 1; i < spectrum.Length - 1; i++)
            {
                Debug.DrawLine(new Vector3(Mathf.Log(i - 1), spectrum[i - 1] - 10, 1), new Vector3(Mathf.Log(i), spectrum[i] - 10, 1), Color.green);
                
            }
        }
    }

    public void ScanCard()
    {
        List<string> files = new List<string>();
        for (int i = 1; i <= 12; i++)
        {
            files.Add("testing-scan-card-" + i.ToString());
        }
        Talk(files, kSharedVoRoot);
    }

    public void OutOfHints()
    {
        List<string> files = new List<string>();
        for (int i = 1; i <= 6; i++)
        {
            files.Add("hints-out-" + i.ToString());
        }
        Talk(files, kSharedVoRoot);
    }

    public void Exclaim()
    {
        List<string> files = new List<string>();
        for (int i = 1; i <= 24; i++)
        {
            files.Add("testing-exclaim-" + i.ToString());
        }

        anim_.Play("didi-cheer");
        Talk(files, kSharedVoRoot);
    }

    public void PromptHint()
    {
        List<string> files = new List<string>();
        for (int i = 1; i <= 9; i++)
        {
            files.Add("hint-prompt-" + i.ToString());
        }
        Talk(files, kSharedVoRoot);
    }

    public void SuggestPrinter()
    {
        List<string> files = new List<string>();
        for (int i = 1; i <= 11; i++)
        {
            files.Add("testing-offer-hint-" + i.ToString());
        }

        float duration = Talk(files, kSharedVoRoot);

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
        float duration = ProgressionTalk(audio[0], root);
        if (duration == -1f) return -1f;

        GoTweenFlow flow = new GoTweenFlow();

        flow.insert(duration + 0.2f, new GoTween(transform, 0.2f, new GoTweenConfig().onComplete(t =>
        {
            List<string> files = new List<string>();
            for (int i = 1; i <= 10; i++)
            {
                files.Add("hints-print-" + i.ToString());
            }
            Talk(files, kSharedVoRoot);
        })));

        flow.insert(duration + 1f, new GoTween(transform, 0.4f, new GoTweenConfig().onComplete(t =>
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

    public float ProgressionTalk(string audio, string root)
    {
        List<string> previous = gameManager_.Storage.GetValue<List<string>>(GameStorage.Key.HintSpeechProgression);
        if (previous == null)
        {
            previous = new List<string>();
        }

        List<string> options = new List<string> { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
        for (int i = 0; i < options.Count; i++)
        {
            string play = audio + "-" + options[i];
            if (previous.Contains(play)) continue;

            float duration = Talk(play, root);
            if (duration > 0f)
            {
                //if playing was successful, play and add to storage
                previous.Add(play);
                gameManager_.Storage.Add<List<string>>(GameStorage.Key.HintSpeechProgression, previous);
                return duration;
            } else if (i > 0)
            {
                //playing was unsuccessful, so play the previous version we know works
                return Talk(audio + "-" + options[i - 1], root);
            } else
            {
                //we weren't able to find a file with a suffix, so let's try playing the original
                return Talk(audio, root);
            }
        }
        return -1f;
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
