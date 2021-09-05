using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum EpisodeState
    {
        Playing,
        Looping
    }

    public enum Type
    {
        Prompter,
        Sharer
    }

    protected EpisodeState currentState_;
    protected Episode episode_;
    protected EpisodeNode currentNode_;

    public void LoadEpisode(string e)
    {
        if (episode_ != null)
        {
            Destroy(episode_.gameObject);
        }
        Episode o = Resources.Load<Episode>("prefabs/episodes/" + e);
        episode_ = Instantiate<Episode>(o);

        PlayNewNode(episode_.StartingNode);
    }

    public virtual void ReceiveAction(string a)
    {
        //stub
    }

    public virtual void PlayNewNode(EpisodeNode n)
    {
        currentNode_ = n;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
