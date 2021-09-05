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

    void Start()
    {
        webcamSelector_.Init(WebcamSelected);
    }

    private void WebcamSelected(string webcam)
    {
        webcamSelector_.gameObject.SetActive(false);

        if (webcam != null)
        {
            StartCoroutine(InitializeWebcam(webcam));
        } else
        {
            webcamImage.gameObject.SetActive(false);
        }
    }

    IEnumerator InitializeWebcam(string webcam)
    {
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            webcamImage.enabled = true;

            Debug.LogError("has auth");
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
