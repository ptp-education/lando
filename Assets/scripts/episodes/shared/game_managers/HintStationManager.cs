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

    private EventObject activeHintObject_;
    private string activeId_;

    protected override void NewRelevantAction(List<string> arguments)
    {
        base.NewRelevantAction(arguments);

        if (!IsStationActive) return;

        bool actionCaught = false;

        if (string.Equals("show-hints", arguments[0]))
        {
            actionCaught = true;
            HandleShowHints(arguments[1], arguments[2], arguments[3]);
        }

        if (string.Equals("no-hints", arguments[0]))
        {
            actionCaught = true;
            HandleNoHints();
        }

        if (!actionCaught)
        {
            AudioPlayer.PlaySfx("turn-on");
        }
    }

    private void HandleNoHints()
    {
        Reset();

        AudioPlayer.PlaySfx("noentry");
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

        if (allHintsList.Count > 0)
        {
            List<string> voiceovers = new List<string>();
            for (int i = 0; i <= 9; i++)
            {
                voiceovers.Add("hint-select-" + i.ToString());
            }
            AudioPlayer.PlayVoiceover(voiceovers, "audio/shared_vo/");
        }

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

                thumbnail.ToggleOverlay(redeemedHintList.Contains(h));
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

        SendNewActionNetworked(string.Format("-hint-used {0} {1}", activeId_, hintUsed));

        EventObject hintObject = GameObject.Instantiate(hint.ObjectToLoad);
        hintObject.name = hintUsed;
        hintObject.Init(EventObject.Type.iPad, this, OnHintComplete);
        hintObject.transform.SetParent(showingHintBackground_.transform);
        hintObject.transform.localScale = Vector3.one;
        hintObject.transform.localPosition = Vector3.zero;

        activeHintObject_ = hintObject;
    }

    public void OnHintComplete()
    {
        SendNewActionNetworked(string.Format("-refresh-station {0} {1}", activeId_, StationName));
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
