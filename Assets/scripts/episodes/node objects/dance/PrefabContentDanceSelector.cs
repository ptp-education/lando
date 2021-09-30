using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrefabContentDanceSelector : PrefabContent
{
    [SerializeField] List<DanceCharacter> characters_;
    [SerializeField] List<Image> charactersAvatars_;
    [SerializeField] Image border_;

    private int counter_ = 0;
    
    void Start()
    {
        SelectCharacter(0);
    }

    public override void Play()
    {
        base.Play();

        ResetCounter();
        SelectCharacter(0);
    }

    private void ResetCounter()
    {
        counter_ = 0;
    }

    public override void ReceiveAction(string action)
    {
        base.ReceiveAction(action);

        if (string.Equals(action, "Left"))
        {
            SelectCharacter(-1);
        } else if (string.Equals(action, "Right"))
        {
            SelectCharacter(1);
        } else if (string.Equals(action, "Up"))
        {
            SelectCharacter(-2);
        }
        else if (string.Equals(action, "Down"))
        {
            SelectCharacter(2);
        }
    }

    private void SelectCharacter(int change)
    {
        int newCounter = counter_ + change;
        if (newCounter < 0)
        {
            counter_ = characters_.Count - 1 - (Mathf.Abs(newCounter) - 1);
        } else if (newCounter > characters_.Count - 1)
        {
            counter_ = newCounter - characters_.Count;
        } else
        {
            counter_ = counter_ + change;
        }

        foreach (DanceCharacter c in characters_)
        {
            c.gameObject.SetActive(false);
        }
        border_.gameObject.transform.localPosition = charactersAvatars_[counter_].gameObject.transform.localPosition;
        characters_[counter_].gameObject.SetActive(true);
        characters_[counter_].Selected();
    }
}
