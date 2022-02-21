using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnedGuide : SpawnedObject
{
    [SerializeField] private Animator tableGuide_;
    [SerializeField] private Animator legosGuide_;
    [SerializeField] private Animator everythingLegosGuide_;
    [SerializeField] private Animator printerGuide_;
    [SerializeField] private Animator legosCheckbox_;
    [SerializeField] private Animator everythingLegosCheckbox_;
    [SerializeField] private Animator printerCheckbox_;
    [SerializeField] private Image legosStop_; 
    [SerializeField] private Image everythingLegosStop_; 
    [SerializeField] private Image printerStop_;

    private const string kGuideKey = "-guide-";

    public override void ReceivedAction(string action)
    {
        base.ReceivedAction(action);

        string[] split = action.Split(' ');

        foreach(string a in split)
        {
            if (a.Contains(kGuideKey))
            {
                bool playCollectedItem = false;
                bool playGuideSuccess = false;
                bool playGuideAppears = false;

                string command = a.Substring(kGuideKey.Length).Trim();
                switch (command)
                {
                    case "setup-table":
                        tableGuide_.Play("appear");
                        playGuideAppears = true;
                        break;
                    case "complete-setup-table":
                        tableGuide_.Play("complete");
                        playGuideSuccess = true;
                        break;
                    case "setup-legos":
                        legosGuide_.Play("appear");
                        playGuideAppears = true;
                        break;
                    case "complete-setup-legos":
                        legosGuide_.Play("complete");
                        playGuideSuccess = true;
                        break;
                    case "setup-printer":
                        printerGuide_.Play("appear");
                        playGuideAppears = true;
                        break;
                    case "complete-setup-printer":
                        printerGuide_.Play("complete");
                        playGuideSuccess = true;
                        break;
                    case "setup-everything-legos":
                        everythingLegosGuide_.Play("appear");
                        playGuideAppears = true;
                        break;
                    case "complete-setup-everything-legos":
                        everythingLegosGuide_.Play("complete");
                        playGuideSuccess = true;
                        break;
                    case "get-legos":
                        legosCheckbox_.Play("todo");
                        legosStop_.gameObject.SetActive(false);
                        playGuideAppears = true;
                        break;
                    case "gotten-legos":
                        legosCheckbox_.Play("checked");
                        playCollectedItem = true;
                        break;
                    case "get-everything-legos":
                        everythingLegosCheckbox_.Play("todo");
                        everythingLegosStop_.gameObject.SetActive(false);
                        playGuideAppears = true;
                        break;
                    case "gotten-everything-legos":
                        everythingLegosCheckbox_.Play("checked");
                        playCollectedItem = true;
                        break;
                    case "get-receipt":
                        printerCheckbox_.Play("todo");
                        printerStop_.gameObject.SetActive(false);
                        playGuideAppears = true;
                        break;
                    case "gotten-receipt":
                        printerCheckbox_.Play("checked");
                        playCollectedItem = true;
                        break;
                    case "get-legos-and-receipt":
                        printerCheckbox_.Play("todo");
                        legosCheckbox_.Play("todo");
                        legosStop_.gameObject.SetActive(false);
                        printerStop_.gameObject.SetActive(false);
                        playGuideAppears = true;
                        break;
                    case "show-stops":
                        legosStop_.gameObject.SetActive(true);
                        everythingLegosStop_.gameObject.SetActive(true);
                        printerStop_.gameObject.SetActive(true);
                        break;
                }

                if (playGuideAppears)
                {
                    AudioPlayer.PlayAudio("audio/sfx/guide-appears");
                }
                if (playGuideSuccess)
                {
                    AudioPlayer.PlayAudio("audio/sfx/guide-success");
                }
                if (playCollectedItem)
                {
                    AudioPlayer.PlayAudio("audio/sfx/collected-item");
                }
            }
        }
    }

    public override void Reset()
    {
        base.Reset();

        ShareManager sm = (ShareManager)gameManager_;
        if (sm != null)
        {
            transform.SetParent(sm.OverlayParent);
        }
    }
}