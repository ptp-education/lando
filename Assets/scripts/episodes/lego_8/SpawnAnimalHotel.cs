using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Lando.Class.Lego8 
{
    public class SpawnAnimalHotel : SpawnedObject
    {
        [System.Serializable]
        public class Animal 
        {
            public Sprite neutral_;
            public Sprite sleeping_;
        }

        [SerializeField] private List<Animal> animals_;
        [SerializeField] private List<Sprite> objectsToPlace_; //this sprites are going to appear in the screen
        [SerializeField] private List<Image> neutralAnimals_;

        [SerializeField] private List<Image> hotel_;
        [SerializeField] private List<Sprite> tipJars_;

        [SerializeField] private GameObject neutralBackground_;
        [SerializeField] private GameObject successBackground_;

        [SerializeField] private Image animalNeutral_;
        [SerializeField] private Image animalSleeping_;
        [SerializeField] private Image tipJar_;

        [SerializeField] private Image leftObject_;
        [SerializeField] private Image middleObject_;
        [SerializeField] private Image rightObject_;

        private int currentLevel_ = 0;
        private int indexObject_;

        int randomLeft_ = 0;
        int randomMiddle_ = 0;
        int randomRight_ = 0;

        private void Start()
        {
            //ShowCurrentAnimal();
        }

        public override void ReceivedAction(string action)
        {
            List<string> args_ = ArgumentHelper.ArgumentsFromCommand("-hotel", action);
            if (args_.Count == 0) return;

            string commandType = args_[0];
            if (args_.Count > 1) 
            {
                int.TryParse(args_[1], out currentLevel_);
            }

            if (hotel_.Count < 3) 
            {
                gameManager_.SendNewActionInternal("-update-options simulator");
            }

            if (args_.Contains("success"))
            {
                RewardSequence();
                gameManager_.SendNewActionInternal("-update-options empty");
            }
            else if (args_.Contains("left") || args_.Contains("middle") || args_.Contains("right")) 
            {
                SelectObjectToPlace(commandType);
                gameManager_.SendNewActionInternal("-update-options empty");
            }
        }

        private void ShowCurrentAnimal() 
        {
            gameManager_.SendNewActionInternal("-update-options default");
            //animalNeutral_?.gameObject.SetActive(false);
            //animalNeutral_ = neutralAnimals_[currentLevel_];
            //animalNeutral_.gameObject.SetActive(true);

            //animalNeutral_.sprite = animals_[currentLevel_].neutral_;
            animalSleeping_.sprite = animals_[currentLevel_].sleeping_;
        }

        private void RewardSequence() 
        {
            float waitTime = 0;
            ShowCurrentAnimal();
            animalSleeping_.gameObject.SetActive(true);
            GoTweenFlow flow = new GoTweenFlow();
            flow.insert(0f, new GoTween(successBackground_.transform, 0.5f, new GoTweenConfig().scale(1.5f)));
            flow.insert(2f, new GoTween(successBackground_.transform, 0.5f, new GoTweenConfig().scale(1f)));
            flow.play();
            Go.to(this, 2.5f, new GoTweenConfig().onComplete(t => {
                waitTime = waitTipJar();
                tipJar_.gameObject.SetActive(true);
                AudioPlayer.PlayAudio("audio/sfx/coins");
                tipJar_.sprite = tipJars_[currentLevel_];
                Go.to(this, waitTime, new GoTweenConfig().onComplete(t => {
                    animalSleeping_.gameObject.SetActive(false);
                    tipJar_.gameObject.SetActive(false);
                    ShowObjectsToPlace();
                }));
            }));
        }

        private float waitTipJar() 
        {
            switch (currentLevel_) 
            {
                case 0:
                    AudioPlayer.PlayAudio("audio/lego_8/hotel-tip-10");
                    return 4.7f;
                case 1:
                    AudioPlayer.PlayAudio("audio/lego_8/hotel-tip-50");
                    return 4.0f;
                case 2:
                    AudioPlayer.PlayAudio("audio/lego_8/hotel-tip-100");
                    return 6f;
                case 3:
                    AudioPlayer.PlayAudio("audio/lego_8/hotel-tip-500");
                    return 4.0f;
                case 4:
                    AudioPlayer.PlayAudio("audio/lego_8/hotel-tip-1000");
                    return 6f;
            }

            return 2f;
        }

        private void ShowObjectsToPlace() 
        {
            randomLeft_ = Random.Range(0, objectsToPlace_.Count);
            randomMiddle_ = Random.Range(0, objectsToPlace_.Count);

            while (randomMiddle_ == randomLeft_) 
            {
                randomMiddle_ = Random.Range(0, objectsToPlace_.Count);
            }

           randomRight_ = Random.Range(0, objectsToPlace_.Count);

            while (randomRight_ == randomLeft_ || randomRight_ == randomMiddle_)
            {
                randomRight_ = Random.Range(0, objectsToPlace_.Count);
            }

            leftObject_.transform.parent.gameObject.SetActive(true);
            middleObject_.transform.parent.gameObject.SetActive(true);
            rightObject_.transform.parent.gameObject.SetActive(true);

            leftObject_.sprite = objectsToPlace_[randomLeft_];
            middleObject_.sprite = objectsToPlace_[randomMiddle_];
            rightObject_.sprite = objectsToPlace_[randomRight_];
            gameManager_.SendNewActionInternal("-update-options choose");
        }

        private void SelectObjectToPlace(string command) 
        {
            leftObject_.transform.parent.gameObject.SetActive(false);
            middleObject_.transform.parent.gameObject.SetActive(false);
            rightObject_.transform.parent.gameObject.SetActive(false);
            switch (command) 
            {
                case "left":
                    hotel_[randomLeft_].gameObject.SetActive(true);
                    hotel_[randomLeft_].sprite = objectsToPlace_[randomLeft_];
                    objectsToPlace_.Remove(objectsToPlace_[randomLeft_]);
                    hotel_.Remove(hotel_[randomLeft_]);
                    break;
                case "middle":
                    hotel_[randomMiddle_].gameObject.SetActive(true);
                    hotel_[randomMiddle_].sprite = objectsToPlace_[randomMiddle_];
                    objectsToPlace_.Remove(objectsToPlace_[randomMiddle_]);
                    hotel_.Remove(hotel_[randomMiddle_]);
                    break;
                case "right":
                    hotel_[randomRight_].gameObject.SetActive(true);
                    hotel_[randomRight_].sprite = objectsToPlace_[randomRight_];
                    objectsToPlace_.Remove(objectsToPlace_[randomRight_]);
                    hotel_.Remove(hotel_[randomRight_]);
                    break;
            }
            AudioPlayer.PlayAudio("audio/sfx/shaking-bush");
            AudioPlayer.PlayAudio("audio/sfx/customization-selection");
            Go.to(this, 2f, new GoTweenConfig().onComplete(t => {
                //successBackground_.SetActive(false);
                currentLevel_++;
                gameManager_.SendNewActionInternal("-update-options default");
            }));
        }
    }
}
