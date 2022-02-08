using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnedLumberTrucksBridge : SpawnedObject
{
    [SerializeField] private Image lumberTruckPrefab_;
    [SerializeField] private Transform truckParent_;

    public override void ReceivedAction(string action)
    {
        base.ReceivedAction(action);

        if (action.Contains("new-truck"))
        {
            Image spawnedTruck = GameObject.Instantiate<Image>(lumberTruckPrefab_, truckParent_);
            spawnedTruck.transform.localPosition = new Vector3(1140f, -297, 0f);
            AudioPlayer.PlayAudio("audio/sfx/car-starting");

            GoTweenFlow flow = new GoTweenFlow();
            flow.insert(0f, new GoTween(spawnedTruck.transform, 1.5f, new GoTweenConfig().vector3Prop("localPosition", new Vector3(-504f, 207f, 0f))));
            flow.insert(1.5f, new GoTween(spawnedTruck.transform, 0.35f, new GoTweenConfig().vector3Prop("localScale", Vector3.zero).onComplete(t =>
            {
                gameManager_.SendNewAction("improve-house");
            })));
            flow.play();
        }
    }

    public override void Reset()
    {
        base.Reset();

        foreach(Image i in truckParent_.GetComponentsInChildren<Image>())
        {
            Destroy(i.gameObject);
        }
    }
}
