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

    public virtual void NewEpisodeEvent(string e)
    {
        if (episode_ != null)
        {
            Destroy(episode_.gameObject);
        }
        Episode o = Resources.Load<Episode>("prefabs/episodes/" + e);
        episode_ = Instantiate<Episode>(o);

        UpdateEpisodeNode(episode_.StartingNode.gameObject.name);
    }

    public virtual void NewNodeEvent(string n)
    {
        foreach(EpisodeNode node in episode_.AllNodes)
        {
            if (n.Equals(node.gameObject.name))
            {
                currentNode_ = node;
                break;
            }
        }
    }

    public virtual void NewStateEvent(string s)
    {
        currentNodeState_ = s;
        //stub
    }

}
