using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lando.Class.Lego3
{
    public class SpawnedArtHolder : SpawnedObject
    {
        [SerializeField] private Image levelHolder_;
        [SerializeField] private Image artPiece1Holder_;
        [SerializeField] private Image artPiece2Holder_;
        [SerializeField] private Image artPiece3Holder_;

        [SerializeField] private List<Image> levelOneSmallHolders = new List<Image>();
        [SerializeField] private List<Image> levelOneMedHolders = new List<Image>();
        [SerializeField] private List<Image> levelOneLargeHolders = new List<Image>();
        [SerializeField] private List<Image> levelTwoSmallHolders = new List<Image>();
        [SerializeField] private List<Image> levelTwoMedHolders = new List<Image>();
        [SerializeField] private List<Image> levelTwoLargeHolders = new List<Image>();
        [SerializeField] private List<Image> levelThreeSmallHolders = new List<Image>();
        [SerializeField] private List<Image> levelThreeMedHolders = new List<Image>();
        [SerializeField] private List<Image> levelThreeLargeHolders = new List<Image>();

        [SerializeField] private Sprite levelOne100Holder_;
        [SerializeField] private Sprite levelOne200Holder_;
        [SerializeField] private Sprite levelOne500Holder_;
        [SerializeField] private Sprite levelTwo100Holder_;
        [SerializeField] private Sprite levelTwo200Holder_;
        [SerializeField] private Sprite levelTwo500Holder_;
        [SerializeField] private Sprite levelThree100Holder_;
        [SerializeField] private Sprite levelThree200Holder_;
        [SerializeField] private Sprite levelThree500Holder_;

        [SerializeField] private List<Sprite> levelOne100_ = new List<Sprite>();
        [SerializeField] private List<Sprite> levelOne200_ = new List<Sprite>();
        [SerializeField] private List<Sprite> levelOne500_ = new List<Sprite>();

        [SerializeField] private List<Sprite> levelTwo100_ = new List<Sprite>();
        [SerializeField] private List<Sprite> levelTwo200_ = new List<Sprite>();
        [SerializeField] private List<Sprite> levelTwo500_ = new List<Sprite>();

        [SerializeField] private List<Sprite> levelThree100_ = new List<Sprite>();
        [SerializeField] private List<Sprite> levelThree200_ = new List<Sprite>();
        [SerializeField] private List<Sprite> levelThree500_ = new List<Sprite>();

        private int level_ = -1;
        private int grams_ = -1;

        private bool sillyHack = false;
        public override void ReceivedAction(string action)
        {
            if (ArgumentHelper.ContainsCommand("-art-holder", action))
            {
                List<string> args = ArgumentHelper.ArgumentsFromCommand("-art-holder", action);

                if (string.Equals(args[0], "show"))
                {
                    AudioPlayer.PlayAudio("audio/sfx/door-open");

                    levelHolder_.transform.localPosition = new Vector3(-631, 320);
                    switch (args[1])
                    {
                        case "1":
                            level_ = 1;
                            switch(args[2])
                            {
                                case "100":
                                    levelHolder_.sprite = levelOne100Holder_;
                                    grams_ = 100;
                                    break;
                                case "200":
                                    levelHolder_.sprite = levelOne200Holder_;
                                    grams_ = 200;
                                    break;
                                case "500":
                                    levelHolder_.sprite = levelOne500Holder_;
                                    grams_ = 500;
                                    break;
                            }
                            break;
                        case "2":
                            level_ = 2;
                            switch (args[2])
                            {
                                case "100":
                                    levelHolder_.sprite = levelTwo100Holder_;
                                    grams_ = 100;
                                    break;
                                case "200":
                                    levelHolder_.sprite = levelTwo200Holder_;
                                    grams_ = 200;
                                    break;
                                case "500":
                                    levelHolder_.sprite = levelTwo500Holder_;
                                    grams_ = 500;
                                    break;
                            }
                            break;
                        case "3":
                            level_ = 3;
                            switch (args[2])
                            {
                                case "100":
                                    levelHolder_.sprite = levelThree100Holder_;
                                    grams_ = 100;
                                    break;
                                case "200":
                                    levelHolder_.sprite = levelThree200Holder_;
                                    grams_ = 200;
                                    break;
                                case "500":
                                    levelHolder_.sprite = levelThree500Holder_;
                                    grams_ = 500;
                                    break;
                            }
                            break;
                    }

                    List<Sprite> sprites = SpritesForArguments(args[1], args[2]);

                    artPiece1Holder_.sprite = sprites[0];
                    artPiece2Holder_.sprite = sprites[1];
                    artPiece3Holder_.sprite = sprites[2];

                    artPiece1Holder_.SetNativeSize();
                    artPiece2Holder_.SetNativeSize();
                    artPiece3Holder_.SetNativeSize();

                } else if (string.Equals(args[0], "hide"))
                {
                    levelHolder_.transform.localPosition = new Vector3(-631, 727);
                    level_ = -1;
                    grams_ = -1;
                }
            }

            if (level_ > 0)
            {
                bool playSound = false;

                if (ArgumentHelper.ContainsCommand("-add-art-1", action))
                {
                    gameManager_.Storage.AddObjectToList<string>(GameStorage.Key.Lego3Art, level_.ToString() + " " + grams_.ToString() + " " + "1");
                    playSound = true;
                } else if (ArgumentHelper.ContainsCommand("-add-art-2", action))
                {
                    gameManager_.Storage.AddObjectToList<string>(GameStorage.Key.Lego3Art, level_.ToString() + " " + grams_.ToString() + " " + "2");
                    playSound = true;
                }
                else if (ArgumentHelper.ContainsCommand("-add-art-3", action))
                {
                    gameManager_.Storage.AddObjectToList<string>(GameStorage.Key.Lego3Art, level_.ToString() + " " + grams_.ToString() + " " + "3");
                    playSound = true;
                }

                if (playSound)
                {
                    AudioPlayer.PlayAudio("audio/sfx/arch");
                }
                RefreshArt(true);
            }
        }

        private void RefreshArt(bool animate)
        {
            RemoveAllArt();

            Image lastImage = null;

            List<string> sculptures = gameManager_.Storage.GetValue<List<string>>(GameStorage.Key.Lego3Art);
            if (sculptures == null) { return; }
            foreach(string s in sculptures)
            {
                string[] split = s.Split(' ');
                List<Sprite> spritesToUse = SpritesForArguments(split[0], split[1]);
                Sprite spriteToUse = null;
                switch(split[2])
                {
                    case "1":
                        spriteToUse = spritesToUse[0];
                        break;
                    case "2":
                        spriteToUse = spritesToUse[1];
                        break;
                    case "3":
                        spriteToUse = spritesToUse[2];
                        break;
                }

                List<Image> toUse = null;

                switch(split[0])
                {
                    case "1":
                        switch (split[1])
                        {
                            case "100":
                                toUse = levelOneSmallHolders;
                                break;
                            case "200":
                                toUse = levelOneMedHolders;
                                break;
                            case "500":
                                toUse = levelOneLargeHolders;
                                break;
                        }
                        break;
                    case "2":
                        switch (split[1])
                        {
                            case "100":
                                toUse = levelTwoSmallHolders;
                                break;
                            case "200":
                                toUse = levelTwoMedHolders;
                                break;
                            case "500":
                                toUse = levelTwoLargeHolders;
                                break;
                        }
                        break;
                    case "3":
                        switch (split[1])
                        {
                            case "100":
                                toUse = levelThreeSmallHolders;
                                break;
                            case "200":
                                toUse = levelThreeMedHolders;
                                break;
                            case "500":
                                toUse = levelThreeLargeHolders;
                                break;
                        }
                        break;
                }

                if (toUse != null && spriteToUse != null)
                {
                    foreach(Image i in toUse)
                    {
                        if (i.sprite == null)
                        {
                            i.color = Color.white;
                            i.sprite = spriteToUse;
                            lastImage = i;
                            break;
                        }
                    }
                }
            }

            if (animate)
            {
                lastImage.transform.localScale = Vector3.zero;
                Go.to(lastImage.transform, 1.5f, new GoTweenConfig().scale(Vector3.one).setEaseType(GoEaseType.BounceInOut));
            }
        }

        public override void Reset()
        {
            level_ = -1;

            RefreshArt(false);
        }

        private void RemoveAllArt()
        {
            foreach (Image i in levelOneSmallHolders)
            {
                i.sprite = null;
                i.color = Color.clear;
            }
            foreach (Image i in levelOneMedHolders)
            {
                i.sprite = null;
                i.color = Color.clear;
            }
            foreach (Image i in levelOneLargeHolders)
            {
                i.sprite = null;
                i.color = Color.clear;
            }
            foreach (Image i in levelTwoSmallHolders)
            {
                i.sprite = null;
                i.color = Color.clear;
            }
            foreach (Image i in levelTwoMedHolders)
            {
                i.sprite = null;
                i.color = Color.clear;
            }
            foreach (Image i in levelTwoLargeHolders)
            {
                i.sprite = null;
                i.color = Color.clear;
            }
            foreach (Image i in levelThreeSmallHolders)
            {
                i.sprite = null;
                i.color = Color.clear;
            }
            foreach (Image i in levelThreeMedHolders)
            {
                i.sprite = null;
                i.color = Color.clear;
            }
            foreach (Image i in levelThreeLargeHolders)
            {
                i.sprite = null;
                i.color = Color.clear;
            }
        }

        private List<Sprite> SpritesForArguments(string level, string grams)
        {
            List<Sprite> spritesToUse = null;

            switch (level)
            {
                case "1":
                    switch (grams)
                    {
                        case "100":
                            spritesToUse = levelOne100_;
                            break;
                        case "200":
                            spritesToUse = levelOne200_;
                            break;
                        case "500":
                            spritesToUse = levelOne500_;
                            break;
                    }
                    break;
                case "2":
                    switch (grams)
                    {
                        case "100":
                            spritesToUse = levelTwo100_;
                            break;
                        case "200":
                            spritesToUse = levelTwo200_;
                            break;
                        case "500":
                            spritesToUse = levelTwo500_;
                            break;
                    }
                    break;
                case "3":
                    switch (grams)
                    {
                        case "100":
                            spritesToUse = levelThree100_;
                            break;
                        case "200":
                            spritesToUse = levelThree200_;
                            break;
                        case "500":
                            spritesToUse = levelThree500_;
                            break;
                    }
                    break;
            }
            return spritesToUse;
        }
    }
}

