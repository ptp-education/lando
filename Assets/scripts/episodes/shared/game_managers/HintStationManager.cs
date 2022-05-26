using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HintStationManager : StationManager
{
    [SerializeField] private Transform hintsBackground_;
    [SerializeField] private HintObjectHolder thumbnailPrefab_;

    [SerializeField] private Image selectHintBackground_;
    [SerializeField] private Image completeMoreChallengeBackground_;
    [SerializeField] private Image showingHintBackground_;

    private HintObject activeHintObject_;
    private string activeId_;

    //all available hints, ranked by recency
    //names of hints, which we can use to pull up the actual hint

    //no-hint reason

    //clicking on a hint will send an action to dispatch that this ID used this hint

    //initializing hints with callbacks, for when to print, when to say VO

    protected override void NewRelevantAction(List<string> arguments)
    {
        base.NewRelevantAction(arguments);

        if (!IsStationActive) return;

        if (string.Equals("show-hints", arguments[0]))
        {
            HandleShowHints(arguments[1], arguments[2], arguments[3]);
        }

        if (string.Equals("no-hints", arguments[0]))
        {
            HandleNoHints();
        }
    }

    private void HandleNoHints()
    {
        Reset();

        completeMoreChallengeBackground_.gameObject.SetActive(true);

        RemoveAllHintThumbnails();
    }

    private void HandleShowHints(string activeId, string allHints, string redeemedHints)
    {
        if (!string.Equals(activeId_, activeId))
        {
            AudioPlayer.PlaySfx("guide-appears");
        }

        activeId_ = activeId;

        Reset();
        selectHintBackground_.gameObject.SetActive(true);
        RemoveAllHintThumbnails();

        List<string> allHintsList = allHints.ConvertFromArgumentList();
        List<string> redeemedHintList = redeemedHints.ConvertFromArgumentList();

        foreach (string h in allHintsList)
        {
            LevelData.Hint hint = FindHint(h);
            if (hint != null)
            {
                HintObjectHolder thumbnail = GameObject.Instantiate<HintObjectHolder>(thumbnailPrefab_);
                thumbnail.SetSprite(hint.Thumbnail);
                thumbnail.transform.SetParent(hintsBackground_);
                thumbnail.transform.localScale = Vector3.one;
                thumbnail.name = h;

                thumbnail.ToggleOverlay(redeemedHints.Contains(h));
            }
        }
    }

    private void RemoveAllHintThumbnails()
    {
        foreach (Image i in hintsBackground_.GetComponentsInChildren<Image>())
        {
            Destroy(i.gameObject);
        }
    }

    public void OnThumbnailClick(Button button)
    {
        Reset();
        showingHintBackground_.gameObject.SetActive(true);

        string hintUsed = button.name;
        LevelData.Hint hint = FindHint(hintUsed);

        if (hint == null)
        {
            Debug.LogWarning("Couldn't find hint: " + button.name);
            return;
        }

        SendNewAction(string.Format("-hint-used {0} {1}", activeId_, hintUsed));

        HintObject hintObject = GameObject.Instantiate(hint.ObjectToLoad);
        hintObject.name = hintUsed;
        hintObject.Init(this, OnHintComplete);
        hintObject.transform.SetParent(showingHintBackground_.transform);
        hintObject.transform.localScale = Vector3.one;
        hintObject.transform.localPosition = Vector3.zero;

        activeHintObject_ = hintObject;
    }

    public void OnHintComplete()
    {
        SendNewAction(string.Format("-refresh-station {0} {1}", activeId_, StationName));
    }

    protected override void Reset()
    {
        if (activeHintObject_ != null)
        {
            Destroy(activeHintObject_.gameObject);
            activeHintObject_ = null;
        }

        selectHintBackground_.gameObject.SetActive(false);
        completeMoreChallengeBackground_.gameObject.SetActive(false);
        showingHintBackground_.gameObject.SetActive(false);
    }
}
