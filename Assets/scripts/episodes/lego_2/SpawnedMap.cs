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
            if (ArgumentHelper.ContainsCommand("-highlightWood", action))
            {
                HideAll();
                highlightWood_.gameObject.SetActive(true);
            } else if (ArgumentHelper.ContainsCommand("-highlightBronze", action))
            {
                HideAll();
                highlightBronze_.gameObject.SetActive(true);
            } else if (ArgumentHelper.ContainsCommand("-highlightSilver", action))
            {
                HideAll();
                highlightSilver_.gameObject.SetActive(true);
            }
            else if (ArgumentHelper.ContainsCommand("-highlightGold", action))
            {
                HideAll();
                highlightGold_.gameObject.SetActive(true);
            }
            else if (ArgumentHelper.ContainsCommand("-highlightObsidian", action))
            {
                HideAll();
                highlightObsidian_.gameObject.SetActive(true);
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

