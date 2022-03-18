using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lando.Class.Lego2
{
    public class SpawnedGuide : SpawnedObject
    {
        [SerializeField] private GameObject staircase_;
        [SerializeField] private GameObject riverGuide_;
        [SerializeField] private GameObject riverGuideBridge_;
        [SerializeField] private GameObject riverGuideWater_;
        [SerializeField] private GameObject riverGuideMonster_;
        [SerializeField] private GameObject checklist_;
        [SerializeField] private GameObject checklistBridge_;
        [SerializeField] private GameObject checklistBridgeTall_;
        [SerializeField] private GameObject checklistBridgeWider_;
        [SerializeField] private GameObject checklistBridgeClimbable_;
        [SerializeField] private GameObject checklistBridgeStronger_;

        [SerializeField] private GameObject hintBaseplate_;
        [SerializeField] private GameObject hintBuildOnTable_;
        [SerializeField] private GameObject hintThicker_;
        [SerializeField] private GameObject hintFence_;
        [SerializeField] private GameObject hintFenceBack_;
        [SerializeField] private GameObject hintFenceFront_;
        [SerializeField] private GameObject hintFenceFence_;

        [SerializeField] private GameObject bridge18Pieces_;
        [SerializeField] private GameObject bridge30Pieces_;
        [SerializeField] private GameObject bridge70Pieces_;

        public override void ReceivedAction(string action)
        {
            if (ArgumentHelper.ContainsCommand("-guideStaircase", action))
            {
                HideAll();
                staircase_.SetActive(true);
            } else if (ArgumentHelper.ContainsCommand("-guideHideStaircase", action))
            {
                HideAll();
                staircase_.SetActive(false);
            } else if (ArgumentHelper.ContainsCommand("-guideRiverGuide", action))
            {
                HideAll();
                riverGuide_.SetActive(true);
            }
            else if (ArgumentHelper.ContainsCommand("-guideRiverGuideBridge", action))
            {
                HideAll();
                riverGuideBridge_.SetActive(true);
            }
            else if (ArgumentHelper.ContainsCommand("-guideRiverGuideWater", action))
            {
                HideAll();
                riverGuideWater_.SetActive(true);
            }
            else if (ArgumentHelper.ContainsCommand("-guideRiverGuideMonster", action))
            {
                HideAll();
                riverGuideMonster_.SetActive(true);
            }
            else if (ArgumentHelper.ContainsCommand("-guideChecklist", action))
            {
                HideAll();
                checklist_.SetActive(true);
            }
            else if (ArgumentHelper.ContainsCommand("-guideBridgeChecklist", action))
            {
                HideAll();
                checklistBridge_.SetActive(true);
            }
            else if (ArgumentHelper.ContainsCommand("-guideBridgeChecklistWider", action))
            {
                HideAll();
                checklistBridgeWider_.SetActive(true);
            }
            else if (ArgumentHelper.ContainsCommand("-guideBridgeChecklistTaller", action))
            {
                HideAll();
                checklistBridgeTall_.SetActive(true);
            }
            else if (ArgumentHelper.ContainsCommand("-guideBridgeChecklistClimbable", action))
            {
                HideAll();
                checklistBridgeClimbable_.SetActive(true);
            }
            else if (ArgumentHelper.ContainsCommand("-guideBridgeChecklistStronger", action))
            {
                HideAll();
                checklistBridgeStronger_.SetActive(true);
            }
            else if (ArgumentHelper.ContainsCommand("-guideHintBaseplate", action))
            {
                HideAll();
                hintBaseplate_.SetActive(true);
            }
            else if (ArgumentHelper.ContainsCommand("-guideHintTable", action))
            {
                HideAll();
                hintBuildOnTable_.SetActive(true);
            }
            else if (ArgumentHelper.ContainsCommand("-guideHintThicker", action))
            {
                HideAll();
                hintThicker_.SetActive(true);
            }
            else if (ArgumentHelper.ContainsCommand("-guideHintFence", action))
            {
                HideAll();
                hintFence_.SetActive(true);
                hintFenceBack_.SetActive(true);
                hintFenceFence_.SetActive(true);
                hintFenceFront_.SetActive(true);
            }
            else if (ArgumentHelper.ContainsCommand("-guideBridge18", action))
            {
                HideAll();
                bridge18Pieces_.SetActive(true);
            }
            else if (ArgumentHelper.ContainsCommand("-guideBridge30", action))
            {
                HideAll();
                bridge30Pieces_.SetActive(true);
            }
            else if (ArgumentHelper.ContainsCommand("-guideBridge70", action))
            {
                HideAll();
                bridge70Pieces_.SetActive(true);
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

