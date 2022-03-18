using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SpawnedFarmhouse : SpawnedObject
{
    [SerializeField] private List<Sprite> farmhouseOptions_ = new List<Sprite>();
    [SerializeField] private Image farmhouse_;
    [SerializeField] private int totalExpectedLumber_;
    [SerializeField] private Image levelProgress_;
    [SerializeField] private Image levelHolder_;
    [SerializeField] private Text levelText_;
    [SerializeField] private Transform animals_;
    [SerializeField] private Image dino_;
    [SerializeField] private Image elephant_;
    [SerializeField] private Image giraffe_;

    private int houseImageLevel_ = 0;

    public override void ReceivedAction(string action)
    {
        base.ReceivedAction(action);

        if (ArgumentHelper.ContainsCommand("-next-stage", action))
        {
            transform.localScale = new Vector3(0.6f, 0.6f);
            transform.localPosition = new Vector3(684f, -258f);
        }

        if (ArgumentHelper.ContainsCommand("-farmhouse-increase", action))
        {
            int houseLevel = 0;
            GameStorage.Integer houseLevelStorage = gameManager_.Storage.GetValue<GameStorage.Integer>(GameStorage.Key.HouseLevel);
            if (houseLevelStorage != null)
            {
                houseLevel = houseLevelStorage.value;
            }

            gameManager_.Storage.Add<GameStorage.Integer>(GameStorage.Key.HouseLevel, new GameStorage.Integer(houseLevel + 1));
            RefreshHouse(true);
        }
    }

    private void RefreshHouse(bool playSound)
    {
        int houseLevel = 0;
        GameStorage.Integer houseLevelStorage = gameManager_.Storage.GetValue<GameStorage.Integer>(GameStorage.Key.HouseLevel);
        if (houseLevelStorage != null)
        {
            houseLevel = houseLevelStorage.value;
        }

        levelText_.text = houseLevel.ToString();

        int increments = totalExpectedLumber_ / farmhouseOptions_.Count;
        float targetWidth = (float)(houseLevel % increments) / (float)increments * levelHolder_.rectTransform.sizeDelta.x;
        levelProgress_.rectTransform.sizeDelta = new Vector2(targetWidth, levelHolder_.rectTransform.sizeDelta.y);

        int newHouseImageLevel = houseLevel / increments;

        farmhouse_.sprite = farmhouseOptions_[Mathf.Min(newHouseImageLevel, farmhouseOptions_.Count - 1)];
        farmhouse_.SetNativeSize();

        if (playSound)
        {
            if (newHouseImageLevel > houseImageLevel_)
            {
                houseImageLevel_ = newHouseImageLevel;
                AudioPlayer.PlayAudio("audio/sfx/new-building");
            }
            else
            {
                AudioPlayer.PlayAudio("audio/sfx/new-level");
            }
        }
    }

    public override void Reset()
    {
        base.Reset();

        RefreshHouse(false);
    }
}
