using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnedSetup : SpawnedObject
{
    [SerializeField] private Animator tableGuide_;
    [SerializeField] private Animator table2Guide_;
    [SerializeField] private Animator legosGuide_;
    [SerializeField] private Animator printerGuide_;
    [SerializeField] private Animator magicPad_;

    public override void ReceivedAction(string action)
    {
        base.ReceivedAction(action);

        List<string> args = ArgumentHelper.ArgumentsFromCommand("-setup", action);

        if (args.Count > 0)
        {
            bool playCollectedItem = false;
            bool playGuideSuccess = false;
            bool playGuideAppears = false;
            switch (args[0])
            {
                case "table":
                    tableGuide_.Play("appear");
                    playGuideAppears = true;
                    break;
                case "two-table":
                    tableGuide_.Play("appear");
                    table2Guide_.Play("appear");
                    playGuideAppears = true;
                    break;
                case "blocks":
                    legosGuide_.Play("appear");
                    playGuideAppears = true;
                    break;
                case "table-and-blocks":
                    tableGuide_.Play("appear");
                    legosGuide_.Play("appear");
                    playGuideAppears = true;
                    break;
                case "two-tables-and-blocks":
                    tableGuide_.Play("appear");
                    table2Guide_.Play("appear");
                    legosGuide_.Play("appear");
                    playGuideAppears = true;
                    break;
                case "complete-table":
                    tableGuide_.Play("complete");
                    playGuideSuccess = true;
                    break;
                case "complete-table2":
                    table2Guide_.Play("complete");
                    playGuideSuccess = true;
                    break;
                case "legos":
                    legosGuide_.Play("appear");
                    playGuideAppears = true;
                    break;
                case "complete-legos":
                    legosGuide_.Play("complete");
                    playGuideSuccess = true;
                    break;
                case "printer":
                    printerGuide_.Play("appear");
                    playGuideAppears = true;
                    break;
                case "complete-printer":
                    printerGuide_.Play("complete");
                    playGuideSuccess = true;
                    break;
                case "magic-pad":
                    magicPad_.Play("appear");
                    playGuideAppears = true;
                    break;
                case "complete-magic-pad":
                    magicPad_.Play("complete");
                    playGuideSuccess = true;
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