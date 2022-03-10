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
    public const string TOGGLE_BLOCKS = "-toggleblocks";
    public const string TOGGLE_CHALLENGE = "-togglechallenge";
    public const string HIDE_ALL = "-hideall";
    public const string SILENCE_COUNTER = "-add-silence-life";
    public const string RESET_SILENCE_COUNTER = "-reset-silence-life";
    public const string DIDI_LIGHT = "-didi-light";
    public const string TERMINAL_COMMAND = "-terminal";
    public const string PRINT_COMMAND = "-print";
    public const string CHARACTER_COMMAND = "-character";
    public const string SPAWN_OPTIONS_COMMAND = "-spawnoption";
    public const string RFID_COMMAND = "-rfid-ui";
    public const string HIDE_SPAWN_OPTIONS_COMMAND = "-hidespawnoption";

    public const string BLOCKS_RED = "-blocks-red";
    public const string BLOCKS_GREEN = "-blocks-green";
    public const string BLOCKS_WHITE = "-blocks-white";
    public const string HINTS_RED = "-hints-red";
    public const string HINTS_GREEN = "-hints-green";
    public const string HINTS_WHITE = "-hints-white";
    public const string CHALLENGE_RED = "-challenge-red";
    public const string CHALLENGE_GREEN = "-challenge-green";
    public const string CHALLENGE_WHITE = "-challenge-white";
    public const string LIGHTS_WHITE = "-lights-off";

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

    public ChallengeData ChallengeData
    {
        get
        {
            return episode_ != null ? episode_.ChallengeData : null;
        }
    }

    public GameStorage Storage = new GameStorage();
    public Dictionary<string, GameStorage> RfidStorage = new Dictionary<string, GameStorage>();

    private string cachedNode_ = "";

    private NetworkManager networkManager_;

    public void Init(NetworkManager nm)
    {
        networkManager_ = nm;
    }

    public GameStorage GameStorageForRfid(string rfidId)
    {
        if (RfidStorage.ContainsKey(rfidId))
        {
            return RfidStorage[rfidId];
        }
        RfidStorage[rfidId] = new GameStorage();
        return RfidStorage[rfidId];
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
