using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lando.Class.Lego6 
{
    public class SpawnCats : SpawnedObject
    {
        [System.Serializable] public class Challenge
        {
            public GameObject container_; //this is the whole gameobject
            public List<Cat> cats_; //left, middle, right cat in the tree
            public Image defaultCatLeft;
            public Image defaultCatMiddle;
            public Image defaultCatRight;
            public Image jumpCat;
            public Image crateCat;
            public Image escapeCat;

            public List<Cat> otherChallengesCats_;
            public List<Image> otherCats_;

            public void ApplySprites() {
                defaultCatLeft.sprite = cats_[0].sitting_;
                defaultCatMiddle.sprite = cats_[1].sitting_;
                defaultCatRight.sprite = cats_[2].sitting_;

                if (otherChallengesCats_.Count > 0) 
                {
                    for (int i = 0; i < otherCats_.Count; i++)
                    {
                        otherCats_[i].sprite = otherChallengesCats_[i].sitting_;
                    }
                }
            }
        }

        [System.Serializable]
        public class Cat
        {
            public Sprite default_;
            public Sprite jumping_;
            public Sprite sitting_;
            public Sprite customization_; //this will be assign later
        }

        [System.Serializable]
        public class Daycare 
        {
            public Sprite background_;

            public int xMin, xMax, yMin, yMax;
        }

        [Header("Cat in the customization screen")]
        [SerializeField] private Image catCustomization_;
        [SerializeField] private Image clothesCustomization_;

        [Header("Customization holders")]
        [SerializeField] private Image top_;
        [SerializeField] private Image middle_;
        [SerializeField] private Image bottom_;


        [Header("Cats Sprite")]
        [Header("Cats Default")]

        [SerializeField] private List<Cat> solidCats_;
        [SerializeField] private List<Cat> tabbyCats_;
        [SerializeField] private List<Cat> spotsCats_;
        [SerializeField] private List<Cat> specialCats_;

        [Header("Customization items")]
        [SerializeField] private List<Sprite> bows_;
        [SerializeField] private List<Sprite> glasses_;
        [SerializeField] private List<Sprite> collars_;
        [SerializeField] private List<Sprite> scarves_;
        [SerializeField] private List<Sprite> wigs_;
        [SerializeField] private List<Sprite> pants_;
        [SerializeField] private List<Sprite> shirts_;

        [Header("Backgrounds Catnip & Cradle")]
        [SerializeField] private Daycare daycareLevel1_;
        [SerializeField] private Daycare daycareLevel2_;
        [SerializeField] private Daycare daycareLevel3_;
        [SerializeField] private Daycare daycareLevel4_;
        [SerializeField] private Daycare daycareLevel5_;
        [SerializeField] private Image daycareBg_;

        [Header("Challenges")]
        [SerializeField] private Challenge level1_;
        [SerializeField] private Challenge level2_;
        [SerializeField] private Challenge level3_;
        [SerializeField] private Challenge level4_;

        [Space]

        [SerializeField] private GameObject newCat_; //cat that'll appear in the daycare
        [SerializeField] private GameObject customizationScreen_;
        [SerializeField] private Sprite emptySprite_;
        [SerializeField] private Image jumpCat_;
        [SerializeField] private Image crateCat_;


        private Cat catPlaceholder_ = new Cat();//From here we'll get the corresponding cat sprite

        private int currentAmountCats_ = 0; //every 3 cats, daycare becomes bigger
        private int currentLevel = 1;
        private int currentLevelScreen = 1;

        float xMin_ = 0, xMax_ = 0, yMin_ = 0, yMax_ = 0;

        private float waitTime = 0;

        private List<RectTransform> catsInDaycare = new List<RectTransform>();

        private GameObject catGO;

        private void Start()
        {
            Hide();
            ShowChallengeScreen();
        }

        public override void Hide() { 
            customizationScreen_.SetActive(false);
            daycareBg_.gameObject.SetActive(false);
            level1_.container_.SetActive(false);
            level2_.container_.SetActive(false);
            level3_.container_.SetActive(false);
            level4_.container_.SetActive(false);
        }

        public override void ReceivedAction(string action)
        {
            List<string> args = ArgumentHelper.ArgumentsFromCommand("-cat", action);
            if (args.Count == 0) return;
            string commandType = args[0];

            if (args.Contains("success"))
            {
                int.TryParse(args[1], out currentLevel);
                CatSafe();
            }
            else if (args.Contains("top") || args.Contains("middle") || args.Contains("bottom"))
            {
                CustomizeYourCat(commandType);
            }
            else if (args.Contains("failure")) 
            {
                CatFailure();
            }
        }

        private void ShowChallengeScreen() 
        {
            int randomLeft_ = Random.Range(0, 15);
            int randomMiddle_ = Random.Range(0, 15);

            while (randomMiddle_ == randomLeft_) 
            {
                randomMiddle_ = Random.Range(0, 15);
            }

            int randomRight_ = Random.Range(0, 15);

            while (randomRight_ == randomLeft_ || randomMiddle_ == randomRight_)
            {
                randomRight_ = Random.Range(0, 15);
            }

            level1_.cats_[0] = solidCats_[randomLeft_];
            level1_.cats_[1] = solidCats_[randomMiddle_];
            level1_.cats_[2] = solidCats_[randomRight_];

            level2_.cats_[0] = tabbyCats_[randomLeft_];
            level2_.cats_[1] = tabbyCats_[randomMiddle_];
            level2_.cats_[2] = tabbyCats_[randomRight_];

            level3_.cats_[0] = spotsCats_[randomLeft_];
            level3_.cats_[1] = spotsCats_[randomMiddle_];
            level3_.cats_[2] = spotsCats_[randomRight_];

            level4_.cats_[0] = specialCats_[randomLeft_];
            level4_.cats_[1] = specialCats_[randomMiddle_];
            level4_.cats_[2] = specialCats_[randomRight_];

            switch (currentLevelScreen) 
            {
                case 1:
                    level1_.container_.SetActive(true);
                    level1_.ApplySprites();
                    break;
                case 2:
                    level2_.container_.SetActive(true);

                    ShowCat(level1_);

                    level1_.defaultCatLeft = level2_.otherCats_[0];
                    level1_.defaultCatMiddle = level2_.otherCats_[1];
                    level1_.defaultCatRight = level2_.otherCats_[2];

                    level2_.otherChallengesCats_.Add(level1_.cats_[0]);
                    level2_.otherChallengesCats_.Add(level1_.cats_[1]);
                    level2_.otherChallengesCats_.Add(level1_.cats_[2]);

                    level2_.ApplySprites();
                    break;
                case 3:
                    level3_.container_.SetActive(true);

                    ShowCat(level1_);
                    ShowCat(level2_);

                    level1_.defaultCatLeft = level3_.otherCats_[0];
                    level1_.defaultCatMiddle = level3_.otherCats_[1];
                    level1_.defaultCatRight = level3_.otherCats_[2];

                    level2_.defaultCatLeft = level3_.otherCats_[3];
                    level2_.defaultCatMiddle = level3_.otherCats_[4];
                    level2_.defaultCatRight = level3_.otherCats_[5];

                    level3_.otherChallengesCats_.Add(level1_.cats_[0]);
                    level3_.otherChallengesCats_.Add(level1_.cats_[1]);
                    level3_.otherChallengesCats_.Add(level1_.cats_[2]);

                    level3_.otherChallengesCats_.Add(level2_.cats_[0]);
                    level3_.otherChallengesCats_.Add(level2_.cats_[1]);
                    level3_.otherChallengesCats_.Add(level2_.cats_[2]);

                    level3_.ApplySprites();
                    break;
                case 4:
                    level4_.container_.SetActive(true);

                    level1_.defaultCatLeft = level4_.otherCats_[0];
                    level1_.defaultCatMiddle = level4_.otherCats_[1];
                    level1_.defaultCatRight = level4_.otherCats_[2];

                    ShowCat(level1_);
                    ShowCat(level2_);
                    ShowCat(level3_);
                    ShowCat(level4_);

                    level2_.defaultCatLeft = level4_.otherCats_[3];
                    level2_.defaultCatMiddle = level4_.otherCats_[4];
                    level2_.defaultCatRight = level4_.otherCats_[5];

                    level3_.defaultCatLeft = level4_.otherCats_[6];
                    level3_.defaultCatMiddle = level4_.otherCats_[7];
                    level3_.defaultCatRight = level4_.otherCats_[8];

                    level4_.otherChallengesCats_.Add(level1_.cats_[0]);
                    level4_.otherChallengesCats_.Add(level1_.cats_[1]);
                    level4_.otherChallengesCats_.Add(level1_.cats_[2]);
                         
                    level4_.otherChallengesCats_.Add(level2_.cats_[0]);
                    level4_.otherChallengesCats_.Add(level2_.cats_[1]);
                    level4_.otherChallengesCats_.Add(level2_.cats_[2]);
                         
                    level4_.otherChallengesCats_.Add(level3_.cats_[0]);
                    level4_.otherChallengesCats_.Add(level3_.cats_[1]);
                    level4_.otherChallengesCats_.Add(level3_.cats_[2]);

                    level4_.ApplySprites();
                    break;
            }
        }

        //Cat randomnly chosed to be saved
        private void CatSafe()
        {
            int randomCat_ = Random.Range(0, 3);
            GetCurrentState();
            GetCat(randomCat_);
            gameManager_.SendNewActionInternal("-update-options empty");
        }

        private void CatFailure() 
        {
            int randomCat_ = Random.Range(0, 3);
            gameManager_.SendNewActionInternal("-update-options empty");
            switch (currentLevel)
            {
                case 1:
                    level1_.escapeCat.sprite = level1_.cats_[randomCat_].jumping_;
                    level1_.jumpCat.sprite = level1_.cats_[randomCat_].jumping_;
                    ShowEscapeSequence(level1_.jumpCat.gameObject, level1_.escapeCat.gameObject, level1_, randomCat_);
                    break;
                case 2:
                    level2_.escapeCat.sprite = level2_.cats_[randomCat_].jumping_;
                    level2_.jumpCat.sprite = level2_.cats_[randomCat_].jumping_;
                    ShowEscapeSequence(level2_.jumpCat.gameObject,level2_.escapeCat.gameObject, level2_, randomCat_);
                    break;
                case 3:
                    level3_.escapeCat.sprite = level3_.cats_[randomCat_].jumping_;
                    level3_.jumpCat.sprite = level3_.cats_[randomCat_].jumping_;
                    ShowEscapeSequence(level3_.jumpCat.gameObject, level3_.escapeCat.gameObject, level3_, randomCat_);
                    break;
                case 4:
                    level4_.escapeCat.sprite = level4_.cats_[randomCat_].jumping_;
                    level4_.jumpCat.sprite = level4_.cats_[randomCat_].jumping_;
                    ShowEscapeSequence(level4_.jumpCat.gameObject, level4_.escapeCat.gameObject, level4_, randomCat_);
                    break;
            }
        }

        private void ShowEscapeSequence(GameObject jumping_,GameObject escape_, Challenge currentChallenge_, int selectedCat_) 
        {
            jumping_.SetActive(true);
            Go.to(this, 1f, new GoTweenConfig().onComplete(t => {
                jumping_.SetActive(false);
                escape_.SetActive(true);
                AudioPlayer.PlayAudio("audio/lego_6/cat-failure");
                Go.to(this, 3.8f, new GoTweenConfig().onComplete(t => {
                    escape_.SetActive(false);
                    switch (selectedCat_)
                    {
                        case 0:
                            currentChallenge_.defaultCatLeft.gameObject.SetActive(true);
                            break;
                        case 1:
                            currentChallenge_.defaultCatMiddle.gameObject.SetActive(true);
                            break;
                        case 2:
                            currentChallenge_.defaultCatRight.gameObject.SetActive(true);
                            break;
                    }
                    gameManager_.SendNewActionInternal("-update-options default");
                }));
            }));
        }

        private void GetCurrentState() 
        {
            if (level1_.container_.activeSelf)
            {
                jumpCat_ = level1_.jumpCat;
                crateCat_ = level1_.crateCat;
            }
            else if (level2_.container_.activeSelf) 
            {
                jumpCat_ = level2_.jumpCat;
                crateCat_ = level2_.crateCat;
            }
            else if (level3_.container_.activeSelf)
            {
                jumpCat_ = level3_.jumpCat;
                crateCat_ = level3_.crateCat;
            }
            else if (level4_.container_.activeSelf)
            {
                jumpCat_ = level4_.jumpCat;
                crateCat_ = level4_.crateCat;
            }
        }

        private void GetCat(int selectionCat) 
        {
            switch (currentLevel)
            {
                case 1:
                    catPlaceholder_ = level1_.cats_[selectionCat];

                    jumpCat_.sprite = catPlaceholder_.jumping_;
                    crateCat_.sprite = catPlaceholder_.sitting_;

                    GetSavedCat(level1_, selectionCat);

                    ShowCatJumpingSequence();
                    break;
                case 2:
                    catPlaceholder_ = level2_.cats_[selectionCat];
                    jumpCat_.sprite = catPlaceholder_.jumping_;
                    crateCat_.sprite = catPlaceholder_.sitting_;
                    GetSavedCat(level2_, selectionCat);
                    ShowCatJumpingSequence();
                    break;
                case 3:
                    catPlaceholder_ = level3_.cats_[selectionCat];
                    jumpCat_.sprite = catPlaceholder_.jumping_;
                    crateCat_.sprite = catPlaceholder_.sitting_;
                    GetSavedCat(level3_, selectionCat);
                    ShowCatJumpingSequence();
                    break;
                case 4:
                    catPlaceholder_ = level4_.cats_[selectionCat];
                    jumpCat_.sprite = catPlaceholder_.jumping_;
                    crateCat_.sprite = catPlaceholder_.sitting_;
                    GetSavedCat(level4_, selectionCat);
                    ShowCatJumpingSequence();
                    break;
            }
        }

        private void GetSavedCat(Challenge currentChallenge_, int selectedCat_) 
        {
            switch (selectedCat_) 
            {
                case 0:
                    currentChallenge_.defaultCatLeft.gameObject.SetActive(false);
                    break;
                case 1:
                    currentChallenge_.defaultCatMiddle.gameObject.SetActive(false);
                    break;
                case 2:
                    currentChallenge_.defaultCatRight.gameObject.SetActive(false);
                    break;
            }
        }

        private void ShowCat(Challenge currentChallenge_)
        {
            currentChallenge_.defaultCatLeft.gameObject.SetActive(true);
            currentChallenge_.defaultCatMiddle.gameObject.SetActive(true);
            currentChallenge_.defaultCatRight.gameObject.SetActive(true);
        }

        private void ShowCatJumpingSequence() {
            waitTime = 0;
            if (currentLevel == 1)
            {
                AudioPlayer.PlayAudio("audio/lego_6/cat-solid");
                waitTime += 3f;
            }
            else if (currentLevel == 2)
            {
                AudioPlayer.PlayAudio("audio/lego_6/cat-tabby");
                waitTime += 5f;
            }
            else if (currentLevel == 3)
            {
                AudioPlayer.PlayAudio("audio/lego_6/cat-spots");
                waitTime += 2f;
            }
            else if (currentLevel == 4) 
            {
                AudioPlayer.PlayAudio("audio/lego_6/cat-special");
                waitTime += 6.5f;
            }
            jumpCat_.gameObject.SetActive(true);
            Go.to(this, 1f,new GoTweenConfig().onComplete(t => {
                jumpCat_.gameObject.SetActive(false);
                crateCat_.gameObject.SetActive(true);
                Go.to(this, waitTime - 1, new GoTweenConfig().onComplete(t => {
                    PlayRandomVOReward();
                    Go.to(this, waitTime, new GoTweenConfig().onComplete(t => {
                        crateCat_.gameObject.SetActive(false);
                        ShowCustomizationScreen();
                    }));
                }));
            }));
        }

        private void PlayRandomVOReward() 
        {
            int randomVO = Random.Range(0, 3);
            waitTime = 0;
            switch (randomVO) 
            {
                case 0:
                    AudioPlayer.PlayAudio("audio/lego_6/cat-into-the-crate");
                    waitTime += 2;
                    break;
                case 1:
                    AudioPlayer.PlayAudio("audio/lego_6/cat-meowvelous");
                    waitTime += 2;
                    break;
                case 2:
                    AudioPlayer.PlayAudio("audio/lego_6/cat-stuck-landing");
                    waitTime += 4.5f;
                    break;
            }
        }

        private void ShowCustomizationScreen()
        {
            Hide();
            catCustomization_.sprite = catPlaceholder_.default_;
            clothesCustomization_.sprite = emptySprite_;
            //select random customization items
            int selectTop_ = Random.Range(0, 7);
            int selectMiddle_ = Random.Range(0, 7);

            while (selectMiddle_ == selectTop_) 
            {
                selectMiddle_ = Random.Range(0, 7);
            }

            int selectBottom_ = Random.Range(0, 7);
            while (selectBottom_ == selectTop_ || selectMiddle_ == selectBottom_)
            {
                selectBottom_ = Random.Range(0, 7);
            }

            top_.sprite = GetClothes(selectTop_);
            middle_.sprite = GetClothes(selectMiddle_);
            bottom_.sprite = GetClothes(selectBottom_);

            customizationScreen_.SetActive(true);
            //3 options appear (Top, Middle, Bottom)
            if (currentLevel == 1)
            {
                AudioPlayer.PlayAudio("audio/lego_6/cat-dressroom-0");
                Go.to(this, 2.5f, new GoTweenConfig().onComplete(t =>
                {
                    AudioPlayer.PlayAudio("audio/lego_6/cat-customization");
                    Go.to(this, 7f, new GoTweenConfig().onComplete(t =>
                    {
                        gameManager_.SendNewActionInternal("-update-options choose");
                    }));
                }));
            }
            else {
                AudioPlayer.PlayAudio("audio/lego_6/cat-customization-1");
                Go.to(this, 2.2f, new GoTweenConfig().onComplete(t =>
                {
                    gameManager_.SendNewActionInternal("-update-options choose");
                }));
            }
        }

        private Sprite GetClothes(int selection) {

            Sprite spriteClothes_ = null;
            int randomCloth = Random.Range(0,5);
            switch (selection) 
            {
                case 0:
                    spriteClothes_ = bows_[randomCloth];
                    break;
                case 1:
                    spriteClothes_ = glasses_[randomCloth];
                    break;
                case 2:
                    spriteClothes_ = collars_[randomCloth];
                    break;
                case 3:
                    spriteClothes_ = scarves_[randomCloth];
                    break;
                case 4:
                    spriteClothes_ = wigs_[randomCloth];
                    break;
                case 5:
                    spriteClothes_ = pants_[randomCloth];
                    break;
                case 6:
                    randomCloth = Random.Range(0, shirts_.Count);
                    spriteClothes_ = shirts_[randomCloth];
                    break;
            }
            return spriteClothes_;
        }

        private void CustomizeYourCat(string command)
        {
            //Select one of the 3 options
            //after a delay daycare screen appear
            switch (command) 
            {
                case "top":
                    catPlaceholder_.customization_ = top_.sprite;
                    break;
                case "middle":
                    catPlaceholder_.customization_ = middle_.sprite;
                    break;
                case "bottom":
                    catPlaceholder_.customization_ = bottom_.sprite;
                    break;

            }
            gameManager_.SendNewActionInternal("-update-options empty");
            clothesCustomization_.sprite = catPlaceholder_.customization_;

            GoTweenFlow flow = new GoTweenFlow();
            flow.insert(0f, new GoTween(catCustomization_.transform, 0.25f, new GoTweenConfig().scale(1.25f)));
            flow.insert(0.25f, new GoTween(catCustomization_.transform, 0.25f, new GoTweenConfig().scale(1f)));
            flow.play();

            Go.to(this, 2f, new GoTweenConfig().onComplete(t => {
                customizationScreen_.SetActive(false);
                ShowDaycareScreen();
            }));
        }

        private void ShowDaycareScreen() 
        {
            currentAmountCats_++;
            daycareBg_.gameObject.SetActive(true);

            switch (currentAmountCats_) 
            {
                case 1:
                    daycareBg_.sprite = daycareLevel1_.background_;
                    xMin_ = daycareLevel1_.xMin;
                    xMax_ = daycareLevel1_.xMax;
                    yMin_ = daycareLevel1_.yMin;
                    yMax_ = daycareLevel1_.yMax;

                    AudioPlayer.PlayAudio("audio/sfx/cat-meow");
                    catGO = Instantiate(newCat_);
                    SetupCat();
                    waitTime = 4;
                    break;
                case 4:
                    SetupDaycare(daycareLevel2_, catGO, "audio/lego_6/cat-expand", 8);
                    break;
                case 7:
                    SetupDaycare(daycareLevel3_, catGO, "audio/lego_6/cat-expand", 8);
                    break;
                case 10:
                    SetupDaycare(daycareLevel4_, catGO, "audio/lego_6/cat-expand", 8);
                    break;
                case 13:
                    SetupDaycare(daycareLevel5_, catGO, "audio/lego_6/cat-finish", 12);
                    break;
                default:
                    AudioPlayer.PlayAudio("audio/sfx/cat-meow");
                    catGO = Instantiate(newCat_);
                    SetupCat();
                    break;
            }

            //after a delay next challenge screen appear
            //Show default options
            Go.to(this, waitTime, new GoTweenConfig().onComplete(t => {
                if (currentLevelScreen < 4) currentLevelScreen++;
                ShowChallengeScreen();
                daycareBg_.gameObject.SetActive(false);
                gameManager_.SendNewActionInternal("-update-options default");
            }));
        }

        private void SetupDaycare(Daycare daycare, GameObject newCat, string voRoot, float wait) 
        {
            xMin_ = daycare.xMin;
            xMax_ = daycare.xMax;
            yMin_ = daycare.yMin;
            yMax_ = daycare.yMax;

            AudioPlayer.PlayAudio(voRoot);
            waitTime = wait;
            Go.to(this, 2.5f, new GoTweenConfig().onComplete(t => {
                gameManager_.SendNewActionInternal("-fadein 2");
                AudioPlayer.PlayAudio("audio/sfx/new-building");
                Go.to(this, 1f, new GoTweenConfig().onComplete(t => {
                    AudioPlayer.PlayAudio("audio/sfx/cat-meow");
                    daycareBg_.sprite = daycare.background_;
                    catGO = Instantiate(newCat_);
                    SetupCat();
                }));
            }));

        }

        private void SetupCat() 
        {
            Vector3 catPosition = new Vector3(Random.Range(xMin_, xMax_), Random.Range(yMin_, yMax_), 0);

            float randomScale = Random.Range(0.7f, 1.2f);
            int facingDirection = Random.Range(-10, 11);

            catGO.transform.SetParent(daycareBg_.transform);

            catGO.GetComponent<RectTransform>().anchoredPosition = (Vector2)catPosition;

            CheckCatSpawnPosition(catGO.GetComponent<RectTransform>());

            if (facingDirection >= 0)
            {
                catGO.GetComponent<RectTransform>().localScale = new Vector3(randomScale, randomScale, randomScale);
            }
            else
            {
                catGO.GetComponent<RectTransform>().localScale = new Vector3(-randomScale, randomScale, randomScale);
            }

            catGO.GetComponent<Image>().sprite = catPlaceholder_.default_;
            catGO.transform.GetChild(0).GetComponent<Image>().sprite = catPlaceholder_.customization_;

            catsInDaycare.Add(catGO.GetComponent<RectTransform>());
            catGO = null;
        }

        private void CheckCatSpawnPosition(RectTransform newCat) 
        {
            foreach (RectTransform cat in catsInDaycare)
            {
                while (newCat.anchoredPosition.x <= cat.anchoredPosition.x + 10 && newCat.anchoredPosition.x >= cat.anchoredPosition.x - 10) 
                {
                    newCat.anchoredPosition = new Vector2(Random.Range(xMin_, xMax_),newCat.anchoredPosition.y);
                }
                while (newCat.anchoredPosition.y <= cat.anchoredPosition.y + 10 && newCat.anchoredPosition.y >= cat.anchoredPosition.y - 10)
                {
                    newCat.anchoredPosition = new Vector2(newCat.anchoredPosition.x, Random.Range(xMin_, xMax_));
                }

                if (newCat.anchoredPosition.y > cat.anchoredPosition.y) 
                {
                    if (cat.GetSiblingIndex() > 0)
                    {
                        newCat.SetSiblingIndex(cat.GetSiblingIndex() - 1);
                    }
                    else 
                    {
                        newCat.SetSiblingIndex(0);
                    }
                }
            }
        }
    }
}
