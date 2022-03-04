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

        #region SPRITE
        [SerializeField] private Sprite bird_;
        [SerializeField] private Sprite birdFailure_;
        [SerializeField] private Sprite birdSuccess_;
        [SerializeField] private Sprite bunny_;
        [SerializeField] private Sprite bunnyFailure_;
        [SerializeField] private Sprite bunnySuccess_;
        [SerializeField] private Sprite ferret_;
        [SerializeField] private Sprite ferretFailure_;
        [SerializeField] private Sprite ferretSuccess_;

        [SerializeField] private Sprite deer_;
        [SerializeField] private Sprite deerFailure_;
        [SerializeField] private Sprite deerSuccess_;
        [SerializeField] private Sprite zebra_;
        [SerializeField] private Sprite zebraFailure_;
        [SerializeField] private Sprite zebraSuccess_;
        [SerializeField] private Sprite seal_;
        [SerializeField] private Sprite sealFailure_;
        [SerializeField] private Sprite sealSuccess_;

        [SerializeField] private Sprite elephant_;
        [SerializeField] private Sprite elephantFailure_;
        [SerializeField] private Sprite elephantSuccess_;
        [SerializeField] private Sprite giraffe_;
        [SerializeField] private Sprite giraffeFailure_;
        [SerializeField] private Sprite giraffeSuccess_;
        [SerializeField] private Sprite hippo_;
        [SerializeField] private Sprite hippoFailure_;
        [SerializeField] private Sprite hippoSuccess_;
        #endregion

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

            List<Sprite> choices = new List<Sprite>();
            List<Sprite> success = new List<Sprite>();
            List<Sprite> failure = new List<Sprite>();

            Image set = null;

            switch(weight)
            {
                case "100":
                    choices.Add(bunny_);
                    success.Add(bunnySuccess_);
                    failure.Add(bunnyFailure_);
                    choices.Add(ferret_);
                    success.Add(ferretSuccess_);
                    failure.Add(ferretFailure_);
                    choices.Add(bird_);
                    success.Add(birdSuccess_);
                    failure.Add(birdFailure_);

                    set = smallAnimal_;
                    smallAnimalSeat_.gameObject.SetActive(true);
                    break;
                case "200":
                    choices.Add(zebra_);
                    success.Add(zebraSuccess_);
                    failure.Add(zebraFailure_);
                    choices.Add(seal_);
                    success.Add(sealSuccess_);
                    failure.Add(sealFailure_);
                    choices.Add(deer_);
                    success.Add(deerSuccess_);
                    failure.Add(deerFailure_);

                    set = mediumAnimal_;
                    mediumAnimalSeat_.gameObject.SetActive(true);
                    break;
                case "500":
                    choices.Add(elephant_);
                    success.Add(elephantSuccess_);
                    failure.Add(elephantFailure_);
                    choices.Add(giraffe_);
                    success.Add(giraffeSuccess_);
                    failure.Add(giraffeFailure_);
                    choices.Add(hippo_);
                    success.Add(hippoSuccess_);
                    failure.Add(hippoFailure_);

                    set = largeAnimal_;
                    largeAnimalSeat_.gameObject.SetActive(true);
                    break;
            }
            int selection = Random.Range(0, choices.Count);

            set.gameObject.SetActive(true);
            set.sprite = choices[selection];
            set.SetNativeSize();
            successImage_.sprite = success[selection];
            failureImage_.sprite = failure[selection];
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
