using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SpawnedFarmhouse : SpawnedObject
{
    [SerializeField] private List<Sprite> farmhouseOptions_ = new List<Sprite>();

    [SerializeField] private List<int> requirements_ = new List<int>();

    [SerializeField] private Image farmhouse_;
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

        transform.localScale = new Vector3(0.6f, 0.6f);
        transform.localPosition = new Vector3(684f, -258f);

        if (ArgumentHelper.ContainsCommand("-farmhouse-increase", action))
        {
            int houseLevel = 0;
            GameStorage.Integer houseLevelStorage = GameManager.Storage.GetValue<GameStorage.Integer>(GameStorage.Key.HouseLevel);
            if (houseLevelStorage != null)
            {
                houseLevel = houseLevelStorage.value;
            }

            GameManager.Storage.Add<GameStorage.Integer>(GameStorage.Key.HouseLevel, new GameStorage.Integer(houseLevel + 1));
            RefreshHouse(true);
        }
    }

    private void RefreshHouse(bool playSound)
    {
        int houseLevel = 0;
        GameStorage.Integer houseLevelStorage = GameManager.Storage.GetValue<GameStorage.Integer>(GameStorage.Key.HouseLevel);
        if (houseLevelStorage != null)
        {
            houseLevel = houseLevelStorage.value;
        }

        levelText_.text = houseLevel.ToString();
        float width = 1f;

        int currentTier = 0;
        for (int i = 0; i < requirements_.Count; i++)
        {
            if (houseLevel < requirements_[i])
            {
                if (i > 0)
                {
                    width = (float)(houseLevel - requirements_[i - 1]) / (float)(requirements_[i] - requirements_[i - 1]);
                } else if (i == 0)
                {
                    width = (float)houseLevel / (float)requirements_[i];
                }
                currentTier = i + 1;
                break;
            }
            if (houseLevel >= requirements_[i] && i == requirements_.Count - 1)
            {
                width = 1f;
                currentTier = i + 1;
                break;
            }
        }
        width = width * levelHolder_.rectTransform.sizeDelta.x;

        levelProgress_.rectTransform.sizeDelta = new Vector2(width, levelHolder_.rectTransform.sizeDelta.y);
        farmhouse_.sprite = farmhouseOptions_[Mathf.Min(currentTier, farmhouseOptions_.Count - 1)];
        farmhouse_.SetNativeSize();

        if (playSound)
        {
            if (currentTier > houseImageLevel_)
            {
                houseImageLevel_ = currentTier;
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
