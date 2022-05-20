using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

namespace Lando.Class.Lego1
{
    public class SpawnedSelector : SpawnedObject
    {
        [SerializeField] private Transform selectorBackground_;
        [SerializeField] private List<Image> holderImages_;

        [SerializeField] private List<FarmOption> sixWideOptions_;
        [SerializeField] private List<FarmOption> eightWideOptions_;
        [SerializeField] private List<FarmOption> tenWideOptions_;

        private List<FarmOption> activeFarmOptions_;

        [Serializable]
        public class FarmOption
        {
            public string Name;
            public Sprite FarmSprite;
        }


        public override void ReceivedAction(string action)
        {
            List<string> farmObjects = GameManager.Storage.GetValue<List<string>>(GameStorage.Key.FarmObjects);
            if (farmObjects == null)
            {
                farmObjects = new List<string>();
            }

            List<string> args = ArgumentHelper.ArgumentsFromCommand("-farm-selector", action);
            if (args.Count > 0)
            {
                switch(args[0])
                {
                    case "show":
                        selectorBackground_.transform.localPosition = new Vector3(0f, 340f, 0);
                        AudioPlayer.PlayAudio("audio/sfx/door-open");

                        List<FarmOption> options = null;
                        switch(args[1])
                        {
                            case "1":
                                options = FarmOptions(1, farmObjects);
                                break;
                            case "2":
                                options = FarmOptions(2, farmObjects);
                                break;
                            case "3":
                                options = FarmOptions(3, farmObjects);
                                break;
                        }

                        activeFarmOptions_ = options;

                        for (int i = 0; i < holderImages_.Count; i++)
                        {
                            if (options.Count > i)
                            {
                                holderImages_[i].sprite = options[i].FarmSprite;
                                holderImages_[i].SetNativeSize();
                            }
                        }

                        break;
                    case "select":
                        string selectedKey = null;
                        switch (args[1])
                        {
                            case "1":
                                if (activeFarmOptions_.Count > 0) selectedKey = activeFarmOptions_[0].Name;
                                break;
                            case "2":
                                if (activeFarmOptions_.Count > 1) selectedKey = activeFarmOptions_[1].Name;
                                break;
                            case "3":
                                if (activeFarmOptions_.Count > 2) selectedKey = activeFarmOptions_[2].Name;
                                break;
                        }
                        if (selectedKey != null)
                        {
                            gameManager_.SendNewAction(
                                GameManager.FADEIN_COMMAND + " 0.8"
                            );
                            Go.to(transform, 0.5f, new GoTweenConfig().onComplete(t =>
                            {
                                gameManager_.SendNewAction(
                                    "add-farm-" + selectedKey +
                                    " -rfid hide");
                            }));
                            Hide();
                        }
                        break;
                    case "hide":
                        Hide();
                        break;
                }
            }
        }

        List<FarmOption> FarmOptions(int level, List<string> farmStorage)
        {
            List<FarmOption> ret = new List<FarmOption>();

            List<FarmOption> options = new List<FarmOption>();
            switch(level)
            {
                case 1:
                    options.AddRange(sixWideOptions_);
                    break;
                case 2:
                    options.AddRange(eightWideOptions_);
                    options.AddRange(sixWideOptions_);
                    break;
                case 3:
                    options.AddRange(tenWideOptions_);
                    options.AddRange(eightWideOptions_);
                    options.AddRange(sixWideOptions_);
                    break;
            }

            foreach(FarmOption farmOption in options)
            {
                if (!farmStorage.Contains(farmOption.Name))
                {
                    ret.Add(farmOption);
                    if (ret.Count > 2) break;
                }
            }

            return ret;
        }

        public override void Hide()
        {
            base.Hide();
            selectorBackground_.transform.localPosition = new Vector3(0f, 752f, 0);
        }

        public override void Reset()
        {

        }
    }
}
