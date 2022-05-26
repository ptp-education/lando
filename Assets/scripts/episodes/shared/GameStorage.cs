using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStorage
{
    public enum Key
    {
        DanceCode,
        SelectedCharacter,
        Counter,
        ChosenColor,
        HouseLevel,
        FarmObjects,
        BridgeColors,
        SpawnedTruck,
        ShowAnimalOutlines,
        Lego2Tower,
        Lego3Art,
        MomoStarter,
        MomoTeenCustomization,
        MomoAdultCustomization,
        UserData,
        HintSpeechProgression
    }

    public enum ResourceType
    {
        CupOfLegos
    }

    public class Integer
    {
        public int value = 0;

        public Integer(int v)
        {
            value = v;
        }
    }

    public class UserData
    {
        public string CurrentChallenge;
        public List<string> CompletedChallenges = new List<string>();
        public List<ResourceType> RedeemedResources = new List<ResourceType>();
        public List<string> RedeemedHints = new List<string>();
    }

    private Dictionary<string, object> dict_ = new Dictionary<string, object>();

    public void Add<T>(Key key, T value) where T : class
    {
        dict_[key.ToString()] = value;
    }

    public T GetValue<T>(Key key) where T : class
    {
        if (dict_.ContainsKey(key.ToString()))
        {
            return dict_[key.ToString()] as T;
        } else
        {
            return null;
        }
    }

    public void AddObjectToList<T>(Key key, T obj)
    {
        List<T> list = GetValue<List<T>>(key);
        if (list == null)
        {
            list = new List<T>();
        }
        list.Add(obj);
        Add<List<T>>(key, list);
    }

    public UserData GetUserData()
    {
        GameStorage.UserData userData = GetValue<GameStorage.UserData>(GameStorage.Key.UserData);
        if (userData == null)
        {
            userData = new GameStorage.UserData();
            Add<GameStorage.UserData>(GameStorage.Key.UserData, userData);
        }
        return userData;
    }

    public void SaveUserData(UserData data)
    {
        Add<GameStorage.UserData>(GameStorage.Key.UserData, data);
    }

    public void ResetStorage()
    {
        dict_ = new Dictionary<string, object>();
    }
}
