using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceButton : MonoBehaviour
{
    [SerializeField] private Text optionNumber_;
    [SerializeField] private Text optionTitle_;
    [SerializeField] private GameObject lock_;

    public void Setup(string option, string title, bool teacherOnly)
    {
        optionNumber_.text = option;
        optionTitle_.text = title;
        lock_.gameObject.SetActive(teacherOnly);
    }
}
