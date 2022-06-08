using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnedChallengeUi : SpawnedObject
{
    [SerializeField] private Transform challengeUi_;
    [SerializeField] private Image challengeImage_;
    [SerializeField] private Transform completedChallengeHolder_;
    [SerializeField] private CompletedChallengeIcon challengeIconPrefab_;
    [SerializeField] private Vector3 challengeHiddenPosition_;
    [SerializeField] private Vector3 challengeShownPosition_;

    private GoTweenFlow challengeFlow_;
    private bool allowMultipleEntry_ = false;
    private string lastRfid_;
    private LevelData.Challenge groupChallenge_;
    private LevelData.Challenge lastRunChallenge_;

    private LevelData ChallengeData
    {
        get
        {
            return gameManager_.ChallengeData;
        }
    }

    private void Start()
    {
        ShareManager sm = (ShareManager)gameManager_;
        if (sm != null)
        {
            transform.SetParent(sm.OverlayParent);
        }
    }

    public override void ReceivedAction(string action)
    {
        base.ReceivedAction(action);

        List<string> args = ArgumentHelper.ArgumentsFromCommand("-rfid", action);
        if (args.Count > 0)
        {
            switch(args[0])
            {
                case "new-id":
                    if (args.Count > 1)
                    {
                        HandleNewRfid(args[1]);
                    }
                    break;
                case "refresh":
                    HandleRefreshChallengeUi(lastRfid_);
                    HandleScanCommand(lastRfid_);
                    break;
                case "fail":
                    HandleFail(lastRfid_);
                    break;
                case "hide":
                    Hide();
                    break;
                case "encourage":
                    //encourage with either default or something specific (set in challenge data)
                    HandleEncourage(lastRfid_);
                    break;
                case "requirement":
                    //read out requirement of the build
                    HandleAnnounceRequirements(lastRfid_);
                    break;
                case "reward":
                    //run command that gives reward
                    //increment progress
                    HandleReward(lastRfid_);
                    break;
                case "next-challenge":
                    //announce next challenge, can be customized
                    HandleAnnounceNextChallenge(lastRfid_);
                    break;
                case "group-allow-multiple-rewards":
                    //turn on internal state to allow multiple rewards
                    HandleToggleMultipleEntry(true);
                    break;
                case "group-disallow-multiple-rewards":
                    //turn off internal state to allow multiple rewards
                    HandleToggleMultipleEntry(false);
                    break;
            }
        }
    }

    public override void Hide()
    {
        base.Hide();
        if (challengeFlow_ != null)
        {
            challengeFlow_.complete();
        }
        if (challengeUi_.transform.localPosition == challengeShownPosition_)
        {
            AudioPlayer.PlayAudio("audio/sfx/small-whoosh");
            challengeFlow_ = new GoTweenFlow();
            challengeFlow_.insert(0f, new GoTween(challengeUi_, 0.6f, new GoTweenConfig().localPosition(challengeHiddenPosition_).setEaseType(GoEaseType.SineOut)));
            challengeFlow_.play();
        }
    }

    private void HandleNewRfid(string rfid)
    {
        //store latest rfid
        //if single, then animate and show current challenge
        //if allow multiple rewards, go to reward
        lastRfid_ = rfid;

        if (!allowMultipleEntry_)
        {
            ShowChallengeUi(rfid);
            HandleScanCommand(rfid);
        } else
        {
            HandleReward(rfid, groupChallenge_);
            HandleRefreshChallengeUi(rfid);
        }
    }

    private void ShowChallengeUi(string rfid)
    {
        if (challengeFlow_ != null)
        {
            challengeFlow_.complete();
        }

        challengeFlow_ = new GoTweenFlow();
        float time = 0f;

        LevelData.Challenge challenge = CurrentChallengeForRfid(rfid);
        GameStorage.UserData userData = UserDataForRfid(rfid);

        if (challengeUi_.transform.localPosition == challengeShownPosition_)
        {
            challengeFlow_.insert(time, new GoTween(challengeUi_, 0.1f, new GoTweenConfig().onComplete(t =>
            {
                AudioPlayer.PlayAudio("audio/sfx/small-whoosh");
            })));
            challengeFlow_.insert(time, new GoTween(challengeUi_, 0.6f, new GoTweenConfig().localPosition(challengeHiddenPosition_).setEaseType(GoEaseType.SineOut)));
            time += 1f;
        }
        challengeFlow_.insert(time, new GoTween(challengeUi_, 0.1f, new GoTweenConfig().onComplete(t =>
        {
            RefreshChallengeUi(userData, challenge);
        })));
        challengeFlow_.insert(time, new GoTween(challengeUi_, 0.6f, new GoTweenConfig().localPosition(challengeShownPosition_).setEaseType(GoEaseType.SineOut)));
        challengeFlow_.insert(time, new GoTween(challengeUi_, 0.1f, new GoTweenConfig().onComplete(t =>
        {
            AudioPlayer.PlayAudio("audio/sfx/small-whoosh");
        })));
        challengeFlow_.play();
    }

    private void HandleRefreshChallengeUi(string rfid)
    {
        if (challengeFlow_ != null)
        {
            challengeFlow_.complete();
        }

        challengeFlow_ = new GoTweenFlow();
        float time = 0f;

        LevelData.Challenge challenge = CurrentChallengeForRfid(rfid);
        GameStorage.UserData userData = UserDataForRfid(rfid);

        RefreshChallengeUi(userData, challenge);
        if (challengeUi_.transform.localPosition != challengeShownPosition_)
        {
            challengeFlow_.insert(time, new GoTween(challengeUi_, 0.6f, new GoTweenConfig().localPosition(challengeShownPosition_).setEaseType(GoEaseType.SineOut)));
            challengeFlow_.insert(time, new GoTween(challengeUi_, 0.1f, new GoTweenConfig().onComplete(t =>
            {
                AudioPlayer.PlayAudio("audio/sfx/small-whoosh");
            })));
            time += 1f;
        }
        challengeFlow_.insert(time, new GoTween(challengeUi_, 0.1f, new GoTweenConfig().onComplete(t =>
        {
            RefreshChallengeUi(userData, challenge, JustCompletedChallengeForRfid(rfid));
        })));
        challengeFlow_.play();
    }

    private void RefreshChallengeUi(GameStorage.UserData userData, LevelData.Challenge challenge, string challengeToHighlight = null)
    {
        challengeImage_.sprite = challenge.Sprite;
        CompletedChallengeIcon[] icons = completedChallengeHolder_.transform.GetComponentsInChildren<CompletedChallengeIcon>();
        for (int i = 0; i < icons.Length; i++)
        {
            Destroy(icons[i].gameObject);
        }

        //foreach (string completedChallenge in userData.CompletedChallenges)
        //{
        //    LevelData.Challenge completedChallengeData = ChallengeData.Challenges.Find(c => string.Equals(c.Name, completedChallenge));
        //    if (completedChallengeData != null)
        //    {
        //        CompletedChallengeIcon icon = GameObject.Instantiate<CompletedChallengeIcon>(challengeIconPrefab_);
        //        icon.transform.SetParent(completedChallengeHolder_);
        //        icon.transform.localScale = Vector3.one;
        //        icon.SetSprite(completedChallengeData.CompletedSprite);

        //        if (shouldHighlightLastChallenge_ && string.Equals(completedChallenge, challengeToHighlight))
        //        {
        //            shouldHighlightLastChallenge_ = false;
        //            AudioPlayer.PlayAudio("audio/sfx/ding");
        //            icon.ToggleHighlight(true);
        //        } else
        //        {
        //            icon.ToggleHighlight(false);
        //        }
        //    }
        //}
    }

    private void HandleFail(string rfid)
    {
        if (rfid == null || rfid.Length == 0)
        {
            return;
        }
        LevelData.Challenge c = CurrentChallengeForRfid(rfid);
        if (c != null && c.FailCommand != null && c.FailCommand.Length > 0)
        {
            gameManager_.SendNewActionInternal(c.FailCommand);
        }
    }

    private void HandleScanCommand(string rfid)
    {
        if (rfid == null || rfid.Length == 0)
        {
            return;
        }
        LevelData.Challenge c = CurrentChallengeForRfid(rfid);
        gameManager_.SendNewActionInternal(c.RequirementsCommand);
    }

    private void HandleEncourage(string rfid)
    {
        if (rfid == null || rfid.Length == 0)
        {
            return;
        }
        LevelData.Challenge c = CurrentChallengeForRfid(rfid);
        GenericEncourage();
    }

    private void GenericEncourage()
    {
        gameManager_.SendNewActionInternal("-character talk ready-to-test-1 ready-to-test-2 ready-to-test-3 ready-to-test-4 ready-to-test-5");
    }

    private void HandleAnnounceRequirements(string rfid)
    {
        if (rfid != null && rfid.Length > 0)
        {
            LevelData.Challenge c = CurrentChallengeForRfid(rfid);
            if (c != null)
            {
                gameManager_.SendNewActionInternal(c.RequirementsCommand);
            }
        }
    }

    private void HandleReward(string rfid, LevelData.Challenge challengeOverride = null)
    {
        if (rfid != null && rfid.Length > 0)
        {
            LevelData.Challenge c = CurrentChallengeForRfid(rfid);
            if (challengeOverride != null)
            {
                c = challengeOverride;
            }
            if (c != null)
            {
                GameStorage.UserData userData = UserDataForRfid(rfid);
                gameManager_.SendNewActionInternal(c.RewardCommand);

                userData.CompletedChallenges.Add(c.Name);
                lastRunChallenge_ = c;
                Debug.LogWarning("setting last run challenge " + lastRunChallenge_.Name);


                LevelData.Challenge nextChallenge = NextChallengeForRfid(rfid);
                if (nextChallenge != null)
                {
                    userData.CurrentChallenge = nextChallenge.Name;
                }
                SaveUserData(rfid, userData);
            }
        }
    }

    private void HandleAnnounceNextChallenge(string rfid)
    {
        if (rfid != null && rfid.Length > 0)
        {
            LevelData.Challenge c = CurrentChallengeForRfid(rfid);
            if (c != null)
            {
                gameManager_.SendNewActionInternal(c.NextChallengeCommand);
            }
        }
    }

    private void HandleToggleMultipleEntry(bool allow)
    {
        if (allow)
        {
            groupChallenge_ = lastRunChallenge_;
            Debug.LogWarning("setting group challenge " + groupChallenge_.Name);
        }
        allowMultipleEntry_ = allow;
    }

    private LevelData.Challenge CurrentChallengeForRfid(string rfid)
    {
        GameStorage.UserData userData = UserDataForRfid(rfid);

        if (userData.CurrentChallenge == null || userData.CurrentChallenge.Length == 0)
        {
            userData.CurrentChallenge = ChallengeData.Challenges[0].Name;
            SaveUserData(rfid, userData);
        }
        foreach (LevelData.Challenge c in ChallengeData.Challenges)
        {
            if (string.Equals(c.Name, userData.CurrentChallenge))
            {
                return c;
            }
        }
        return null;
    }

    private LevelData.Challenge NextChallengeForRfid(string rfid)
    {
        GameStorage.UserData userData = UserDataForRfid(rfid);
        for (int i = 0; i < ChallengeData.Challenges.Count; i++)
        {
            LevelData.Challenge c = ChallengeData.Challenges[i];
            if (string.Equals(c.Name, userData.CurrentChallenge))
            {
                if (i + 1 < ChallengeData.Challenges.Count)
                {
                    return ChallengeData.Challenges[i + 1];
                } else
                {
                    return null;
                }
            }
        }
        return null;
    }

    private string JustCompletedChallengeForRfid(string rfid)
    {
        GameStorage.UserData userData = UserDataForRfid(rfid);
        if (userData.CompletedChallenges.Count > 0)
        {
            return userData.CompletedChallenges[userData.CompletedChallenges.Count - 1];
        }
        return null;
    }

    private GameStorage.UserData UserDataForRfid(string rfid)
    {
        GameStorage gs = gameManager_.GameStorageForUserId(rfid);
        GameStorage.UserData userData = gs.GetValue<GameStorage.UserData>(GameStorage.Key.UserData);
        if (userData == null) userData = new GameStorage.UserData();
        return userData;
    }

    private void SaveUserData(string rfid, GameStorage.UserData userData)
    {
        GameStorage gs = gameManager_.GameStorageForUserId(rfid);
        gs.Add<GameStorage.UserData>(GameStorage.Key.UserData, userData);
    }
}
