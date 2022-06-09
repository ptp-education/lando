using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Lando.SmartObjects;

public class CommandDispatch
{
    private GameManager gameManager_;
    private Dictionary<string, string> nfcAtStation_ = new Dictionary<string, string>();

    public enum ValidatorResponse
    {
        Success,
        Failure,
        BeforeTest,
        ScanWristband
    }

    public void Init(GameManager gameManager)
    {
        gameManager_ = gameManager;
    }

    public void NewNfcScan(string id, SmartObjectType stationType)
    {
        nfcAtStation_[stationType.ToString()] = id;

        switch(stationType)
        {
            case SmartObjectType.ResourceStation:
                OnResourceStationScan(id, stationType.ToString());
                break;
            case SmartObjectType.TestingStation:
                OnTestStationScan(id, stationType.ToString());
                break;
            case SmartObjectType.HintStation:
                OnHintStationScan(id, stationType.ToString());
                break;
        }
    }

    public void OnRefresh(string id, string station)
    {
        SmartObjectType parsedResponse = (SmartObjectType)Enum.Parse(typeof(SmartObjectType), station);

        switch(parsedResponse)
        {
            case SmartObjectType.HintStation:
                OnHintStationScan(id, station);
                break;
        }
    }

    private void OnHintStationScan(string id, string station)
    {
        //TODO Write a check that makes sure LevelData doesn't give repeat hints. Otherwise breaks this logic

        List<LevelData.Hint> hintsAvailableToUser = gameManager_.AllHintsForUserId(id);
        GameStorage.UserData userData = gameManager_.UserDataForUserId(id);

        if (hintsAvailableToUser.Count > 0)
        {
            List<string> allHintsAvailableCopy = new List<string>();
            foreach (LevelData.Hint h in hintsAvailableToUser)
            {
                allHintsAvailableCopy.Add(h.Name);
            }

            gameManager_.SendNewActionNetworked(string.Format(
                "-station {0} show-hints {1} {2} {3}",
                station,
                nfcAtStation_[station],
                allHintsAvailableCopy.ConvertToStringArgument(),
                userData.RedeemedHints.ConvertToStringArgument()));
        } else
        {
            gameManager_.SendNewActionNetworked(string.Format(
                "-station {0} no-hints {1}",
                station));
        }
    }

    private void OnResourceStationScan(string id, string station)
    {
        List<GameStorage.ResourceType> allResourcesAvailable = gameManager_.AllResourcesForUserId(id);
        GameStorage.UserData userData = gameManager_.UserDataForUserId(id);

        if (userData.RedeemedResources.Count < allResourcesAvailable.Count)
        {
            //show that you can get resources, and redeem

            List<GameStorage.ResourceType> allResourcesAvailableCopy = new List<GameStorage.ResourceType>(allResourcesAvailable);
            List<GameStorage.ResourceType> redeemedResourcesCopy = new List<GameStorage.ResourceType>(userData.RedeemedResources);

            for (int i = 0; i < allResourcesAvailableCopy.Count; i++)
            {
                for (int ii = 0; ii < redeemedResourcesCopy.Count; ii++)
                {
                    if (redeemedResourcesCopy[ii] == allResourcesAvailableCopy[i])
                    {
                        redeemedResourcesCopy.RemoveAt(ii);
                        allResourcesAvailableCopy.RemoveAt(ii);
                    }
                }
            }

            gameManager_.SendNewActionNetworked(string.Format(
                "-station {0} give-resources {1}",
                station,
                string.Join(" ", allResourcesAvailableCopy)));

            userData.RedeemedResources = new List<GameStorage.ResourceType>(allResourcesAvailable);
            gameManager_.SaveUserData(userData, id);
        } else
        {
            //no resources available

            LevelData.Challenge nextChallengeWithResource = gameManager_.NextChallengeWithResourcesForUserId(id);
            if (nextChallengeWithResource != null)
            {
                //there are more resources available, show progress needed for more resources instead
                int nextChallengeIndex = 0;
                for(int i = 0; i < gameManager_.ChallengeData.Challenges.Count; i++)
                {
                    if (string.Equals(gameManager_.ChallengeData.Challenges[i].Name, nextChallengeWithResource.Name))
                    {
                        nextChallengeIndex = i;
                        break;
                    }
                }

                gameManager_.SendNewActionNetworked(string.Format(
                    "-station {0} more-resources {1} {2} {3}",
                    station,
                    userData.CompletedChallenges.Count.ToString(),
                    gameManager_.ChallengeData.Challenges.Count.ToString(),
                    (nextChallengeIndex+1).ToString()
                ));
            } else
            {
                //there are no more resources available in this class

                gameManager_.SendNewActionNetworked(string.Format("-station {0} no-resources", station));
            }
        }
    }

