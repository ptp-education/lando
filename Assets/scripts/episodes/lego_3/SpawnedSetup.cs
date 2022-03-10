using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lando.Class.Lego3
{
    public class SpawnedSetup : SpawnedObject
    {
        [SerializeField] private GameObject craneOverlay_;
        [SerializeField] private GameObject craneOverlayWeight_;
        [SerializeField] private GameObject checklist_;
        [SerializeField] private GameObject hint1Cup_;
        [SerializeField] private GameObject hintAdditionalBeam_;
        [SerializeField] private GameObject hintCounterweight_;
        [SerializeField] private GameObject hintAdditionalCounterweight_;
        [SerializeField] private GameObject hintNotch_;
        [SerializeField] private GameObject hintFurtherCounterweight_;
        [SerializeField] private GameObject hintInterlocking_;
        [SerializeField] private GameObject hintMoreLayers_;
        [SerializeField] private GameObject crane1_;
        [SerializeField] private GameObject crane2_;
        [SerializeField] private GameObject crane3_;


        public override void ReceivedAction(string action)
        {
            if (ArgumentHelper.ContainsCommand("-guideCraneOverlay", action))
            {
                HideAll();
                craneOverlay_.SetActive(true);
            } else if (ArgumentHelper.ContainsCommand("-guideCraneOverlayWeight", action))
            {
                HideAll();
                craneOverlayWeight_.SetActive(true);
            } else if (ArgumentHelper.ContainsCommand("-guideCrane1Cup", action))
            {
                HideAll();
                hint1Cup_.SetActive(true);
            }
            else if (ArgumentHelper.ContainsCommand("-guideHintAdditionalBeam", action))
            {
                HideAll();
                hintAdditionalBeam_.SetActive(true);
            }
            else if (ArgumentHelper.ContainsCommand("-guideHintCounterweight", action))
            {
                HideAll();
                hintCounterweight_.SetActive(true);
            }
            else if (ArgumentHelper.ContainsCommand("-guideHintAdditionalCounterweight", action))
            {
                HideAll();
                hintAdditionalCounterweight_.SetActive(true);
            }
            else if (ArgumentHelper.ContainsCommand("-guideHintNotch", action))
            {
                HideAll();
                hintNotch_.SetActive(true);
            }
            else if (ArgumentHelper.ContainsCommand("-guideHintFurtherCounterweight", action))
            {
                HideAll();
                hintFurtherCounterweight_.SetActive(true);
            }
            else if (ArgumentHelper.ContainsCommand("-guideHintInterlocking", action))
            {
                HideAll();
                hintInterlocking_.SetActive(true);
            }
            else if (ArgumentHelper.ContainsCommand("-guideHintMoreLayers", action))
            {
                HideAll();
                hintMoreLayers_.SetActive(true);
            }
            else if (ArgumentHelper.ContainsCommand("-guideChecklist", action))
            {
                HideAll();
                checklist_.SetActive(true);
            }
            else if (ArgumentHelper.ContainsCommand("-guideRealCrane", action))
            {
                HideAll();
                crane1_.SetActive(true);
            }
            else if (ArgumentHelper.ContainsCommand("-guideCraneSculpture", action))
            {
                HideAll();
                crane2_.SetActive(true);
            }
            else if (ArgumentHelper.ContainsCommand("-guideCraneCounterweight", action))
            {
                HideAll();
                crane3_.SetActive(true);
            }
            else if (ArgumentHelper.ContainsCommand("-guideHideGuides", action))
            {
                HideAll();
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

