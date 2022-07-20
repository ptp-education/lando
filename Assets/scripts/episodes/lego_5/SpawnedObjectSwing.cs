using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lando.Class.Lego5
{
    public class SpawnedObjectSwing : SpawnedObject
    {
        [SerializeField] private Image smallAnimal_;
        [SerializeField] private Image mediumAnimal_;
        [SerializeField] private Image largeAnimal_;
        [SerializeField] private Image smallAnimalSeat_;
        [SerializeField] private Image mediumAnimalSeat_;
        [SerializeField] private Image largeAnimalSeat_;
        [SerializeField] private Image successImage_;
        [SerializeField] private Image failureImage_;
        [SerializeField] private Image animalWaiting_;

        [SerializeField] private Image iconPrefab_;
        [SerializeField] private Image rsvpHolder_;
        
        [Header("Small animals")]
        [SerializeField] private Choice bird_;
        [SerializeField] private Choice bunny_;
        [SerializeField] private Choice ferret_;

        [Header("Medium animals")]
        [SerializeField] private Choice deer_;
        [SerializeField] private Choice zebra_;
        [SerializeField] private Choice seal_;

        [Header("Big animals")]
        [SerializeField] private Choice elephant_;
        [SerializeField] private Choice giraffe_;
        [SerializeField] private Choice hippo_;

        [SerializeField] private Choice dino1_;
        [SerializeField] private Choice dino2_;
        [SerializeField] private Choice dino3_;

        private Image animalInSwing_;
        private Image correctSizeSwing_;

        private Sprite currentIcon_;

        private int lastSetCounter_;

        [System.Serializable]
        public class Choice
        {
            public Sprite Animal;
            public Sprite AnimalFailure;
            public Sprite AnimalSuccess;
            public Sprite AnimalIcon;
            public Sprite AnimalWaiting;
            public string Sound;
        }

        public override void Hide()
        {
            successImage_.gameObject.SetActive(false);
            failureImage_.gameObject.SetActive(false);
            smallAnimalSeat_.gameObject.SetActive(false);
            mediumAnimalSeat_.gameObject.SetActive(false);
            largeAnimalSeat_.gameObject.SetActive(false);
            smallAnimal_.gameObject.SetActive(false);
            mediumAnimal_.gameObject.SetActive(false);
            largeAnimal_.gameObject.SetActive(false);
            animalWaiting_.gameObject.SetActive(false);
        }

        public override void ReceivedAction(string action)
        {
            if (ArgumentHelper.ContainsCommand("-swing", action))
            {
                List<string> args = ArgumentHelper.ArgumentsFromCommand("-swing", action);
                if (args.Count > 0)
                {
                    switch(args[0])
                    {
                        case "spawn":
                            if (args.Count > 1)
                            {
                                HandleSpawnAnimal(int.Parse(args[1]));
                            }
                            break;
                        case "load":
                            HandleLoad();
                            break;
                        case "success":
                            HandleSuccess();
                            break;
                        case "crash":
                            HandleCrash();
                            break;
                        case "increment":
                            HandleIncrementCounter();
                            break;
                        case "hide":
                            Hide();
                            break;
                    }
                }
            }
        }

        private void HandleSpawnAnimal(int counter)
        {
            Hide();

            List<Choice> choices = new List<Choice>();

            Image set = null;

            lastSetCounter_ = counter;

            switch(counter)
            {
                case 0:
                    choices.Add(bunny_);
                    choices.Add(ferret_);
                    choices.Add(bird_);

                    set = smallAnimal_;
                    correctSizeSwing_ = smallAnimalSeat_;
                    break;
                case 1:
                    choices.Add(zebra_);
                    choices.Add(seal_);
                    choices.Add(deer_);

                    set = mediumAnimal_;
                    correctSizeSwing_ = mediumAnimalSeat_;
                    break;
                case 2:
                    choices.Add(elephant_);
                    choices.Add(giraffe_);
                    choices.Add(hippo_);

                    set = largeAnimal_;
                    correctSizeSwing_ = largeAnimalSeat_;
                    break;
                case 3:
                    choices.Add(dino1_);
                    choices.Add(dino2_);
                    choices.Add(dino3_);

                    set = largeAnimal_;
                    correctSizeSwing_ = largeAnimalSeat_;
                    break;
            }

            int selection = Random.Range(0, choices.Count);

            animalWaiting_.gameObject.SetActive(true);
            animalWaiting_.sprite = choices[selection].AnimalWaiting;
            animalWaiting_.SetNativeSize();

            animalInSwing_ = set;
            set.gameObject.SetActive(false);
            set.sprite = choices[selection].Animal;
            set.SetNativeSize();
            successImage_.sprite = choices[selection].AnimalSuccess;
            failureImage_.sprite = choices[selection].AnimalFailure;

            currentIcon_ = choices[selection].AnimalIcon;

            AudioPlayer.PlayAudio(choices[selection].Sound);
        }

        //Put animal in the swing
        private void HandleLoad()
        {
            if (correctSizeSwing_ != null && animalInSwing_ != null)
            {
                animalWaiting_.gameObject.SetActive(false);

                correctSizeSwing_.gameObject.SetActive(true);
                animalInSwing_.gameObject.SetActive(true);
                AudioPlayer.PlayAudio("audio/sfx/bubble-pop");

                Go.to(this, 2f, new GoTweenConfig().onComplete(t => {
                    HandleSuccess();
                    
                }));
            }
        }

        private void HandleSuccess()
        {
            Hide();
            successImage_.gameObject.SetActive(true);
            AudioPlayer.PlayAudio("audio/sfx/applausetrumpet");
            Go.to(this, 3f, new GoTweenConfig().onComplete(t => {
                HandleIncrementCounter();
            }));
        }

        private void HandleIncrementCounter()
        {
            Hide();
            Image icon = Instantiate<Image>(iconPrefab_);
            icon.transform.localScale = Vector3.zero;
            icon.transform.SetParent(rsvpHolder_.transform);
            icon.transform.localScale = Vector3.one;
            icon.sprite = currentIcon_;
            Go.addTween(new GoTween(icon, 0.5f, new GoTweenConfig().scale(1f).setEaseType(GoEaseType.BounceIn)));
            AudioPlayer.PlayAudio("audio/sfx/ding");
            HandleSpawnAnimal(Mathf.Min(lastSetCounter_ + 1, 3));
        }

        private void HandleCrash()
        {
            Hide();
            failureImage_.gameObject.SetActive(true);
        }

        public override void Reset()
        {

        }
    }
}
