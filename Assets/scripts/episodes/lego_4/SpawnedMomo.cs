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
    //[SerializeField] private List<Image> evolvingMomos_ = new List<Image>();


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

    #region OLD SPRITES
    //[SerializeField] private Sprite childGreenSuccess_;
    [SerializeField] private Sprite childGreenEvolving_;
    //[SerializeField] private Sprite childGreenFail_;
    ////[SerializeField] private Sprite teenGreen_;

    [SerializeField] private Sprite childBlueEvolving_;
    //[SerializeField] private Sprite childBlueSuccess_;
    //[SerializeField] private Sprite childBlueFail_;
    ////[SerializeField] private Sprite teenBlue_;

    [SerializeField] private Sprite childRedEvolving_;
    //[SerializeField] private Sprite childRedSuccess_;
    //[SerializeField] private Sprite childRedFail_;
    ////[SerializeField] private Sprite teenRed_;

    //[SerializeField] private Sprite teenGreenTailEvolving_;
    //[SerializeField] private Sprite teenGreenSpotsEvolving_;
    //[SerializeField] private Sprite teenGreenAntennaEvolving_;
    //[SerializeField] private Sprite teenRedTailEvolving_;
    //[SerializeField] private Sprite teenRedSpotsEvolving_;
    //[SerializeField] private Sprite teenRedAntennaEvolving_;
    //[SerializeField] private Sprite teenBlueTailEvolving_;
    //[SerializeField] private Sprite teenBlueSpotsEvolving_;
    //[SerializeField] private Sprite teenBlueAntennaEvolving_;

    //[SerializeField] private Sprite teenGreenAntenna_;
    //[SerializeField] private Sprite teenGreenAntennaSuccess_;
    //[SerializeField] private Sprite teenGreenAntennaFail_;
    //[SerializeField] private Sprite teenGreenSpots_;
    //[SerializeField] private Sprite teenGreenSpotsSuccess_;
    //[SerializeField] private Sprite teenGreenSpotsFail_;
    //[SerializeField] private Sprite teenGreenTail_;
    //[SerializeField] private Sprite teenGreenTailSuccess_;
    //[SerializeField] private Sprite teenGreenTailFail_;

    //[SerializeField] private Sprite teenBlueAntenna_;
    //[SerializeField] private Sprite teenBlueAntennaSuccess_;
    //[SerializeField] private Sprite teenBlueAntennaFail_;
    //[SerializeField] private Sprite teenBlueSpots_;
    //[SerializeField] private Sprite teenBlueSpotsSuccess_;
    //[SerializeField] private Sprite teenBlueSpotsFail_;
    //[SerializeField] private Sprite teenBlueTail_;
    //[SerializeField] private Sprite teenBlueTailSuccess_;
    //[SerializeField] private Sprite teenBlueTailFail_;

    //[SerializeField] private Sprite teenRedAntenna_;
    //[SerializeField] private Sprite teenRedAntennaSuccess_;
    //[SerializeField] private Sprite teenRedAntennaFail_;
    //[SerializeField] private Sprite teenRedSpots_;
    //[SerializeField] private Sprite teenRedSpotsSuccess_;
    //[SerializeField] private Sprite teenRedSpotsFail_;
    //[SerializeField] private Sprite teenRedTail_;
    //[SerializeField] private Sprite teenRedTailSuccess_;
    //[SerializeField] private Sprite teenRedTailFail_;

    //[SerializeField] private Sprite adultGreenAntenna_;
    //[SerializeField] private Sprite adultGreenSpots_;
    //[SerializeField] private Sprite adultGreenTail_;

    //[SerializeField] private Sprite adultBlueAntenna_;
    //[SerializeField] private Sprite adultBlueSpots_;
    //[SerializeField] private Sprite adultBlueTail_;

    //[SerializeField] private Sprite adultRedAntenna_;
    //[SerializeField] private Sprite adultRedSpots_;
    //[SerializeField] private Sprite adultRedTail_;

    //[SerializeField] private Sprite adultGreenAntennaWhiskers_;
    //[SerializeField] private Sprite adultGreenAntennaWhiskersSuccess_;
    //[SerializeField] private Sprite adultGreenAntennaWhiskersFailure_;
    //[SerializeField] private Sprite adultGreenAntennaMohawk_;
    //[SerializeField] private Sprite adultGreenAntennaMohawkSuccess_;
    //[SerializeField] private Sprite adultGreenAntennaMohawkFailure_;
    //[SerializeField] private Sprite adultGreenAntennaStripes_;
    //[SerializeField] private Sprite adultGreenAntennaStripesSuccess_;
    //[SerializeField] private Sprite adultGreenAntennaStripesFailure_;
    //[SerializeField] private Sprite adultGreenSpotsWhiskers_;
    //[SerializeField] private Sprite adultGreenSpotsWhiskersSuccess_;
    //[SerializeField] private Sprite adultGreenSpotsWhiskersFailure_;
    //[SerializeField] private Sprite adultGreenSpotsMohawk_;
    //[SerializeField] private Sprite adultGreenSpotsMohawkSuccess_;
    //[SerializeField] private Sprite adultGreenSpotsMohawkFailure_;
    //[SerializeField] private Sprite adultGreenSpotsStripes_;
    //[SerializeField] private Sprite adultGreenSpotsStripesSuccess_;
    //[SerializeField] private Sprite adultGreenSpotsStripesFailure_;
    //[SerializeField] private Sprite adultGreenTailWhiskers_;
    //[SerializeField] private Sprite adultGreenTailWhiskersSuccess_;
    //[SerializeField] private Sprite adultGreenTailWhiskersFailure_;
    //[SerializeField] private Sprite adultGreenTailMohawk_;
    //[SerializeField] private Sprite adultGreenTailMohawkSuccess_;
    //[SerializeField] private Sprite adultGreenTailMohawkFailure_;
    //[SerializeField] private Sprite adultGreenTailStripes_;
    //[SerializeField] private Sprite adultGreenTailStripesSuccess_;
    //[SerializeField] private Sprite adultGreenTailStripesFailure_;

    //[SerializeField] private Sprite adultBlueAntennaWhiskers_;
    //[SerializeField] private Sprite adultBlueAntennaWhiskersSuccess_;
    //[SerializeField] private Sprite adultBlueAntennaWhiskersFailure_;
    //[SerializeField] private Sprite adultBlueAntennaMohawk_;
    //[SerializeField] private Sprite adultBlueAntennaMohawkSuccess_;
    //[SerializeField] private Sprite adultBlueAntennaMohawkFailure_;
    //[SerializeField] private Sprite adultBlueAntennaStripes_;
    //[SerializeField] private Sprite adultBlueAntennaStripesSuccess_;
    //[SerializeField] private Sprite adultBlueAntennaStripesFailure_;
    //[SerializeField] private Sprite adultBlueSpotsWhiskers_;
    //[SerializeField] private Sprite adultBlueSpotsWhiskersSuccess_;
    //[SerializeField] private Sprite adultBlueSpotsWhiskersFailure_;
    //[SerializeField] private Sprite adultBlueSpotsMohawk_;
    //[SerializeField] private Sprite adultBlueSpotsMohawkSuccess_;
    //[SerializeField] private Sprite adultBlueSpotsMohawkFailure_;
    //[SerializeField] private Sprite adultBlueSpotsStripes_;
    //[SerializeField] private Sprite adultBlueSpotsStripesSuccess_;
    //[SerializeField] private Sprite adultBlueSpotsStripesFailure_;
    //[SerializeField] private Sprite adultBlueTailWhiskers_;
    //[SerializeField] private Sprite adultBlueTailWhiskersSuccess_;
    //[SerializeField] private Sprite adultBlueTailWhiskersFailure_;
    //[SerializeField] private Sprite adultBlueTailMohawk_;
    //[SerializeField] private Sprite adultBlueTailMohawkSuccess_;
    //[SerializeField] private Sprite adultBlueTailMohawkFailure_;
    //[SerializeField] private Sprite adultBlueTailStripes_;
    //[SerializeField] private Sprite adultBlueTailStripesSuccess_;
    //[SerializeField] private Sprite adultBlueTailStripesFailure_;

    //[SerializeField] private Sprite adultRedAntennaWhiskers_;
    //[SerializeField] private Sprite adultRedAntennaWhiskersSuccess_;
    //[SerializeField] private Sprite adultRedAntennaWhiskersFailure_;
    //[SerializeField] private Sprite adultRedAntennaMohawk_;
    //[SerializeField] private Sprite adultRedAntennaMohawkSuccess_;
    //[SerializeField] private Sprite adultRedAntennaMohawkFailure_;
    //[SerializeField] private Sprite adultRedAntennaStripes_;
    //[SerializeField] private Sprite adultRedAntennaStripesSuccess_;
    //[SerializeField] private Sprite adultRedAntennaStripesFailure_;
    //[SerializeField] private Sprite adultRedSpotsWhiskers_;
    //[SerializeField] private Sprite adultRedSpotsWhiskersSuccess_;
    //[SerializeField] private Sprite adultRedSpotsWhiskersFailure_;
    //[SerializeField] private Sprite adultRedSpotsMohawk_;
    //[SerializeField] private Sprite adultRedSpotsMohawkSuccess_;
    //[SerializeField] private Sprite adultRedSpotsMohawkFailure_;
    //[SerializeField] private Sprite adultRedSpotsStripes_;
    //[SerializeField] private Sprite adultRedSpotsStripesSuccess_;
    //[SerializeField] private Sprite adultRedSpotsStripesFailure_;
    //[SerializeField] private Sprite adultRedTailWhiskers_;
    //[SerializeField] private Sprite adultRedTailWhiskersSuccess_;
    //[SerializeField] private Sprite adultRedTailWhiskersFailure_;
    //[SerializeField] private Sprite adultRedTailMohawk_;
    //[SerializeField] private Sprite adultRedTailMohawkSuccess_;
    //[SerializeField] private Sprite adultRedTailMohawkFailure_;
    //[SerializeField] private Sprite adultRedTailStripes_;
    //[SerializeField] private Sprite adultRedTailStripesSuccess_;
    //[SerializeField] private Sprite adultRedTailStripesFailure_;
    #endregion

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
    private bool inTesting_ = false;

    private GoTweenFlow dismissingFlow_;

    private void Start()
    {
        ShareManager sm = (ShareManager)gameManager_;
        if (sm != null)
        {
            starterBackground_.transform.SetParent(sm.OverlayParent);
        }

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
        Debug.LogWarning(action);
        if (args.Count == 0) return;

        //-momo success 2830192

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
            //update options to Left Middle Right choices
            //gameManager_.SendNewActionInternal("-update-options choose");
            //on finish choose, start reward sequence
        }
        else {
            StartCoroutine(DisplayMomo(commandType_, false));
        }

        #region Comments
        //example reward call: -momo success 2830192
        //2830192 = NFC id

        //pseudo code / logic
        /*
         if (LevelOfMomo(nfcId) == 0)
            HandleStarterPicker()     //this asks users to select their starter Momo (red, green, blue)
            //we need to update the choices to be Left Middle Right, so that the player can select their Momo
                //Look to Lego 1 reward to see how we do it (challenge name 20-pound-4-wide)
            //after user selects their Starter, we need to go to main reward

         main reward sequence:
         //show the customized Momo of the player for a few seconds: HandleTesting();
         //show the success image: HandleSuccess();
         //show customize: HandleCustomize();
         //update the choices to be Left Middle Right, so that the player can customize
         */
        #endregion
    }

    private IEnumerator DisplayMomo(string command, bool start) {
        if (!start)
        {
            if (command.Contains("success"))
            {
                ShowMomoOnScreen();
                yield return new WaitForSeconds(1);
                ShowMomoEatingBerry();
                yield return new WaitForSeconds(1);
                RewardSequence();
                Debug.LogWarning("reward");
            }
            else
            {
                HandleCustomizeSelection(command);
                Debug.LogWarning("customize");
            }
        }
        else {
            ShowMomoOnScreen();
            yield return new WaitForSeconds(1);
            ShowMomoEatingBerry();
        }
    }

    private void RewardSequence()
    {
        //ShowMomoOnScreen();

        //ShowMomoEatingBerry();

        ShowMomoUpgrade();

        //update options to Left Middle Right choices
        //gameManager_.SendNewActionInternal("-update-options choose");

        //after choice is complete, bring options back to default
        //gameManager_.SendNewActionInternal("-update-options default");
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
        evolveBackground_.gameObject.SetActive(false);
        //neutralBackground_.gameObject.SetActive(false);
        //successBackground_.gameObject.SetActive(false);
        //failureBackground_.gameObject.SetActive(false);

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
        Debug.LogWarning("starter picker");

    }

    //Select momo's color and initial setup
    private void HandleStarterPickerSelection(string choice)
    {
        if (currentRfid_ == null || currentRfid_.Length == 0) return;
        Debug.LogWarning("picker selection");
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
            StartCoroutine(DisplayMomo(commandType_, true));
            gameManager_.SendNewActionInternal("-update-options default");
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
            gameManager_.SendNewActionInternal("-character talk momo-adult");
            Go.to(this, 5f, new GoTweenConfig().onComplete(t => {
                gameManager_.SendNewActionInternal("-character talk momo-customization");
            }));
        }

        if (level == 2)
        {
            //customizeAdultMomo_.sprite = sprites[1];
            //customizeAdultMomo_.SetNativeSize();
            //customizeAdultBackground_.gameObject.SetActive(true);

            customizeAdult_.momo_.sprite = sprites[1];
            customizeAdult_.background_.gameObject.SetActive(true);

            gameManager_.SendNewActionInternal("-character talk momo-adult");
            Go.to(this, 2.5f, new GoTweenConfig().onComplete(t => {
                gameManager_.SendNewActionInternal("-character talk momo-customization");
            }));
        }
        if (level == 1)
        {
            //customizeTeenMomo_.sprite = sprites[1];
            //customizeTeenMomo_.SetNativeSize();
            //customizeTeenBackground_.gameObject.SetActive(true);

            customizeTeen_.momo_.sprite = sprites[1];

            customizeTeen_.background_.gameObject.SetActive(true);

            gameManager_.SendNewActionInternal("-character talk momo-teen");
            Go.to(this, 5f, new GoTweenConfig().onComplete(t => {
                gameManager_.SendNewActionInternal("-character talk momo-customization");
            }));
        }
    }

    private void HandleCustomizeSelection(string choice)
    {
        if (currentRfid_ == null || currentRfid_.Length == 0) return;

        //bool teenActive = customizeTeenBackground_.gameObject.activeSelf;
        //bool adultActive = customizeAdultBackground_.gameObject.activeSelf;
        bool teenActive = customizeTeen_.background_.gameObject.activeSelf;
        bool adultActive = customizeAdult_.background_.gameObject.activeSelf;
        bool seniorActive = customizeSenior_.background_.gameObject.activeSelf;

        GameStorage gs = gameManager_.GameStorageForUserId(currentRfid_);

        List<Sprite> sprites = SpritesEvolveForRfid(currentRfid_);

        //if (teenActive)
        //{
        //    //customizeTeenMomo_.sprite = sprites[1];
        //    //customizeTeenMomo_.SetNativeSize();
        //}
        //else
        //{
        //    //customizeAdultMomo_.sprite = sprites[1];
        //    //customizeAdultMomo_.SetNativeSize();
        //}

        //Select accesories for the momo
        if (teenActive)
        {
            string selection = null;
            customizeTeen_.momo_.sprite = sprites[1];
            switch (choice)
            {
                case "left":
                    selection = kSpots;
                    break;
                case "middle":
                    selection = kAntenna;
                    break;
                case "right":
                    selection = kTail;
                    break;
            }
            gs.Add<string>(GameStorage.Key.MomoTeenCustomization, selection);
            sprites = SpritesEvolveForRfid(currentRfid_);
        } else if (adultActive)
        {
            string selection = null;
            customizeAdult_.momo_.sprite = sprites[1];
            switch (choice)
            {
                case "left":
                    selection = kWhiskers;
                    break;
                case "middle":
                    selection = kStripes;
                    break;
                case "right":
                    selection = kMohawk;
                    break;
            }
            gs.Add<string>(GameStorage.Key.MomoAdultCustomization, selection);
            sprites = SpritesEvolveForRfid(currentRfid_);
        }
        else if (seniorActive)
        {
            string selection = null;
            customizeSenior_.momo_.sprite = sprites[1];
            switch (choice)
            {
                case "left":
                    selection = kWings;
                    break;
                case "middle":
                    selection = kNose;
                    break;
                case "right":
                    selection = kClaws;
                    break;
            }
            gs.Add<string>(GameStorage.Key.MomoSeniorCustomization, selection);
            sprites = SpritesEvolveForRfid(currentRfid_);
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
            flow.play();
        } else
        {
            SetSprite(customizeAdultMomo_, currentRfid_, Status.Neutral);

            GoTweenFlow flow = new GoTweenFlow();
            flow.insert(0f, new GoTween(customizeAdultMomo_.transform, 0f, new GoTweenConfig().scale(0.35f)));
            flow.insert(0.25f, new GoTween(customizeAdultMomo_.transform, 0.25f, new GoTweenConfig().scale(0.3f)));
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

        neutralBackground_.gameObject.SetActive(true);
        SetSprite(neutralMomo_, currentRfid_, Status.Neutral);
    }

    private void ShowMomoEatingBerry()
    {
        if (currentRfid_ == null || currentRfid_.Length == 0) return;

        HideAllScenes();

        AudioPlayer.PlayAudio("audio/sfx/momo-grunt");

        successBackground_.gameObject.SetActive(true);
        //SetSprite(successMomo_, currentRfid_, Status.Success);
        SetSprite(success_.momo_, currentRfid_, Status.Success);

    }

    private void HandleFailure()
    {
        if (currentRfid_ == null || currentRfid_.Length == 0) return;

        HideAllScenes();

        AudioPlayer.PlayAudio("audio/sfx/wall-crash");

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
            Debug.LogWarning(3);
            return 3;
        }
        if (teenCustomization != null && teenCustomization.Length > 0)
        {
            Debug.LogWarning(2);
            return 2;
        }
        if (starter != null && starter.Length > 0)
        {
            Debug.LogWarning(1);
            return 1;
        }

        return 0;
    }

    //return sprites based on the momo's level
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
                        //childGreenEvolving_,
                        teenGreen_
                    };
                case kBlue:
                    return new List<Sprite>()
                    {
                        childBlue_,
                        //childBlueEvolving_,
                        teenBlue_
                    };
                case kRed:
                    return new List<Sprite>()
                    {
                        childRed_,
                        //childRedEvolving_,
                        teenRed_
                    };
            }
        }
        else if (LevelOfMomo(rfid) == 2)
        {
            #region comments
            //switch (starter)
            //{
            //    case kGreen:
            //        switch(teenCustomization)
            //        {
            //            case kAntenna:
            //                return new List<Sprite>()
            //                {
            //                    teenGreenAntenna_,
            //                    teenGreenAntennaEvolving_,
            //                    adultGreenAntenna_
            //                };
            //            case kSpots:
            //                return new List<Sprite>()
            //                {
            //                    teenGreenSpots_,
            //                    teenGreenSpotsEvolving_,
            //                    adultGreenSpots_
            //                };
            //            case kTail:
            //                return new List<Sprite>()
            //                {
            //                    teenGreenTail_,
            //                    teenGreenTailEvolving_,
            //                    adultGreenTail_
            //                };
            //        }
            //        break;
            //    case kBlue:
            //        switch (teenCustomization)
            //        {
            //            case kAntenna:
            //                return new List<Sprite>()
            //                {
            //                    teenBlueAntenna_,
            //                    teenBlueAntennaEvolving_,
            //                    adultBlueAntenna_
            //                };
            //            case kSpots:
            //                return new List<Sprite>()
            //                {
            //                    teenBlueSpots_,
            //                    teenBlueSpotsEvolving_,
            //                    adultBlueSpots_
            //                };
            //            case kTail:
            //                return new List<Sprite>()
            //                {
            //                    teenBlueTail_,
            //                    teenBlueTailEvolving_,
            //                    adultBlueTail_
            //                };
            //        }
            //        break;
            //    case kRed:
            //        switch (teenCustomization)
            //        {
            //            case kAntenna:
            //                return new List<Sprite>()
            //                {
            //                    teenRedAntenna_,
            //                    teenRedAntennaEvolving_,
            //                    adultRedAntenna_
            //                };
            //            case kSpots:
            //                return new List<Sprite>()
            //                {
            //                    teenRedSpots_,
            //                    teenRedSpotsEvolving_,
            //                    adultRedSpots_
            //                };
            //            case kTail:
            //                return new List<Sprite>()
            //                {
            //                    teenRedTail_,
            //                    teenRedTailEvolving_,
            //                    adultRedTail_
            //                };
            //        }
            //        break;
            //}
            #endregion
            Debug.LogWarning("level 2");
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
                        //childGreenEvolving_,
                        adultGreen_
                    };
                case kBlue:
                    return new List<Sprite>()
                    {
                        teenBlue_,
                        //childBlueEvolving_,
                        adultBlue_
                    };
                case kRed:
                    return new List<Sprite>()
                    {
                        teenRed_,
                        //childRedEvolving_,
                        adultRed_
                    };
            }
        }
        else if (LevelOfMomo(rfid) == 3) {

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
                    break;
            }

            switch (adultCustomization)
            {
                case kWhiskers:
                    customizeSenior_.whiskers_.gameObject.SetActive(true);
                    customizeSenior_.whiskers_.sprite = adultAntennaNeutral_;
                    break;
                case kStripes:
                    customizeSenior_.stripes_.gameObject.SetActive(true);
                    customizeSenior_.stripes_.sprite = adultSpotsNeutral_;
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
                        //childGreenEvolving_,
                        adultGreen_
                    };
                case kBlue:
                    return new List<Sprite>()
                    {
                        adultBlue_,
                        //childBlueEvolving_,
                        adultBlue_
                    };
                case kRed:
                    return new List<Sprite>()
                    {
                        adultRed_,
                        //childRedEvolving_,
                        adultRed_
                    };
            }
        }

        return null;
    }

    private void SetSprite(Image image, string rfid, Status status)
    {
        image.sprite = MomoForRfid(rfid, status);
        //image.SetNativeSize();
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
            #region comments
            //switch(starter)
            //{
            //    case kGreen:
            //        switch (teenCustomization)
            //        {
            //            case kAntenna:
            //                switch(adultCustomization)
            //                {
            //                    case kWhiskers:
            //                        switch(status)
            //                        {
            //                            case Status.Neutral: return adultGreenAntennaWhiskers_;
            //                            case Status.Success: return adultGreenAntennaWhiskersSuccess_;
            //                            case Status.Failure: return adultGreenAntennaWhiskersFailure_;
            //                            case Status.Transforming: return null;
            //                        }
            //                        break;
            //                    case kMohawk:
            //                        switch (status)
            //                        {
            //                            case Status.Neutral: return adultGreenAntennaMohawk_;
            //                            case Status.Success: return adultGreenAntennaMohawkSuccess_;
            //                            case Status.Failure: return adultGreenAntennaMohawkFailure_;
            //                            case Status.Transforming: return null;
            //                        }
            //                        break;
            //                    case kStripes:
            //                        switch (status)
            //                        {
            //                            case Status.Neutral: return adultGreenAntennaStripes_;
            //                            case Status.Success: return adultGreenAntennaStripesSuccess_;
            //                            case Status.Failure: return adultGreenAntennaStripesFailure_;
            //                            case Status.Transforming: return null;
            //                        }
            //                        break;
            //                }
            //                break;
            //            case kSpots:
            //                switch (adultCustomization)
            //                {
            //                    case kWhiskers:
            //                        switch (status)
            //                        {
            //                            case Status.Neutral: return adultGreenSpotsWhiskers_;
            //                            case Status.Success: return adultGreenSpotsWhiskersSuccess_;
            //                            case Status.Failure: return adultGreenSpotsWhiskersFailure_;
            //                            case Status.Transforming: return null;
            //                        }
            //                        break;
            //                    case kMohawk:
            //                        switch (status)
            //                        {
            //                            case Status.Neutral: return adultGreenSpotsMohawk_;
            //                            case Status.Success: return adultGreenSpotsMohawkSuccess_;
            //                            case Status.Failure: return adultGreenSpotsMohawkFailure_;
            //                            case Status.Transforming: return null;
            //                        }
            //                        break;
            //                    case kStripes:
            //                        switch (status)
            //                        {
            //                            case Status.Neutral: return adultGreenSpotsStripes_;
            //                            case Status.Success: return adultGreenSpotsStripesSuccess_;
            //                            case Status.Failure: return adultGreenSpotsStripesFailure_;
            //                            case Status.Transforming: return null;
            //                        }
            //                        break;
            //                }
            //                break;
            //            case kTail:
            //                switch (adultCustomization)
            //                {
            //                    case kWhiskers:
            //                        switch (status)
            //                        {
            //                            case Status.Neutral: return adultGreenTailWhiskers_;
            //                            case Status.Success: return adultGreenTailWhiskersSuccess_;
            //                            case Status.Failure: return adultGreenTailWhiskersFailure_;
            //                            case Status.Transforming: return null;
            //                        }
            //                        break;
            //                    case kMohawk:
            //                        switch (status)
            //                        {
            //                            case Status.Neutral: return adultGreenTailMohawk_;
            //                            case Status.Success: return adultGreenTailMohawkSuccess_;
            //                            case Status.Failure: return adultGreenTailMohawkFailure_;
            //                            case Status.Transforming: return null;
            //                        }
            //                        break;
            //                    case kStripes:
            //                        switch (status)
            //                        {
            //                            case Status.Neutral: return adultGreenTailStripes_;
            //                            case Status.Success: return adultGreenTailStripesSuccess_;
            //                            case Status.Failure: return adultGreenTailStripesFailure_;
            //                            case Status.Transforming: return null;
            //                        }
            //                        break;
            //                }
            //                break;
            //        }
            //        break;
            //    case kBlue:
            //        switch (teenCustomization)
            //        {
            //            case kAntenna:
            //                switch (adultCustomization)
            //                {
            //                    case kWhiskers:
            //                        switch (status)
            //                        {
            //                            case Status.Neutral: return adultBlueAntennaWhiskers_;
            //                            case Status.Success: return adultBlueAntennaWhiskersSuccess_;
            //                            case Status.Failure: return adultBlueAntennaWhiskersFailure_;
            //                            case Status.Transforming: return null;
            //                        }
            //                        break;
            //                    case kMohawk:
            //                        switch (status)
            //                        {
            //                            case Status.Neutral: return adultBlueAntennaMohawk_;
            //                            case Status.Success: return adultBlueAntennaMohawkSuccess_;
            //                            case Status.Failure: return adultBlueAntennaMohawkFailure_;
            //                            case Status.Transforming: return null;
            //                        }
            //                        break;
            //                    case kStripes:
            //                        switch (status)
            //                        {
            //                            case Status.Neutral: return adultBlueAntennaStripes_;
            //                            case Status.Success: return adultBlueAntennaStripesSuccess_;
            //                            case Status.Failure: return adultBlueAntennaStripesFailure_;
            //                            case Status.Transforming: return null;
            //                        }
            //                        break;
            //                }
            //                break;
            //            case kSpots:
            //                switch (adultCustomization)
            //                {
            //                    case kWhiskers:
            //                        switch (status)
            //                        {
            //                            case Status.Neutral: return adultBlueSpotsWhiskers_;
            //                            case Status.Success: return adultBlueSpotsWhiskersSuccess_;
            //                            case Status.Failure: return adultBlueSpotsWhiskersFailure_;
            //                            case Status.Transforming: return null;
            //                        }
            //                        break;
            //                    case kMohawk:
            //                        switch (status)
            //                        {
            //                            case Status.Neutral: return adultBlueSpotsMohawk_;
            //                            case Status.Success: return adultBlueSpotsMohawkSuccess_;
            //                            case Status.Failure: return adultBlueSpotsMohawkFailure_;
            //                            case Status.Transforming: return null;
            //                        }
            //                        break;
            //                    case kStripes:
            //                        switch (status)
            //                        {
            //                            case Status.Neutral: return adultBlueSpotsStripes_;
            //                            case Status.Success: return adultBlueSpotsStripesSuccess_;
            //                            case Status.Failure: return adultBlueSpotsStripesFailure_;
            //                            case Status.Transforming: return null;
            //                        }
            //                        break;
            //                }
            //                break;
            //            case kTail:
            //                switch (adultCustomization)
            //                {
            //                    case kWhiskers:
            //                        switch (status)
            //                        {
            //                            case Status.Neutral: return adultBlueTailWhiskers_;
            //                            case Status.Success: return adultBlueTailWhiskersSuccess_;
            //                            case Status.Failure: return adultBlueTailWhiskersFailure_;
            //                            case Status.Transforming: return null;
            //                        }
            //                        break;
            //                    case kMohawk:
            //                        switch (status)
            //                        {
            //                            case Status.Neutral: return adultBlueTailMohawk_;
            //                            case Status.Success: return adultBlueTailMohawkSuccess_;
            //                            case Status.Failure: return adultBlueTailMohawkFailure_;
            //                            case Status.Transforming: return null;
            //                        }
            //                        break;
            //                    case kStripes:
            //                        switch (status)
            //                        {
            //                            case Status.Neutral: return adultBlueTailStripes_;
            //                            case Status.Success: return adultBlueTailStripesSuccess_;
            //                            case Status.Failure: return adultBlueTailStripesFailure_;
            //                            case Status.Transforming: return null;
            //                        }
            //                        break;
            //                }
            //                break;
            //        }
            //        break;
            //    case kRed:
            //        switch (teenCustomization)
            //        {
            //            case kAntenna:
            //                switch (adultCustomization)
            //                {
            //                    case kWhiskers:
            //                        switch (status)
            //                        {
            //                            case Status.Neutral: return adultRedAntennaWhiskers_;
            //                            case Status.Success: return adultRedAntennaWhiskersSuccess_;
            //                            case Status.Failure: return adultRedAntennaWhiskersFailure_;
            //                            case Status.Transforming: return null;
            //                        }
            //                        break;
            //                    case kMohawk:
            //                        switch (status)
            //                        {
            //                            case Status.Neutral: return adultRedAntennaMohawk_;
            //                            case Status.Success: return adultRedAntennaMohawkSuccess_;
            //                            case Status.Failure: return adultRedAntennaMohawkFailure_;
            //                            case Status.Transforming: return null;
            //                        }
            //                        break;
            //                    case kStripes:
            //                        switch (status)
            //                        {
            //                            case Status.Neutral: return adultRedAntennaStripes_;
            //                            case Status.Success: return adultRedAntennaStripesSuccess_;
            //                            case Status.Failure: return adultRedAntennaStripesFailure_;
            //                            case Status.Transforming: return null;
            //                        }
            //                        break;
            //                }
            //                break;
            //            case kSpots:
            //                switch (adultCustomization)
            //                {
            //                    case kWhiskers:
            //                        switch (status)
            //                        {
            //                            case Status.Neutral: return adultRedSpotsWhiskers_;
            //                            case Status.Success: return adultRedSpotsWhiskersSuccess_;
            //                            case Status.Failure: return adultRedSpotsWhiskersFailure_;
            //                            case Status.Transforming: return null;
            //                        }
            //                        break;
            //                    case kMohawk:
            //                        switch (status)
            //                        {
            //                            case Status.Neutral: return adultRedSpotsMohawk_;
            //                            case Status.Success: return adultRedSpotsMohawkSuccess_;
            //                            case Status.Failure: return adultRedSpotsMohawkFailure_;
            //                            case Status.Transforming: return null;
            //                        }
            //                        break;
            //                    case kStripes:
            //                        switch (status)
            //                        {
            //                            case Status.Neutral: return adultRedSpotsStripes_;
            //                            case Status.Success: return adultRedSpotsStripesSuccess_;
            //                            case Status.Failure: return adultRedSpotsStripesFailure_;
            //                            case Status.Transforming: return null;
            //                        }
            //                        break;
            //                }
            //                break;
            //            case kTail:
            //                switch (adultCustomization)
            //                {
            //                    case kWhiskers:
            //                        switch (status)
            //                        {
            //                            case Status.Neutral: return adultRedTailWhiskers_;
            //                            case Status.Success: return adultRedTailWhiskersSuccess_;
            //                            case Status.Failure: return adultRedTailWhiskersFailure_;
            //                            case Status.Transforming: return null;
            //                        }
            //                        break;
            //                    case kMohawk:
            //                        switch (status)
            //                        {
            //                            case Status.Neutral: return adultRedTailMohawk_;
            //                            case Status.Success: return adultRedTailMohawkSuccess_;
            //                            case Status.Failure: return adultRedTailMohawkFailure_;
            //                            case Status.Transforming: return null;
            //                        }
            //                        break;
            //                    case kStripes:
            //                        switch (status)
            //                        {
            //                            case Status.Neutral: return adultRedTailStripes_;
            //                            case Status.Success: return adultRedTailStripesSuccess_;
            //                            case Status.Failure: return adultRedTailStripesFailure_;
            //                            case Status.Transforming: return null;
            //                        }
            //                        break;
            //                }
            //                break;
            //        }
            //        break;
            //}
            #endregion
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
