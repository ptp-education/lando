using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestoreMaterial : MonoBehaviour
{
    private Material mat_;

    private void Awake()
    {
        mat_ = GetComponent<Renderer>().material;
    }

    private void OnDisable()
    {
        GetComponent<Renderer>().material = mat_;
    }

    private void OnApplicationQuit()
    {
        GetComponent<Renderer>().material = mat_;
    }
}
