using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Image progressBar_;
    [SerializeField] private ShareManager shareManager_;

    void Update()
    {
        RectTransform rt = GetComponent<RectTransform>();
        progressBar_.rectTransform.sizeDelta = new Vector2(shareManager_.ProgressPercentage * rt.rect.width, rt.rect.height);
    }
}
