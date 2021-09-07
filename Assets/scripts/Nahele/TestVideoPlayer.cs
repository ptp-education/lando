using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TestVideoPlayer : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    [SerializeField]
    private string fileToPlay;
    
    void Start()
    {
        AppendAndPlayVideo();
    }

    //We don't need to reference the videos directly in an editor field, we can just get the video name from the field and append it like this     
    private void AppendAndPlayVideo()
    {
        videoPlayer = FindObjectOfType<VideoPlayer>();
        videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, "Dance 1 - Save the Honbees/", fileToPlay);
        videoPlayer.Play();
    }
}
