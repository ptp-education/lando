using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lando.Class.Lego4
{
    public class SpawnedSetup : SpawnedObject
    {
        [SerializeField] private GameObject checklist_;
        [SerializeField] private GameObject hintSupports_;
        [SerializeField] private GameObject hintInterlocking_;
        [SerializeField] private GameObject hintSupportsAndWalls_;
        [SerializeField] private GameObject hintMoreWalls_;
        [SerializeField] private GameObject hintFurtherApart_;
        [SerializeField] private GameObject baseplate_;
        [SerializeField] private GameObject taller_;

        public override void ReceivedAction(string action)
        {
            if (ArgumentHelper.ContainsCommand("-guideChecklist", action))
            {
                HideAll();
                checklist_.SetActive(true);
            } else if (ArgumentHelper.ContainsCommand("-guideHintInterlocking", action))
            {
                HideAll();
                hintInterlocking_.SetActive(true);
            } else if (ArgumentHelper.ContainsCommand("-guideHintSupports", action))
            {
                HideAll();
                hintSupports_.SetActive(true);
            }
            else if (ArgumentHelper.ContainsCommand("-guideHintSupportsAndWall", action))
            {
                HideAll();
                hintSupportsAndWalls_.SetActive(true);
            }
            else if (ArgumentHelper.ContainsCommand("-guideHintMoreWalls", action))
            {
                HideAll();
                hintMoreWalls_.SetActive(true);
            }
            else if (ArgumentHelper.ContainsCommand("-guideHintFurtherApart", action))
            {
                HideAll();
                hintFurtherApart_.SetActive(true);
            }
            else if (ArgumentHelper.ContainsCommand("-guideBaseplate", action))
            {
                HideAll();
                baseplate_.SetActive(true);
            }
            else if (ArgumentHelper.ContainsCommand("-guideTaller", action))
            {
                HideAll();
                taller_.SetActive(true);
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

