using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAudioPlayer : MonoBehaviour
{
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

    public static float AudioLength(string path)
    {
        AudioClip clip = Resources.Load<AudioClip>(path);
        return clip.length;
    }
}