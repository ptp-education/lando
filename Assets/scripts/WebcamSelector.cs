using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebcamSelector : MonoBehaviour
{
    public delegate void WebcamSelected(string w);

    [SerializeField] Button button_;

    private WebcamSelected webcamSelected_;

    public void Init(WebcamSelected webcamSelected)
    {
        webcamSelected_ = webcamSelected;
    }

    void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        
        for (int i = 0; i <= devices.Length; i++)
        {
            Button b = Instantiate<Button>(button_);
            b.transform.SetParent(transform, true);

            b.transform.localPosition = new Vector3(0, -45 * i);

            if (i < devices.Length)
            {
                string s = null;
                s = devices[i].name;
                b.GetComponentInChildren<Text>().text = s;
                b.onClick.AddListener(delegate { ButtonClicked(s); });
            }
            else
            {
                b.GetComponentInChildren<Text>().text = "No webcam";
                b.onClick.AddListener(delegate { ButtonClicked(null); });
            }
        }
    }

    private void ButtonClicked(string webcam)
    {
        webcamSelected_.Invoke(webcam);
    }
}
