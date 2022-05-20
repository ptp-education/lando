using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonToggle : MonoBehaviour
{
    [SerializeField] Sprite selectedSprite_;
    [SerializeField] Sprite deselectedSprite_;
    [SerializeField] List<Button> buttons_;

    private Button selectedButton_;

    public string Selected
    {
        get
        {
            return selectedButton_.name;
        }
    }

    private void Start()
    {
        EnableButton(buttons_[0]);
    }

    public void ButtonUpdated(Button updatedButton)
    {
        EnableButton(updatedButton);
    }

    private void EnableButton(Button enabledButton)
    {
        selectedButton_ = enabledButton;
        enabledButton.GetComponent<Image>().sprite = selectedSprite_;

        foreach(Button b in buttons_)
        {
            if (b != enabledButton)
            {
                b.GetComponent<Image>().sprite = deselectedSprite_;
            }
        }
    }
}
