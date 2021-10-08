using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrefabContentCodeEditor : PrefabContent
{
    public const string DANCE_CODE_KEY = "dance_code";

    [SerializeField] List<DanceCharacter> characters_;
    [SerializeField] List<Image> codeOptions_;
    [SerializeField] Image border_;
    [SerializeField] Text codeText_;

    private int counter_ = 0;
    private DanceCharacter activeCharacter_;
    private DanceCode code_;

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

        code_ = new DanceCode();
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
            activeCharacter_.PlayAnimation(codeOptions_[counter_].name);
        } else if (string.Equals(action, "Enter"))
        {
            AddCode(codeOptions_[counter_].name);
        } else if (string.Equals(action, "Delete"))
        {
            DeleteCode();
        }
    }

    private void SelectCode(int counter)
    {
        counter_ = counter;

        if (counter_ <= 0) counter_ = codeOptions_.Count - 1;
        if (counter >= codeOptions_.Count) counter_ = 0;

        border_.transform.localPosition = codeOptions_[counter_].transform.localPosition;
    }

    private void AddCode(string animation)
    {
        code_.AddCommand(animation);
        RefreshCodeText();
    }

    private void DeleteCode()
    {
        code_.RemoveCommand();
        RefreshCodeText();
    }

    private void RefreshCodeText()
    {
        codeText_.text = code_.ToString();
    }
}
