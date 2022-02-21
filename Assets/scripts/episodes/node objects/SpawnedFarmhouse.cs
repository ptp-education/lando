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

        int houseLevel = 0;
        GameStorage.Integer houseLevelStorage = gameManager_.Storage.GetValue<GameStorage.Integer>(GameStorage.Key.HouseLevel);
        if (houseLevelStorage != null)
        {
            houseLevel = houseLevelStorage.value;
        }

        if (action.Contains("truck-add-2lb"))
        {
            gameManager_.Storage.Add<GameStorage.Integer>(GameStorage.Key.HouseLevel, new GameStorage.Integer(houseLevel + 1));
            RefreshHouse(true);
        } else if (action.Contains("truck-add-5lb"))
        {
            gameManager_.Storage.Add<GameStorage.Integer>(GameStorage.Key.HouseLevel, new GameStorage.Integer(houseLevel + 2));
            RefreshHouse(true);
        }
        else if (action.Contains("truck-add-10lb"))
        {
            gameManager_.Storage.Add<GameStorage.Integer>(GameStorage.Key.HouseLevel, new GameStorage.Integer(houseLevel + 3));
            RefreshHouse(true);
        } else if (action.Contains("truck-add-15lb"))
        {
            gameManager_.Storage.Add<GameStorage.Integer>(GameStorage.Key.HouseLevel, new GameStorage.Integer(houseLevel + 4));
            RefreshHouse(true);
        } else if (action.Contains("show-outlines")) {
            gameManager_.Storage.Add<string>(GameStorage.Key.ShowAnimalOutlines, "show");
            AudioPlayer.PlayAudio("audio/sfx/arch");
            RefreshAnimals(false);
        } else if (action.Contains("add-dino"))
        {
            gameManager_.Storage.AddObjectToList<string>(GameStorage.Key.FarmObjects, "dino");
            RefreshAnimals(true);
        } else if (action.Contains("add-giraffe"))
        {
            gameManager_.Storage.AddObjectToList<string>(GameStorage.Key.FarmObjects, "giraffe");
            RefreshAnimals(true);
        } else if (action.Contains("add-elephant"))
        {
            gameManager_.Storage.AddObjectToList<string>(GameStorage.Key.FarmObjects, "elephant");
            RefreshAnimals(true);
        }
    }

    private void Refresh(bool playSound)
    {
        RefreshHouse(playSound);
        RefreshAnimals(playSound);
    }

    private void RefreshAnimals(bool playSound)
    {
        if (gameManager_.Storage.GetValue<string>(GameStorage.Key.ShowAnimalOutlines) != null)
        {
            animals_.gameObject.SetActive(true);
        }

        List<string> farmObjects = gameManager_.Storage.GetValue<List<string>>(GameStorage.Key.FarmObjects);
        if (farmObjects != null)
        {
            if (farmObjects.FindAll(s => string.Equals("dino", s)).Count > 0)
            {
                dino_.color = Color.white;
                if (playSound) AudioPlayer.PlayAudio("audio/sfx/dino");
            }
            if (farmObjects.FindAll(s => string.Equals("giraffe", s)).Count > 0)
            {
                giraffe_.color = Color.white;
                if (playSound) AudioPlayer.PlayAudio("audio/sfx/giraffe");
            }
            if (farmObjects.FindAll(s => string.Equals("elephant", s)).Count > 0)
            {
                elephant_.color = Color.white;
                if (playSound) AudioPlayer.PlayAudio("audio/sfx/elephant");
            }
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

        Refresh(false);
    }
}
