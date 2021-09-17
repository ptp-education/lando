using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebcamPlayer : MonoBehaviour
{
    [SerializeField] WebcamSelector webcamSelector_;
    [SerializeField] Text statusText_;
    [SerializeField] RawImage webcamImage;
    [SerializeField] Nexweron.WebCamPlayer.WebCamPlayer externalWebcamPlayer_;

    [SerializeField] bool hideSelectorOnTap_;

    void Start()
    {
        webcamSelector_.Init(WebcamSelected);
    }

    private void WebcamSelected(string webcam)
    {
        if (webcam != null)
        {
            InitializeWebcam(webcam);
        } else
        {
            webcamImage.gameObject.SetActive(false);
        }

        if (hideSelectorOnTap_)
        {
            webcamSelector_.gameObject.SetActive(false);
        }
    }

    private void InitializeWebcam(string webcam)
    {
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            webcamImage.enabled = true;

            WebCamTexture wct = new WebCamTexture(webcam);
            externalWebcamPlayer_.webCamTexture = wct;

            wct.requestedFPS = 60;
            wct.requestedWidth = 1080;
            wct.requestedHeight = 1920;
            wct.Play();
        }
        else
        {
            Debug.LogError("does not has auth");
            statusText_.text = "Please restart the application and grant permission to access webcam.";
        }
    }
}
