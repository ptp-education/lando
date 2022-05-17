using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lando.Class.Lego1
{
    public class SpawnedGuide : SpawnedObject
    {
        [SerializeField] private GameObject checklist_;
        [SerializeField] private GameObject exampleBridge_;
        [SerializeField] private GameObject exampleBridgeWater_;
        [SerializeField] private GameObject exampleBridgeBoulder_;
        [SerializeField] private GameObject exampleBridgePounds_;
        [SerializeField] private GameObject exampleSixBlocksWide_;
        [SerializeField] private GameObject hintCombineBridge_;
        [SerializeField] private GameObject hintPier_;
        [SerializeField] private GameObject hintThicker_;
        [SerializeField] private GameObject hintInterlocking_;

        public override void ReceivedAction(string action)
        {
            List<string> args = ArgumentHelper.ArgumentsFromCommand("-guide", action);
            if (args.Count > 0)
            {
                Hide();
                switch(args[0])
                {
                    case "checklist":
                        checklist_.gameObject.SetActive(true);
                        break;
                    case "bridge":
                        exampleBridge_.gameObject.SetActive(true);
                        AudioPlayer.PlayAudio("audio/sfx/bubble-pop");
                        break;
                    case "bridge-cross-water":
                        exampleBridgeWater_.gameObject.SetActive(true);
                        AudioPlayer.PlayAudio("audio/sfx/bubble-pop");
                        break;
                    case "bridge-over-boulder":
                        exampleBridgeBoulder_.gameObject.SetActive(true);
                        AudioPlayer.PlayAudio("audio/sfx/bubble-pop");
                        break;
                    case "bridge-2-pounds":
                        exampleBridgePounds_.gameObject.SetActive(true);
                        AudioPlayer.PlayAudio("audio/sfx/bubble-pop");
                        break;
                    case "combine-bridge":
                        hintCombineBridge_.gameObject.SetActive(true);
                        break;
                    case "pier-hint":
                        hintPier_.gameObject.SetActive(true);
                        break;
                    case "thicker-hint":
                        hintThicker_.gameObject.SetActive(true);
                        break;
                    case "interlocking-hint":
                        hintInterlocking_.gameObject.SetActive(true);
                        break;
                    case "6-blocks-wide":
                        exampleSixBlocksWide_.gameObject.SetActive(true);
                        break;
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

