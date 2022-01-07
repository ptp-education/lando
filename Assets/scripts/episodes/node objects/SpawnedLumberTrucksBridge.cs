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
            spawnedTruck.transform.localPosition = new Vector3(1109f, -699f, 0f);
            AudioPlayer.PlayAudio("audio/sfx/car-starting");
            Go.addTween(new GoTween(spawnedTruck.transform, 2f, new GoTweenConfig().vector3Prop("localPosition", new Vector3(-469f, 694f, 0f)).onComplete(t =>
            {
                gameManager_.NewNodeAction(GameManager.ACTION_PREFIX + "increase-counter");
            })));
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
