using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using SimpleJSON;

public class WebcamSelector : MonoBehaviour
{
    public delegate void WebcamSelected(string w);

    [SerializeField] Button button_;
    [SerializeField] int xOffset_;
    [SerializeField] int yOffset_;

    private WebcamSelected webcamSelected_;

    public void Init(WebcamSelected webcamSelected)
    {
        webcamSelected_ = webcamSelected;
    }

    void Start()
    {
        GetCameras();
    }

    void GetCameras()
    {
        Application.ExternalCall("GetCameras");
    }

    public void Cameras(string camerasJson)
    {
        var parsedJSON = JSON.Parse(camerasJson);
        JSONArray devicesArray = parsedJSON.AsArray;

        for (int i = 0; i <= devicesArray.Count; i++)
        {
            if (devicesArray[i]["kind"] == "videoinput")
            {
                Button b = Instantiate<Button>(button_);
                b.transform.SetParent(transform, true);

                b.transform.localPosition = new Vector3(xOffset_ * i, yOffset_ * i);

                if (i < devicesArray.Count)
                {
                    string s = null;
                    string deviceId = devicesArray[i]["deviceId"];
                    Debug.Log("unity: " + deviceId);
                    s = devicesArray[i]["label"];
                    b.GetComponentInChildren<Text>().text = s;
                    b.onClick.AddListener(delegate {
                        ButtonClicked(s);
                        Application.ExternalCall("getStream", deviceId);
                    });
                }
                else
                {
                    b.GetComponentInChildren<Text>().text = "No webcam";
                    b.onClick.AddListener(delegate { ButtonClicked(null); });
                }
            }
        }

    }

    private void ButtonClicked(string webcam)
    {
        webcamSelected_.Invoke(webcam);
    }
}
