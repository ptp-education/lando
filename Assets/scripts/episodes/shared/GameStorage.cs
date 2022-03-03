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
        MomoAdultCustomization
    }

    public class Integer
    {
        public int value = 0;

        public Integer(int v)
        {
            value = v;
        }
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

    public void ResetStorage()
    {
        dict_ = new Dictionary<string, object>();
    }
}
