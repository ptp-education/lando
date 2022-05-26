using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListExtensions
{
    public static string ConvertToStringArgument(this List<string> list)
    {
        string ret = "{";
        foreach(string s in list)
        {
            ret += s + "&";
        }
        if (list.Count > 0)
        {
            ret = ret.Remove(ret.Length - 1);
        }
        ret += "}";
        return ret;
    }
}
