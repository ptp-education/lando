using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public const string ACTION_PREFIX = "action:";
    public const string NODE_PREFIX = "node:";
    public const string RADIO_COMMAND = "radio";
    public const string NODE_COMMAND = "-node";
    public const string FADEOUT_COMMAND = "-fadeout";
    public const string FADEIN_COMMAND = "-fadein";
    public const string HIDE_ALL = "-hideall";
    public const string DIDI_HMMM = "-didi-hmm";
    public const string TERMINAL_COMMAND = "-terminal";
    public const string PRINT_COMMAND = "-print";
    public const string CHARACTER_COMMAND = "-character";
    public const string RFID_COMMAND = "-rfid";
    public const string HIDE_CHOICES = "-hide-choices";
    public const string SHOW_CHOICES = "-show-choices";
    public const string OPTION_SELECT = "-option-select";
    public const string CLAIM_REWARD = "-claim-reward";

    [SerializeField] protected bool PlaySounds = false;

    public static bool MuteAll = false;

    protected Episode episode_;
    protected EpisodeNode currentNode_;
    protected string currentNodeState_;

    public LevelData ChallengeData
    {
        get
        {
            return episode_ != null ? episode_.LevelData : null;
        }
    }

    public string VoRoot
    {
        get
        {
            if (episode_ != null)
            {
                return episode_.VORoot;
            }
            return null;
        }
    }

    public static GameStorage Storage = new GameStorage();

    private Dictionary<string, GameStorage> rfidStorage_;

    public Dictionary<string, GameStorage> RfidStorage
    {
        get
        {
            if (rfidStorage_ == null)
            {
                rfidStorage_ = new Dictionary<string, GameStorage>();
            }
            return rfidStorage_;
        }
    }

    private string cachedNode_ = "";

    protected NetworkManager networkManager_;

    public virtual void Init(NetworkManager nm)
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

    public virtual void SendNewActionInternal(string a)
    {
        NewNodeAction(ACTION_PREFIX + a);
    }

    public void SendNewActionNetworked(string a)
    {
        if (networkManager_ != null)
        {
            networkManager_.SendNewEpisodeNodeMessage(ACTION_PREFIX + a);
        }
    }

    public void LoadNewNode(string nodeName)
    {
        if (networkManager_ != null)
        {
            networkManager_.SendNewEpisodeNodeMessage(NODE_PREFIX + nodeName);
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

        LoadNewNode(e.StartingNode.gameObject.name);

        AudioPlayer.StopRadio();

        Storage = new GameStorage();
        rfidStorage_ = null;
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
            strippedActions = strippedActions.Trim();
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

    #region USER_DATA

    public GameStorage GameStorageForUserId(string id)
    {
        if (RfidStorage.ContainsKey(id))
        {
            return RfidStorage[id];
        }
        RfidStorage[id] = new GameStorage();
        return RfidStorage[id];
    }

    public GameStorage.UserData UserDataForUserId(string id)
    {
        return GameStorageForUserId(id).GetUserData();
    }

    public void SaveUserData(GameStorage.UserData data, string id)
    {
        GameStorageForUserId(id).SaveUserData(data);
    }

    public void UsedHint(string id, string hint)
    {
        GameStorage.UserData userData = UserDataForUserId(id);

        if (!userData.RedeemedHints.Contains(hint))
        {
            userData.RedeemedHints.Add(hint);
        }
        SaveUserData(userData, id);
    }

    public List<LevelData.Hint> AllHintsForUserId(string id)
    {
        List<LevelData.Challenge> completedChallenges = new List<LevelData.Challenge>();

        foreach(string c in UserDataForUserId(id).CompletedChallenges)
        {
            LevelData.Challenge challenge = FindChallenge(c);
            if (challenge != null)
            {
                completedChallenges.Add(challenge);
            }
        }

        //return hints with last unlocked first
        completedChallenges.Reverse();

        List<LevelData.Hint> ret = new List<LevelData.Hint>();
        foreach(LevelData.Challenge c in completedChallenges)
        {
            foreach(string h in c.HintRewards)
            {
                LevelData.Hint hint = FindHint(h);
                if (hint != null)
                {
                    ret.Add(hint);
                }
            }
        }

        foreach(string h in ChallengeData.StartingHints)
        {
            LevelData.Hint hint = FindHint(h);
            if (hint != null)
            {
                ret.Add(hint);
            }
        }

        return ret;
    }

    public List<GameStorage.ResourceType> AllResourcesForUserId(string id)
    {
        List<LevelData.Challenge> completedChallenges = new List<LevelData.Challenge>();

        foreach (string c in UserDataForUserId(id).CompletedChallenges)
        {
            LevelData.Challenge challenge = FindChallenge(c);
            if (challenge != null)
            {
                completedChallenges.Add(challenge);
            }
        }

        List<GameStorage.ResourceType> ret = new List<GameStorage.ResourceType>();
        foreach (LevelData.Challenge c in completedChallenges)
        {
            ret.AddRange(c.ResourceRewards);
        }

        ret.AddRange(ChallengeData.StartingResources);

        return ret;
    }

    public LevelData.Challenge NextChallengeForUserId(string id)
    {
        GameStorage.UserData userData = UserDataForUserId(id);
        for (int i = 0; i < ChallengeData.Challenges.Count; i++)
        {
            LevelData.Challenge c = ChallengeData.Challenges[i];
            if (string.Equals(c.Name, CurrentChallengeForUserId(id).Name))
            {
                if (i + 1 < ChallengeData.Challenges.Count)
                {
                    return ChallengeData.Challenges[i + 1];
                }
                else
                {
                    return null;
                }
            }
        }
        return null;
    }

    public LevelData.Challenge NextChallengeWithResourcesForUserId(string id)
    {
        GameStorage.UserData userData = UserDataForUserId(id);

        foreach (LevelData.Challenge c in ChallengeData.Challenges)
        {
            if (!userData.CompletedChallenges.Contains(c.Name))
            {
                if (c.ResourceRewards.Count > 0)
                {
                    return c;
                }
            }
        }

        return null;
    }

    public LevelData.Challenge NextChallengeForCurrentChallenge(string challenge)
    {
        for (int i = 0; i < ChallengeData.Challenges.Count; i++)
        {
            LevelData.Challenge currentChallenge = ChallengeData.Challenges[i];
            if (string.Equals(currentChallenge.Name, challenge))
            {
                if (i < ChallengeData.Challenges.Count - 1)
                {
                    return ChallengeData.Challenges[i + 1];
                }
            }
        }

        return null;
    }

    public LevelData.Challenge FindChallenge(string name)
    {
        foreach (LevelData.Challenge c in ChallengeData.Challenges)
        {
            if (string.Equals(c.Name, name))
            {
                return c;
            }
        }
        return null;
    }

    public LevelData.Hint FindHint(string name)
    {
        foreach(LevelData.Hint h in ChallengeData.Hints)
        {
            if (string.Equals(h.Name, name))
            {
                return h;
            }
        }
        return null;
    }

    public LevelData.Challenge CurrentChallengeForUserId(string id)
    {
        GameStorage.UserData userData = GameStorageForUserId(id).GetUserData();

        if (userData.CurrentChallenge == null || userData.CurrentChallenge.Length == 0)
        {
            userData.CurrentChallenge = ChallengeData.Challenges[0].Name;
            GameStorageForUserId(id).SaveUserData(userData);
        }
        return FindChallenge(userData.CurrentChallenge);
    }

    public string ActionForOptionSelect(int option, bool isTeacher)
    {
        if (option < currentNode_.OptionsToSpawn.Count)
        {
            EpisodeNode.Options o = currentNode_.OptionsToSpawn[option];
            if(o.TeacherOnly)
            {
                if (!isTeacher)
                {
                    if (PlaySounds)
                    {
                        AudioPlayer.PlayVoiceover("option-teacher-only", "audio/shared_vo/");
                    }
                    return null;
                } else
                {
                    return o.Command;
                }
            }
            else
            {
                return o.Command;
            }
        }
        return null;
    }

    #endregion
}
