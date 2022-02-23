using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameManager))]
public class TesterLoader : MonoBehaviour
{
    [SerializeField] private string startingEpisode_;
    [SerializeField] private string startingNode_;

    void Start()
    {
        GameManager gm = GetComponent<GameManager>();

        gm.NewEpisodeEvent(startingEpisode_);
        gm.NewNodeAction(startingNode_);
    }
}
