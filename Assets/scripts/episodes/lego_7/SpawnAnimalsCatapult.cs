using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lando.Class.Lego7 
{
    public class SpawnAnimalsCatapult : SpawnedObject
    {
        [System.Serializable]
        public class Animal 
        {
            public Sprite neutral_;
            public Sprite inCatapult_;
            public Sprite flying_;
            public Sprite failure_;
            public Sprite success_;
        }

        public List<Animal> level1_;
        public List<Animal> level2_;
        public List<Animal> level3_;
        public List<Animal> level4_;

        private int currentLevel_ = 1;

        [SerializeField] private Image leftAnimal_;
        [SerializeField] private Image middleAnimal_;
        [SerializeField] private Image rightAnimal_;
        [SerializeField] private Image inCatapultAnimal_;
        [SerializeField] private Image flyingAnimal_;
        [SerializeField] private Image failureAnimal_;

        [SerializeField] private Image frogSuccess_;
        [SerializeField] private Image fishSuccess_;
        [SerializeField] private Image lizardSuccess_;
        [SerializeField] private Image gliderSuccess_;

        [SerializeField] private GameObject select_;
        [SerializeField] private GameObject inCatapult_;
        [SerializeField] private GameObject flying_;
        [SerializeField] private GameObject success_;
        [SerializeField] private GameObject failure_;

        private List<Animal> selectedAnimal_= new List<Animal>();

        private void Start()
        {
            ShowAnimalSelection();
        }

        public override void ReceivedAction(string action)
        {
            List<string> args = ArgumentHelper.ArgumentsFromCommand("-animal", action);
            if (args.Count == 0) return;

            string commandType = args[0];

            if (args.Contains("left") || args.Contains("middle") || args.Contains("right"))
            {
                SelectAnimal(commandType);
            }
            else if (args.Contains("success"))
            {
                ShowAnimalFlying();
            }
            else if (args.Contains("failure")) 
            {
                ShowAnimalFailure();
            }
        }

        public override void Hide()
        {
            select_.SetActive(false);
            inCatapult_.SetActive(false);
            flying_.SetActive(false);
            success_.SetActive(false);
            failure_.SetActive(false);
        }

        private void ShowAnimalSelection() 
        {
            Hide();
            gameManager_.SendNewActionInternal("-update-options choose");
            select_.SetActive(true);
            List<int> randomAnimals = new List<int>();
            int number = 0;
            for (int i = 0; i < 3; i++)
            {
                while (randomAnimals.Contains(number)) 
                {
                    number = UnityEngine.Random.Range(0, 5);
                }
                randomAnimals.Add(number);
            }

            switch (currentLevel_) 
            {
                case 1:
                    leftAnimal_.sprite = level1_[randomAnimals[0]].neutral_;
                    middleAnimal_.sprite = level1_[randomAnimals[1]].neutral_;
                    rightAnimal_.sprite = level1_[randomAnimals[2]].neutral_;

                    selectedAnimal_.Add(level1_[randomAnimals[0]]);
                    selectedAnimal_.Add(level1_[randomAnimals[1]]);
                    selectedAnimal_.Add(level1_[randomAnimals[2]]);
                    break;
                case 2:
                    leftAnimal_.sprite = level2_[randomAnimals[0]].neutral_;
                    middleAnimal_.sprite = level2_[randomAnimals[1]].neutral_;
                    rightAnimal_.sprite = level2_[randomAnimals[2]].neutral_;

                    selectedAnimal_.Add(level2_[randomAnimals[0]]);
                    selectedAnimal_.Add(level2_[randomAnimals[1]]);
                    selectedAnimal_.Add(level2_[randomAnimals[2]]);
                    break;
                case 3:
                    leftAnimal_.sprite = level3_[randomAnimals[0]].neutral_;
                    middleAnimal_.sprite = level3_[randomAnimals[1]].neutral_;
                    rightAnimal_.sprite = level3_[randomAnimals[2]].neutral_;

                    selectedAnimal_.Add(level3_[randomAnimals[0]]);
                    selectedAnimal_.Add(level3_[randomAnimals[1]]);
                    selectedAnimal_.Add(level3_[randomAnimals[2]]);
                    break;
                case 4:
                    leftAnimal_.sprite = level4_[randomAnimals[0]].neutral_;
                    middleAnimal_.sprite = level4_[randomAnimals[1]].neutral_;
                    rightAnimal_.sprite = level4_[randomAnimals[2]].neutral_;

                    selectedAnimal_.Add(level4_[randomAnimals[0]]);
                    selectedAnimal_.Add(level4_[randomAnimals[1]]);
                    selectedAnimal_.Add(level4_[randomAnimals[2]]);
                    break;
            }

        }

        private void SelectAnimal(string command)
        {
            select_.SetActive(false);
            switch (command)
            {
                case "left":
                    inCatapultAnimal_.sprite = selectedAnimal_[0].inCatapult_;
                    flyingAnimal_.sprite = selectedAnimal_[0].flying_;

                    DisplayAnimalSuccess(selectedAnimal_[0]);
                    break;
                case "middle":
                    inCatapultAnimal_.sprite = selectedAnimal_[1].inCatapult_;
                    flyingAnimal_.sprite = selectedAnimal_[1].flying_;

                    DisplayAnimalSuccess(selectedAnimal_[1]);
                    break;
                case "right":
                    inCatapultAnimal_.sprite = selectedAnimal_[2].inCatapult_;
                    flyingAnimal_.sprite = selectedAnimal_[2].flying_;

                    DisplayAnimalSuccess(selectedAnimal_[2]);
                    break;
            }

            ShowAnimalInCatapult();
        }

        private void DisplayAnimalSuccess(Animal currentAnimal_) 
        {
            switch (currentLevel_)
            {
                case 1:
                    frogSuccess_.gameObject.SetActive(true);
                    frogSuccess_.sprite = currentAnimal_.success_;
                    break;
                case 2:
                    fishSuccess_.gameObject.SetActive(true);
                    fishSuccess_.sprite = currentAnimal_.success_;
                    break;
                case 3:
                    lizardSuccess_.gameObject.SetActive(true);
                    lizardSuccess_.sprite = currentAnimal_.success_;
                    break;
                case 4:
                    gliderSuccess_.gameObject.SetActive(true);
                    gliderSuccess_.sprite = currentAnimal_.success_;
                    break;
            }
        }

        private void ShowAnimalInCatapult()
        {
            //Show animal in catapult screen
            //If test is successfull show animal flying

            inCatapult_.SetActive(true);
            gameManager_.SendNewActionInternal("-update-options default");
        }

        private void ShowAnimalFailure()
        {
            failure_.SetActive(true);
            gameManager_.SendNewActionInternal("-update-options empty");
            //This timing should be based on the VO
            Go.to(this, 1f, new GoTweenConfig().onComplete(t => {
                ShowAnimalInCatapult();
            }));
        }

        private void ShowAnimalFlying()
        {
            inCatapult_.SetActive(false);
            flying_.SetActive(true);
            gameManager_.SendNewActionInternal("-update-options empty");
            Go.to(this, 1f, new GoTweenConfig().onComplete(t => {
                ShowAnimalSuccess();
            }));
        }

        private void ShowAnimalSuccess() 
        {
            flying_.SetActive(false);
            success_.SetActive(true);
            Go.to(this, 2f, new GoTweenConfig().onComplete(t => {
                currentLevel_++;
                selectedAnimal_.Clear();
                ShowAnimalSelection();
            }));
        }

    }
}
