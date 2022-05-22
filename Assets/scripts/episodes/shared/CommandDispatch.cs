using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Lando.SmartObjects;

public class CommandDispatch
{
    private GameManager gameManager_;
    private Dictionary<string, string> nfcAtStation_ = new Dictionary<string, string>();

    public enum ValidatorResponse
    {
        Success,
        Failure,
        BeforeTest
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
                break;
            case SmartObjectType.TestingStation:
                OnTestStationScan(id, stationType.ToString());
                break;
            case SmartObjectType.HintStation:
                break;
        }
    }

    private void OnTestStationScan(string id, string station)
    {
        LevelData.Challenge c = gameManager_.CurrentChallengeForUserId(id);
        gameManager_.SendNewAction(string.Format("-station {0} load {1}", station, c.Name));
    }

    public void NewValidatorAction(string station, string command, List<string> arguments)
    {
        //dispatch series of VO, show inventory
        //set state, give inventory
        if (!nfcAtStation_.ContainsKey(station))
        {
            Debug.LogWarning("Cannot find id for station: " + station);
            return;
        }

        ValidatorResponse parsedResponse = (ValidatorResponse)Enum.Parse(typeof(ValidatorResponse), command);
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

        gameManager_.SendNewAction(
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
        }
        
        gameManager_.SaveUserData(userData, id);
    }

    private void OnValidatorFailed(string station, string id)
    {
        GameStorage.UserData userData = gameManager_.UserDataForUserId(id);

        bool haveRedeemableHints = gameManager_.AllHintsForUserId(id).Count > userData.RedeemedHints.Count;

        gameManager_.SendNewAction(
            string.Format(
                "-station {0} {1} {2}",
                station,
                CommandDispatch.ValidatorResponse.Failure.ToString(),
                haveRedeemableHints ? "show-hint" : "dont-show-hints"));
    }

    private void OnValidatorProblem(string station, string id, string problem)
    {
        gameManager_.SendNewAction(
            string.Format(
                "-station {0} {1} {2}",
                station,
                CommandDispatch.ValidatorResponse.BeforeTest.ToString(),
                problem));
    }
}
