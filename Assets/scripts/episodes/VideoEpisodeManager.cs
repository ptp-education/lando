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
        LoadEpisode("test_episode");
    }

    public override void PlayNewNode(EpisodeNode n)
    {
        base.PlayNewNode(n);

        videoPlayer_.clip = currentNode_.Video;
        videoPlayer_.isLooping = false;
        videoPlayer_.Play();
        currentState_ = EpisodeState.Playing;
    }

    public override void ReceiveAction(string a)
    {
        base.ReceiveAction(a);

        if (a.Equals("space"))
        {
            if (currentNode_.NextNode != null)
            {
                PlayNewNode(currentNode_.NextNode);
            }
        } else
        {
            int index = -1;
            try
            {
                index = System.Int32.Parse(a);
            }
            catch (System.FormatException e)
            {
                Debug.LogError(string.Format("Error parsing action with string {0} and error {1}", a, e.Message));
            }
            if (index >= 0 && index < currentNode_.Options.Count)
            {
                PlayNewNode(currentNode_.Options[index].Node);
            }
        }
    }

    private void VideoFinished(VideoPlayer vp)
    {
        if (currentState_ == EpisodeState.Playing)
        {
            videoPlayer_.clip = currentNode_.VideoLoop;
            videoPlayer_.isLooping = true;
            videoPlayer_.Play();
            currentState_ = EpisodeState.Looping;
        }
    }
}
