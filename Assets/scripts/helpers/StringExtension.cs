using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StringExtensions
{
    public static string StripExtensions(this string text)
    {
        string[] split = text.Split('/');
        string fn = split[split.Length - 1];

        string[] split2 = fn.Split('.');
        return split2[0];
    }

    public static string StripResources(this string text)
    {
        return text.Substring("Resources/".Length);
    }

    public static string StripFileExtension(this string text)
    {
        string[] split = text.Split('.');
        string ret = "";
        for(int i = 0; i < split.Length - 1; i++)
        {
            ret += split[i];
        }
        return ret;
    }
}
