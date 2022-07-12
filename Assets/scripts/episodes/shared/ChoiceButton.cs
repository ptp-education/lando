using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceButton : MonoBehaviour
{
    [SerializeField] private Text optionTitle_;
    [SerializeField] private GameObject lock_;

    public void Setup(string title, bool teacherOnly)
    {
        optionTitle_.text = title;
        lock_.gameObject.SetActive(teacherOnly);
    }
}
