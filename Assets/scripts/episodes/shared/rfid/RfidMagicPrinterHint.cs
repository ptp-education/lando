using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RfidMagicPrinterHint : MonoBehaviour
{
    [SerializeField] private Image hintOn_;
    [SerializeField] private Image hintOff_;

    public void SetOn()
    {
        hintOff_.gameObject.SetActive(false);
        hintOn_.gameObject.SetActive(true);
    }

    public void SetOff()
    {
        hintOff_.gameObject.SetActive(true);
        hintOn_.gameObject.SetActive(false);
    }
}
