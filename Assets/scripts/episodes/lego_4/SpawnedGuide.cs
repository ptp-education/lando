using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lando.Class.Lego4
{
    public class SpawnedGuide : SpawnedObject
    {
        [SerializeField] private GameObject checklist_;
        [SerializeField] private GameObject hintSupports_;
        [SerializeField] private GameObject hintInterlocking_;
        [SerializeField] private GameObject hintSupportsAndWalls_;
        [SerializeField] private GameObject hintMoreWalls_;
        [SerializeField] private GameObject hintFurtherApart_;
        [SerializeField] private GameObject baseplate_;
        [SerializeField] private GameObject taller_;
        [SerializeField] private GameObject explainer1_;
        [SerializeField] private GameObject explainer2_;
        [SerializeField] private GameObject explainer3_;
        [SerializeField] private GameObject explainer4_;

        public override void ReceivedAction(string action)
        {
            List<string> args = ArgumentHelper.ArgumentsFromCommand("-guide", action);
            if (args.Count > 0)
            {
                Hide();
                switch (args[0])
                {
                    case "checklist":
                        checklist_.SetActive(true);
                        break;

                    case "hintInterlocking":
                        hintInterlocking_.SetActive(true);
                        break;

                    case "hintSupports":
                        hintSupports_.SetActive(true);
                        break;

                    case "hintSupportsAndWalls":
                        hintSupportsAndWalls_.SetActive(true);
                        break;

                    case "hintMoreWalls":
                        hintMoreWalls_.SetActive(true);
                        break;

                    case "hintFurtherApart":
                        hintFurtherApart_.SetActive(true);
                        break;

                    case "baseplate":
                        baseplate_.SetActive(true);
                        break;

                    case "taller":
                        taller_.SetActive(true);
                        break;

                    case "explainer-1":
                        explainer1_.SetActive(true);
                        break;

                    case "explainer-2":
                        explainer2_.SetActive(true);
                        break;

                    case "explainer-3":
                        explainer3_.SetActive(true);
                        break;

                    case "explainer-4":
                        explainer4_.SetActive(true);
                        break;

                }
            }
        }

        public override void Hide()
        {
            base.Hide();
            HideAll();

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

        public override void Reset()
        {
            HideAll();

            //ShareManager sm = (ShareManager)gameManager_;
            //if (sm != null)
            //{
            //    transform.SetParent(sm.OverlayParent);
            //}
        }
    }
}

