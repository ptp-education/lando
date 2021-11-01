using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameManager))]
public class TesterLoader : MonoBehaviour
{
    [SerializeField] private GameObject startingEpisode_;
    [SerializeField] private string startingNode_;
    void Start()
    {
        GameManager gm = GetComponent<GameManager>();
        gm.NewEpisodeEvent(startingEpisode_.name);
        gm.NewNodeAction(startingNode_);
    }
}
