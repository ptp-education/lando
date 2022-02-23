using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SpawnedColorBridge : SpawnedObject
{
    [SerializeField] private List<Image> blueSplats_;
    [SerializeField] private List<Image> goldSplats_;
    [SerializeField] private List<Image> greenSplats_;
    [SerializeField] private List<Image> orangeSplats_;
    [SerializeField] private List<Image> pinkSplats_;
    [SerializeField] private List<Image> purpleSplats_;
    [SerializeField] private List<Image> redSplats_;
    [SerializeField] private List<Image> silverSplats_;
    [SerializeField] private List<Image> yellowSplats_;

    private GoTweenFlow flow_;

    public override void ReceivedAction(string action)
    {
        base.ReceivedAction(action);

        if (action.Contains("bridge-color-"))
        {
            string[] split = action.Split('-');

            string color = split[split.Length - 1];

            List<string> colors = gameManager_.Storage.GetValue<List<string>>(GameStorage.Key.BridgeColors);
            if (colors == null)
            {
                colors = new List<string>();
            }
            colors.Add(color);
            gameManager_.Storage.Add<List<string>>(GameStorage.Key.BridgeColors, colors);

            AudioPlayer.PlayAudio("audio/sfx/splat");

            RefreshColors();
        }
    }

    public override void Reset()
    {
        base.Reset();

        RefreshColors();
    }

    public void RefreshColors()
    {
        List<string> colors = gameManager_.Storage.GetValue<List<string>>(GameStorage.Key.BridgeColors);
        if (colors == null || colors.Count == 0) return;

        int blues = colors.FindAll(s => string.Equals("blue", s)).Count;
        int golds = colors.FindAll(s => string.Equals("gold", s)).Count;
        int greens = colors.FindAll(s => string.Equals("green", s)).Count;
        int oranges = colors.FindAll(s => string.Equals("orange", s)).Count;
        int pinks = colors.FindAll(s => string.Equals("pink", s)).Count;
        int purples = colors.FindAll(s => string.Equals("purple", s)).Count;
        int reds = colors.FindAll(s => string.Equals("red", s)).Count;
        int silvers = colors.FindAll(s => string.Equals("silver", s)).Count;
        int yellows = colors.FindAll(s => string.Equals("yellow", s)).Count;

        for (int i = 0; i < blueSplats_.Count; i++) blueSplats_[i].gameObject.SetActive(i < blues);
        for (int i = 0; i < goldSplats_.Count; i++) goldSplats_[i].gameObject.SetActive(i < golds);
        for (int i = 0; i < greenSplats_.Count; i++) greenSplats_[i].gameObject.SetActive(i < greens);
        for (int i = 0; i < orangeSplats_.Count; i++) orangeSplats_[i].gameObject.SetActive(i < oranges);
        for (int i = 0; i < pinkSplats_.Count; i++) pinkSplats_[i].gameObject.SetActive(i < pinks);
        for (int i = 0; i < purpleSplats_.Count; i++) purpleSplats_[i].gameObject.SetActive(i < purples);
        for (int i = 0; i < redSplats_.Count; i++) redSplats_[i].gameObject.SetActive(i < reds);
        for (int i = 0; i < silverSplats_.Count; i++) silverSplats_[i].gameObject.SetActive(i < silvers);
        for (int i = 0; i < yellowSplats_.Count; i++) yellowSplats_[i].gameObject.SetActive(i < yellows);

    }
}
