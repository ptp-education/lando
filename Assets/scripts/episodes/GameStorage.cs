using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStorage
{
    public enum Key
    {
        DanceCode,
        SelectedCharacter
    }
    private Dictionary<string, object> dict_ = new Dictionary<string, object>();

    public void Add<T>(Key key, T value) where T : class
    {
        dict_[key.ToString()] = value;
        // dict_.Add(key, value);
    }

    public T GetValue<T>(Key key) where T : class
    {
        return dict_[key.ToString()] as T;
    }

    public void ResetStorage()
    {
        dict_ = new Dictionary<string, object>();
    }
}
