using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lando.Class.Lego2
{
    public class SpawnedTower : SpawnedObject
    {
        [SerializeField] private Transform towerParent_;
        [SerializeField] private Transform map_;

        [SerializeField] private Image woodPrefab_;
        [SerializeField] private Image bronzePrefab_;
        [SerializeField] private Image silverPrefab_;
        [SerializeField] private Image goldPrefab_;
        [SerializeField] private Image obsidianPrefab_;

        private const string kWood = "wood";
        private const string kBronze = "bronze";
        private const string kSilver = "silver";
        private const string kGold = "gold";
        private const string kObsidian = "obsidian";

        private GoTweenFlow flow_;

        public override void ReceivedAction(string action)
        {
            string add = null;
            if (ArgumentHelper.ContainsCommand("-tower-add-wood", action))
            {
                add = kWood;
            } else if (ArgumentHelper.ContainsCommand("-tower-add-bronze", action))
            {
                add = kBronze;
            } else if (ArgumentHelper.ContainsCommand("-tower-add-silver", action))
            {
                add = kSilver;
            } else if (ArgumentHelper.ContainsCommand("-tower-add-gold", action))
            {
                add = kGold;
            } else if (ArgumentHelper.ContainsCommand("-tower-add-obsidian", action))
            {
                add = kObsidian;
            }

            if (add != null)
            {
                gameManager_.Storage.AddObjectToList<string>(GameStorage.Key.Lego2Tower, add);
                RefreshTower(true);
            }
        }

        private void RefreshTower(bool animate)
        {
            foreach(Image i in towerParent_.GetComponentsInChildren<Image>())
            {
                Destroy(i.gameObject);
            }

            if (flow_ != null)
            {
                flow_.complete();
            }

            flow_ = new GoTweenFlow();

            List<string> tower = gameManager_.Storage.GetValue<List<string>>(GameStorage.Key.Lego2Tower);

            if (tower == null)
            {
                return;
            }

            for (int i = 0; i < tower.Count; i++)
            {
                string t = tower[i];
                Image newTower = null;
                switch(t)
                {
                    case kWood:
                        newTower = Instantiate<Image>(woodPrefab_);
                        break;
                    case kBronze:
                        newTower = Instantiate<Image>(bronzePrefab_);
                        break;
                    case kSilver:
                        newTower = Instantiate<Image>(silverPrefab_);
                        break;
                    case kGold:
                        newTower = Instantiate<Image>(goldPrefab_);
                        break;
                    case kObsidian:
                        newTower = Instantiate<Image>(obsidianPrefab_);
                        break;
                }
                if (newTower != null)
                {
                    newTower.transform.SetParent(towerParent_);
                    newTower.transform.localPosition = new Vector3(0f, 350f + i * 350f, 0f);
                    newTower.transform.localScale = Vector3.one;
                }
            }

            Vector3 targetPosition = new Vector3(0f, 1183);
            Vector3 targetScale = new Vector3(1f, 1f);

            if (tower.Count > 5 && tower.Count <= 9)
            {
                targetPosition = new Vector3(0f, 836f);
                targetScale = new Vector3(0.75f, 0.75f);
            }
            else if (tower.Count > 9 && tower.Count <= 16)
            {
                targetPosition = new Vector3(0f, 478f);
                targetScale = new Vector3(0.5f, 0.5f);
            }
            else if (tower.Count > 16 && tower.Count <= 24)
            {
                targetPosition = new Vector3(0f, 175f);
                targetScale = new Vector3(0.5f, 0.5f);
            }
            else if (tower.Count > 24 && tower.Count <= 36)
            {
                targetPosition = new Vector3(0f, -282f);
                targetScale = new Vector3(0.5f, 0.5f);
            }
            else if (tower.Count > 36)
            {
                targetPosition = new Vector3(0f, -471f);
                targetScale = new Vector3(0.5f, 0.5f);
            }

            if (animate)
            {
                if (map_.transform.localPosition.y != targetPosition.y)
                {
                    flow_.insert(0f, new GoTween(map_.transform, 1.5f, new GoTweenConfig().localPosition(targetPosition)));
                    flow_.insert(0f, new GoTween(map_.transform, 1.5f, new GoTweenConfig().scale(targetScale)));
                    flow_.play();
                }
            } else
            {
                map_.transform.localPosition = targetPosition;
                map_.transform.localScale = targetScale;
            }
        }

        public override void Reset()
        {
            RefreshTower(false);
        }
    }
}