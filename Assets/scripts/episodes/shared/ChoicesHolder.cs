using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoicesHolder : MonoBehaviour
{
    [SerializeField] ChoiceButton choicePrefab_;
    [SerializeField] Transform choicesParent_;

    private List<ChoiceButton> spawnedChoices_ = new List<ChoiceButton>();
    private List<EpisodeNode.Options> options_;

    public void UpdateChoices(List<EpisodeNode.Options> options)
    {
        DeleteOptions();

        options_ = options;

        for (int i = 0; i < options.Count; i++)
        {
            EpisodeNode.Options o = options[i];

            ChoiceButton c = Instantiate(choicePrefab_, choicesParent_);
            c.Setup((i + 1).ToString(), o.ButtonName, o.TeacherOnly);
            spawnedChoices_.Add(c);
        }
    }

    public string CommandForAction(int optionSelected, bool teacher)
    {
        if (optionSelected < options_.Count)
        {
            EpisodeNode.Options o = options_[optionSelected];
            if (o.TeacherOnly)
            {
                return teacher ? o.Command : null;
            } else
            {
                return o.Command;
            }
        }
        return null;
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
