using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    private string cachedAction_ = "";

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

    public void SendNewAction(string a)
    {
        if (networkManager_ != null)
        {
            networkManager_.SendNewEpisodeNodeMessage(ACTION_PREFIX + a);
        }
    }

    public void NewEpisodeEvent(string e)
    {
        cachedEpisode_ = e;
        cachedNode_ = "";
        cachedState_ = "";
        cachedAction_ = "";

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
            cachedAction_ = "";

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

            //currently not checking for cached action so that action can repeat
            string strippedActions = StripAndRunActions(command);
            NewActionInternal(strippedActions);

            cachedAction_ = command;
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

    private string StripAndRunActions(string action)
    {
        const string kRunCommand = "-run";

        string ret = action;

        while (ret.IndexOf(kRunCommand) != -1)
        {
            int runStart = ret.IndexOf(kRunCommand);
            int firstQuote = ret.IndexOf('\"', runStart);
            if (firstQuote == -1)
            {
                Debug.LogWarning("No matching action for argument -run: " + action);
                return ret;
            }
            int secondQuote = ret.IndexOf('\"', firstQuote + 1);
            if (secondQuote == -1)
            {
                Debug.LogWarning("Did not close quotations for " + action);
                return ret;
            }

            bool isPromptManager = this.GetComponent<PrompterManager>() == null ? false : true;

            NewActionInternal(ret.Substring(firstQuote + 1, secondQuote - firstQuote - 1));
            ret = ret.Remove(runStart, secondQuote - runStart + 1);
        }
        return ret;
    }
}
