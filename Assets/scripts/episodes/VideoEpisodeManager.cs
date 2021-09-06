using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoEpisodeManager : GameManager
{
    [SerializeField] private VideoPlayer videoPlayer_;

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer_.loopPointReached += VideoFinished;
        //LoadEpisode("test_episode");
    }

    public override void NewNodeEvent(string n)
    {
        base.NewNodeEvent(n);

        videoPlayer_.clip = currentNode_.Video;
        videoPlayer_.isLooping = false;
        videoPlayer_.Play();

        UpdateNodeState(NodeState.Playing);
    }

    private void VideoFinished(VideoPlayer vp)
    {
        if (currentNodeState_ == NodeState.Playing)
        {
            videoPlayer_.clip = currentNode_.VideoLoop;
            videoPlayer_.isLooping = true;
            videoPlayer_.Play();

            UpdateNodeState(NodeState.Looping);
        }
    }
}
