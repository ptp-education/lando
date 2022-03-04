using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lando.Class.Lego5
{
    public class SpawnedSetup : SpawnedObject
    {
        [SerializeField] private GameObject swing_;
        [SerializeField] private GameObject swingAnimal_;
        [SerializeField] private GameObject checklist_;
        [SerializeField] private GameObject hintUseString_;
        [SerializeField] private GameObject hintTightString_;
        [SerializeField] private GameObject hintEachString_;
        [SerializeField] private GameObject hintMoreString_;
        [SerializeField] private GameObject hintWiderTower_;

        public override void ReceivedAction(string action)
        {
            if (ArgumentHelper.ContainsCommand("-guideOverlaySwing", action))
            {
                HideAll();
                swing_.SetActive(true);
            } else if (ArgumentHelper.ContainsCommand("-guideOverlaySwingAnimal", action))
            {
                HideAll();
                swingAnimal_.SetActive(true);
            } else if (ArgumentHelper.ContainsCommand("-guideChecklist", action))
            {
                HideAll();
                checklist_.SetActive(true);
            }
            else if (ArgumentHelper.ContainsCommand("-guideHintUseString", action))
            {
                HideAll();
                hintUseString_.SetActive(true);
            }
            else if (ArgumentHelper.ContainsCommand("-guideHintTightString", action))
            {
                HideAll();
                hintTightString_.SetActive(true);
            }
            else if (ArgumentHelper.ContainsCommand("-guideHintEachSide", action))
            {
                HideAll();
                hintEachString_.SetActive(true);
            }
            else if (ArgumentHelper.ContainsCommand("-guideHintMoreString", action))
            {
                HideAll();
                hintMoreString_.SetActive(true);
            }
            else if (ArgumentHelper.ContainsCommand("-guideHintWiderTower", action))
            {
                HideAll();
                hintWiderTower_.SetActive(true);
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

