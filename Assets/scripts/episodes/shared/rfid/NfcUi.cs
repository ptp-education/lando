using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NfcUi : SpawnedObject
{
    protected ChallengeData ChallengeData
    {
        get
        {
            return gameManager_.ChallengeData;
        }
    }

    protected List<GameStorage.InventoryType> InventoryForRfid(string rfid)
    {
        GameStorage.UserData userData = UserDataForRfid(rfid);
        return userData.Inventory;
    }

    protected int CupsOfLegoForRfid(string rfid)
    {
        GameStorage.UserData userData = UserDataForRfid(rfid);
        List<GameStorage.InventoryType> inventory = userData.Inventory;
        return inventory.Where(i => i == GameStorage.InventoryType.CupOfLegos).Count();
    }

    protected int HintsForRfid(string rfid)
    {
        GameStorage.UserData userData = UserDataForRfid(rfid);
        List<GameStorage.InventoryType> inventory = userData.Inventory;
        return inventory.Where(i => i == GameStorage.InventoryType.Hint).Count();
    }

    protected GameStorage.UserData UserDataForRfid(string rfid)
    {
        GameStorage gs = gameManager_.GameStorageForRfid(rfid);
        GameStorage.UserData userData = gs.GetValue<GameStorage.UserData>(GameStorage.Key.RfidUserData);
        if (userData == null) userData = new GameStorage.UserData();
        return userData;
    }

    protected void SaveUserData(string rfid, GameStorage.UserData userData)
    {
        GameStorage gs = gameManager_.GameStorageForRfid(rfid);
        gs.Add<GameStorage.UserData>(GameStorage.Key.RfidUserData, userData);
    }

    protected ChallengeData.Challenge NextChallengeForRfid(string rfid)
    {
        GameStorage.UserData userData = UserDataForRfid(rfid);
        for (int i = 0; i < ChallengeData.Challenges.Count; i++)
        {
            ChallengeData.Challenge c = ChallengeData.Challenges[i];
            if (string.Equals(c.Name, userData.CurrentChallenge))
            {
                if (i + 1 < ChallengeData.Challenges.Count)
                {
                    return ChallengeData.Challenges[i + 1];
                }
                else
                {
                    return null;
                }
            }
        }
        return null;
    }

    protected ChallengeData.Challenge CurrentChallengeForRfid(string rfid)
    {
        GameStorage.UserData userData = UserDataForRfid(rfid);

        if (userData.CurrentChallenge == null || userData.CurrentChallenge.Length == 0)
        {
            userData.CurrentChallenge = ChallengeData.Challenges[0].Name;
            SaveUserData(rfid, userData);
        }
        foreach (ChallengeData.Challenge c in ChallengeData.Challenges)
        {
            if (string.Equals(c.Name, userData.CurrentChallenge))
            {
                return c;
            }
        }
        return null;
    }

    protected void AddInventoryForRfid(string rfid, List<GameStorage.InventoryType> inventoryToAdd)
    {
        GameStorage.UserData userData = UserDataForRfid(rfid);

        List<GameStorage.InventoryType> inv = userData.Inventory;
        for (int i = 0; i < inventoryToAdd.Count; i++)
        {
            inv.Add(inventoryToAdd[i]);
        }
        SaveUserData(rfid, userData);
    }
}
