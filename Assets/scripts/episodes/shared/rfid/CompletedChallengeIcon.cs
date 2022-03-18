using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompletedChallengeIcon : MonoBehaviour
{
    [SerializeField] private Image icon_;
    [SerializeField] private Image highlight_;

    public void SetSprite(Sprite s)
    {
        icon_.sprite = s;
        icon_.SetNativeSize();
    }

    public void ToggleHighlight(bool highlight)
    {
        highlight_.gameObject.SetActive(highlight);
    }
}
