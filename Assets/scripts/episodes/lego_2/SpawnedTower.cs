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
        [SerializeField] private Text levelText_;

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
                GameManager.Storage.AddObjectToList<string>(GameStorage.Key.Lego2Tower, add);
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

            List<string> tower = GameManager.Storage.GetValue<List<string>>(GameStorage.Key.Lego2Tower);

            if (tower == null)
            {
                return;
            }

            levelText_.transform.localScale = tower.Count == 0 ? Vector3.zero : Vector3.one;

            Vector3 targetPosition = new Vector3(0f, 1830);
            Vector3 targetScale = new Vector3(1f, 1f);
            Vector3 zoomPosition = new Vector3(0, 2826);
            Vector3 zoomScale = new Vector3(2f, 2f);

            if (tower.Count > 2 && tower.Count <= 4)
            {
                targetPosition = new Vector3(0f, 1532f);
                targetScale = new Vector3(0.85f, 0.85f);
                zoomPosition = new Vector3(0, 2335);
                zoomScale = new Vector3(2f, 2f);
            }
            else if (tower.Count > 4 && tower.Count <= 6)
            {
                targetPosition = new Vector3(0f, 1320f);
                targetScale = new Vector3(0.75f, 0.75f);
                zoomPosition = new Vector3(0, 1960);
                zoomScale = new Vector3(2f, 2f);
            }
            else if (tower.Count > 6 && tower.Count <= 10)
            {
                targetPosition = new Vector3(0f, 813f);
                targetScale = new Vector3(0.5f, 0.5f);
                zoomPosition = new Vector3(0, 990);
                zoomScale = new Vector3(1.5f, 1.5f);
            }
            else if (tower.Count > 10 && tower.Count <= 14)
            {
                targetPosition = new Vector3(0f, 813f);
                targetScale = new Vector3(0.5f, 0.5f);
                zoomPosition = new Vector3(0, 658);
                zoomScale = new Vector3(1.5f, 1.5f);
            }
            else if (tower.Count > 14 && tower.Count <= 20)
            {
                targetPosition = new Vector3(0f, 525f);
                targetScale = new Vector3(0.5f, 0.5f);
                zoomPosition = new Vector3(0, 71);
                zoomScale = new Vector3(1.5f, 1.5f);
            }
            else if (tower.Count > 20 && tower.Count <= 25)
            {
                targetPosition = new Vector3(0f, 125f);
                targetScale = new Vector3(0.5f, 0.5f);
                zoomPosition = new Vector3(0, -773);
                zoomScale = new Vector3(1.5f, 1.5f);
            }
            else if (tower.Count > 25 && tower.Count <= 30)
            {
                targetPosition = new Vector3(0f, 125f);
                targetScale = new Vector3(0.5f, 0.5f);
                zoomPosition = new Vector3(0, -1337);
                zoomScale = new Vector3(1.5f, 1.5f);
            }
            else if (tower.Count > 30)
            {
                targetPosition = new Vector3(0f, -136f);
                targetScale = new Vector3(0.5f, 0.5f);
                zoomPosition = new Vector3(0, -136f);
                zoomScale = new Vector3(0.5f, 0.5f);
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
                    newTower.transform.SetAsFirstSibling();
                    newTower.transform.localPosition = new Vector3(0f, 350f + i * 350f, 0f);
                    newTower.transform.localScale = new Vector3(1f, 1f);

                    if (animate && i == tower.Count - 1)
                    {
                        newTower.transform.localPosition = new Vector3(0f, 350f + (i-1) * 350f, 0f);
                        newTower.transform.localScale = new Vector3(0.5f, 0.5f);

                        flow_.insert(2f, new GoTween(map_.transform, 0.01f, new GoTweenConfig().onComplete(t =>
                        {
                            AudioPlayer.PlayAudio("audio/sfx/wind-whoosh");
                        })));
                        flow_.insert(2f, new GoTween(map_.transform, 1f, new GoTweenConfig().localPosition(zoomPosition)));
                        flow_.insert(2f, new GoTween(map_.transform, 1f, new GoTweenConfig().scale(zoomScale)));
                        flow_.insert(3f, new GoTween(newTower.transform, 1.5f, new GoTweenConfig().scale(Vector3.one)));
                        flow_.insert(3f, new GoTween(newTower.transform, 1.5f, new GoTweenConfig().localPosition(new Vector3(0f, 350f + i * 350f, 0f))));
                        flow_.insert(3f, new GoTween(map_.transform, 0.01f, new GoTweenConfig().onComplete(t =>
                        {
                            AudioPlayer.PlayAudio("audio/sfx/drum-cheer");
                        })));
                        flow_.insert(3f, new GoTween(levelText_.transform, 1.5f, new GoTweenConfig().localPosition(new Vector3(0f, 800f + i * 350f, 0f))));
                        flow_.insert(5.5f, new GoTween(map_.transform, 0.01f, new GoTweenConfig().onComplete(t =>
                        {
                            AudioPlayer.PlayAudio("audio/sfx/bubble-pop");
                            levelText_.text = "Level " + tower.Count.ToString();
                        })));
                        flow_.insert(7.5f, new GoTween(levelText_.transform, 0.01f, new GoTweenConfig().onComplete(t =>
                        {
                            AudioPlayer.PlayAudio("audio/sfx/wind-whoosh");
                        })));
                        flow_.insert(7.5f, new GoTween(map_.transform, 1f, new GoTweenConfig().localPosition(targetPosition)));
                        flow_.insert(7.5f, new GoTween(map_.transform, 1f, new GoTweenConfig().scale(targetScale)));

                        flow_.play();
                    } else
                    {
                        levelText_.transform.localPosition = new Vector3(0f, 800f + i * 350f, 0f);
                        levelText_.text = "Level " + tower.Count.ToString();
                    }
                }
            }

            if (!animate)
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