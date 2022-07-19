using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public const int kMain = 1;
    public const int kRadio = 2;
    private static Dictionary<int, KeyValuePair<string,AudioSource>> loopingSources_ = new Dictionary<int, KeyValuePair<string, AudioSource>>();

    private static GoTweenFlow radioFlow_;
    private static AudioPlayer instance_;

    private static AudioSource voiceoverAudio_;
    private static AudioSource newAudioSource_;

    private static string kSharedVoRoot = "audio/shared_vo/";

    private static AudioPlayer Instance
    {
        get
        {
            if (instance_ == null)
            {
                GameObject o = new GameObject();
                o.name = "AudioPlayer";
                instance_ = o.AddComponent(typeof(AudioPlayer)) as AudioPlayer;
            }
            return instance_;
        }
    }

    #region CORE_FUNCTIONS

    public static float PlayAudio(string path, bool expectFailure = false, bool useVoiceover = false)
    {
        if (GameManager.MuteAll)
        {
            return -1f;
        }

        AudioSource audioSource = null;
        AudioSource a = Resources.Load<AudioSource>("prefabs/episode_objects/audio_player");
        audioSource = GameObject.Instantiate(a);

        if (useVoiceover)
        {
            if (voiceoverAudio_ != null)
            {
                if (voiceoverAudio_.isPlaying)
                {
                    voiceoverAudio_.Stop();
                }
            }
            voiceoverAudio_ = GameObject.Instantiate(Resources.Load<AudioSource>("prefabs/episode_objects/audio_player"));
            audioSource = voiceoverAudio_;
        }

        AudioClip clip = Resources.Load<AudioClip>(path);

        if (clip == null)
        {
            if (!expectFailure)
            {
                Debug.LogWarning("Could not find audio file for path: " + path);
            }
            return -1f;
        }

        audioSource.PlayOneShot(clip);
        Debug.LogWarning(path);
        if (path.Contains("audio/lego")) { 
            newAudioSource_ = audioSource;
        }
        Go.to(audioSource.transform, clip.length * 2f, new GoTweenConfig().onComplete(t =>
        {
            Destroy(audioSource.gameObject);
        }));

        return clip.length;
    }

    public static AudioSource GetAudioSourcePlaying() {
        return newAudioSource_;
    }

    public static AudioSource LoopAudio(string path, int layer = kMain)
    {
        if (loopingSources_.ContainsKey(layer)
            && string.Equals(loopingSources_[layer].Key, path)
            && loopingSources_[layer].Value != null
            && loopingSources_[layer].Value.isPlaying)
        {
            //we're already playing this loop
            return loopingSources_[layer].Value;
        }

        AudioSource a = Resources.Load<AudioSource>("prefabs/episode_objects/audio_player");
        AudioSource audioSource = GameObject.Instantiate(a);

        audioSource.clip = Resources.Load<AudioClip>(path);
        audioSource.Play();
        audioSource.loop = true;
        audioSource.volume = 1f;

        if (loopingSources_.ContainsKey(layer) && loopingSources_[layer].Value != null)
        {
            audioSource.volume = 0f;

            AudioSource previousSource = loopingSources_[layer].Value;

            GoTweenFlow f = new GoTweenFlow();
            f.insert(0f, new GoTween(previousSource, 0.2f, new GoTweenConfig().floatProp("volume", 0f)));
            f.insert(0.1f, new GoTween(audioSource, 0.2f, new GoTweenConfig().floatProp("volume", 1f)));
            f.insert(0f, new GoTween(Instance, 1f, new GoTweenConfig().onComplete(t =>
            {
                Destroy(previousSource.gameObject);
            })));

            f.play();
        }

        loopingSources_[layer] = new KeyValuePair<string, AudioSource>(path, audioSource);

        return audioSource;
    }

    public static void StopLoop(int track)
    {
        if (!loopingSources_.ContainsKey(track)) return;

        AudioSource s = loopingSources_[track].Value;

        if (s != null)
        {
            Go.to(s, 0.2f, new GoTweenConfig().floatProp("volume", 0f));
            Go.to(Instance, 0.25f, new GoTweenConfig().onComplete(t =>
            {
                Destroy(s.gameObject);
            }));
        }
    }

    public static float AudioLength(string path)
    {
        AudioClip clip = Resources.Load<AudioClip>(path);

        if (clip == null)
        {
            return -1;
        }

        return clip.length;
    }

    public static float AudioLength(string path, string root)
    {
        float duration = AudioLength(root + path);

        if (duration == -1f)
        {
            duration = AudioLength(kSharedVoRoot + root);
        }

        return duration;
    }

    #endregion

    public static float PlayPrint()
    {
        List<string> files = new List<string>();
        for (int i = 1; i <= 10; i++)
        {
            files.Add("hints-print-" + i.ToString());
        }

        return PlayVoiceover(files, "audio/shared_vo/");
    }

    public static float PlayCheer()
    {
        List<string> cheers = new List<string>()
        {
            "airhorn",
            "applausetrumpet",
            "cheer"
        };

        return PlaySfx(cheers);
    }

    public static float PlaySfx(string file)
    {
        return AudioPlayer.PlayAudio("audio/sfx/" + file);
    }

    public static float PlaySfx(List<string> files)
    {
        string file = files[Random.Range(0, files.Count)];
        return AudioPlayer.PlaySfx(file);
    }

    public static float PlayVoiceover(string path, string root)
    {
        return PlayVoiceover(new List<string>() { path }, root);
    }

    public static float PlayVoiceover(List<string> paths, string root)
    {
        string vo = paths[Random.Range(0, paths.Count)];

        float duration = AudioPlayer.PlayAudio(root + vo, expectFailure: true, useVoiceover: true);

        if (duration == -1f)
        {
            duration = AudioPlayer.PlayAudio(kSharedVoRoot + vo, useVoiceover: true);
        }

        return duration;
    }

    public static void StartRadio()
    {
        StopRadio();

        TextAsset fileNamesAsset = Resources.Load<TextAsset>("all_songs");
        StringsFile sf = JsonUtility.FromJson<StringsFile>(fileNamesAsset.text);

        string nextSong = null;

        if (sf.FileNames.Length == 0)
        {
            return;
        }
        else if (sf.FileNames.Length < 2)
        {
            nextSong = sf.FileNames[0];
        }
        else
        {
            string currentlyPlaying = loopingSources_.ContainsKey(kRadio) ? loopingSources_[kRadio].Key : null;
            nextSong = currentlyPlaying;
            while (string.Equals(nextSong, currentlyPlaying))
            {
                System.Random rnd = new System.Random();
                nextSong = sf.FileNames[rnd.Next(sf.FileNames.Length)];
            }
        }

        LoopAudio(nextSong, kRadio);

        radioFlow_ = new GoTweenFlow();
        radioFlow_.insert(0f, new GoTween(Instance, AudioLength(nextSong), new GoTweenConfig().onComplete(t =>
        {
            StartRadio();
        })));

        radioFlow_.play();
    }

    public static void StopRadio()
    {
        if (radioFlow_ != null)
        {
            radioFlow_.destroy();
            radioFlow_ = null;
        }
        StopLoop(kRadio);
    }
}