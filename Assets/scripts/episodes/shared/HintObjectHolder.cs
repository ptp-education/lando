using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintObjectHolder : MonoBehaviour
{
    [SerializeField] private Image mainImage_;
    [SerializeField] private Image overlay_;
    [SerializeField] private Image border_;

    public void SetSprite(Sprite mainImage)
    {
        mainImage_.sprite = mainImage;
    }

    public void ToggleOverlay(bool show)
    {
        overlay_.gameObject.SetActive(show);
        border_.color = show ? Color.white : new Color(255, 255, 255, 90);
    }
}
