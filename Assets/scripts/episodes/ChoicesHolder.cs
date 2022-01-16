using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoicesHolder : MonoBehaviour
{
    [SerializeField] GameObject choicePrefab_;
    [SerializeField] Transform choicesParent_;

    private List<GameObject> spawnedChoices_ = new List<GameObject>();

    public void AddOption(string option)
    {
        GameObject c = Instantiate(choicePrefab_, choicesParent_);
        c.GetComponentInChildren<Text>().text = option;
        spawnedChoices_.Add(c);
    }

    public void DeleteOptions()
    {
        foreach(GameObject o in spawnedChoices_)
        {
            Destroy(o.gameObject);
        }
        spawnedChoices_ = new List<GameObject>();
    }
}
