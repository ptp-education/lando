using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SpawnWaterAlien : SpawnedObject
{
    [System.Serializable]
    public class Alien
    {
        public Sprite baby_;
        public Sprite babySick_;
        public Sprite teen_;
        public Sprite adult_;

        public Image babyAlien_;
        public Image teenAlien_;
        public Image adultAlien_;

        public Alien(Sprite baby, Sprite babySick, Sprite teen, Sprite adult, Image babyImage, Image teenImage, Image adultImage)
        {
            this.baby_ = baby;
            this.babySick_ = babySick;
            this.babySick_ = babySick;
            this.teen_ = teen;
            this.adult_ = adult;
            this.babyAlien_ = babyImage;
            this.teenAlien_ = teenImage;
            this.adultAlien_ = adultImage;
        }

        public void ApplySprites()
        {
            babyAlien_.sprite = babySick_;
            teenAlien_.sprite = teen_;
            adultAlien_.sprite = adult_;
        }
    }

    [SerializeField] private List<Alien> aliens_;

    [SerializeField] private GameObject garbage_;

    private Dictionary<string, Alien> studentsAliens_ = new Dictionary<string, Alien>();

    private Alien currentAlien_;

    private int currentLevel_ = 0;

    private string nfcId;

    [SerializeField] private GameObject alienPrefab_;

    [SerializeField] private List<RectTransform> spawnPositions_;

    public override void ReceivedAction(string action)
    {
        List<string> args_ = ArgumentHelper.ArgumentsFromCommand("-alien", action);

        if (args_.Count == 0) return;

        //-alien success 0 423424

        nfcId = args_[2];
        Debug.LogWarning($"nfc id: {nfcId}");

        int.TryParse(args_[1], out currentLevel_);

        if (args_.Contains("success"))
        {
            CheckIfHaveAlien(nfcId);
        }
    }

    private void CheckIfHaveAlien(string id)
    {
        if (studentsAliens_.ContainsKey(id))
        {
            Debug.LogWarning("have id");
            currentAlien_ = studentsAliens_[id];
            CleanGarbage();
        }
        else
        {
            Debug.LogWarning("doesnt have id");
            CreateAlien(id);
        }
    }

    private void CreateAlien(string id)
    {
        int randomAlien = Random.Range(0, aliens_.Count);
        int randomPosition = Random.Range(0, spawnPositions_.Count);

        GameObject alien = Instantiate(alienPrefab_);
        alien.transform.SetParent(this.transform);
        alien.GetComponent<RectTransform>().anchoredPosition = spawnPositions_[randomPosition].anchoredPosition;

        spawnPositions_.Remove(spawnPositions_[randomPosition]);

        Alien newAlien = new Alien(
                                   aliens_[randomAlien].baby_,
                                   aliens_[randomAlien].babySick_,
                                   aliens_[randomAlien].teen_,
                                   aliens_[randomAlien].adult_,
                                   alien.transform.GetChild(0).GetComponent<Image>(),
                                   alien.transform.GetChild(1).GetComponent<Image>(),
                                   alien.transform.GetChild(2).GetComponent<Image>());
        newAlien.ApplySprites();
        studentsAliens_.Add(id, newAlien);
        currentAlien_ = newAlien;

        Go.to(this, 2f, new GoTweenConfig().onComplete(t => {
            CleanGarbage();
        }));
    }

    private void CleanGarbage()
    {
        //deactivate garbage
        //play clean sound
        //make alien grow or just happy
        garbage_.SetActive(false);
        Go.to(this, 2f, new GoTweenConfig().onComplete(t => {
            EvolveAlien();
            Go.to(this, 2f, new GoTweenConfig().onComplete(t => {
                garbage_.SetActive(true);
                currentAlien_ = null;
            }));
        }));
    }

    private void EvolveAlien() 
    {
        Transform alienTransform = null;
        switch (currentLevel_) 
        {
            case 0:
                currentAlien_.babyAlien_.sprite = currentAlien_.baby_;
                alienTransform = currentAlien_.babyAlien_.transform;
                break;
            case 1:
                currentAlien_.babyAlien_.gameObject.SetActive(false);
                currentAlien_.teenAlien_.gameObject.SetActive(true);
                alienTransform = currentAlien_.teenAlien_.transform;
                break;
            case 2:
                currentAlien_.teenAlien_.gameObject.SetActive(false);
                currentAlien_.adultAlien_.gameObject.SetActive(true);
                alienTransform = currentAlien_.adultAlien_.transform;
                break;
        }

        GoTweenFlow flow = new GoTweenFlow();
        flow.insert(0f, new GoTween(alienTransform, 0.25f, new GoTweenConfig().scale(1.25f)));
        flow.insert(0.25f, new GoTween(alienTransform, 0.25f, new GoTweenConfig().scale(1f)));
        flow.play();
    }

    private void FirstStage() 
    {
        
    }
}
