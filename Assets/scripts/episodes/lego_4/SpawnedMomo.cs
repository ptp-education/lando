using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnedMomo : SpawnedObject
{
    [SerializeField] private Image starterBackground_;
    [SerializeField] private Transform starterSelected_;
    [SerializeField] private Image starterMomo_;

    [SerializeField] private Image customizeTeenBackground_;
    [SerializeField] private Image customizeTeenMomo_;

    [SerializeField] private Image customizeAdultBackground_;
    [SerializeField] private Image customizeAdultMomo_;

    [SerializeField] private Image evolveBackground_;
    [SerializeField] private List<Image> evolvingMomos_ = new List<Image>();

    [SerializeField] private Image neutralBackground_;
    [SerializeField] private Image neutralMomo_;

    [SerializeField] private Image successBackground_;
    [SerializeField] private Image successMomo_;

    [SerializeField] private Image failureBackground_;
    [SerializeField] private Image failureMomo_;

    #region SPRITES
    [SerializeField] private Sprite childGreen_;
    [SerializeField] private Sprite childGreenEvolving_;
    [SerializeField] private Sprite childGreenSuccess_;
    [SerializeField] private Sprite childGreenFail_;
    [SerializeField] private Sprite teenGreen_;

    [SerializeField] private Sprite childBlue_;
    [SerializeField] private Sprite childBlueEvolving_;
    [SerializeField] private Sprite childBlueSuccess_;
    [SerializeField] private Sprite childBlueFail_;
    [SerializeField] private Sprite teenBlue_;

    [SerializeField] private Sprite childRed_;
    [SerializeField] private Sprite childRedEvolving_;
    [SerializeField] private Sprite childRedSuccess_;
    [SerializeField] private Sprite childRedFail_;
    [SerializeField] private Sprite teenRed_;

    [SerializeField] private Sprite teenGreenTailEvolving_;
    [SerializeField] private Sprite teenGreenSpotsEvolving_;
    [SerializeField] private Sprite teenGreenAntennaEvolving_;
    [SerializeField] private Sprite teenRedTailEvolving_;
    [SerializeField] private Sprite teenRedSpotsEvolving_;
    [SerializeField] private Sprite teenRedAntennaEvolving_;
    [SerializeField] private Sprite teenBlueTailEvolving_;
    [SerializeField] private Sprite teenBlueSpotsEvolving_;
    [SerializeField] private Sprite teenBlueAntennaEvolving_;

    [SerializeField] private Sprite teenGreenAntenna_;
    [SerializeField] private Sprite teenGreenAntennaSuccess_;
    [SerializeField] private Sprite teenGreenAntennaFail_;
    [SerializeField] private Sprite teenGreenSpots_;
    [SerializeField] private Sprite teenGreenSpotsSuccess_;
    [SerializeField] private Sprite teenGreenSpotsFail_;
    [SerializeField] private Sprite teenGreenTail_;
    [SerializeField] private Sprite teenGreenTailSuccess_;
    [SerializeField] private Sprite teenGreenTailFail_;

    [SerializeField] private Sprite teenBlueAntenna_;
    [SerializeField] private Sprite teenBlueAntennaSuccess_;
    [SerializeField] private Sprite teenBlueAntennaFail_;
    [SerializeField] private Sprite teenBlueSpots_;
    [SerializeField] private Sprite teenBlueSpotsSuccess_;
    [SerializeField] private Sprite teenBlueSpotsFail_;
    [SerializeField] private Sprite teenBlueTail_;
    [SerializeField] private Sprite teenBlueTailSuccess_;
    [SerializeField] private Sprite teenBlueTailFail_;

    [SerializeField] private Sprite teenRedAntenna_;
    [SerializeField] private Sprite teenRedAntennaSuccess_;
    [SerializeField] private Sprite teenRedAntennaFail_;
    [SerializeField] private Sprite teenRedSpots_;
    [SerializeField] private Sprite teenRedSpotsSuccess_;
    [SerializeField] private Sprite teenRedSpotsFail_;
    [SerializeField] private Sprite teenRedTail_;
    [SerializeField] private Sprite teenRedTailSuccess_;
    [SerializeField] private Sprite teenRedTailFail_;

    [SerializeField] private Sprite adultGreenAntenna_;
    [SerializeField] private Sprite adultGreenSpots_;
    [SerializeField] private Sprite adultGreenTail_;
    
    [SerializeField] private Sprite adultBlueAntenna_;
    [SerializeField] private Sprite adultBlueSpots_;
    [SerializeField] private Sprite adultBlueTail_;

    [SerializeField] private Sprite adultRedAntenna_;
    [SerializeField] private Sprite adultRedSpots_;
    [SerializeField] private Sprite adultRedTail_;

    [SerializeField] private Sprite adultGreenAntennaWhiskers_;
    [SerializeField] private Sprite adultGreenAntennaWhiskersSuccess_;
    [SerializeField] private Sprite adultGreenAntennaWhiskersFailure_;
    [SerializeField] private Sprite adultGreenAntennaMohawk_;
    [SerializeField] private Sprite adultGreenAntennaMohawkSuccess_;
    [SerializeField] private Sprite adultGreenAntennaMohawkFailure_;
    [SerializeField] private Sprite adultGreenAntennaStripes_;
    [SerializeField] private Sprite adultGreenAntennaStripesSuccess_;
    [SerializeField] private Sprite adultGreenAntennaStripesFailure_;
    [SerializeField] private Sprite adultGreenSpotsWhiskers_;
    [SerializeField] private Sprite adultGreenSpotsWhiskersSuccess_;
    [SerializeField] private Sprite adultGreenSpotsWhiskersFailure_;
    [SerializeField] private Sprite adultGreenSpotsMohawk_;
    [SerializeField] private Sprite adultGreenSpotsMohawkSuccess_;
    [SerializeField] private Sprite adultGreenSpotsMohawkFailure_;
    [SerializeField] private Sprite adultGreenSpotsStripes_;
    [SerializeField] private Sprite adultGreenSpotsStripesSuccess_;
    [SerializeField] private Sprite adultGreenSpotsStripesFailure_;
    [SerializeField] private Sprite adultGreenTailWhiskers_;
    [SerializeField] private Sprite adultGreenTailWhiskersSuccess_;
    [SerializeField] private Sprite adultGreenTailWhiskersFailure_;
    [SerializeField] private Sprite adultGreenTailMohawk_;
    [SerializeField] private Sprite adultGreenTailMohawkSuccess_;
    [SerializeField] private Sprite adultGreenTailMohawkFailure_;
    [SerializeField] private Sprite adultGreenTailStripes_;
    [SerializeField] private Sprite adultGreenTailStripesSuccess_;
    [SerializeField] private Sprite adultGreenTailStripesFailure_;

    [SerializeField] private Sprite adultBlueAntennaWhiskers_;
    [SerializeField] private Sprite adultBlueAntennaWhiskersSuccess_;
    [SerializeField] private Sprite adultBlueAntennaWhiskersFailure_;
    [SerializeField] private Sprite adultBlueAntennaMohawk_;
    [SerializeField] private Sprite adultBlueAntennaMohawkSuccess_;
    [SerializeField] private Sprite adultBlueAntennaMohawkFailure_;
    [SerializeField] private Sprite adultBlueAntennaStripes_;
    [SerializeField] private Sprite adultBlueAntennaStripesSuccess_;
    [SerializeField] private Sprite adultBlueAntennaStripesFailure_;
    [SerializeField] private Sprite adultBlueSpotsWhiskers_;
    [SerializeField] private Sprite adultBlueSpotsWhiskersSuccess_;
    [SerializeField] private Sprite adultBlueSpotsWhiskersFailure_;
    [SerializeField] private Sprite adultBlueSpotsMohawk_;
    [SerializeField] private Sprite adultBlueSpotsMohawkSuccess_;
    [SerializeField] private Sprite adultBlueSpotsMohawkFailure_;
    [SerializeField] private Sprite adultBlueSpotsStripes_;
    [SerializeField] private Sprite adultBlueSpotsStripesSuccess_;
    [SerializeField] private Sprite adultBlueSpotsStripesFailure_;
    [SerializeField] private Sprite adultBlueTailWhiskers_;
    [SerializeField] private Sprite adultBlueTailWhiskersSuccess_;
    [SerializeField] private Sprite adultBlueTailWhiskersFailure_;
    [SerializeField] private Sprite adultBlueTailMohawk_;
    [SerializeField] private Sprite adultBlueTailMohawkSuccess_;
    [SerializeField] private Sprite adultBlueTailMohawkFailure_;
    [SerializeField] private Sprite adultBlueTailStripes_;
    [SerializeField] private Sprite adultBlueTailStripesSuccess_;
    [SerializeField] private Sprite adultBlueTailStripesFailure_;

    [SerializeField] private Sprite adultRedAntennaWhiskers_;
    [SerializeField] private Sprite adultRedAntennaWhiskersSuccess_;
    [SerializeField] private Sprite adultRedAntennaWhiskersFailure_;
    [SerializeField] private Sprite adultRedAntennaMohawk_;
    [SerializeField] private Sprite adultRedAntennaMohawkSuccess_;
    [SerializeField] private Sprite adultRedAntennaMohawkFailure_;
    [SerializeField] private Sprite adultRedAntennaStripes_;
    [SerializeField] private Sprite adultRedAntennaStripesSuccess_;
    [SerializeField] private Sprite adultRedAntennaStripesFailure_;
    [SerializeField] private Sprite adultRedSpotsWhiskers_;
    [SerializeField] private Sprite adultRedSpotsWhiskersSuccess_;
    [SerializeField] private Sprite adultRedSpotsWhiskersFailure_;
    [SerializeField] private Sprite adultRedSpotsMohawk_;
    [SerializeField] private Sprite adultRedSpotsMohawkSuccess_;
    [SerializeField] private Sprite adultRedSpotsMohawkFailure_;
    [SerializeField] private Sprite adultRedSpotsStripes_;
    [SerializeField] private Sprite adultRedSpotsStripesSuccess_;
    [SerializeField] private Sprite adultRedSpotsStripesFailure_;
    [SerializeField] private Sprite adultRedTailWhiskers_;
    [SerializeField] private Sprite adultRedTailWhiskersSuccess_;
    [SerializeField] private Sprite adultRedTailWhiskersFailure_;
    [SerializeField] private Sprite adultRedTailMohawk_;
    [SerializeField] private Sprite adultRedTailMohawkSuccess_;
    [SerializeField] private Sprite adultRedTailMohawkFailure_;
    [SerializeField] private Sprite adultRedTailStripes_;
    [SerializeField] private Sprite adultRedTailStripesSuccess_;
    [SerializeField] private Sprite adultRedTailStripesFailure_;
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

    private const string kCommand = "-momo";

    private enum Status
    {
        Neutral,
        Success,
        Failure,
        Transforming
    }

    private string currentRfid_;
    private bool inTesting_ = false;

    private GoTweenFlow dismissingFlow_;

    private void Start()
    {
        ShareManager sm = (ShareManager)gameManager_;
        if (sm != null)
        {
            starterBackground_.transform.SetParent(sm.OverlayParent);
        }
    }

    public override void Hide()
    {
        HideAllScenes();
    }

    public override void ReceivedAction(string action)
    {
        if (ArgumentHelper.ContainsCommand(GameManager.RFID_COMMAND, action))
        {
            List<string> args = ArgumentHelper.ArgumentsFromCommand(GameManager.RFID_COMMAND, action);
            if (args.Count > 1)
            {
                currentRfid_ = args[1];

                if (LevelOfMomo(currentRfid_) == 0)
                {
                    HandleStarterPicker();
                } else if (LevelOfMomo(currentRfid_) == 1 && !inTesting_)
                {
                    HandleStarterPicker();
                }
                if (inTesting_ && LevelOfMomo(currentRfid_) > 0)
                {
                    HandleTesting();
                }
            }
        }

        if (ArgumentHelper.ContainsCommand(kCommand, action))
        {
            List<string> args = ArgumentHelper.ArgumentsFromCommand(kCommand, action);
            if (args.Count > 0)
            {
                switch (args[0])
                {
                    case "starter":
                        HandleStarterPicker();
                        break;
                    case "starter-select":
                        if (args.Count > 1) HandleStarterPickerSelection(args[1]);
                        break;
                    case "customize":
                        HandleCustomize();
                        break;
                    case "customize-select":
                        if (args.Count > 1) HandleCustomizeSelection(args[1]);
                        break;
                    case "testing":
                        HandleTesting();
                        break;
                    case "success":
                        HandleSuccess();
                        break;
                    case "failure":
                        HandleFailure();
                        break;
                    case "hide":
                        Hide();
                        break;
                    case "set-in-testing":
                        inTesting_ = true;
                        break;
                }
            }
        }
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
        neutralBackground_.gameObject.SetActive(false);
        successBackground_.gameObject.SetActive(false);
        failureBackground_.gameObject.SetActive(false);
    }

    private void HandleStarterPicker()
    {
        HideAllScenes();
        starterBackground_.gameObject.SetActive(true);
        starterSelected_.gameObject.SetActive(false);

        AudioPlayer.PlayAudio("audio/sfx/new-option");
    }

    private void HandleStarterPickerSelection(string choice)
    {
        if (currentRfid_ == null || currentRfid_.Length == 0) return;

        HideAllScenes();
        starterBackground_.gameObject.SetActive(true);
        starterSelected_.gameObject.SetActive(true);

        GameStorage gs = gameManager_.GameStorageForUserId(currentRfid_);

        string selection = null;
        switch(choice)
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

        starterMomo_.transform.localPosition = new Vector3(0, 1200, 0);
        Go.to(starterMomo_.transform, 1f, new GoTweenConfig().localPosition(Vector3.zero).setEaseType(GoEaseType.SineIn).onComplete(t =>
        {
            AudioPlayer.PlayAudio("audio/sfx/momo-grunt");
        }));

        GoTweenFlow flow = new GoTweenFlow();
        flow.insert(1f, new GoTween(starterMomo_.transform, 0.25f, new GoTweenConfig().scale(1.1f)));
        flow.insert(1.25f, new GoTween(starterMomo_.transform, 0.25f, new GoTweenConfig().scale(1f)));
        flow.play();

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
        })));
        dismissingFlow_.play();
    }

    private void HandleCustomize()
    {
        if (currentRfid_ == null || currentRfid_.Length == 0) return;

        HideAllScenes();

        AudioPlayer.PlayAudio("audio/sfx/new-option");

        int level = LevelOfMomo(currentRfid_);
        GameStorage gs = gameManager_.GameStorageForUserId(currentRfid_);
        string teenCustomization = gs.GetValue<string>(GameStorage.Key.MomoTeenCustomization);
        string adultCustomization = gs.GetValue<string>(GameStorage.Key.MomoAdultCustomization);

        List<Sprite> sprites = SpritesEvolveForRfid(currentRfid_);

        if (level == 2)
        {
            customizeAdultMomo_.sprite = sprites[2];
            customizeAdultMomo_.SetNativeSize();
            customizeAdultBackground_.gameObject.SetActive(true);
        }
        if (level == 1)
        {
            customizeTeenMomo_.sprite = sprites[2];
            customizeTeenMomo_.SetNativeSize();
            customizeTeenBackground_.gameObject.SetActive(true);
        }
    }

    private void HandleCustomizeSelection(string choice)
    {
        if (currentRfid_ == null || currentRfid_.Length == 0) return;

        bool teenActive = customizeTeenBackground_.gameObject.activeSelf;
        bool adultActive = customizeAdultBackground_.gameObject.activeSelf;

        GameStorage gs = gameManager_.GameStorageForUserId(currentRfid_);

        List<Sprite> sprites = SpritesEvolveForRfid(currentRfid_);

        if (teenActive)
        {
            customizeTeenMomo_.sprite = sprites[2];
            customizeTeenMomo_.SetNativeSize();
        }
        else
        {
            customizeAdultMomo_.sprite = sprites[2];
            customizeAdultMomo_.SetNativeSize();
        }

        if (teenActive)
        {
            string selection = null;
            switch(choice)
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
        } else if (adultActive)
        {
            string selection = null;
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
        })));
        dismissingFlow_.play();
    }

    private void HandleTesting()
    {
        if (currentRfid_ == null || currentRfid_.Length == 0) return;

        HideAllScenes();

        AudioPlayer.PlayAudio("audio/sfx/momo-grunt");

        neutralBackground_.gameObject.SetActive(true);
        SetSprite(neutralMomo_, currentRfid_, Status.Neutral);
    }

    private void HandleSuccess()
    {
        if (currentRfid_ == null || currentRfid_.Length == 0) return;

        HideAllScenes();

        AudioPlayer.PlayAudio("audio/sfx/momo-grunt");

        successBackground_.gameObject.SetActive(true);
        SetSprite(successMomo_, currentRfid_, Status.Success);
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
            HandleTesting();
        } else if (successBackground_.gameObject.activeSelf)
        {
            HandleSuccess();
        } else if (failureBackground_.gameObject.activeSelf)
        {
            HandleFailure();
        }
    }

    public override void Reset()
    {

    }

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

    private List<Sprite> SpritesEvolveForRfid(string rfid)
    {
        GameStorage gs = gameManager_.GameStorageForUserId(rfid);
        string starter = gs.GetValue<string>(GameStorage.Key.MomoStarter);
        string teenCustomization = gs.GetValue<string>(GameStorage.Key.MomoTeenCustomization);

        if (LevelOfMomo(rfid) == 1)
        {
            switch (starter)
            {
                case kGreen:
                    return new List<Sprite>()
                    {
                        childGreen_,
                        childGreenEvolving_,
                        teenGreen_
                    };
                case kBlue:
                    return new List<Sprite>()
                    {
                        childBlue_,
                        childBlueEvolving_,
                        teenBlue_
                    };
                case kRed:
                    return new List<Sprite>()
                    {
                        childRed_,
                        childRedEvolving_,
                        teenRed_
                    };
            }
        } else if (LevelOfMomo(rfid) == 2)
        {
            switch (starter)
            {
                case kGreen:
                    switch(teenCustomization)
                    {
                        case kAntenna:
                            return new List<Sprite>()
                            {
                                teenGreenAntenna_,
                                teenGreenAntennaEvolving_,
                                adultGreenAntenna_
                            };
                        case kSpots:
                            return new List<Sprite>()
                            {
                                teenGreenSpots_,
                                teenGreenSpotsEvolving_,
                                adultGreenSpots_
                            };
                        case kTail:
                            return new List<Sprite>()
                            {
                                teenGreenTail_,
                                teenGreenTailEvolving_,
                                adultGreenTail_
                            };
                    }
                    break;
                case kBlue:
                    switch (teenCustomization)
                    {
                        case kAntenna:
                            return new List<Sprite>()
                            {
                                teenBlueAntenna_,
                                teenBlueAntennaEvolving_,
                                adultBlueAntenna_
                            };
                        case kSpots:
                            return new List<Sprite>()
                            {
                                teenBlueSpots_,
                                teenBlueSpotsEvolving_,
                                adultBlueSpots_
                            };
                        case kTail:
                            return new List<Sprite>()
                            {
                                teenBlueTail_,
                                teenBlueTailEvolving_,
                                adultBlueTail_
                            };
                    }
                    break;
                case kRed:
                    switch (teenCustomization)
                    {
                        case kAntenna:
                            return new List<Sprite>()
                            {
                                teenRedAntenna_,
                                teenRedAntennaEvolving_,
                                adultRedAntenna_
                            };
                        case kSpots:
                            return new List<Sprite>()
                            {
                                teenRedSpots_,
                                teenRedSpotsEvolving_,
                                adultRedSpots_
                            };
                        case kTail:
                            return new List<Sprite>()
                            {
                                teenRedTail_,
                                teenRedTailEvolving_,
                                adultRedTail_
                            };
                    }
                    break;
            }
        }

        return null;
    }

    private void SetSprite(Image image, string rfid, Status status)
    {
        image.sprite = MomoForRfid(rfid, status);
        image.SetNativeSize();
    }

    private Sprite MomoForRfid(string rfid, Status status)
    {
        GameStorage gs = gameManager_.GameStorageForUserId(rfid);
        string starter = gs.GetValue<string>(GameStorage.Key.MomoStarter);
        string teenCustomization = gs.GetValue<string>(GameStorage.Key.MomoTeenCustomization);
        string adultCustomization = gs.GetValue<string>(GameStorage.Key.MomoAdultCustomization);

        if (adultCustomization != null && adultCustomization.Length > 0)
        {
            switch(starter)
            {
                case kGreen:
                    switch (teenCustomization)
                    {
                        case kAntenna:
                            switch(adultCustomization)
                            {
                                case kWhiskers:
                                    switch(status)
                                    {
                                        case Status.Neutral: return adultGreenAntennaWhiskers_;
                                        case Status.Success: return adultGreenAntennaWhiskersSuccess_;
                                        case Status.Failure: return adultGreenAntennaWhiskersFailure_;
                                        case Status.Transforming: return null;
                                    }
                                    break;
                                case kMohawk:
                                    switch (status)
                                    {
                                        case Status.Neutral: return adultGreenAntennaMohawk_;
                                        case Status.Success: return adultGreenAntennaMohawkSuccess_;
                                        case Status.Failure: return adultGreenAntennaMohawkFailure_;
                                        case Status.Transforming: return null;
                                    }
                                    break;
                                case kStripes:
                                    switch (status)
                                    {
                                        case Status.Neutral: return adultGreenAntennaStripes_;
                                        case Status.Success: return adultGreenAntennaStripesSuccess_;
                                        case Status.Failure: return adultGreenAntennaStripesFailure_;
                                        case Status.Transforming: return null;
                                    }
                                    break;
                            }
                            break;
                        case kSpots:
                            switch (adultCustomization)
                            {
                                case kWhiskers:
                                    switch (status)
                                    {
                                        case Status.Neutral: return adultGreenSpotsWhiskers_;
                                        case Status.Success: return adultGreenSpotsWhiskersSuccess_;
                                        case Status.Failure: return adultGreenSpotsWhiskersFailure_;
                                        case Status.Transforming: return null;
                                    }
                                    break;
                                case kMohawk:
                                    switch (status)
                                    {
                                        case Status.Neutral: return adultGreenSpotsMohawk_;
                                        case Status.Success: return adultGreenSpotsMohawkSuccess_;
                                        case Status.Failure: return adultGreenSpotsMohawkFailure_;
                                        case Status.Transforming: return null;
                                    }
                                    break;
                                case kStripes:
                                    switch (status)
                                    {
                                        case Status.Neutral: return adultGreenSpotsStripes_;
                                        case Status.Success: return adultGreenSpotsStripesSuccess_;
                                        case Status.Failure: return adultGreenSpotsStripesFailure_;
                                        case Status.Transforming: return null;
                                    }
                                    break;
                            }
                            break;
                        case kTail:
                            switch (adultCustomization)
                            {
                                case kWhiskers:
                                    switch (status)
                                    {
                                        case Status.Neutral: return adultGreenTailWhiskers_;
                                        case Status.Success: return adultGreenTailWhiskersSuccess_;
                                        case Status.Failure: return adultGreenTailWhiskersFailure_;
                                        case Status.Transforming: return null;
                                    }
                                    break;
                                case kMohawk:
                                    switch (status)
                                    {
                                        case Status.Neutral: return adultGreenTailMohawk_;
                                        case Status.Success: return adultGreenTailMohawkSuccess_;
                                        case Status.Failure: return adultGreenTailMohawkFailure_;
                                        case Status.Transforming: return null;
                                    }
                                    break;
                                case kStripes:
                                    switch (status)
                                    {
                                        case Status.Neutral: return adultGreenTailStripes_;
                                        case Status.Success: return adultGreenTailStripesSuccess_;
                                        case Status.Failure: return adultGreenTailStripesFailure_;
                                        case Status.Transforming: return null;
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
                case kBlue:
                    switch (teenCustomization)
                    {
                        case kAntenna:
                            switch (adultCustomization)
                            {
                                case kWhiskers:
                                    switch (status)
                                    {
                                        case Status.Neutral: return adultBlueAntennaWhiskers_;
                                        case Status.Success: return adultBlueAntennaWhiskersSuccess_;
                                        case Status.Failure: return adultBlueAntennaWhiskersFailure_;
                                        case Status.Transforming: return null;
                                    }
                                    break;
                                case kMohawk:
                                    switch (status)
                                    {
                                        case Status.Neutral: return adultBlueAntennaMohawk_;
                                        case Status.Success: return adultBlueAntennaMohawkSuccess_;
                                        case Status.Failure: return adultBlueAntennaMohawkFailure_;
                                        case Status.Transforming: return null;
                                    }
                                    break;
                                case kStripes:
                                    switch (status)
                                    {
                                        case Status.Neutral: return adultBlueAntennaStripes_;
                                        case Status.Success: return adultBlueAntennaStripesSuccess_;
                                        case Status.Failure: return adultBlueAntennaStripesFailure_;
                                        case Status.Transforming: return null;
                                    }
                                    break;
                            }
                            break;
                        case kSpots:
                            switch (adultCustomization)
                            {
                                case kWhiskers:
                                    switch (status)
                                    {
                                        case Status.Neutral: return adultBlueSpotsWhiskers_;
                                        case Status.Success: return adultBlueSpotsWhiskersSuccess_;
                                        case Status.Failure: return adultBlueSpotsWhiskersFailure_;
                                        case Status.Transforming: return null;
                                    }
                                    break;
                                case kMohawk:
                                    switch (status)
                                    {
                                        case Status.Neutral: return adultBlueSpotsMohawk_;
                                        case Status.Success: return adultBlueSpotsMohawkSuccess_;
                                        case Status.Failure: return adultBlueSpotsMohawkFailure_;
                                        case Status.Transforming: return null;
                                    }
                                    break;
                                case kStripes:
                                    switch (status)
                                    {
                                        case Status.Neutral: return adultBlueSpotsStripes_;
                                        case Status.Success: return adultBlueSpotsStripesSuccess_;
                                        case Status.Failure: return adultBlueSpotsStripesFailure_;
                                        case Status.Transforming: return null;
                                    }
                                    break;
                            }
                            break;
                        case kTail:
                            switch (adultCustomization)
                            {
                                case kWhiskers:
                                    switch (status)
                                    {
                                        case Status.Neutral: return adultBlueTailWhiskers_;
                                        case Status.Success: return adultBlueTailWhiskersSuccess_;
                                        case Status.Failure: return adultBlueTailWhiskersFailure_;
                                        case Status.Transforming: return null;
                                    }
                                    break;
                                case kMohawk:
                                    switch (status)
                                    {
                                        case Status.Neutral: return adultBlueTailMohawk_;
                                        case Status.Success: return adultBlueTailMohawkSuccess_;
                                        case Status.Failure: return adultBlueTailMohawkFailure_;
                                        case Status.Transforming: return null;
                                    }
                                    break;
                                case kStripes:
                                    switch (status)
                                    {
                                        case Status.Neutral: return adultBlueTailStripes_;
                                        case Status.Success: return adultBlueTailStripesSuccess_;
                                        case Status.Failure: return adultBlueTailStripesFailure_;
                                        case Status.Transforming: return null;
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
                case kRed:
                    switch (teenCustomization)
                    {
                        case kAntenna:
                            switch (adultCustomization)
                            {
                                case kWhiskers:
                                    switch (status)
                                    {
                                        case Status.Neutral: return adultRedAntennaWhiskers_;
                                        case Status.Success: return adultRedAntennaWhiskersSuccess_;
                                        case Status.Failure: return adultRedAntennaWhiskersFailure_;
                                        case Status.Transforming: return null;
                                    }
                                    break;
                                case kMohawk:
                                    switch (status)
                                    {
                                        case Status.Neutral: return adultRedAntennaMohawk_;
                                        case Status.Success: return adultRedAntennaMohawkSuccess_;
                                        case Status.Failure: return adultRedAntennaMohawkFailure_;
                                        case Status.Transforming: return null;
                                    }
                                    break;
                                case kStripes:
                                    switch (status)
                                    {
                                        case Status.Neutral: return adultRedAntennaStripes_;
                                        case Status.Success: return adultRedAntennaStripesSuccess_;
                                        case Status.Failure: return adultRedAntennaStripesFailure_;
                                        case Status.Transforming: return null;
                                    }
                                    break;
                            }
                            break;
                        case kSpots:
                            switch (adultCustomization)
                            {
                                case kWhiskers:
                                    switch (status)
                                    {
                                        case Status.Neutral: return adultRedSpotsWhiskers_;
                                        case Status.Success: return adultRedSpotsWhiskersSuccess_;
                                        case Status.Failure: return adultRedSpotsWhiskersFailure_;
                                        case Status.Transforming: return null;
                                    }
                                    break;
                                case kMohawk:
                                    switch (status)
                                    {
                                        case Status.Neutral: return adultRedSpotsMohawk_;
                                        case Status.Success: return adultRedSpotsMohawkSuccess_;
                                        case Status.Failure: return adultRedSpotsMohawkFailure_;
                                        case Status.Transforming: return null;
                                    }
                                    break;
                                case kStripes:
                                    switch (status)
                                    {
                                        case Status.Neutral: return adultRedSpotsStripes_;
                                        case Status.Success: return adultRedSpotsStripesSuccess_;
                                        case Status.Failure: return adultRedSpotsStripesFailure_;
                                        case Status.Transforming: return null;
                                    }
                                    break;
                            }
                            break;
                        case kTail:
                            switch (adultCustomization)
                            {
                                case kWhiskers:
                                    switch (status)
                                    {
                                        case Status.Neutral: return adultRedTailWhiskers_;
                                        case Status.Success: return adultRedTailWhiskersSuccess_;
                                        case Status.Failure: return adultRedTailWhiskersFailure_;
                                        case Status.Transforming: return null;
                                    }
                                    break;
                                case kMohawk:
                                    switch (status)
                                    {
                                        case Status.Neutral: return adultRedTailMohawk_;
                                        case Status.Success: return adultRedTailMohawkSuccess_;
                                        case Status.Failure: return adultRedTailMohawkFailure_;
                                        case Status.Transforming: return null;
                                    }
                                    break;
                                case kStripes:
                                    switch (status)
                                    {
                                        case Status.Neutral: return adultRedTailStripes_;
                                        case Status.Success: return adultRedTailStripesSuccess_;
                                        case Status.Failure: return adultRedTailStripesFailure_;
                                        case Status.Transforming: return null;
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
            }
        }
        if (teenCustomization != null && teenCustomization.Length > 0)
        {
            switch (starter)
            {
                case kGreen:
                    switch(teenCustomization)
                    {
                        case kAntenna:
                            switch(status)
                            {
                                case Status.Neutral: return teenGreenAntenna_;
                                case Status.Success: return teenGreenAntennaSuccess_;
                                case Status.Failure: return teenGreenAntennaFail_;
                                case Status.Transforming: return null;
                            }
                            break;
                        case kSpots:
                            switch (status)
                            {
                                case Status.Neutral: return teenGreenSpots_;
                                case Status.Success: return teenGreenSpotsSuccess_;
                                case Status.Failure: return teenGreenSpotsFail_;
                                case Status.Transforming: return null;
                            }
                            break;
                        case kTail:
                            switch (status)
                            {
                                case Status.Neutral: return teenGreenTail_;
                                case Status.Success: return teenGreenTailSuccess_;
                                case Status.Failure: return teenGreenTailFail_;
                                case Status.Transforming: return null;
                            }
                            break;
                    }
                    break;
                case kBlue:
                    switch (teenCustomization)
                    {
                        case kAntenna:
                            switch (status)
                            {
                                case Status.Neutral: return teenBlueAntenna_;
                                case Status.Success: return teenBlueAntennaSuccess_;
                                case Status.Failure: return teenBlueAntennaFail_;
                                case Status.Transforming: return null;
                            }
                            break;
                        case kSpots:
                            switch (status)
                            {
                                case Status.Neutral: return teenBlueSpots_;
                                case Status.Success: return teenBlueSpotsSuccess_;
                                case Status.Failure: return teenBlueSpotsFail_;
                                case Status.Transforming: return null;
                            }
                            break;
                        case kTail:
                            switch (status)
                            {
                                case Status.Neutral: return teenBlueTail_;
                                case Status.Success: return teenBlueTailSuccess_;
                                case Status.Failure: return teenBlueTailFail_;
                                case Status.Transforming: return null;
                            }
                            break;
                    }
                    break;
                case kRed:
                    switch (teenCustomization)
                    {
                        case kAntenna:
                            switch (status)
                            {
                                case Status.Neutral: return teenRedAntenna_;
                                case Status.Success: return teenRedAntennaSuccess_;
                                case Status.Failure: return teenRedAntennaFail_;
                                case Status.Transforming: return null;
                            }
                            break;
                        case kSpots:
                            switch (status)
                            {
                                case Status.Neutral: return teenRedSpots_;
                                case Status.Success: return teenRedSpotsSuccess_;
                                case Status.Failure: return teenRedSpotsFail_;
                                case Status.Transforming: return null;
                            }
                            break;
                        case kTail:
                            switch (status)
                            {
                                case Status.Neutral: return teenRedTail_;
                                case Status.Success: return teenRedTailSuccess_;
                                case Status.Failure: return teenRedTailFail_;
                                case Status.Transforming: return null;
                            }
                            break;
                    }
                    break;
            }
        }
        switch (starter)
        {
            case kGreen:
                switch (status)
                {
                    case Status.Neutral: return childGreen_;
                    case Status.Success: return childGreenSuccess_;
                    case Status.Failure: return childGreenFail_;
                    case Status.Transforming: return childGreenEvolving_;
                }
                break;
            case kBlue:
                switch (status)
                {
                    case Status.Neutral: return childBlue_;
                    case Status.Success: return childBlueSuccess_;
                    case Status.Failure: return childBlueFail_;
                    case Status.Transforming: return childBlueEvolving_;
                }
                break;
            case kRed:
                switch (status)
                {
                    case Status.Neutral: return childRed_;
                    case Status.Success: return childRedSuccess_;
                    case Status.Failure: return childRedFail_;
                    case Status.Transforming: return childRedEvolving_;
                }
                break;
        }

        return null;
    }
}