    private void OnTestStationScan(string id, string station)
    {
        LevelData.Challenge c = gameManager_.CurrentChallengeForUserId(id);
        gameManager_.SendNewActionNetworked(string.Format("-station {0} load {1}", station, c.Name));
        gameManager_.SendNewActionNetworked(string.Format("-validator-controller {0} load {1}", station, c.Name));
    }

    public void OnUsedHint(string id, string hint)
    {
        gameManager_.UsedHint(id, hint);
    }

    public void NewValidatorAction(string station, string command, List<string> arguments)
    {
        //dispatch series of VO, show inventory
        //set state, give inventory
        ValidatorResponse parsedResponse = (ValidatorResponse)Enum.Parse(typeof(ValidatorResponse), command);

        if (parsedResponse == ValidatorResponse.ScanWristband)
        {
            gameManager_.SendNewActionNetworked(string.Format("-station {0} {1}", station, ValidatorResponse.ScanWristband.ToString()));
            return;
        }

        if (!nfcAtStation_.ContainsKey(station))
        {
            Debug.LogWarning("Cannot find id for station: " + station);
            return;
        }

        switch(parsedResponse)
        {
            case ValidatorResponse.Success:
                OnValidatorSuccess(station, nfcAtStation_[station]);
                break;
            case ValidatorResponse.Failure:
                OnValidatorFailed(station, nfcAtStation_[station]);
                break;
            case ValidatorResponse.BeforeTest:
                if (arguments.Count > 0)
                {
                    OnValidatorProblem(station, nfcAtStation_[station], arguments[0]);
                }
                break;
        }
    }

    private void OnValidatorSuccess(string station, string id)
    {
        LevelData.Challenge c = gameManager_.CurrentChallengeForUserId(id);

        gameManager_.SendNewActionNetworked(
            string.Format(
                "-station {0} {1} {2}",
                station,
                CommandDispatch.ValidatorResponse.Success.ToString(),
                c.Name));

        GameStorage.UserData userData = gameManager_.UserDataForUserId(id);

        userData.CompletedChallenges.Add(c.Name);

        LevelData.Challenge nextChallenge = gameManager_.NextChallengeForUserId(id);
        if (nextChallenge != null)
        {
            userData.CurrentChallenge = nextChallenge.Name;
            gameManager_.SendNewActionNetworked(string.Format("-validator-controller {0} load {1}", station, nextChallenge.Name));
        }

        gameManager_.SaveUserData(userData, id);
    }

    private void OnValidatorFailed(string station, string id)
    {
        GameStorage.UserData userData = gameManager_.UserDataForUserId(id);

        bool haveRedeemableHints = gameManager_.AllHintsForUserId(id).Count > 0;

        gameManager_.SendNewActionNetworked(
            string.Format(
                "-station {0} {1} {2}",
                station,
                CommandDispatch.ValidatorResponse.Failure.ToString(),
                haveRedeemableHints ? "show-hint" : "dont-show-hints"));
    }

    private void OnValidatorProblem(string station, string id, string problem)
    {
        gameManager_.SendNewActionNetworked(
            string.Format(
                "-station {0} {1} {2}",
                station,
                CommandDispatch.ValidatorResponse.BeforeTest.ToString(),
                problem));
    }
}
