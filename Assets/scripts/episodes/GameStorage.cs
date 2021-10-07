using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStorage
{
    private Dictionary<string, object> dict_ = new Dictionary<string, object>();

    public void Add<T>(string key, T value) where T : class
    {
        dict_[key] = value;
        // dict_.Add(key, value);
    }

    public T GetValue<T>(string key) where T : class
    {
        return dict_[key] as T;
    }

    public void ResetStorage()
    {
        dict_ = new Dictionary<string, object>();
    }
}
