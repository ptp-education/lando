using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Lando.SmartObjects;

public class CommandDispatch
{
    private LevelData levelData_;
    private GameManager gameManager_;
    private Dictionary<string, string> nfcAtStation_ = new Dictionary<string, string>();

    public enum ValidatorResponse
    {
        Success,
        Failure,
        BeforeTest
    }

    public void Init(LevelData levelData, GameManager gameManager)
    {
        levelData_ = levelData;
        gameManager_ = gameManager;
    }

    public void NewNfc(string station, string id)
    {
        nfcAtStation_[station] = id;

        SmartObjectType stationType = (SmartObjectType)Enum.Parse(typeof(SmartObjectType), station);
        switch(stationType)
        {
            case SmartObjectType.ResourceStation:
                break;
            case SmartObjectType.TestingStation:
                OnTestStationScan(id, station);
                break;
            case SmartObjectType.HintStation:
                break;
        }
    }

    private void OnTestStationScan(string id, string station)
    {
        LevelData.Challenge c = CurrentChallengeForId(id);
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
                break;
            case ValidatorResponse.BeforeTest:
                break;
        }
    }

    private void OnValidatorSuccess(string station, string id)
    {
        LevelData.Challenge c = CurrentChallengeForId(id);

        gameManager_.SendNewAction(string.Format("-station {0} completed-challenge {1}", station, c.Name)); //expecting screen to read out VO, show new inventory

        GameStorage.UserData userData = UserDataForId(id);

        userData.CompletedChallenges.Add(c.Name);

        LevelData.Challenge nextChallenge = NextChallengeForRfid(id);
        if (nextChallenge != null)
        {
            userData.CurrentChallenge = nextChallenge.Name;
        }

        SaveUserData(id, userData);
    }

    private void OnValidatorFailed(string station, string id)
    {
        //based on whether user has hints, dispatch show hint screen, or just dispatch failed
    }

    #region QUERIES

    protected GameStorage.UserData UserDataForId(string id)
    {
        GameStorage gs = gameManager_.GameStorageForRfid(id);
        GameStorage.UserData userData = gs.GetValue<GameStorage.UserData>(GameStorage.Key.UserData);
        if (userData == null) userData = new GameStorage.UserData();
        return userData;
    }

    protected void SaveUserData(string id, GameStorage.UserData userData)
    {
        GameStorage gs = gameManager_.GameStorageForRfid(id);
        gs.Add<GameStorage.UserData>(GameStorage.Key.UserData, userData);
    }

    protected List<string> RedeemableHints(string id)
    {
        List<string> ret = new List<string>();

        GameStorage.UserData userData = UserDataForId(id);

        for (int i = 0; i < levelData_.Challenges.Count; i++)
        {
            
        }

        return ret;
    }

    protected LevelData.Challenge NextChallengeForRfid(string id)
    {
        GameStorage.UserData userData = UserDataForId(id);
        for (int i = 0; i < levelData_.Challenges.Count; i++)
        {
            LevelData.Challenge c = levelData_.Challenges[i];
            if (string.Equals(c.Name, userData.CurrentChallenge))
            {
                if (i + 1 < levelData_.Challenges.Count)
                {
                    return levelData_.Challenges[i + 1];
                }
                else
                {
                    return null;
                }
            }
        }
        return null;
    }

    protected LevelData.Challenge CurrentChallengeForId(string id)
    {
        GameStorage.UserData userData = UserDataForId(id);

        if (userData.CurrentChallenge == null || userData.CurrentChallenge.Length == 0)
        {
            userData.CurrentChallenge = levelData_.Challenges[0].Name;
            SaveUserData(id, userData);
        }
        foreach (LevelData.Challenge c in levelData_.Challenges)
        {
            if (string.Equals(c.Name, userData.CurrentChallenge))
            {
                return c;
            }
        }
        return null;
    }

    #endregion
}
