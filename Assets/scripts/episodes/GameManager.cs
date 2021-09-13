using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;
using Photon;

public class GameManager : MonoBehaviour
{
    public class NodeState
    {
        public const string Playing = "playing";
        public const string Looping = "looping";
    }

    public enum Type
    {
        Prompter,
        Sharer
    }

    protected Episode episode_;
    protected EpisodeNode currentNode_;
    protected string currentNodeState_;

    private string cachedEpisode_ = "";
    private string cachedNode_ = "";
    private string cachedState_ = "";

    private NetworkManager networkManager_;

    public void Init(NetworkManager nm)
    {
        networkManager_ = nm;
    }

    public void UpdateEpisode(string e)
    {
        networkManager_.SendNewEpisodeMessage(e);
    }

    public void UpdateEpisodeNode(string n)
    {
        networkManager_.SendNewEpisodeNodeMessage(n);
    }

    public void UpdateNodeState(string s)
    {
        networkManager_.SendNewNodeStateMessage(s);
    }

    public void NewEpisodeEvent(string e)
    {
        if (string.Equals(e, cachedEpisode_))
            return;

        cachedEpisode_ = e;
        cachedNode_ = "";
        cachedState_ = "";

        if (episode_ != null)
        {
            Destroy(episode_.gameObject);
        }
        Episode o = Resources.Load<Episode>("prefabs/episodes/" + e);
        episode_ = Instantiate<Episode>(o);

        NewEpisodeEventInternal(episode_);
    }

    protected virtual void NewEpisodeEventInternal(Episode e)
    {
        episode_ = e;
        UpdateEpisodeNode(e.StartingNode.gameObject.name);
    }

    public void NewNodeEvent(string n)
    {
        if (string.Equals(n, cachedNode_))
            return;

        cachedNode_ = n;
        cachedState_ = "";

        foreach(EpisodeNode node in episode_.AllNodes)
        {
            if (string.Equals(n, node.gameObject.name))
            {
                NewNodeEventInternal(node);
                break;
            }
        }
    }

    protected virtual void NewNodeEventInternal(EpisodeNode n)
    {
        currentNode_ = n;
        //stub
    }

    public void NewStateEvent(string s)
    {
        if (string.Equals(s, cachedState_))
            return;

        cachedState_ = s;

        NewStateEventInternal(s);
    }

    protected virtual void NewStateEventInternal(string s)
    {
        currentNodeState_ = s;
        //stub
    }
}
