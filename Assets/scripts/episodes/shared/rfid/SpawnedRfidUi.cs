using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SpawnedRfidUi : SpawnedObject
{
    [SerializeField] private Transform overallParent_;

    [SerializeField] private Image buildingBlocksBackground_;
    [SerializeField] private Image buildingSelectedBlocksBackground_;
    [SerializeField] private Image magicPrinterBackground_;
    [SerializeField] private Image magicSelectedPrinterBackground_;
    [SerializeField] private Image challengeBackground_;
    [SerializeField] private Image challengeSelectedBackground_;

    [SerializeField] private Image buildingBlocksNo_;
    [SerializeField] private Image buildingBlocksQuarter_;
    [SerializeField] private Image buildingBlocksHalf_;
    [SerializeField] private Image buildingBlocksFull_;

    [SerializeField] private Text magicPrinterStatus_;
    [SerializeField] private Transform magicPrinterHintHolders_;
    [SerializeField] private RfidMagicPrinterHint magicPrinterHintPrefab_;
    private List<RfidMagicPrinterHint> magicPrinterHints_ = new List<RfidMagicPrinterHint>();

    [SerializeField] private Text challengeStatus_;
    [SerializeField] private Image challengeRequirementHolder_;
    [SerializeField] private Transform challengeStarsHolder_;
    [SerializeField] private Image starPrefab_;
    private List<Image> challengeStars_ = new List<Image>();

    public static string SelectedRfid = "";
    public static bool OutOfBlocks = false;

    private enum State
    {
        None,
        BuildingBlocks,
        MagicPrinter,
        Challenge
    }

    private string CurrentChallenge
    {
        get
        {
            GameStorage gs = gameManager_.GameStorageForRfid(SelectedRfid);
            GameStorage.User userData = gs.GetValue<GameStorage.User>(GameStorage.Key.RfidUserData);
            if (userData == null) userData = new GameStorage.User();

            if (userData.CurrentChallenge == null ||
                userData.CurrentChallenge.Length == 0 &&
                userData.CompletedChallenges.Count == 0)
            {
                userData.CurrentChallenge = gameManager_.ChallengeData.FirstChallenge;
                gs.Add<GameStorage.User>(GameStorage.Key.RfidUserData, userData);
            }
            return userData.CurrentChallenge;
        }
    }

    public override void ReceivedAction(string action)
    {
        if (ArgumentHelper.ContainsCommand("-rfid-ui", action))
        {
            List<string> args = ArgumentHelper.ArgumentsFromCommand("-rfid-ui", action);

            if (args.Count > 0)
            {
                switch(args[0])
                {
                    case "select-mode":
                        if (args.Count > 1) StateSelected(args[1]);
                        break;
                    case "scanned":
                        //say hmm if hint, wait for Felix to press button
                        //blocks is automatic
                        //challenge say nothing, show screen for current challenge. wait for Felix to say Green light
                        //or say can't do it
                        if (args.Count > 1) RfidScanned(args[1]);
                        break;
                    case "received-hint":
                        HandleReceivedHint();
                        break;
                    case "received-blocks":
                        HandleReceiveBlocks();
                        break;
                    case "give-challenge-reward":
                        HandleGiveChallenge();
                        HandleCompletedChallenge();
                        break;
                    case "out-of-blocks":
                        HandleOutOfBlocks();
                        break;
                    case "announce-next-challenge":
                        HandleAnnounceNextChallenge();
                        break;
                    case "reset":
                        HandleReset();
                        break;
                }
            }
        }
    }

    private void StateSelected(string state)
    {
        if (SelectedRfid == null || SelectedRfid.Length == 0)
        {
            return;
        }

        GameStorage gs = gameManager_.GameStorageForRfid(SelectedRfid);
        GameStorage.User userData = gs.GetValue<GameStorage.User>(GameStorage.Key.RfidUserData);
        if (userData == null) userData = new GameStorage.User();

        switch (state)
        {
            case "BuildingBlocks":
                HandleSelectedBuildingBlocks(userData, gameManager_.ChallengeData);
                break;
            case "MagicPrinter":
                HandleSelectedMagicPrinter(userData, gameManager_.ChallengeData);
                break;
            case "Challenge":
                HandleSelectedChallenge(userData, gameManager_.ChallengeData);
                break;
        }
    }

    private void RfidScanned(string rfid)
    {
        SelectedRfid = rfid;
    }

    private void HandleSelectedBuildingBlocks(GameStorage.User userData, ChallengeData rfidData)
    {
        ResetAllScreens();
        buildingSelectedBlocksBackground_.gameObject.SetActive(true);

        if (OutOfBlocks)
        {
            gameManager_.SendNewAction("-character talk out-of-blocks");
            buildingBlocksNo_.gameObject.SetActive(true);

            gameManager_.SendNewAction(GameManager.BLOCKS_RED);
        }
        else if (userData.BlocksGiven.ContainsKey(CurrentChallenge))
        {
            gameManager_.SendNewAction("-character talk blocks-no-1 blocks-no-2");
            if (userData.BlocksGiven[CurrentChallenge])
            {
                buildingBlocksNo_.gameObject.SetActive(true);
            }

            gameManager_.SendNewAction(GameManager.BLOCKS_RED);
        } else
        {
            gameManager_.SendNewAction(GameManager.BLOCKS_GREEN);

            ChallengeData.Challenge challengeData = rfidData.Challenges.Find(c => string.Equals(c.Name, CurrentChallenge));
            switch(challengeData.ExtraBlocksAllowed)
            {
                case "full":
                    gameManager_.SendNewAction("-character talk blocks-full");
                    buildingBlocksFull_.gameObject.SetActive(true);
                    break;
                case "half":
                    gameManager_.SendNewAction("-character talk blocks-half");
                    buildingBlocksHalf_.gameObject.SetActive(true);
                    break;
                case "quarter":
                    gameManager_.SendNewAction("-character talk blocks-quarter");
                    buildingBlocksQuarter_.gameObject.SetActive(true);
                    break;
            }
        }
    }

    private void HandleSelectedMagicPrinter(GameStorage.User userData, ChallengeData rfidData)
    {
        ResetAllScreens();
        magicSelectedPrinterBackground_.gameObject.SetActive(true);
        magicPrinterHintHolders_.gameObject.SetActive(true);

        gameManager_.SendNewAction("-character talk hm-1 hm-2 hm-3 hm-4 hm-5 hm-6 hm-7 hm-8 hm-9 hm-10");
        for (int i = 0; i < rfidData.TotalHints; i++)
        {
            RfidMagicPrinterHint hint = GameObject.Instantiate<RfidMagicPrinterHint>(magicPrinterHintPrefab_);
            hint.transform.SetParent(magicPrinterHintHolders_);
            hint.transform.localScale = Vector3.one;
            if (i < userData.HintsGiven)
            {
                hint.SetOn();
            } else
            {
                hint.SetOff();
            }
            magicPrinterHints_.Add(hint);
        }

        magicPrinterStatus_.text = (userData.HintsGiven + 1).ToString() + "/" + rfidData.TotalHints.ToString();
    }

    private void HandleReceivedHint()
    {
        if (SelectedRfid == null || SelectedRfid.Length == 0) return;

        //gameManager_.SendNewAction(GameManager.HINTS_GREEN);

        GameStorage gs = gameManager_.GameStorageForRfid(SelectedRfid);
        GameStorage.User userData = gs.GetValue<GameStorage.User>(GameStorage.Key.RfidUserData);
        if (userData == null) userData = new GameStorage.User();

        userData.HintsGiven++;
        gs.Add<GameStorage.User>(GameStorage.Key.RfidUserData, userData);
    }

    private void HandleCompletedChallenge()
    {
        GameStorage gs = gameManager_.GameStorageForRfid(SelectedRfid);
        GameStorage.User userData = gs.GetValue<GameStorage.User>(GameStorage.Key.RfidUserData);
        if (userData == null) userData = new GameStorage.User();

        HandleSelectedChallenge(userData, gameManager_.ChallengeData);

        Image star = GameObject.Instantiate<Image>(starPrefab_);
        star.transform.SetParent(challengeStarsHolder_);
        challengeStars_.Add(star);

        if (!userData.CompletedChallenges.Contains(CurrentChallenge))
        {
            userData.CompletedChallenges.Add(CurrentChallenge);
        }

        ChallengeData rfidData = gameManager_.ChallengeData;
        for (int i = 0; i < rfidData.Challenges.Count; i++)
        {
            if (string.Equals(CurrentChallenge, rfidData.Challenges[i].Name))
            {
                if (i + 1 < rfidData.Challenges.Count)
                {
                    userData.CurrentChallenge = rfidData.Challenges[i + 1].Name;
                    break;
                }
            }
        }

        gs.Add<GameStorage.User>(GameStorage.Key.RfidUserData, userData);
    }

    private void HandleGiveChallenge()
    {
        if (CurrentChallenge != null && CurrentChallenge.Length > 0)
        {
            foreach(ChallengeData.Challenge c in gameManager_.ChallengeData.Challenges)
            {
                if (string.Equals(c.Name, CurrentChallenge))
                {
                    gameManager_.SendNewAction(c.RewardCommand);
                    break;
                }
            }
        }
    }

    private void HandleReceiveBlocks()
    {
        GameStorage gs = gameManager_.GameStorageForRfid(SelectedRfid);
        GameStorage.User userData = gs.GetValue<GameStorage.User>(GameStorage.Key.RfidUserData);
        if (userData == null) userData = new GameStorage.User();

        if (CurrentChallenge == null || CurrentChallenge.Length == 0) return;

        userData.BlocksGiven[CurrentChallenge] = true;

        gs.Add<GameStorage.User>(GameStorage.Key.RfidUserData, userData);
    }

    private void HandleOutOfBlocks()
    {
        OutOfBlocks = true;
    }

    private void HandleReset()
    {
        ResetAllScreens();
    }

    private void HandleAnnounceNextChallenge()
    {
        for (int i = 0; i < gameManager_.ChallengeData.Challenges.Count; i++)
        {
            ChallengeData.Challenge c = gameManager_.ChallengeData.Challenges[i];
            if (string.Equals(c.Name, CurrentChallenge))
            {
                if (i+1 < gameManager_.ChallengeData.Challenges.Count)
                {
                    gameManager_.SendNewAction(gameManager_.ChallengeData.Challenges[i].NextChallengeCommand);
                }
            }
        }
    }

    private void HandleSelectedChallenge(GameStorage.User userData, ChallengeData rfidData)
    {
        ResetAllScreens();
        challengeSelectedBackground_.gameObject.SetActive(true);
        challengeStarsHolder_.gameObject.SetActive(true);
        challengeRequirementHolder_.gameObject.SetActive(true);

        gameManager_.SendNewAction(GameManager.CHALLENGE_GREEN);

        gameManager_.SendNewAction("-character talk ready-to-test-1 ready-to-test-2 ready-to-test-3 ready-to-test-4 ready-to-test-5");

        foreach(string achieved in userData.CompletedChallenges)
        {
            Image star = GameObject.Instantiate<Image>(starPrefab_);
            star.transform.SetParent(challengeStarsHolder_);
            challengeStars_.Add(star);
        }
        Sprite s = null;
        foreach(ChallengeData.Challenge c in rfidData.Challenges)
        {
            if (string.Equals(CurrentChallenge, c.Name))
            {
                s = c.Sprite;
                break;
            }
        }
        challengeRequirementHolder_.sprite = s;

        challengeStatus_.text = (userData.CompletedChallenges.Count + 1) + "/" + (rfidData.Challenges.Count + 1).ToString();
    }

    private void ResetAllScreens()
    {
        buildingBlocksBackground_.gameObject.SetActive(true);
        magicPrinterBackground_.gameObject.SetActive(true);
        challengeBackground_.gameObject.SetActive(true);

        buildingSelectedBlocksBackground_.gameObject.SetActive(false);
        magicSelectedPrinterBackground_.gameObject.SetActive(false);
        challengeSelectedBackground_.gameObject.SetActive(false);

        magicPrinterStatus_.text = "";
        challengeStatus_.text = "";

        buildingBlocksNo_.gameObject.SetActive(false);
        buildingBlocksQuarter_.gameObject.SetActive(false);
        buildingBlocksHalf_.gameObject.SetActive(false);
        buildingBlocksFull_.gameObject.SetActive(false);

        for (int i = 0; i < magicPrinterHints_.Count; i++)
        {
            RfidMagicPrinterHint p = magicPrinterHints_[i];
            Destroy(p.gameObject);
        }
        magicPrinterHints_ = new List<RfidMagicPrinterHint>();

        challengeRequirementHolder_.gameObject.SetActive(false);
        for (int i = 0; i < challengeStars_.Count; i++)
        {
            Image image = challengeStars_[i];
            Destroy(image.gameObject);
        }
        challengeStars_ = new List<Image>();
    }

    public override void Reset()
    {

    }

    public override void Hide()
    {
        overallParent_.gameObject.SetActive(true);
    }
}
