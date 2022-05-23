using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonToggle : MonoBehaviour
{
    [SerializeField] List<Button> buttons_;

    private System.Action<string> onButtonPress_;

    public void Init(System.Action<string> onButtonPress)
    {
        onButtonPress_ = onButtonPress;
    }

    public void ButtonUpdated(Button updatedButton)
    {
        onButtonPress_(updatedButton.name);
        DisableAllButtons();
    }

    public void DisableAllButtons()
    {
        foreach(Button b in buttons_)
        {
            b.interactable = false;
        }
    }

    public void EnableAllButtons()
    {
        foreach (Button b in buttons_)
        {
            b.interactable = true;
        }
    }
}
