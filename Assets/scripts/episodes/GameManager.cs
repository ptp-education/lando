using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public const string ACTION_PREFIX = "action:";
    public const string NODE_PREFIX = "node:";
    public const string RADIO_COMMAND = "radio";
    public const string FADE_COMMAND = "-fade";
    public const string FADEOUT_COMMAND = "-fadeout";
    public const string ZONE_ACTIVE = "-zoneactive";
    public const string TERMINAL_COMMAND = "-terminal";
    public const string PRINT_COMMAND = "-print";
    public const string CHARACTER_COMMAND = "-character";
    public const string SPAWN_OPTIONS_COMMAND = "-spawnoption";

    public enum Type
    {
        Prompter,
        Sharer
    }

    public static bool PromptActive = false;
    public static string SelectedCharacter;
    public static bool MuteAll = false;
    public static bool Master = true;

    protected Episode episode_;
    protected EpisodeNode currentNode_;
    protected string currentNodeState_;

    public GameStorage Storage = new GameStorage();

    private string cachedNode_ = "";

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

    public void SendNewAction(string a, bool masterOnly = true)
    {
        if (masterOnly && !GameManager.Master)
        {
            return;
        }
        if (networkManager_ != null)
        {
            networkManager_.SendNewEpisodeNodeMessage(ACTION_PREFIX + a);
        }
    }

    public void NewEpisodeEvent(string e)
    {
        cachedNode_ = "";

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
            strippedActions = StripAndRunSfxActions(command);
            NewActionInternal(strippedActions);
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

            NewActionInternal(ret.Substring(firstQuote + 1, secondQuote - firstQuote - 1));
            ret = ret.Remove(runStart, secondQuote - runStart + 1);
        }
        return ret;
    }

    //TODO: condense logic
    private string StripAndRunSfxActions(string action)
    {
        const string kRunCommand = "-sfx";

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

            AudioPlayer.PlayAudio(ret.Substring(firstQuote + 1, secondQuote - firstQuote - 1));

            ret = ret.Remove(runStart, secondQuote - runStart + 1);
        }
        return ret;
    }
}
