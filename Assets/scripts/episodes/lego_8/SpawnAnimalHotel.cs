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

        private List<Sprite> objectsAlreadyInHotel = new List<Sprite>(); //this list is to check what objects have been already placed in the hotel

        [SerializeField] private List<Sprite> tipJars_;

        [SerializeField] private GameObject neutralBackground_;
        [SerializeField] private GameObject successBackground_;

        [SerializeField] private Image animalNeutral_;
        [SerializeField] private Image animalSleeping_;
        [SerializeField] private Image tipJar_;

        [SerializeField] private Image leftObject_;
        [SerializeField] private Image middleObject_;
        [SerializeField] private Image rightObject_;

        [SerializeField] private List<Image> hotel_;

        private int currentLevel_ = 1;
        private int indexObject_;

        public override void ReceivedAction(string action)
        {
            List<string> args_ = ArgumentHelper.ArgumentsFromCommand("-hotel", action);
            if (args_.Count == 0) return;

            string commandType = args_[0];

            if (args_.Contains("success"))
            {
                RewardSequence();
            }
            else if (args_.Contains("left") || args_.Contains("middle") || args_.Contains("right")) 
            {
                SelectObjectToPlace(commandType);
            }
        }

        private void ShowCurrentAnimal() 
        {
            animalNeutral_.sprite = animals_[currentLevel_ - 1].neutral_;
            animalSleeping_.sprite = animals_[currentLevel_ - 1].sleeping_;
        }

        private void RewardSequence() 
        {
            animalNeutral_.gameObject.SetActive(false);
            animalSleeping_.gameObject.SetActive(true);

            Go.to(this, 2f, new GoTweenConfig().onComplete(t => {
                tipJar_.sprite = tipJars_[currentLevel_ - 1];
                successBackground_.SetActive(true);
                Go.to(this, 2f, new GoTweenConfig().onComplete(t => {
                    ShowObjectsToPlace();
                }));
            }));
        }

        private void ShowObjectsToPlace() 
        {
            int randomLeft = Random.Range(0, 4);
            int randomMiddle = Random.Range(0, 4);
            int randomRight = Random.Range(0, 4);
        }

        private void SelectObjectToPlace(string command) 
        {
            switch (command) 
            {
                case "left":
                    break;
                case "middle":
                    break;
                case "right":
                    break;
            }
        }
    }
}
