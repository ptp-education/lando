using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnedMonster : MonoBehaviour
{
    [SerializeField] private Image woodMonster_;
    [SerializeField] private Image bronzeMonster_;
    [SerializeField] private Image silverMonster_;
    [SerializeField] private Image goldMonster_;
    [SerializeField] private Image obsidianMonster_;

    GoTweenFlow flow_;


    private void Start()
    {
        if (flow_ != null)
        {
            flow_.complete();
        }

        flow_ = new GoTweenFlow(new GoTweenCollectionConfig().setIterations(-1));

        flow_.insert(0f, new GoTween(woodMonster_.transform, 1f,
            new GoTweenConfig().vector3Prop("localPosition", new Vector3(woodMonster_.transform.localPosition.x, woodMonster_.transform.localPosition.y + 10))));
        flow_.insert(1f, new GoTween(woodMonster_.transform, 1f,
            new GoTweenConfig().vector3Prop("localPosition", new Vector3(woodMonster_.transform.localPosition.x, woodMonster_.transform.localPosition.y))));
        flow_.insert(0f, new GoTween(bronzeMonster_.transform, 1f,
            new GoTweenConfig().vector3Prop("localPosition", new Vector3(bronzeMonster_.transform.localPosition.x, bronzeMonster_.transform.localPosition.y + 15))));
        flow_.insert(1f, new GoTween(bronzeMonster_.transform, 1f,
            new GoTweenConfig().vector3Prop("localPosition", new Vector3(bronzeMonster_.transform.localPosition.x, bronzeMonster_.transform.localPosition.y))));
        flow_.insert(0f, new GoTween(silverMonster_.transform, 1f,
            new GoTweenConfig().vector3Prop("localPosition", new Vector3(silverMonster_.transform.localPosition.x, silverMonster_.transform.localPosition.y + 20))));
        flow_.insert(1f, new GoTween(silverMonster_.transform, 1f,
            new GoTweenConfig().vector3Prop("localPosition", new Vector3(silverMonster_.transform.localPosition.x, silverMonster_.transform.localPosition.y))));
        flow_.insert(0f, new GoTween(goldMonster_.transform, 1f,
            new GoTweenConfig().vector3Prop("localPosition", new Vector3(goldMonster_.transform.localPosition.x, goldMonster_.transform.localPosition.y + 25))));
        flow_.insert(1f, new GoTween(goldMonster_.transform, 1f,
            new GoTweenConfig().vector3Prop("localPosition", new Vector3(goldMonster_.transform.localPosition.x, goldMonster_.transform.localPosition.y))));
        flow_.insert(0f, new GoTween(obsidianMonster_.transform, 1f,
            new GoTweenConfig().vector3Prop("localPosition", new Vector3(obsidianMonster_.transform.localPosition.x, obsidianMonster_.transform.localPosition.y + 30))));
        flow_.insert(1f, new GoTween(obsidianMonster_.transform, 1f,
            new GoTweenConfig().vector3Prop("localPosition", new Vector3(obsidianMonster_.transform.localPosition.x, obsidianMonster_.transform.localPosition.y))));

        flow_.play();
    }

    private void OnDestroy()
    {
        if (flow_ != null)
        {
            flow_.destroy();
        }
    }
}
