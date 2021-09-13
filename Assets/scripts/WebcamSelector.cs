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
    [SerializeField] bool spawnNoWebcam_ = true;

    private WebcamSelected webcamSelected_;

    private int buttonCounter_ = 0;

    public void Init(WebcamSelected webcamSelected)
    {
        webcamSelected_ = webcamSelected;
    }

    void Start()
    {
        SpawnButtons();
    }

    private void SpawnButtons()
    {
        buttonCounter_ = 0;

        if (spawnNoWebcam_)
        {
            Button b = Instantiate<Button>(button_);
            b.transform.SetParent(transform, true);

            b.transform.localPosition = buttonPosition();
            b.GetComponentInChildren<Text>().text = "No Webcam";
            b.onClick.AddListener(delegate { ButtonClicked(null); });
        }

        Application.ExternalCall("GetCameras");
    }

    public void Cameras(string camerasJson)
    {
        var parsedJSON = JSON.Parse(camerasJson);
        JSONArray devicesArray = parsedJSON.AsArray;

        int offsetCounter = 0;
        for (int i = 0; i < devicesArray.Count; i++)
        {
            if (devicesArray[i]["kind"] == "videoinput")
            {
                Button b = Instantiate<Button>(button_);
                b.transform.SetParent(transform, true);

                b.transform.localPosition = buttonPosition();

                string s = null;
                string deviceId = devicesArray[i]["deviceId"];
                s = devicesArray[i]["label"];
                b.GetComponentInChildren<Text>().text = s;
                b.onClick.AddListener(delegate {
                    ButtonClicked(s);
                    Application.ExternalCall("getStream", deviceId);
                });

                offsetCounter++;
            }
        }
    }

    private Vector3 buttonPosition()
    {
        Vector3 p = new Vector3(xOffset_ * buttonCounter_, yOffset_ * buttonCounter_);
        buttonCounter_++;
        return p;
    }

    private void ButtonClicked(string webcam)
    {
        webcamSelected_.Invoke(webcam);
    }
}
