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

    public const string ACTION_PREFIX = "action:";
    public const string NODE_PREFIX = "node:";
    public const string RADIO_COMMAND = "radio";

    public enum Type
    {
        Prompter,
        Sharer
    }

    public static bool PromptActive = false;

    protected Episode episode_;
    protected EpisodeNode currentNode_;
    protected string currentNodeState_;

    public GameStorage Storage = new GameStorage();

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
        if (networkManager_ != null)
        {
            networkManager_.SendNewEpisodeMessage(e);
        }
    }

    public void UpdateEpisodeNode(string n)
    {
        if (networkManager_ != null)
        {
            networkManager_.SendNewEpisodeNodeMessage(n);
        }
    }

    public void UpdateNodeState(string s)
    {
        if (networkManager_ != null)
        {
            networkManager_.SendNewNodeStateMessage(s);
        }
    }

    public void NewEpisodeEvent(string e)
    {
        cachedEpisode_ = e;
        cachedNode_ = "";
        cachedState_ = "";

        Storage.ResetStorage();

        if (episode_ != null)
        {
            Destroy(episode_.gameObject);
        }
        Episode o = Resources.Load<Episode>(e);
        episode_ = Instantiate<Episode>(o);

        NewEpisodeEventInternal(episode_);

        AudioPlayer.StopRadio();
    }

    protected virtual void NewEpisodeEventInternal(Episode e)
    {
        episode_ = e;
        UpdateEpisodeNode(NODE_PREFIX + e.StartingNode.gameObject.name);

        AudioPlayer.StopRadio();
    }

    public void NewNodeAction(string a)
    {
        if (a.Contains(NODE_PREFIX))
        {
            string node = a.Substring(NODE_PREFIX.Length);

            if (string.Equals(node, cachedNode_))
                return;

            cachedNode_ = node;
            cachedState_ = "";

            foreach (EpisodeNode n in episode_.AllNodes)
            {
                if (string.Equals(node, n.gameObject.name))
                {
                    NewNodeEventInternal(n);
                    break;
                }
            }
        } else if (a.Contains(ACTION_PREFIX))
        {
            string command = a.Substring(ACTION_PREFIX.Length);
            NewActionInternal(command);
        }
    }

    protected virtual void NewActionInternal(string a)
    {
        //stub
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
