using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceStationManager : StationManager
{
    [SerializeField] private Image collectBlocksScreen_;
    [SerializeField] private Image noMoreBlocksScreen_;
    [SerializeField] private Image completeMoreChallengesScreen_;

    [SerializeField] private Transform moreChallengesHolder_;
    [SerializeField] private Transform currentPositionIndicatorPrefab_;
    [SerializeField] private Transform positionIndicatorPrefab_;
    [SerializeField] private Transform positionWithRewardPrefab_;

    [SerializeField] private Image wristband_;
    [SerializeField] private Transform wristbandStart_;
    [SerializeField] private Image rewardPrefab_;

    private float kDefaultTimeout = 15f;
    private float timeBeforeReset_ = -1f;
    private GoTweenFlow giveResourceFlow_;

    private List<Image> newResources_ = new List<Image>();
    private List<Transform> moreChallengeNodes_ = new List<Transform>();

    protected override void NewRelevantAction(List<string> arguments)
    {
        base.NewRelevantAction(arguments);

        bool actionCaught = false;

        if (string.Equals("give-resources", arguments[0]))
        {
            GiveResources(new List<string>(arguments.GetRange(1, arguments.Count - 1)));
            actionCaught = true;
        }

        if (string.Equals("more-resources", arguments[0]))
        {
            if (arguments.Count > 3)
            {
                int position = int.Parse(arguments[1]); //player's current position
                int total = int.Parse(arguments[2]);    //total challenges available
                int nextResource = int.Parse(arguments[3]); //where the next resource is
                MoreResources(position, total, nextResource);
            }
            actionCaught = true;
        }

        if (string.Equals("no-resources", arguments[0]))
        {
            NoResources();
            actionCaught = true;
        }

        if (!actionCaught)
        {
            AudioPlayer.PlaySfx("turn-on");
        }
    }

    private void Update()
    {
        if (timeBeforeReset_ > 0f)
        {
            timeBeforeReset_ -= Time.deltaTime;

            if (timeBeforeReset_ <= 0f)
            {
                timeBeforeReset_ = -1f;
                Reset();
                background_.sprite = IsStationActive ? activeSprite_ : inactiveSprite_;
            }
        }
    }

    private void GiveResources(List<string> resources)
    {
        if (giveResourceFlow_ != null && giveResourceFlow_.state == GoTweenState.Running)
        {
            return;
        }

        Reset();
        AudioPlayer.PlaySfx("guide-appears");
        AudioPlayer.PlayVoiceover("store-grab-" + (resources.Count > 10 ? "10" : resources.Count.ToString()), "audio/shared_vo/");

        collectBlocksScreen_.gameObject.SetActive(true);

        for (int i = 0; i < newResources_.Count; i++)
        {
            Destroy(newResources_[i].gameObject);
        }

        newResources_ = new List<Image>();

        foreach (string s in resources)
        {
            Image resourceToMove = GameObject.Instantiate(rewardPrefab_);

            resourceToMove.transform.SetParent(collectBlocksScreen_.transform);
            resourceToMove.transform.localScale = Vector3.zero;
            resourceToMove.transform.localPosition = wristbandStart_.localPosition;
            newResources_.Add(resourceToMove);
        }

        giveResourceFlow_ = new GoTweenFlow();
        float time = 0f;

        float startingX = (125 + 50) * (newResources_.Count - 1) / 2f * -1f;
        for (int i = 0; i < newResources_.Count; i++)
        {
            Image newResource = newResources_[i];

            Vector2 targetLocation = new Vector2(startingX + (125 + 50) * i, 45f);

            Vector2 inflection = new Vector2(
                (targetLocation.x - newResource.transform.localPosition.x) * .66f + newResource.transform.localPosition.x,
                targetLocation.y + 100f);

            giveResourceFlow_.insert(time + i * .9f, new GoTween(wristband_, 0.1f,
                new GoTweenConfig().onComplete(t =>
                {
                    AudioPlayer.PlaySfx("whoosh");
                })));

            giveResourceFlow_.insert(time + i * .9f, new GoTween(wristband_.transform, 0.1f,
                new GoTweenConfig().scale(1.05f)));

            giveResourceFlow_.insert(time + i * .9f + 0.1f, new GoTween(wristband_.transform, 0.1f,
                new GoTweenConfig().scale(1f)));

            giveResourceFlow_.insert(time + i * .9f, new GoTween(newResource.transform, 0.4f,
                new GoTweenConfig().vector3XProp("localPosition", inflection.x)));

            giveResourceFlow_.insert(time + i * .9f, new GoTween(newResource.transform, 0.4f,
                new GoTweenConfig().scale(1f).setEaseType(GoEaseType.SineOut)));

            giveResourceFlow_.insert(time + i * .9f, new GoTween(newResource.transform, 0.4f,
                new GoTweenConfig().vector3YProp("localPosition", inflection.y).setEaseType(GoEaseType.SineOut)));

            giveResourceFlow_.insert(time + i * .9f + 0.4f, new GoTween(newResource.transform, 0.2f,
                new GoTweenConfig().vector3XProp("localPosition", targetLocation.x)));

            giveResourceFlow_.insert(time + i * .9f + 0.4f, new GoTween(newResource.transform, 0.2f,
                new GoTweenConfig().vector3YProp("localPosition", targetLocation.y).setEaseType(GoEaseType.SineIn)));
        }

        giveResourceFlow_.play();

        timeBeforeReset_ = kDefaultTimeout;
    }

    private void MoreResources(int position, int total, int nextResource)
    {
        if (giveResourceFlow_ != null && giveResourceFlow_.state == GoTweenState.Running)
        {
            return;
        }

        Reset();
        AudioPlayer.PlaySfx("noentry");
        AudioPlayer.PlayVoiceover("store-complete-more", "audio/shared_vo/");

        completeMoreChallengesScreen_.gameObject.SetActive(true);
        timeBeforeReset_ = kDefaultTimeout;

        for (int i = 0; i < moreChallengeNodes_.Count; i++)
        {
            Destroy(moreChallengeNodes_[i].gameObject);
        }
        moreChallengeNodes_ = new List<Transform>();

        for (int i = 0; i < total; i++)
        {
            Transform prefab = null;
            if (i == position)
            {
                prefab = currentPositionIndicatorPrefab_;
            } else if (i == nextResource)
            {
                prefab = positionWithRewardPrefab_;
            } else
            {
                prefab = positionIndicatorPrefab_;
            }

            Transform node = GameObject.Instantiate(prefab);
            node.transform.SetParent(moreChallengesHolder_);
            node.transform.localScale = Vector3.one;
            moreChallengeNodes_.Add(node);
        }
    }

    private void NoResources()
    {
        if (giveResourceFlow_ != null && giveResourceFlow_.state == GoTweenState.Running)
        {
            return;
        }

        Reset();
        AudioPlayer.PlaySfx("noentry");
        AudioPlayer.PlayVoiceover("store-no-more", "audio/shared_vo/");

        timeBeforeReset_ = kDefaultTimeout;

        noMoreBlocksScreen_.gameObject.SetActive(true);
    }

    protected override void Reset()
    {
        base.Reset();

        collectBlocksScreen_.gameObject.SetActive(false);
        noMoreBlocksScreen_.gameObject.SetActive(false);
        completeMoreChallengesScreen_.gameObject.SetActive(false);
    }
}
