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

        [SerializeField] private Choice bird_;
        [SerializeField] private Choice bunny_;
        [SerializeField] private Choice ferret_;

        [SerializeField] private Choice deer_;
        [SerializeField] private Choice zebra_;
        [SerializeField] private Choice seal_;

        [SerializeField] private Choice elephant_;
        [SerializeField] private Choice giraffe_;
        [SerializeField] private Choice hippo_;

        [System.Serializable]
        public class Choice
        {
            public Sprite Animal;
            public Sprite AnimalFailure;
            public Sprite AnimalSuccess;
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
                            if (args.Count > 1) HandleSpawnAnimal(args[1]);
                            break;
                        case "success":
                            HandleSuccess();
                            break;
                        case "crash":
                            HandleCrash();
                            break;
                    }
                }
            }
        }

        private void HandleSpawnAnimal(string weight)
        {
            Hide();

            List<Choice> choices = new List<Choice>();

            Image set = null;

            switch(weight)
            {
                case "100":
                    choices.Add(bunny_);
                    choices.Add(ferret_);
                    choices.Add(bird_);

                    set = smallAnimal_;
                    smallAnimalSeat_.gameObject.SetActive(true);
                    break;
                case "200":
                    choices.Add(zebra_);
                    choices.Add(seal_);
                    choices.Add(deer_);

                    set = mediumAnimal_;
                    mediumAnimalSeat_.gameObject.SetActive(true);
                    break;
                case "500":
                    choices.Add(elephant_);
                    choices.Add(giraffe_);
                    choices.Add(hippo_);

                    set = largeAnimal_;
                    largeAnimalSeat_.gameObject.SetActive(true);
                    break;
            }
            int selection = Random.Range(0, choices.Count);

            set.gameObject.SetActive(true);
            set.sprite = choices[selection].Animal;
            set.SetNativeSize();
            successImage_.sprite = choices[selection].AnimalSuccess;
            failureImage_.sprite = choices[selection].AnimalFailure;
            AudioPlayer.PlayAudio(choices[selection].Sound);
        }

        private void HandleSuccess()
        {
            Hide();
            successImage_.gameObject.SetActive(true);
            gameManager_.SendNewAction("-increase-counter");
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
