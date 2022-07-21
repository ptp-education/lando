using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnedMomo : SpawnedObject
{
    [System.Serializable]
    private class MomoDisplay
    {
        public Image background_;
        public Image momo_;
        public Image antenna_;
        public Image tail_;
        public Image spots_;
        public Image claws_;
        public Image mohawk_;
        public Image nose_;
        public Image stripes_;
        public Image whiskers_;
        public Image wings_;

        public void InitCustomization() {
            background_?.gameObject.SetActive(false);
            antenna_?.gameObject.SetActive(false);
            tail_?.gameObject.SetActive(false);
            spots_?.gameObject.SetActive(false);
            claws_?.gameObject.SetActive(false);
            mohawk_?.gameObject.SetActive(false);
            nose_?.gameObject.SetActive(false);
            stripes_?.gameObject.SetActive(false);
            whiskers_?.gameObject.SetActive(false);
            wings_?.gameObject.SetActive(false);
        }

        public void TurnOffCustomizations() {
            antenna_?.gameObject.SetActive(false);
            tail_?.gameObject.SetActive(false);
            spots_?.gameObject.SetActive(false);
            claws_?.gameObject.SetActive(false);
            mohawk_?.gameObject.SetActive(false);
            nose_?.gameObject.SetActive(false);
            stripes_?.gameObject.SetActive(false);
            whiskers_?.gameObject.SetActive(false);
            wings_?.gameObject.SetActive(false);
        }
    }

    [Header("Scene")]
    [SerializeField] private MomoDisplay neutral_;
    [SerializeField] private MomoDisplay success_;
    [SerializeField] private MomoDisplay failure_;
    [SerializeField] private MomoDisplay customizeTeen_;
    [SerializeField] private MomoDisplay customizeAdult_;
    [SerializeField] private MomoDisplay customizeSenior_;

    [Space]

    [SerializeField] private Image starterBackground_;
    [SerializeField] private Transform starterSelected_;
    [SerializeField] private Image starterMomo_;

    [SerializeField] private Image customizeTeenBackground_;
    [SerializeField] private Image customizeTeenMomo_;

    [SerializeField] private Image customizeAdultBackground_;
    [SerializeField] private Image customizeAdultMomo_;

    [SerializeField] private Image evolveBackground_;

    [SerializeField] private Image neutralBackground_;
    [SerializeField] private Image neutralMomo_;

    [SerializeField] private Image successBackground_;
    [SerializeField] private Image successMomo_;

    [SerializeField] private Image failureBackground_;
    [SerializeField] private Image failureMomo_;

    #region Sprites
    [Header("Child")]
    [Header("Green")]
    [SerializeField] private Sprite childGreen_;
    [SerializeField] private Sprite childGreenHappy_;
    [SerializeField] private Sprite childGreenSad_;

    [Header("Blue")]
    [SerializeField] private Sprite childBlue_;
    [SerializeField] private Sprite childBlueHappy_;
    [SerializeField] private Sprite childBlueSad_;

    [Header("Red")]
    [SerializeField] private Sprite childRed_;
    [SerializeField] private Sprite childRedHappy_;
    [SerializeField] private Sprite childRedSad_;

    [Header("Teen")]
    [Header("Green")]
    [SerializeField] private Sprite teenGreen_;
    [SerializeField] private Sprite teenGreenHappy_;
    [SerializeField] private Sprite teenGreenSad_;

    [Header("Blue")]
    [SerializeField] private Sprite teenBlue_;
    [SerializeField] private Sprite teenBlueHappy_;
    [SerializeField] private Sprite teenBlueSad_;

    [Header("Red")]
    [SerializeField] private Sprite teenRed_;
    [SerializeField] private Sprite teenRedHappy_;
    [SerializeField] private Sprite teenRedSad_;

    [Header("Adult")]
    [Header("Green")]
    [SerializeField] private Sprite adultGreen_;
    [SerializeField] private Sprite adultGreenHappy_;
    [SerializeField] private Sprite adultGreenSad_;

    [Header("Blue")]
    [SerializeField] private Sprite adultBlue_;
    [SerializeField] private Sprite adultBlueHappy_;
    [SerializeField] private Sprite adultBlueSad_;

    [Header("Red")]
    [SerializeField] private Sprite adultRed_;
    [SerializeField] private Sprite adultRedHappy_;
    [SerializeField] private Sprite adultRedSad_;

    [Header("Customization Teen")]
    [Header("Neutral")]
    [SerializeField] private Sprite teenAntennaNeutral_;
    [SerializeField] private Sprite teenSpotsNeutral_;
    [SerializeField] private Sprite teenStripesNeutral_;
    [SerializeField] private Sprite teenWhiskersNeutral_;

    [Header("Happy")]
    [SerializeField] private Sprite teenAntennaHappy_;
    [SerializeField] private Sprite teenSpotsHappy_;
    [SerializeField] private Sprite teenStripesHappy_;
    [SerializeField] private Sprite teenWhiskersHappy_;

    [Header("Sad")]
    [SerializeField] private Sprite teenAntennaSad_;
    [SerializeField] private Sprite teenSpotsSad_;
    [SerializeField] private Sprite teenStripesSad_;
    [SerializeField] private Sprite teenWhiskersSad_;

    [Header("All")]
    [SerializeField] private Sprite teenTail_;
    [SerializeField] private Sprite teenMohawk_;

    [Header("Customization Adult")]
    [Header("Neutral")]
    [SerializeField] private Sprite adultAntennaNeutral_;
    [SerializeField] private Sprite adultClawsNeutral_;
    [SerializeField] private Sprite adultNoseNeutral_;
    [SerializeField] private Sprite adultWingsNeutral_;
    [SerializeField] private Sprite adultSpotsNeutral_;
    [SerializeField] private Sprite adultStripesNeutral_;
    [SerializeField] private Sprite adultWhiskersNeutral_;

    [Header("Happy")]
    [SerializeField] private Sprite adultAntennaHappy_;
    [SerializeField] private Sprite adultClawsHappy_;
    [SerializeField] private Sprite adultNoseHappy_;
    [SerializeField] private Sprite adultWingsHappy_;
    [SerializeField] private Sprite adultSpotsHappy_;
    [SerializeField] private Sprite adultStripesHappy_;
    [SerializeField] private Sprite adultWhiskersHappy_;

    [Header("Sad")]
    [SerializeField] private Sprite adultAntennaSad_;
    [SerializeField] private Sprite adultClawsSad_;
    [SerializeField] private Sprite adultNoseSad_;
    [SerializeField] private Sprite adultWingsSad_;
    [SerializeField] private Sprite adultSpotsSad_;
    [SerializeField] private Sprite adultStripesSad_;
    [SerializeField] private Sprite adultWhiskersSad_;

    [Header("All")]
    [SerializeField] private Sprite adultTail_;
    [SerializeField] private Sprite adultMohawk_;


    #endregion

    [SerializeField] private Sprite childGreenEvolving_;
    [SerializeField] private Sprite childBlueEvolving_;
    [SerializeField] private Sprite childRedEvolving_;

    private const string kGreen = "green";
    private const string kRed = "red";
    private const string kBlue = "blue";
    private const string kAntenna = "antenna";
    private const string kSpots = "spots";
    private const string kTail = "tail";
    private const string kWhiskers = "tail";
    private const string kMohawk = "mohawk";
    private const string kStripes = "stripes";
    private const string kWings = "wings";
    private const string kNose = "nose";
    private const string kClaws = "claws";


    private const string kCommand = "-momo";

    private enum Status
    {
        Neutral,
        Success,
        Failure,
        Transforming
    }

    private string currentRfid_;

    private string nfcId_;
    private string commandType_;
    private bool isRunning = false;

    private GoTweenFlow dismissingFlow_;

    private void Start()
    {
        ShareManager sm = (ShareManager)gameManager_;
        if (sm != null)
        {
            starterBackground_.transform.SetParent(sm.OverlayParent);
        }

        InitCustomization();
    }

    private void InitCustomization() 
    {
        neutral_.InitCustomization();
        success_.InitCustomization();
        failure_.InitCustomization();
        customizeTeen_.InitCustomization();
        customizeAdult_.InitCustomization();
        customizeSenior_.InitCustomization();
    }

    public override void Hide()
    {
        HideAllScenes();
    }

    public override void ReceivedAction(string action)
    {
        List<string> args = ArgumentHelper.ArgumentsFromCommand("-momo", action);
        if (args.Count == 0) return;

        //InitCustomization();

        commandType_ = args[0];
        if (args.Count > 1)
        {
            nfcId_ = args[1];

            //testing
            currentRfid_ = args[1];
        }


        if (LevelOfMomo(nfcId_) > 3)
        {
            gameManager_.SendNewActionInternal("-node next");
            return;
        }
        if (LevelOfMomo(nfcId_) == 0)
        {
            //show starter Momo
            if (commandType_.Contains("success"))
            {
                HandleStarterPicker();
            }
            else
            {
                HandleStarterPickerSelection(commandType_);
            }
        }
        else {
            if (!isRunning) 
            { 
                StartCoroutine(DisplayMomo());
            }
        }
    }

    private IEnumerator DisplayMomo() {

        if (LevelOfMomo(nfcId_) > 1)
        {
            if (commandType_.Contains("success"))
            {
                ShowMomoOnScreen();
                yield return new WaitForSeconds(1);
                ShowMomoEatingBerry();
                yield return new WaitForSeconds(1);
                RewardSequence();
            }
            if (commandType_.Contains("show"))
            {
                ShowMomoOnScreen();
            }
        }
        else {
            isRunning = true;
            gameManager_.SendNewActionInternal("-update-options empty");
            ShowMomoOnScreen();
            yield return new WaitForSeconds(1);
            ShowMomoEatingBerry();
            yield return new WaitForSeconds(1);
            RewardSequence();
            yield return new WaitForSeconds(1);
            commandType_ = null;
            yield return new WaitUntil(() => commandType_ != null);
            HandleCustomizeSelection(commandType_);
            isRunning = false;
        }
    }

    private void RewardSequence()
    {
        ShowMomoUpgrade();
    }

    private void HideAllScenes()
    {
        if (dismissingFlow_ != null)
        {
            dismissingFlow_.destroy();
            dismissingFlow_ = null;
        }

        starterBackground_.gameObject.SetActive(false);
        customizeTeenBackground_.gameObject.SetActive(false);
        customizeAdultBackground_.gameObject.SetActive(false);
        customizeSenior_.background_.gameObject.SetActive(false);
        evolveBackground_.gameObject.SetActive(false);

        neutral_.background_.gameObject.SetActive(false);
        success_.background_.gameObject.SetActive(false);
        failure_.background_.gameObject.SetActive(false);
    }

    private void HandleStarterPicker()
    {
        HideAllScenes();
        starterBackground_.gameObject.SetActive(true);
        starterSelected_.gameObject.SetActive(false);

        AudioPlayer.PlayAudio("audio/sfx/new-option");
        gameManager_.SendNewActionInternal("-character talk momo-pick");

    }

    //Select momo's color and initial setup
    private void HandleStarterPickerSelection(string choice)
    {
        if (currentRfid_ == null || currentRfid_.Length == 0) return;
        HideAllScenes();
        starterBackground_.gameObject.SetActive(true);
        starterSelected_.gameObject.SetActive(true);

        GameStorage gs = gameManager_.GameStorageForUserId(currentRfid_);

        string selection = null;
        switch (choice)
        {
            case "left":
                selection = kRed;
                break;
            case "middle":
                selection = kBlue;
                break;
            case "right":
                selection = kGreen;
                break;
        }

        if (selection != null)
        {
            gs.Add<string>(GameStorage.Key.MomoStarter, selection);

            SetSprite(starterMomo_, currentRfid_, Status.Neutral);
        }

        AudioPlayer.PlayAudio("audio/sfx/arch");
        AudioPlayer.PlayAudio("audio/sfx/whoosh");

        //----Momo's movement----
        starterMomo_.transform.localPosition = new Vector3(0, 1200, 0);
        Go.to(starterMomo_.transform, 1f, new GoTweenConfig().localPosition(Vector3.zero).setEaseType(GoEaseType.SineIn).onComplete(t =>
        {
            AudioPlayer.PlayAudio("audio/sfx/momo-grunt");
        }));

        GoTweenFlow flow = new GoTweenFlow();
        flow.insert(1f, new GoTween(starterMomo_.transform, 0.25f, new GoTweenConfig().scale(1.1f)));
        flow.insert(1.25f, new GoTween(starterMomo_.transform, 0.25f, new GoTweenConfig().scale(1f)));
        flow.play();
        //-----------------------

        if (dismissingFlow_ != null)
        {
            dismissingFlow_.destroy();
            dismissingFlow_ = null;
        }
        dismissingFlow_ = new GoTweenFlow();
        dismissingFlow_.insert(0.5f, new GoTween(this, 2f, new GoTweenConfig().onComplete(t =>
        {
            starterBackground_.gameObject.SetActive(false);
            starterSelected_.gameObject.SetActive(false);
            StartCoroutine(DisplayMomo());
        })));
        dismissingFlow_.play();
    }

    //Show the current upgrade that the user can do to their momo
    private void ShowMomoUpgrade()
    {
        if (currentRfid_ == null || currentRfid_.Length == 0) return;

        HideAllScenes();

        AudioPlayer.PlayAudio("audio/sfx/new-option");

        int level = LevelOfMomo(currentRfid_);

        List<Sprite> sprites = SpritesEvolveForRfid(currentRfid_);

        if (level == 3) 
        {
            customizeSenior_.momo_.sprite = sprites[1];
            customizeSenior_.background_.gameObject.SetActive(true);

            //TODO: This should change to senior VO
            gameManager_.SendNewActionInternal("-character talk momo-customization");
            Go.to(this, 2.5f, new GoTweenConfig().onComplete(y => {
                gameManager_.SendNewActionInternal("-update-options choose");
            }
            ));
        }

        if (level == 2)
        {
            customizeAdult_.momo_.sprite = sprites[1];
            customizeAdult_.background_.gameObject.SetActive(true);

            gameManager_.SendNewActionInternal("-character talk momo-adult");
            Go.to(this, 2.5f, new GoTweenConfig().onComplete(t =>
            {
                gameManager_.SendNewActionInternal("-character talk momo-customization");
                Go.to(this, 2.5f, new GoTweenConfig().onComplete(y => {
                    gameManager_.SendNewActionInternal("-update-options choose");
                }
                ));
            }));
        }
        if (level == 1)
        {
            customizeTeen_.background_.gameObject.SetActive(true);
            customizeTeen_.momo_.sprite = sprites[1];

            gameManager_.SendNewActionInternal("-character talk momo-teen");
            Go.to(this, 5.8f, new GoTweenConfig().onComplete(t =>
            {
                gameManager_.SendNewActionInternal("-update-options choose");
            }));
        }
    }

    private void HandleCustomizeSelection(string choice)
    {
        if (currentRfid_ == null || currentRfid_.Length == 0) return;

        bool teenActive = customizeTeen_.background_.gameObject.activeSelf;
        bool adultActive = customizeAdult_.background_.gameObject.activeSelf;
        bool seniorActive = customizeSenior_.background_.gameObject.activeSelf;

        GameStorage gs = gameManager_.GameStorageForUserId(currentRfid_);

        List<Sprite> sprites = SpritesEvolveForRfid(currentRfid_);

        //Select accesories for the momo
        if (teenActive)
        {
            string selection = null;
            customizeTeen_.momo_.sprite = sprites[1];
            switch (choice)
            {
                case "left":
                    selection = kSpots;
                    customizeTeen_.spots_.gameObject.SetActive(true);
                    customizeTeen_.spots_.sprite = teenSpotsNeutral_;
                    break;
                case "middle":
                    selection = kAntenna;
                    customizeTeen_.antenna_.gameObject.SetActive(true);
                    customizeTeen_.antenna_.sprite = teenAntennaNeutral_;
                    break;
                case "right":
                    selection = kTail;
                    customizeTeen_.tail_.gameObject.SetActive(true);
                    customizeTeen_.tail_.sprite = teenTail_;
                    break;
            }
            gs.Add<string>(GameStorage.Key.MomoTeenCustomization, selection);
        } else if (adultActive)
        {
            string selection = null;
            customizeAdult_.momo_.sprite = sprites[1];
            switch (choice)
            {
                case "left":
                    selection = kWhiskers;
                    customizeAdult_.whiskers_.gameObject.SetActive(true);
                    customizeAdult_.whiskers_.sprite = adultWhiskersNeutral_;
                    break;
                case "middle":
                    selection = kStripes;
                    customizeAdult_.stripes_.gameObject.SetActive(true);
                    customizeAdult_.stripes_.sprite = adultStripesNeutral_;
                    break;
                case "right":
                    selection = kMohawk;
                    customizeAdult_.mohawk_.gameObject.SetActive(true);
                    customizeAdult_.mohawk_.sprite = adultMohawk_;
                    break;
            }
            gs.Add<string>(GameStorage.Key.MomoAdultCustomization, selection);
        }
        else if (seniorActive)
        {
            string selection = null;
            customizeSenior_.momo_.sprite = sprites[1];
            switch (choice)
            {
                case "left":
                    selection = kWings;
                    customizeSenior_.wings_.gameObject.SetActive(true);
                    customizeSenior_.wings_.sprite = adultWingsNeutral_;
                    break;
                case "middle":
                    selection = kNose;
                    customizeSenior_.nose_.gameObject.SetActive(true);
                    customizeSenior_.nose_.sprite = adultNoseNeutral_;
                    break;
                case "right":
                    selection = kClaws;
                    customizeSenior_.claws_.gameObject.SetActive(true);
                    customizeSenior_.claws_.sprite = adultClawsNeutral_;
                    break;
            }
            gs.Add<string>(GameStorage.Key.MomoSeniorCustomization, selection);
        }

        AudioPlayer.PlayAudio("audio/sfx/customization-selection");
        Go.to(this, 0.5f, new GoTweenConfig().onComplete(t =>
        {
            AudioPlayer.PlayAudio("audio/sfx/momo-happy");
        }));

        AudioPlayer.PlayAudio("audio/sfx/pop");

        if (teenActive)
        {
            SetSprite(customizeTeenMomo_, currentRfid_, Status.Neutral);

            GoTweenFlow flow = new GoTweenFlow();
            flow.insert(0f, new GoTween(customizeTeenMomo_.transform, 0.25f, new GoTweenConfig().scale(0.55f)));
            flow.insert(0.25f, new GoTween(customizeTeenMomo_.transform, 0.25f, new GoTweenConfig().scale(0.5f)));

            flow.insert(0f, new GoTween(customizeTeen_.tail_.transform, 0.25f, new GoTweenConfig().scale(0.55f)));
            flow.insert(0.25f, new GoTween(customizeTeen_.tail_.transform, 0.25f, new GoTweenConfig().scale(0.5f)));

            flow.play();
        }
        else if (adultActive)
        {
            SetSprite(customizeAdultMomo_, currentRfid_, Status.Neutral);

            GoTweenFlow flow = new GoTweenFlow();
            flow.insert(0f, new GoTween(customizeAdultMomo_.transform, 0f, new GoTweenConfig().scale(0.35f)));
            flow.insert(0.25f, new GoTween(customizeAdultMomo_.transform, 0.25f, new GoTweenConfig().scale(0.3f)));

            flow.insert(0f, new GoTween(customizeAdult_.tail_.transform, 0f, new GoTweenConfig().scale(0.55f)));
            flow.insert(0.25f, new GoTween(customizeAdult_.tail_.transform, 0.25f, new GoTweenConfig().scale(0.5f)));
            flow.play();
        }
        else if (seniorActive) 
        {
            SetSprite(customizeAdultMomo_, currentRfid_, Status.Neutral);

            GoTweenFlow flow = new GoTweenFlow();
            flow.insert(0f, new GoTween(customizeSenior_.momo_.transform, 0f, new GoTweenConfig().scale(0.35f)));
            flow.insert(0.25f, new GoTween(customizeSenior_.momo_.transform, 0.25f, new GoTweenConfig().scale(0.3f)));

            flow.insert(0f, new GoTween(customizeSenior_.tail_.transform, 0f, new GoTweenConfig().scale(0.55f)));
            flow.insert(0.25f, new GoTween(customizeSenior_.tail_.transform, 0.25f, new GoTweenConfig().scale(0.5f)));

            flow.insert(0f, new GoTween(customizeSenior_.wings_.transform, 0f, new GoTweenConfig().scale(0.35f)));
            flow.insert(0.25f, new GoTween(customizeSenior_.wings_.transform, 0.25f, new GoTweenConfig().scale(0.3f)));
            flow.play();
        }

        if (dismissingFlow_ != null)
        {
            dismissingFlow_.destroy();
            dismissingFlow_ = null;
        }
        dismissingFlow_ = new GoTweenFlow();
        dismissingFlow_.insert(2f, new GoTween(this, 1f, new GoTweenConfig().onComplete(t =>
        {
            customizeTeenBackground_.gameObject.SetActive(false);
            customizeAdultBackground_.gameObject.SetActive(false);
            customizeSenior_.background_.gameObject.SetActive(false);
            ShowMomoOnScreen();
            gameManager_.SendNewActionInternal("-update-options default");
        })));
        dismissingFlow_.play();
    }

    private void ShowMomoOnScreen()
    {
        if (currentRfid_ == null || currentRfid_.Length == 0) return;
        HideAllScenes();

        AudioPlayer.PlayAudio("audio/sfx/momo-grunt");

        neutral_.TurnOffCustomizations();

        neutralBackground_.gameObject.SetActive(true);
        SetSprite(neutralMomo_, currentRfid_, Status.Neutral);
    }

    private void ShowMomoEatingBerry()
    {
        if (currentRfid_ == null || currentRfid_.Length == 0) return;

        HideAllScenes();

        AudioPlayer.PlayAudio("audio/sfx/momo-grunt");
        AudioPlayer.PlayAudio("audio/sfx/jump");
        success_.TurnOffCustomizations();

        successBackground_.gameObject.SetActive(true);
        SetSprite(success_.momo_, currentRfid_, Status.Success);

    }

    private void HandleFailure()
    {
        if (currentRfid_ == null || currentRfid_.Length == 0) return;

        HideAllScenes();

        AudioPlayer.PlayAudio("audio/sfx/wall-crash");

        failure_.TurnOffCustomizations();

        failureBackground_.gameObject.SetActive(true);
        SetSprite(failureMomo_, currentRfid_, Status.Failure);
    }

    private void RefreshScreen()
    {
        if (neutralBackground_.gameObject.activeSelf)
        {
            ShowMomoOnScreen();
        } else if (successBackground_.gameObject.activeSelf)
        {
            ShowMomoEatingBerry();
        } else if (failureBackground_.gameObject.activeSelf)
        {
            HandleFailure();
        }
    }

    public override void Reset()
    {

    }

    //Handle level of momo
    private int LevelOfMomo(string rfid)
    {
        GameStorage gs = gameManager_.GameStorageForUserId(rfid);
        string starter = gs.GetValue<string>(GameStorage.Key.MomoStarter);
        string teenCustomization = gs.GetValue<string>(GameStorage.Key.MomoTeenCustomization);
        string adultCustomization = gs.GetValue<string>(GameStorage.Key.MomoAdultCustomization);

        if (adultCustomization != null && adultCustomization.Length > 0)
        {
            return 3;
        }
        if (teenCustomization != null && teenCustomization.Length > 0)
        {
            return 2;
        }
        if (starter != null && starter.Length > 0)
        {
            return 1;
        }

        return 0;
    }

    //return sprites based on the momo's level
    //Sprites for customization screen
    private List<Sprite> SpritesEvolveForRfid(string rfid)
    {
        GameStorage gs = gameManager_.GameStorageForUserId(rfid);
        string starter = gs.GetValue<string>(GameStorage.Key.MomoStarter);
        string teenCustomization = gs.GetValue<string>(GameStorage.Key.MomoTeenCustomization);
        string adultCustomization = gs.GetValue<string>(GameStorage.Key.MomoAdultCustomization);

        if (LevelOfMomo(rfid) == 1)
        {
            switch (starter)
            {
                case kGreen:
                    return new List<Sprite>()
                    {
                        childGreen_,
                        teenGreen_
                    };
                case kBlue:
                    return new List<Sprite>()
                    {
                        childBlue_,
                        teenBlue_
                    };
                case kRed:
                    return new List<Sprite>()
                    {
                        childRed_,
                        teenRed_
                    };
            }


        }
        else if (LevelOfMomo(rfid) == 2)
        {
            customizeTeen_.TurnOffCustomizations();
            switch (teenCustomization)
            {
                case kAntenna:
                    customizeAdult_.antenna_.gameObject.SetActive(true);
                    customizeAdult_.antenna_.sprite = adultAntennaNeutral_;
                    break;
                case kSpots:
                    customizeAdult_.spots_.gameObject.SetActive(true);
                    customizeAdult_.spots_.sprite = adultSpotsNeutral_;
                    break;
                case kTail:
                    customizeAdult_.tail_.gameObject.SetActive(true);
                    break;
            }
            switch (starter)
            {
                case kGreen:

                    return new List<Sprite>()
                    {
                        teenGreen_,
                        adultGreen_
                    };
                case kBlue:
                    return new List<Sprite>()
                    {
                        teenBlue_,
                        adultBlue_
                    };
                case kRed:
                    return new List<Sprite>()
                    {
                        teenRed_,
                        adultRed_
                    };
            }
        }
        else if (LevelOfMomo(rfid) == 3) {
            customizeTeen_.TurnOffCustomizations();
            customizeAdult_.TurnOffCustomizations();
            switch (teenCustomization)
            {
                case kAntenna:
                    customizeSenior_.antenna_.gameObject.SetActive(true);
                    customizeSenior_.antenna_.sprite = adultAntennaNeutral_;
                    break;  
                case kSpots:
                    customizeSenior_.spots_.gameObject.SetActive(true);
                    customizeSenior_.spots_.sprite = adultSpotsNeutral_;
                    break;   
                case kTail:  
                    customizeSenior_.tail_.gameObject.SetActive(true);
                    customizeSenior_.tail_.sprite = adultTail_;
                    break;
            }

            switch (adultCustomization)
            {
                case kWhiskers:
                    customizeSenior_.whiskers_.gameObject.SetActive(true);
                    customizeSenior_.whiskers_.sprite = adultWhiskersNeutral_;
                    break;
                case kStripes:
                    customizeSenior_.stripes_.gameObject.SetActive(true);
                    customizeSenior_.stripes_.sprite = adultStripesNeutral_;
                    break;
                case kMohawk:
                    customizeSenior_.mohawk_.gameObject.SetActive(true);
                    customizeSenior_.mohawk_.sprite = adultMohawk_;
                    break;
            }

            switch (starter)
            {
                case kGreen:

                    return new List<Sprite>()
                    {
                        adultGreen_,
                        adultGreen_
                    };
                case kBlue:
                    return new List<Sprite>()
                    {
                        adultBlue_,
                        adultBlue_
                    };
                case kRed:
                    return new List<Sprite>()
                    {
                        adultRed_,
                        adultRed_
                    };
            }
        }

        return null;
    }

    private void SetSprite(Image image, string rfid, Status status)
    {
        image.sprite = MomoForRfid(rfid, status);
    }

    private Sprite MomoForRfid(string rfid, Status status)
    {
        GameStorage gs = gameManager_.GameStorageForUserId(rfid);
        string starter = gs.GetValue<string>(GameStorage.Key.MomoStarter);
        string teenCustomization = gs.GetValue<string>(GameStorage.Key.MomoTeenCustomization);
        string adultCustomization = gs.GetValue<string>(GameStorage.Key.MomoAdultCustomization);
        string seniorCustomization = gs.GetValue<string>(GameStorage.Key.MomoSeniorCustomization);

        //Senior Customization
        if (seniorCustomization != null && seniorCustomization.Length > 0)
        {
            neutral_.nose_.gameObject.SetActive(false);
            neutral_.claws_.gameObject.SetActive(false);
            neutral_.wings_.gameObject.SetActive(false);

            success_.nose_.gameObject.SetActive(false);
            success_.claws_.gameObject.SetActive(false);
            success_.wings_.gameObject.SetActive(false);

            failure_.nose_.gameObject.SetActive(false);
            failure_.claws_.gameObject.SetActive(false);
            failure_.wings_.gameObject.SetActive(false);
            switch (seniorCustomization)
            {
                case kWings:
                    
                    switch (status)
                    {
                        case Status.Neutral:
                            neutral_.wings_.gameObject.SetActive(true);
                            neutral_.wings_.sprite = adultWingsNeutral_;
                            break;
                        case Status.Success:
                            success_.wings_.gameObject.SetActive(true);
                            success_.wings_.sprite = adultWingsHappy_;
                            break;
                        case Status.Failure:
                            failure_.wings_.gameObject.SetActive(true);
                            failure_.wings_.sprite = adultWingsSad_;
                            break;
                        case Status.Transforming: return null;
                    }
                    break;
                case kNose:
                    switch (status)
                    {
                        case Status.Neutral:
                            neutral_.nose_.gameObject.SetActive(true);
                            neutral_.nose_.sprite = adultNoseNeutral_;
                            break;
                        case Status.Success:
                            success_.nose_.gameObject.SetActive(true);
                            success_.nose_.sprite = adultNoseHappy_;
                            break;
                        case Status.Failure:
                            failure_.nose_.gameObject.SetActive(true);
                            failure_.nose_.sprite = adultNoseSad_;
                            break;
                        case Status.Transforming: return null;
                    }
                    break;
                case kClaws:
                    switch (status)
                    {
                        case Status.Neutral:
                            neutral_.claws_.gameObject.SetActive(true);
                            neutral_.claws_.sprite = adultClawsNeutral_;
                            break;
                        case Status.Success:
                            success_.claws_.gameObject.SetActive(true);
                            success_.claws_.sprite = adultClawsHappy_;
                            break;
                        case Status.Failure:
                            failure_.claws_.gameObject.SetActive(true);
                            failure_.claws_.sprite = adultClawsSad_;
                            break;
                        case Status.Transforming: return null;
                    }
                    break;
            }
            switch (adultCustomization)
            {
                case kWhiskers:
                    switch (status)
                    {
                        case Status.Neutral:
                            neutral_.whiskers_.gameObject.SetActive(true);
                            neutral_.whiskers_.sprite = adultWhiskersNeutral_;
                            break;
                        case Status.Success:
                            success_.whiskers_.gameObject.SetActive(true);
                            success_.whiskers_.sprite = adultWhiskersHappy_;
                            break;
                        case Status.Failure:
                            failure_.whiskers_.gameObject.SetActive(true);
                            failure_.whiskers_.sprite = adultWhiskersSad_;
                            break;
                        case Status.Transforming: return null;
                    }
                    break;
                case kMohawk:
                    neutral_.mohawk_.gameObject.SetActive(true);
                    success_.mohawk_.gameObject.SetActive(true);
                    failure_.mohawk_.gameObject.SetActive(true);
                    break;
                case kStripes:
                    switch (status)
                    {
                        case Status.Neutral:
                            neutral_.stripes_.gameObject.SetActive(true);
                            neutral_.stripes_.sprite = adultStripesNeutral_;
                            break;
                        case Status.Success:
                            success_.stripes_.gameObject.SetActive(true);
                            success_.stripes_.sprite = adultStripesHappy_;
                            break;
                        case Status.Failure:
                            failure_.stripes_.gameObject.SetActive(true);
                            failure_.stripes_.sprite = adultStripesSad_;
                            break;
                        case Status.Transforming: return null;
                    }
                    break;
            }
            switch (teenCustomization)
            {
                case kAntenna:
                    switch (status)
                    {
                        case Status.Neutral:
                            neutral_.antenna_.gameObject.SetActive(true);
                            neutral_.antenna_.sprite = adultAntennaNeutral_;
                            break;
                        case Status.Success:
                            success_.antenna_.gameObject.SetActive(true);
                            success_.antenna_.sprite = adultAntennaHappy_;
                            break;
                        case Status.Failure:
                            failure_.antenna_.gameObject.SetActive(true);
                            failure_.antenna_.sprite = adultAntennaSad_;
                            break;
                        case Status.Transforming: return null;
                    }
                    break;
                case kSpots:
                    switch (status)
                    {
                        case Status.Neutral:
                            neutral_.spots_.gameObject.SetActive(true);
                            neutral_.spots_.sprite = adultSpotsNeutral_;
                            break;
                        case Status.Success:
                            success_.spots_.gameObject.SetActive(true);
                            success_.spots_.sprite = adultSpotsHappy_;
                            break;
                        case Status.Failure:
                            failure_.spots_.gameObject.SetActive(true);
                            failure_.spots_.sprite = adultSpotsSad_;
                            break;
                        case Status.Transforming: return null;
                    }
                    break;
                case kTail:
                    neutral_.tail_.gameObject.SetActive(true);
                    neutral_.tail_.sprite = adultTail_;
                    success_.tail_.gameObject.SetActive(true);
                    success_.tail_.sprite = adultTail_;
                    failure_.tail_.gameObject.SetActive(true);
                    failure_.tail_.sprite = adultTail_;
                    break;
            }
            switch (starter)
            {
                case kRed:
                    switch (status)
                    {
                        case Status.Neutral:
                            return adultRed_;
                        case Status.Success:
                            return adultRedHappy_;
                        case Status.Failure:
                            return adultRedSad_;
                    }
                    break;
                case kGreen:
                    switch (status)
                    {
                        case Status.Neutral:
                            return adultGreen_;
                        case Status.Success:
                            return adultGreenHappy_;
                        case Status.Failure:
                            return adultGreenSad_;
                    }
                    break;
                case kBlue:
                    switch (status)
                    {
                        case Status.Neutral:
                            return adultBlue_;
                        case Status.Success:
                            return adultBlueHappy_;
                        case Status.Failure:
                            return adultBlueSad_;
                    }
                    break;
            }
        }

        //Adult customization
        if (adultCustomization != null && adultCustomization.Length > 0)
        {
            neutral_.whiskers_.gameObject.SetActive(false);
            success_.whiskers_.gameObject.SetActive(false);
            failure_.whiskers_.gameObject.SetActive(false);

            neutral_.mohawk_.gameObject.SetActive(false);
            success_.mohawk_.gameObject.SetActive(false);
            failure_.mohawk_.gameObject.SetActive(false);

            neutral_.stripes_.gameObject.SetActive(false);
            success_.stripes_.gameObject.SetActive(false);
            failure_.stripes_.gameObject.SetActive(false);

            neutral_.antenna_.gameObject.SetActive(false);
            success_.antenna_.gameObject.SetActive(false);
            failure_.antenna_.gameObject.SetActive(false);

            neutral_.spots_.gameObject.SetActive(false);
            success_.spots_.gameObject.SetActive(false);
            failure_.spots_.gameObject.SetActive(false);

            neutral_.tail_.gameObject.SetActive(false);
            success_.tail_.gameObject.SetActive(false);
            failure_.tail_.gameObject.SetActive(false);
            switch (adultCustomization)
            {
                case kWhiskers:
                    switch (status)
                    {
                        case Status.Neutral:
                            neutral_.whiskers_.gameObject.SetActive(true);
                            neutral_.whiskers_.sprite = adultWhiskersNeutral_;
                            break;
                        case Status.Success:
                            success_.whiskers_.gameObject.SetActive(true);
                            success_.whiskers_.sprite = adultWhiskersHappy_;
                            break;
                        case Status.Failure:
                            failure_.whiskers_.gameObject.SetActive(true);
                            failure_.whiskers_.sprite = adultWhiskersSad_;
                            break;
                        case Status.Transforming: return null;
                    }
                    break;
                case kMohawk:
                    neutral_.mohawk_.gameObject.SetActive(true);
                    success_.mohawk_.gameObject.SetActive(true);
                    failure_.mohawk_.gameObject.SetActive(true);
                    break;
                case kStripes:
                    switch (status)
                    {
                        case Status.Neutral:
                            neutral_.stripes_.gameObject.SetActive(true);
                            neutral_.stripes_.sprite = adultStripesNeutral_;
                            break;
                        case Status.Success:
                            success_.stripes_.gameObject.SetActive(true);
                            success_.stripes_.sprite = adultStripesHappy_;
                            break;
                        case Status.Failure:
                            failure_.stripes_.gameObject.SetActive(true);
                            failure_.stripes_.sprite = adultStripesSad_;
                            break;
                        case Status.Transforming: return null;
                    }
                    break;
            }
            switch (teenCustomization)
            {
                case kAntenna:
                    switch (status)
                    {
                        case Status.Neutral:
                            neutral_.antenna_.gameObject.SetActive(true);
                            neutral_.antenna_.sprite = adultAntennaNeutral_;
                            break;
                        case Status.Success:
                            success_.antenna_.gameObject.SetActive(true);
                            success_.antenna_.sprite = adultAntennaHappy_;
                            break;
                        case Status.Failure:
                            failure_.antenna_.gameObject.SetActive(true);
                            failure_.antenna_.sprite = adultAntennaSad_;
                            break;
                        case Status.Transforming: return null;
                    }
                    break;
                case kSpots:
                    switch (status)
                    {
                        case Status.Neutral:
                            neutral_.spots_.gameObject.SetActive(true);
                            neutral_.spots_.sprite = adultSpotsNeutral_;
                            break;
                        case Status.Success:
                            success_.spots_.gameObject.SetActive(true);
                            success_.spots_.sprite = adultSpotsHappy_;
                            break;
                        case Status.Failure:
                            failure_.spots_.gameObject.SetActive(true);
                            failure_.spots_.sprite = adultSpotsSad_;
                            break;
                        case Status.Transforming: return null;
                    }
                    break;
                case kTail:
                    neutral_.tail_.gameObject.SetActive(true);
                    neutral_.tail_.sprite = adultTail_;
                    success_.tail_.gameObject.SetActive(true);
                    success_.tail_.sprite = adultTail_;
                    failure_.tail_.gameObject.SetActive(true);
                    failure_.tail_.sprite = adultTail_;
                    break;
            }
            switch (starter)
            {
                case kRed:
                    switch (status) 
                    {
                        case Status.Neutral:
                            return adultRed_;
                        case Status.Success:
                            return adultRedHappy_;
                        case Status.Failure:
                            return adultRedSad_;
                    }
                    break;
                case kGreen:
                    switch (status)
                    {
                        case Status.Neutral:
                            return adultGreen_;
                        case Status.Success:
                            return adultGreenHappy_;
                        case Status.Failure:
                            return adultGreenSad_;
                    }
                    break;
                case kBlue:
                    switch (status)
                    {
                        case Status.Neutral:
                            return adultBlue_;
                        case Status.Success:
                            return adultBlueHappy_;
                        case Status.Failure:
                            return adultBlueSad_;
                    }
                    break;
            }
        }
        
        //Teen customization
        if (teenCustomization != null && teenCustomization.Length > 0)
        {
            neutral_.antenna_.gameObject.SetActive(false);
            success_.antenna_.gameObject.SetActive(false);
            failure_.antenna_.gameObject.SetActive(false);

            neutral_.spots_.gameObject.SetActive(false);
            success_.spots_.gameObject.SetActive(false);
            failure_.spots_.gameObject.SetActive(false);

            neutral_.tail_.gameObject.SetActive(false);
            success_.tail_.gameObject.SetActive(false);
            failure_.tail_.gameObject.SetActive(false);
            switch (teenCustomization)
            {
                case kAntenna:
                    switch (status)
                    {
                        case Status.Neutral:
                            neutral_.antenna_.gameObject.SetActive(true);
                            neutral_.antenna_.sprite = teenAntennaNeutral_;
                            break;
                        case Status.Success:
                            success_.antenna_.gameObject.SetActive(true);
                            success_.antenna_.sprite = teenAntennaHappy_;
                            break;
                        case Status.Failure:
                            failure_.antenna_.gameObject.SetActive(true);
                            failure_.antenna_.sprite = teenAntennaSad_;
                            break;
                        case Status.Transforming: return null;
                    }
                    break;
                case kSpots:
                    switch (status)
                    {
                        case Status.Neutral:
                            neutral_.spots_.gameObject.SetActive(true);
                            neutral_.spots_.sprite = teenSpotsNeutral_;
                            break;
                        case Status.Success:
                            success_.spots_.gameObject.SetActive(true);
                            success_.spots_.sprite = teenSpotsHappy_;
                            break;
                        case Status.Failure:
                            failure_.spots_.gameObject.SetActive(true);
                            failure_.spots_.sprite = teenSpotsSad_;
                            break;
                        case Status.Transforming: return null;
                    }
                    break;
                case kTail:
                    neutral_.tail_.gameObject.SetActive(true);
                    success_.tail_.gameObject.SetActive(true);
                    failure_.tail_.gameObject.SetActive(true);
                    break;
            }
            switch (starter)
            {
                case kRed:
                    switch (status)
                    {
                        case Status.Neutral:
                            return teenRed_;
                        case Status.Success:
                            return teenRedHappy_;
                        case Status.Failure:
                            return teenRedSad_;
                    }
                    break;
                case kGreen:
                    switch (status)
                    {
                        case Status.Neutral:
                            return teenGreen_;
                        case Status.Success:
                            return teenGreenHappy_;
                        case Status.Failure:
                            return teenGreenSad_;
                    }
                    break;
                case kBlue:
                    switch (status)
                    {
                        case Status.Neutral:
                            return teenBlue_;
                        case Status.Success:
                            return teenBlueHappy_;
                        case Status.Failure:
                            return teenBlueSad_;
                    }
                    break;
            }
        }

        //Child customization
        switch (starter)
        {
            case kGreen:
                switch (status)
                {
                    case Status.Neutral: return childGreen_;
                    case Status.Success: return childGreenHappy_;
                    case Status.Failure: return childGreenSad_;
                    case Status.Transforming: return childGreenEvolving_;
                }
                break;
            case kBlue:
                switch (status)
                {
                    case Status.Neutral: return childBlue_;
                    case Status.Success: return childBlueHappy_;
                    case Status.Failure: return childBlueSad_;
                    case Status.Transforming: return childBlueEvolving_;
                }
                break;
            case kRed:
                switch (status)
                {
                    case Status.Neutral: return childRed_;
                    case Status.Success: return childRedHappy_;
                    case Status.Failure: return childRedSad_;
                    case Status.Transforming: return childRedEvolving_;
                }
                break;
        }

        return null;
    }
}
