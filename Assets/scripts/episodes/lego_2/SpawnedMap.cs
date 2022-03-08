using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lando.Class.Lego2
{
    public class SpawnedMap : SpawnedObject
    {
        [SerializeField] private Image highlightWood_;
        [SerializeField] private Image highlightBronze_;
        [SerializeField] private Image highlightSilver_;
        [SerializeField] private Image highlightGold_;
        [SerializeField] private Image highlightObsidian_;

        public override void ReceivedAction(string action)
        {
            if (ArgumentHelper.ContainsCommand("-highlightMap", action))
            {
                List<string> args = ArgumentHelper.ArgumentsFromCommand("-highlightMap", action);

                HideAll();

                Image setActive = null;
                switch(args[0])
                {
                    case "wood":
                        setActive = highlightWood_;
                        break;
                    case "bronze":
                        setActive = highlightBronze_;
                        break;
                    case "silver":
                        setActive = highlightSilver_;
                        break;
                    case "gold":
                        setActive = highlightGold_;
                        break;
                    case "obsidian":
                        setActive = highlightObsidian_;
                        break;
                }

                if (args.Count > 1)
                {
                    float delay = float.Parse(args[1]);
                    Go.to(this, delay, new GoTweenConfig().onComplete(t =>
                    {
                        setActive.gameObject.SetActive(true);
                    }));
                } else
                {
                    setActive.gameObject.SetActive(true);
                }
            }
        }

        private void HideAll()
        {
            highlightWood_.gameObject.SetActive(false);
            highlightBronze_.gameObject.SetActive(false);
            highlightSilver_.gameObject.SetActive(false);
            highlightGold_.gameObject.SetActive(false);
            highlightObsidian_.gameObject.SetActive(false);
        }

        public override void Reset()
        {
            HideAll();
        }
    }
}

