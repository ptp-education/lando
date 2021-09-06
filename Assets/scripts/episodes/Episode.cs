using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Episode : MonoBehaviour
{
    [SerializeField] public EpisodeNode StartingNode;

    public EpisodeNode[] AllNodes
    {
        get
        {
            return GetComponentsInChildren<EpisodeNode>();
        }
    }
}
