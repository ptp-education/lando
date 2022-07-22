using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnscreenCharacter : MonoBehaviour
{
    [SerializeField] private GameObject thoughtBubble_;

    private static string kSharedVoRoot = "audio/shared_vo/";

    private GameManager gameManager_;

    private float changeVolumeTiming = 0f;

    public void Init(GameManager gm)
    {
        gameManager_ = gm;
    }

    protected virtual void HandleSpeaking(bool isSpeaking)
    {
        if (thoughtBubble_ != null)
        {
            thoughtBubble_.gameObject.SetActive(isSpeaking);
        }
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

        //Go.to(transform, duration, new GoTweenConfig().onComplete(t =>
        //{
        //    voiceBubble_.gameObject.SetActive(false);
        //}));

        return duration;
    }

    private void Update() 
    {
        if (AudioPlayer.GetAudioSourcePlaying() != null)
        {
            float[] spectrum = new float[64];
            AudioPlayer.GetAudioSourcePlaying().GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);
           
            if (spectrum[10] - 10 <= -9.999999)
            {
                changeVolumeTiming = 0;
            }
            else 
            {
                changeVolumeTiming += Time.deltaTime;
            }
            //This value can be changed to reactivate the bubble
            HandleSpeaking(changeVolumeTiming >= 0.15f);
        }
    }

    public void DelayedTalk(string delay, List<string> audio, string root)
    {
        float d = float.Parse(delay);
        Go.to(transform, d, new GoTweenConfig().onComplete(t =>
        {
            Talk(audio, root);
        }));
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
}
