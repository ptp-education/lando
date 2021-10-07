using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrefabContentCodeEditor : PrefabContent
{
    [SerializeField] List<DanceCharacter> characters_;
    [SerializeField] List<Image> codeOptions_;
    [SerializeField] Image border_;
    [SerializeField] Text codeText_;

    private int counter_ = 0;
    private DanceCharacter activeCharacter_;

    public override void Play() 
    {
        base.Play();

        string selectedCharacter = gameManager_.Storage.GetValue<string>(PrefabContentDanceSelector.DANCE_CHARACTER_KEY);
        foreach(DanceCharacter c in characters_) 
        {
            bool rightCharacter = string.Equals(c.gameObject.name, selectedCharacter);
            c.gameObject.SetActive(rightCharacter);
            if (rightCharacter)
            {
                activeCharacter_ = c;
            }
        }
    }

    public override void ReceiveAction(string action)
    {
        base.ReceiveAction(action);

        if (string.Equals(action, "Up"))
        {
            SelectCode(counter_ - 1);
        } else if (string.Equals(action, "Down"))
        {
            SelectCode(counter_ + 1);
        }
        else if (string.Equals(action, "Play"))
        {

        } else if (string.Equals(action, "Enter"))
        {

        } else if (string.Equals(action, "Delete"))
        {

        }
    }

    private void SelectCode(int counter)
    {
        counter_ = counter;

        if (counter_ <= 0) counter_ = codeOptions_.Count - 1;
        if (counter >= codeOptions_.Count) counter_ = 0;

        border_.transform.localPosition = codeOptions_[counter_].transform.localPosition;
    }

    private void PlayCodeAnimation(string animation)
    {
        activeCharacter_.PlayAnimation(animation);
    }

    private void AddCode(string code)
    {
        codeText_.text = codeText_.text + "\n" + code;
    }

    private void DeleteCode()
    {

    }
}
