using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoEpisodeManager : GameManager
{
    [SerializeField] private VideoPlayer videoPlayer_;

    private string internalState_ = "";

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer_.loopPointReached += VideoFinished;
    }

    protected override void NewNodeEventInternal(EpisodeNode node)
    {
        base.NewNodeEventInternal(node);

        videoPlayer_.url = System.IO.Path.Combine(Application.streamingAssetsPath, node.VideoFilePath);
        videoPlayer_.isLooping = false;
        videoPlayer_.Play();

        internalState_ = NodeState.Playing;
        UpdateNodeState(NodeState.Playing);
    }

    private void VideoFinished(VideoPlayer vp)
    {
        if (internalState_.Equals(NodeState.Playing))
        {
            videoPlayer_.url = System.IO.Path.Combine(Application.streamingAssetsPath, currentNode_.VideoLoopFilePath);
            videoPlayer_.isLooping = true;
            videoPlayer_.Play();

            internalState_ = NodeState.Looping;
            UpdateNodeState(NodeState.Looping);
        }
    }
}
