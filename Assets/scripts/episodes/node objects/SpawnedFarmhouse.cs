using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnedFarmhouse : SpawnedObject
{
    [SerializeField] private List<Sprite> farmhouseOptions_ = new List<Sprite>();
    [SerializeField] private Image farmhouse_;
    [SerializeField] private int totalExpectedLumber_;
    [SerializeField] private Image levelProgress_;
    [SerializeField] private Image levelHolder_;

    private int houseImageLevel_ = 0;

    public override void ReceivedAction(string action)
    {
        base.ReceivedAction(action);

        if (action.Contains("improve-house"))
        {
            int houseLevel = 0;
            GameStorage.Integer houseLevelStorage = gameManager_.Storage.GetValue<GameStorage.Integer>(GameStorage.Key.HouseLevel);
            if (houseLevelStorage != null)
            {
                houseLevel = houseLevelStorage.value;
            }
            gameManager_.Storage.Add<GameStorage.Integer>(GameStorage.Key.HouseLevel, new GameStorage.Integer(houseLevel + 1));

            RefreshHouse(playSound: true);
        }
    }

    private void RefreshHouse(bool playSound = false)
    {
        int houseLevel = 0;
        GameStorage.Integer houseLevelStorage = gameManager_.Storage.GetValue<GameStorage.Integer>(GameStorage.Key.HouseLevel);
        if (houseLevelStorage != null)
        {
            houseLevel = houseLevelStorage.value;
        }

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
            } else
            {
                AudioPlayer.PlayAudio("audio/sfx/new-level");
            }
        }
    }

    public override void Reset()
    {
        base.Reset();

        RefreshHouse();
    }
}
