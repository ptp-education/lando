using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TestStationManager : StationManager
{
    [SerializeField] private Image currentChallengeBackground_;
    [SerializeField] private Image challengeSuccessBackground_;
    [SerializeField] private Image challengeProblemBackground_;
    [SerializeField] private Image challengeFailedBackground_;
    [SerializeField] private Image challengeImage_;

    [SerializeField] private Transform indicatorHolder_;
    [SerializeField] private Image indicatorPassivePrefab_;
    [SerializeField] private Image indicatorActivePrefab_;

    [SerializeField] private Transform wristBand_;
    [SerializeField] private Transform wristBandCenter_;
    [SerializeField] private Transform rewardHolder_;
    [SerializeField] private Image blocksRewardPrefab_;
    [SerializeField] private Image hintRewardPrefab_;
    [SerializeField] private Image gameRewardPrefab_;

    private List<Image> indicators_ = new List<Image>();
    private List<Image> spawnedRewards_ = new List<Image>();

    private EventObject activeHintObject_;
    private GoTweenFlow resetToCurrentChallengeFlow_;
    private GoTweenFlow rewardFlow_ = new GoTweenFlow();

    protected override void NewRelevantAction(List<string> arguments)
    {
        base.NewRelevantAction(arguments);

        if (!IsStationActive) return;

        bool actionCaught = false;

        if (string.Equals("load", arguments[0]))
        {
            HandleNewChallange(arguments[1]);
            actionCaught = true;
        }

        if (string.Equals(CommandDispatch.ValidatorResponse.Success.ToString(), arguments[0]))
        {
            HandleChallengeCompleted(arguments[1]);
            actionCaught = true;
        }

        if (string.Equals(CommandDispatch.ValidatorResponse.Failure.ToString(), arguments[0]))
        {
            bool showHint = false;
            if (string.Equals(arguments[1], "show-hint"))
            {
                showHint = true;
            }
            else if (string.Equals(arguments[1], "dont-show-hint"))
            {
                showHint = false;
            }
            HandleChallengeFailed(showHint);
            actionCaught = true;
        }

        if (string.Equals(CommandDispatch.ValidatorResponse.BeforeTest.ToString(), arguments[0]))
        {
            HandleTestingProblem(arguments[1]);
            actionCaught = true;
        }

        if (string.Equals(CommandDispatch.ValidatorResponse.ScanWristband.ToString(), arguments[0]))
        {
            HandleScanWristband();
            actionCaught = true;
        }

        if (!actionCaught)
        {
            AudioPlayer.PlaySfx("turn-on");
        }
    }

    private void HandleNewChallange(string challengeName, bool skipCheckForRewardFlow = false)
    {
        if (!skipCheckForRewardFlow && rewardFlow_ != null && rewardFlow_.state == GoTweenState.Running) return;

        Reset();

        RefreshIndicatorHolder(challengeName);

        AudioPlayer.PlaySfx("guide-appears");

        currentChallengeBackground_.gameObject.SetActive(true);

        LevelData.Challenge challenge = ChallengeData.Challenges.Find(c => string.Equals(challengeName, c.Name));
        if (challenge == null)
        {
            Debug.LogWarning("Couldn't find challenge with name: " + challengeName);
            return;
        }

        challengeImage_.sprite = challenge.Sprite;
    }

    private void HandleChallengeCompleted(string challengeName)
    {
        if (rewardFlow_ != null && rewardFlow_.state == GoTweenState.Running) return;

        Reset();

        for (int i = 0; i < this.spawnedRewards_.Count; i++)
        {
            Destroy(this.spawnedRewards_[i].gameObject);
        }
        spawnedRewards_ = new List<Image>();

        challengeSuccessBackground_.gameObject.SetActive(true);

        AudioPlayer.PlayCheer();

        List<Image> prefabs = new List<Image>();
        prefabs.Add(gameRewardPrefab_);

        LevelData.Challenge currentChallenge = FindChallenge(challengeName);
        for (int i = 0; i < currentChallenge.HintRewards.Count; i++)
        {
            prefabs.Add(hintRewardPrefab_);
        }
        for (int i = 0; i < currentChallenge.ResourceRewards.Count; i++)
        {
            prefabs.Add(blocksRewardPrefab_);
        }

        List<string> voiceoverOptions = new List<string>();
        voiceoverOptions.Add("reward-new-a");
        voiceoverOptions.Add("reward-new-b");

        if (currentChallenge.HintRewards.Count > 0)
        {
            voiceoverOptions.Add("reward-new-hints-a");
            voiceoverOptions.Add("reward-new-hints-b");
            voiceoverOptions.Add("reward-new-hints-c");
        }

        if (currentChallenge.ResourceRewards.Count > 0)
        {
            voiceoverOptions.Add("reward-new-blocks-a");
            voiceoverOptions.Add("reward-new-blocks-b");
            voiceoverOptions.Add("reward-new-blocks-c");
        }

        AudioPlayer.PlayVoiceover(voiceoverOptions, "audio/shared_vo/");

        rewardFlow_ = new GoTweenFlow();

        float time = 2f;
        for (int i = 0; i < prefabs.Count; i++) 
        {
            Image prefab = prefabs[i];
            Image reward = GameObject.Instantiate(prefab);

            reward.transform.SetParent(rewardHolder_);
            reward.transform.localScale = Vector3.zero;

            rewardFlow_.insert(time + i * 0.3f, new GoTween(reward.transform, 0.2f, new GoTweenConfig().scale(1f).setEaseType(GoEaseType.SineIn)));
            rewardFlow_.insert(time + i * 0.3f, new GoTween(reward.transform, 0.01f, new GoTweenConfig().onComplete(t =>
            {
                AudioPlayer.PlaySfx("pop");
            })));

            spawnedRewards_.Add(reward);
        }

        time += prefabs.Count * 0.3f + 1.5f;

        rewardFlow_.insert(time - 1f, new GoTween(transform, 0.1f, new GoTweenConfig().onComplete(t =>
        {
            foreach(Image i in spawnedRewards_)
            {
                i.transform.SetParent(i.transform.parent.parent);
            }
        })));

        int separateCounter = 0;
        for (int i = spawnedRewards_.Count - 1; i >= 0; i--)
        {
            Image reward = spawnedRewards_[i];
            rewardFlow_.insert(time + separateCounter * 0.6f,
                new GoTween(
                    reward.transform,
                    0.9f,
                    new GoTweenConfig().vector3XProp("localPosition", wristBandCenter_.localPosition.x)));

            rewardFlow_.insert(time + separateCounter * 0.6f,
                new GoTween(
                    reward.transform,
                    0.45f,
                    new GoTweenConfig().vector3YProp("localPosition", wristBandCenter_.localPosition.y + 250f).setEaseType(GoEaseType.SineOut)));

            rewardFlow_.insert(time + separateCounter * 0.6f,
                new GoTween(
                    reward.transform,
                    0.45f,
                    new GoTweenConfig().scale(1.15f).setEaseType(GoEaseType.SineOut)));

            rewardFlow_.insert(time + separateCounter * 0.6f + 0.45f,
                new GoTween(
                    reward.transform,
                    0.45f,
                    new GoTweenConfig().vector3YProp("localPosition", wristBandCenter_.localPosition.y).setEaseType(GoEaseType.SineIn)));

            rewardFlow_.insert(time + separateCounter * 0.6f + 0.45f,
                new GoTween(
                    reward.transform,
                    0.45f,
                    new GoTweenConfig().scale(1f).setEaseType(GoEaseType.SineIn)));

            rewardFlow_.insert(time + separateCounter * 0.6f, new GoTween(reward.transform, 0.01f, new GoTweenConfig().onComplete(t =>
            {
                AudioPlayer.PlaySfx("whoosh");
            })));

            rewardFlow_.insert(time + separateCounter * 0.6f + 0.9f - 0.1f,
                new GoTween(
                    reward.transform,
                    0.1f,
                    new GoTweenConfig().scale(0f).setEaseType(GoEaseType.SineIn)));

            rewardFlow_.insert(time + separateCounter * 0.6f + 0.9f,
                new GoTween(
                    wristBand_,
                    0.075f,
                    new GoTweenConfig().scale(1.05f)));

            rewardFlow_.insert(time + separateCounter * 0.6f + 0.9f,
                new GoTween(
                    wristBand_,
                    0.075f,
                    new GoTweenConfig().onComplete(t =>
                    {
                        AudioPlayer.PlaySfx("beep");
                    })));

            rewardFlow_.insert(time + separateCounter * 0.6f + 0.9f + 0.075f,
                new GoTween(
                    wristBand_,
                    0.075f,
                    new GoTweenConfig().scale(1f)));

            separateCounter++;
        }

        time += spawnedRewards_.Count * 0.6f + 1.5f;

        LevelData.Challenge nextChallenge = NextChallengeForCurrentChallenge(challengeName);
        if (nextChallenge != null)
        {
            float nextChallengeDuration = AudioPlayer.AudioLength(nextChallenge.NextChallengeCommand, episode_.VORoot);

            rewardFlow_.insert(time, new GoTween(transform, 0.01f, new GoTweenConfig().onComplete(t =>
            {
                AudioPlayer.PlayVoiceover(nextChallenge.NextChallengeCommand, episode_.VORoot);
            })));

            rewardFlow_.insert(time + nextChallengeDuration - 0.5f, new GoTween(transform, 0.01f, new GoTweenConfig().onComplete(t =>
            {
                HandleNewChallange(nextChallenge.Name, skipCheckForRewardFlow: true);
                AudioPlayer.PlaySfx("ding");
            })));
        }

        rewardFlow_.play();
    }

    private void HandleChallengeFailed(bool showHint)
    {
        if(rewardFlow_ != null && rewardFlow_.state == GoTweenState.Running) return;

        Reset();

        ResetToCurrentChallenge(5f);

        AudioPlayer.PlaySfx("noentry");

        if (showHint)
        {
            challengeFailedBackground_.gameObject.SetActive(true);
            List<string> files = new List<string>();
            for (int i = 1; i <= 11; i++)
            {
                files.Add("testing-offer-hint-" + i.ToString());
            }
            NewVoiceover(files);
        } else
        {
            challengeProblemBackground_.gameObject.SetActive(true);
        }
    }

    private void HandleTestingProblem(string problem)
    {
        if (rewardFlow_ != null && rewardFlow_.state == GoTweenState.Running) return;

        Reset();

        ResetToCurrentChallenge(5f);
        challengeProblemBackground_.gameObject.SetActive(true);

        LevelData.BeforeTestFail fail = ChallengeData.WaysToFail.Find(f => string.Equals(f.Name, problem));
        if (fail == null)
        {
            Debug.LogWarning("Couldn't find object for fail reason: " + problem);
            return;
        }

        EventObject eo = GameObject.Instantiate(fail.ObjectToLoad);
        eo.Init(EventObject.Type.iPad, this, () =>
        {
            DestroyHintObject();
            ResetToCurrentChallenge();
        });

        eo.transform.SetParent(challengeProblemBackground_.transform);
        eo.transform.localScale = Vector3.one;
        eo.transform.localPosition = Vector3.zero;
        activeHintObject_ = eo;
    }

    private void HandleScanWristband()
    {
        List<string> files = new List<string>();
        for (int i = 1; i <= 12; i++)
        {
            files.Add("testing-scan-card-" + i.ToString());
        }

        NewVoiceover(files);
    }

    protected override void Reset()
    {
        currentChallengeBackground_.gameObject.SetActive(false);
        challengeSuccessBackground_.gameObject.SetActive(false);
        challengeProblemBackground_.gameObject.SetActive(false);
        challengeFailedBackground_.gameObject.SetActive(false);

        DestroyHintObject();
    }

    private void RefreshIndicatorHolder(string challengeName)
    {
        for (int i = 0; i < indicators_.Count; i++)
        {
            Destroy(indicators_[i].gameObject);
        }
        indicators_ = new List<Image>();

        int position = ChallengeData.Challenges.FindIndex(c => string.Equals(c.Name, challengeName));

        for (int i = 0; i < ChallengeData.Challenges.Count; i++) 
        {
            Image indicator = GameObject.Instantiate(i == position ? indicatorActivePrefab_ : indicatorPassivePrefab_);
            indicator.transform.SetParent(indicatorHolder_);
            indicator.transform.localScale = Vector3.one;
            indicators_.Add(indicator);
        }
    }

    private void ResetToCurrentChallenge(float time)
    {
        if (resetToCurrentChallengeFlow_ != null)
        {
            resetToCurrentChallengeFlow_.destroy();
            resetToCurrentChallengeFlow_ = null;
        }

        resetToCurrentChallengeFlow_ = new GoTweenFlow();
        resetToCurrentChallengeFlow_.insert(0, new GoTween(this.transform, time, new GoTweenConfig().onComplete(t =>
        {
            ResetToCurrentChallenge();
        })));
        resetToCurrentChallengeFlow_.play();
    }

    private void ResetToCurrentChallenge()
    {
        Reset();
        currentChallengeBackground_.gameObject.SetActive(true);
    }

    private void DestroyHintObject()
    {
        if (activeHintObject_ != null)
        {
            Destroy(activeHintObject_.gameObject);
            activeHintObject_ = null;
        }
    }
}

