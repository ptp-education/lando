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

        if (show)
        {
            AudioPlayer.PlaySfx("turn-off");
        }
    }

    public bool IsActive
    {
        get
        {
            return choicesParent_.gameObject.activeSelf;
        }
    }

    public void UpdateChoices(EpisodeNode.OptionsHolder holder)
    {
        DeleteOptions();

        if (holder.Options.Count > 0 && holder.StartShown)
        {
            ToggleVisbility(true);
        }

        for (int i = 0; i < holder.Options.Count; i++)
        {
            EpisodeNode.Option o = holder.Options[i];

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

        ToggleVisbility(false);
    }
}
