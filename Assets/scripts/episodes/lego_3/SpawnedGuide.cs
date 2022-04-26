using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lando.Class.Lego3
{
    public class SpawnedGuide : SpawnedObject
    {
        [SerializeField] private GameObject craneExplainer1_;
        [SerializeField] private GameObject craneExplainer2_;
        [SerializeField] private GameObject craneExplainer3_;
        [SerializeField] private GameObject craneExplainer4_;
        [SerializeField] private GameObject craneExplainer5_;
        [SerializeField] private GameObject craneExplainer6_;
        [SerializeField] private GameObject checklist_;

        [SerializeField] private GameObject hintAdditionalBeam_;
        [SerializeField] private GameObject hintCounterweight_;
        [SerializeField] private GameObject hintMoreCounterweight_;
        [SerializeField] private GameObject hintFurtherCounterweight_;
        [SerializeField] private GameObject hintNotch_;
        [SerializeField] private GameObject hintInterlocking_;
        [SerializeField] private GameObject hintLayers_;

        public override void ReceivedAction(string action)
        {
            List<string> args = ArgumentHelper.ArgumentsFromCommand("-guide", action);
            if (args.Count > 0)
            {
                Hide();
                switch (args[0])
                {
                    case "explainer-1":
                        craneExplainer1_.SetActive(true);
                        break;
                    case "explainer-2":
                        craneExplainer2_.SetActive(true);
                        break;
                    case "explainer-3":
                        craneExplainer3_.SetActive(true);
                        break;
                    case "explainer-4":
                        craneExplainer4_.SetActive(true);
                        break;
                    case "explainer-5":
                        craneExplainer5_.SetActive(true);
                        break;
                    case "explainer-6":
                        craneExplainer6_.SetActive(true);
                        break;
                    case "additional-beam":
                        hintAdditionalBeam_.SetActive(true);
                        break;
                    case "counterweight":
                        hintCounterweight_.SetActive(true);
                        break;
                    case "more-counterweight":
                        hintMoreCounterweight_.SetActive(true);
                        break;
                    case "further-counterweight":
                        hintFurtherCounterweight_.SetActive(true);
                        break;
                    case "notch":
                        hintNotch_.SetActive(true);
                        break;
                    case "interlocking":
                        hintInterlocking_.SetActive(true);
                        break;
                    case "layers":
                        hintLayers_.SetActive(true);
                        break; ;
                }
            }
        }

        public override void Hide()
        {
            base.Hide();
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
            Hide();

            //ShareManager sm = (ShareManager)gameManager_;
            //if (sm != null)
            //{
            //    transform.SetParent(sm.OverlayParent);
            //}
        }
    }
}

