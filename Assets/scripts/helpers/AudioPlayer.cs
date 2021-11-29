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

    public static float PlayAudio(string path)
    {
        AudioSource a = Resources.Load<AudioSource>("prefabs/episode_objects/audio_player");
        AudioSource audioSource = GameObject.Instantiate(a);

        AudioClip clip = Resources.Load<AudioClip>(path);
        audioSource.PlayOneShot(clip);

        Go.to(audioSource.transform, clip.length, new GoTweenConfig().onComplete(t =>
        {
            Destroy(audioSource.gameObject);
        }));

        return clip.length;
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

        loopingSources_[layer] = new KeyValuePair<string, AudioSource>(path, audioSource);

        if (loopingSources_.ContainsKey(layer) && loopingSources_[layer].Value != null)
        {
            audioSource.volume = 0f;

            AudioSource previousClip = loopingSources_[layer].Value;

            GoTweenFlow f = new GoTweenFlow();
            f.insert(0f, new GoTween(Instance, 0.2f, new GoTweenConfig().floatProp("volume", 0f)));
            f.insert(0.1f, new GoTween(Instance, 0.2f, new GoTweenConfig().floatProp("volume", 1f)));
            f.insert(1f, new GoTween(Instance, 0.01f, new GoTweenConfig().onComplete(t =>
            {
                Destroy(previousClip.gameObject);
            })));

            f.play();
        }
        return audioSource;
    }

    public static void StopLoop(int track)
    {
        if (!loopingSources_.ContainsKey(track)) return;

        AudioSource s = loopingSources_[track].Value;

        if (s != null)
        {
            Go.to(Instance, 0.2f, new GoTweenConfig().floatProp("volume", 0f).onComplete(t =>
            {
                Destroy(s.gameObject);
            }));
        }
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
        } else if (sf.FileNames.Length < 2)
        {
            nextSong = sf.FileNames[0];
        } else
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

    public static float AudioLength(string path)
    {
        AudioClip clip = Resources.Load<AudioClip>(path);
        return clip.length;
    }
}