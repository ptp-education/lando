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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
