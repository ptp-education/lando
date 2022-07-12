using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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

        [SerializeField] private List<Animal> level1_;
        [SerializeField] private List<Animal> level2_;
        [SerializeField] private List<Animal> level3_;
        [SerializeField] private List<Animal> level4_;

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

        private float waitTime_ = 1f;
        private int currentLevel_ = 0;
        private int previousLevel = 1;

        private void Start()
        {
            select_.SetActive(true);
        }

        public override void ReceivedAction(string action)
        {
            List<string> args_ = ArgumentHelper.ArgumentsFromCommand("-animal", action);
            if (args_.Count == 0) return;

            string commandType = args_[0];

            if (args_.Count > 1)
            {
                int.TryParse(args_[1], out currentLevel_);
            }

            if (args_.Contains("left") || args_.Contains("middle") || args_.Contains("right"))
            {
                SelectAnimal(commandType);
            }
            else if (args_.Contains("success"))
            {
                ShowAnimalSelection();
            }
            else if (args_.Contains("failure")) 
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

        private void HideOptions() 
        {
            leftAnimal_.gameObject.SetActive(false);
            middleAnimal_.gameObject.SetActive(false);
            rightAnimal_.gameObject.SetActive(false);
        }

        private void ShowOptions()
        {
            leftAnimal_.gameObject.SetActive(true);
            middleAnimal_.gameObject.SetActive(true);
            rightAnimal_.gameObject.SetActive(true);
        }

        private void ShowAnimalSelection() 
        {
            Hide();
            ShowOptions();
            select_.SetActive(true);
            gameManager_.SendNewActionInternal("-update-options choose");
            List<int> randomAnimals = new List<int>();
            int number = 0;
            for (int i = 0; i < 3; i++)
            {
                while (randomAnimals.Contains(number)) 
                {
                    number = Random.Range(0, 5);
                }
                randomAnimals.Add(number);
            }

            switch (currentLevel_) 
            {
                case 0:
                    leftAnimal_.sprite = level1_[randomAnimals[0]].neutral_;
                    middleAnimal_.sprite = level1_[randomAnimals[1]].neutral_;
                    rightAnimal_.sprite = level1_[randomAnimals[2]].neutral_;

                    selectedAnimal_.Add(level1_[randomAnimals[0]]);
                    selectedAnimal_.Add(level1_[randomAnimals[1]]);
                    selectedAnimal_.Add(level1_[randomAnimals[2]]);
                    break;
                case 1:
                    leftAnimal_.sprite = level2_[randomAnimals[0]].neutral_;
                    middleAnimal_.sprite = level2_[randomAnimals[1]].neutral_;
                    rightAnimal_.sprite = level2_[randomAnimals[2]].neutral_;

                    selectedAnimal_.Add(level2_[randomAnimals[0]]);
                    selectedAnimal_.Add(level2_[randomAnimals[1]]);
                    selectedAnimal_.Add(level2_[randomAnimals[2]]);
                    break;
                case 2:
                    leftAnimal_.sprite = level3_[randomAnimals[0]].neutral_;
                    middleAnimal_.sprite = level3_[randomAnimals[1]].neutral_;
                    rightAnimal_.sprite = level3_[randomAnimals[2]].neutral_;

                    selectedAnimal_.Add(level3_[randomAnimals[0]]);
                    selectedAnimal_.Add(level3_[randomAnimals[1]]);
                    selectedAnimal_.Add(level3_[randomAnimals[2]]);
                    break;
                case 3:
                    leftAnimal_.sprite = level4_[randomAnimals[0]].neutral_;
                    middleAnimal_.sprite = level4_[randomAnimals[1]].neutral_;
                    rightAnimal_.sprite = level4_[randomAnimals[2]].neutral_;

                    selectedAnimal_.Add(level4_[randomAnimals[0]]);
                    selectedAnimal_.Add(level4_[randomAnimals[1]]);
                    selectedAnimal_.Add(level4_[randomAnimals[2]]);
                    break;
            }

        }

        private void SelectAnimalToSend() 
        {
            gameManager_.SendNewActionInternal("-update-options choose");
            if (previousLevel <= currentLevel_) 
            {
                ShowAnimalSelection();
            }
        }

        private void SelectAnimal(string command)
        {
            gameManager_.SendNewActionInternal("-update-options empty");
            //
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
                case 0:
                    frogSuccess_.gameObject.SetActive(true);
                    frogSuccess_.sprite = currentAnimal_.success_;
                    break;
                case 1:
                    fishSuccess_.gameObject.SetActive(true);
                    fishSuccess_.sprite = currentAnimal_.success_;
                    break;
                case 2:
                    lizardSuccess_.gameObject.SetActive(true);
                    lizardSuccess_.sprite = currentAnimal_.success_;
                    break;
                case 3:
                    gliderSuccess_.gameObject.SetActive(true);
                    gliderSuccess_.sprite = currentAnimal_.success_;
                    break;
            }
        }

        private void ShowAnimalInCatapult()
        {
            //Show animal in catapult screen
            //If test is successfull show animal flying
            gameManager_.SendNewActionInternal("-fadein 2.5");
            AudioPlayer.PlayAudio("audio/sfx/catapult");
            Go.to(this, 2f, new GoTweenConfig().onComplete(t => {
                switch (currentLevel_) 
                {
                    case 0:
                        AudioPlayer.PlayAudio("audio/sfx/frog");
                        break;
                    case 1:
                        AudioPlayer.PlayAudio("audio/sfx/bubbles");
                        break;
                    case 2:
                        AudioPlayer.PlayAudio("audio/sfx/snake-hiss");
                        break;
                    case 3:
                        AudioPlayer.PlayAudio("audio/sfx/momo-happy");
                        break;
                }
                
                inCatapult_.SetActive(true);
                select_.SetActive(false);
                Go.to(this, 2f, new GoTweenConfig().onComplete(t => {
                    ShowAnimalFlying();
                }));
            }));
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

            waitTime_ = SelectAudioSuccess();
            AudioPlayer.PlayAudio("audio/sfx/wind-whoosh");
            Go.to(this, waitTime_, new GoTweenConfig().onComplete(t => {
                ShowAnimalSuccess();
            }));
        }

        private float SelectAudioSuccess() 
        {
            int randomAudio = 0;
            switch (currentLevel_)
            {
                case 0:
                    randomAudio = Random.Range(0, 2);
                    AudioPlayer.PlayAudio($"audio/lego_7/animal-frog-success-{randomAudio}");
                    if (randomAudio == 0)
                    {
                        return 3.5f;
                    }
                    else 
                    {
                        return 1.5f;
                    }
                case 1:
                    randomAudio = Random.Range(0, 3);
                    AudioPlayer.PlayAudio($"audio/lego_7/animal-fish-success-{randomAudio}");
                    if (randomAudio == 0)
                    {
                        return 5f;
                    }
                    else if (randomAudio == 1)
                    {
                        return 3f;
                    }
                    else 
                    {
                        return 3.1f;
                    }
                case 2:
                    randomAudio = Random.Range(0, 4);
                    AudioPlayer.PlayAudio($"audio/lego_7/animal-lizard-success-{randomAudio}");
                    if (randomAudio == 0)
                    {
                        return 2.6f;
                    }
                    else if (randomAudio == 1)
                    {
                        return 2.7f;
                    }
                    else if (randomAudio == 2)
                    {
                        return 1.2f;
                    }
                    else
                    {
                        return 2.6f;
                    }
                case 3:
                    randomAudio = Random.Range(0, 3);
                    AudioPlayer.PlayAudio($"audio/lego_7/animal-sugar-success-{randomAudio}");
                    if (randomAudio == 0)
                    {
                        return 5.3f;
                    }
                    else if (randomAudio == 1)
                    {
                        return 3.3f;
                    }
                    else
                    {
                        return 4.7f;
                    }
            }
            return 1;
        }

        private void ShowAnimalSuccess() 
        {
            flying_.SetActive(false);
            success_.SetActive(true);
            AudioPlayer.PlayAudio("audio/sfx/cheer");
            Go.to(this, 2f, new GoTweenConfig().onComplete(t => {
                //currentLevel_++;
                previousLevel = currentLevel_;
                selectedAnimal_.Clear();
                Hide();
                select_.SetActive(true);
                HideOptions();
                gameManager_.SendNewActionInternal("-update-options default");
            }));
        }

    }
}
