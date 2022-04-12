using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lando.Class.Lego5
{
    public class SpawnedGuide : SpawnedObject
    {
        [SerializeField] private GameObject swing_;
        [SerializeField] private GameObject swingAnimal_;
        [SerializeField] private GameObject checklist_;
        [SerializeField] private GameObject exampleSwing1_;
        [SerializeField] private GameObject exampleSwing2_;
        [SerializeField] private GameObject exampleSwing3_;
        [SerializeField] private GameObject exampleSwing4_;
        [SerializeField] private GameObject exampleSwing5_;
        [SerializeField] private GameObject hintUseString_;
        [SerializeField] private GameObject hintTightString_;
        [SerializeField] private GameObject hintEachString_;
        [SerializeField] private GameObject hintMoreString_;
        [SerializeField] private GameObject hintWiderTower_;
        [SerializeField] private GameObject hintArch_;
        [SerializeField] private GameObject hintHigherUp_;

        public override void ReceivedAction(string action)
        {
            List<string> args = ArgumentHelper.ArgumentsFromCommand("-guide", action);
            if (args.Count > 0)
            {
                HideAll();
                switch(args[0])
                {
                    case "swing":
                        swing_.SetActive(true);
                        AudioPlayer.PlayAudio("audio/sfx/bubble-pop");
                        break;
                    case "animal":
                        swingAnimal_.SetActive(true);
                        AudioPlayer.PlayAudio("audio/sfx/bubble-pop");
                        break;
                    case "checklist":
                        checklist_.SetActive(true);
                        break;
                    case "hint-string":
                        hintUseString_.SetActive(true);
                        break;
                    case "hint-tight-string":
                        hintTightString_.SetActive(true);
                        break;
                    case "hint-each-side":
                        hintEachString_.SetActive(true);
                        break;
                    case "hint-more-string":
                        hintMoreString_.SetActive(true);
                        break;
                    case "hint-wider-tower":
                        hintWiderTower_.SetActive(true);
                        break;
                    case "hint-arch":
                        hintArch_.SetActive(true);
                        break;
                    case "hint-higher":
                        hintHigherUp_.SetActive(true);
                        break;
                    case "explainer-1":
                        exampleSwing1_.SetActive(true);
                        break;
                    case "explainer-2":
                        exampleSwing2_.SetActive(true);
                        break;
                    case "explainer-3":
                        exampleSwing3_.SetActive(true);
                        break;
                    case "explainer-4":
                        exampleSwing4_.SetActive(true);
                        break;
                    case "explainer-5":
                        exampleSwing5_.SetActive(true);
                        break;
                }
            }
        }

        private void HideAll()
        {
            Transform[] objects = GetComponentsInChildren<Transform>();
            for (int i = 0; i < objects.Length; i++)
            {
                if (objects[i].gameObject != this.gameObject)
                {
                    objects[i].gameObject.SetActive(false);
                }
            }
        }

        public override void Hide()
        {
            base.Hide();
            HideAll();
        }

        public override void Reset()
        {
            HideAll();

            ShareManager sm = (ShareManager)gameManager_;
            if (sm != null)
            {
                transform.SetParent(sm.OverlayParent);
            }
        }
    }
}

