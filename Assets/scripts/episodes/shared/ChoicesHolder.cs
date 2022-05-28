using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoicesHolder : MonoBehaviour
{
    [SerializeField] ChoiceButton choicePrefab_;
    [SerializeField] Transform choicesParent_;

    private List<ChoiceButton> spawnedChoices_ = new List<ChoiceButton>();

    public void ToggleVisbility(bool show)
    {
        choicesParent_.gameObject.SetActive(show);
    }

    public bool IsActive
    {
        get
        {
            return choicesParent_.gameObject.activeSelf;
        }
    }

    public void UpdateChoices(List<EpisodeNode.Options> options)
    {
        DeleteOptions();

        if (options.Count > 0)
        {
            ToggleVisbility(true);
        }

        for (int i = 0; i < options.Count; i++)
        {
            EpisodeNode.Options o = options[i];

            ChoiceButton c = Instantiate(choicePrefab_, choicesParent_);
            c.Setup((i + 1).ToString(), o.ButtonName, o.TeacherOnly);
            spawnedChoices_.Add(c);
        }
    }

    public void DeleteOptions()
    {
        foreach(ChoiceButton o in spawnedChoices_)
        {
            Destroy(o.gameObject);
        }
        spawnedChoices_ = new List<ChoiceButton>();
    }
}
