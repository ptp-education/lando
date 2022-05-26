using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HintStationManager : StationManager
{
    [SerializeField] private Transform hintsBackground_;
    [SerializeField] private Image thumbnailPrefab_;

    private HintObject activeHintObject_;
    private string activeId_;
    private List<string> hintsToShow_;

    //all available hints, ranked by recency
    //names of hints, which we can use to pull up the actual hint

    //no-hint reason

    //clicking on a hint will send an action to dispatch that this ID used this hint

    //initializing hints with callbacks, for when to print, when to say VO

    protected override void NewRelevantAction(List<string> arguments)
    {
        base.NewRelevantAction(arguments);

        if (string.Equals("show-hints", arguments[0]))
        {
            HandleShowHints(arguments[1], new List<string>(arguments.GetRange(2, arguments.Count - 2)));
        }

        if (string.Equals("no-hints", arguments[0]))
        {
            HandleNoHints();
        }
    }

    private void HandleNoHints()
    {
        Debug.Log("complete more challenges to see hints");
        RemoveAllHintThumbnails();
    }

    private void HandleShowHints(string activeId, List<string> hints)
    {
        if (activeHintObject_ != null)
        {
            return;     //don't accept new ids until the current hint is over
        }

        activeId_ = activeId;
        hintsToShow_ = hints;

        RemoveAllHintThumbnails();

        foreach(string h in hints)
        {
            LevelData.Hint hint = FindHint(h);
            if (hint != null)
            {
                Image thumbnail = GameObject.Instantiate<Image>(thumbnailPrefab_);
                thumbnail.sprite = hint.Thumbnail;
                thumbnail.transform.SetParent(hintsBackground_);
                thumbnail.transform.localScale = Vector3.one;
                thumbnail.name = h;
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
        string hintUsed = button.name;
        LevelData.Hint hint = FindHint(hintUsed);

        if (hint == null)
        {
            Debug.LogWarning("Couldn't find hint: " + button.name);
            return;
        }

        HintObject hintObject = GameObject.Instantiate(hint.ObjectToLoad);
        hintObject.name = hintUsed;
        hintObject.Init(this, OnHintComplete);
        hintObject.transform.SetParent(transform);
        hintObject.transform.localScale = Vector3.one;
        hintObject.transform.localPosition = Vector3.zero;

        activeHintObject_ = hintObject;
    }

    public void OnHintComplete()
    {
        string destroyedHint = activeHintObject_.name;

        Destroy(activeHintObject_.gameObject);
        activeHintObject_ = null;

        hintsToShow_.Remove(destroyedHint);
        HandleShowHints(activeId_, hintsToShow_);
    }
}
