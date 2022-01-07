using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SpawnedLumberTrucksMap : SpawnedObject
{
    [SerializeField] Image lumberTruckPrefab_;
    [SerializeField] private Transform truckParent_;

    private List<Image> trucks_ = new List<Image>();

    private GoTweenFlow flow_;

    public override void ReceivedAction(string action)
    {
        base.ReceivedAction(action);

        if (action.Contains("start-construction"))
        {
            if (flow_ != null)
            {
                flow_.destroy();
                flow_ = null;
            }

            flow_ = new GoTweenFlow();

            float startTime = 0f;
            Vector3 firstDestination = new Vector3(240, -44, 0f);
            Vector3 secondDestination = new Vector3(-629, 152, 0f);

            for (int i = 0; i < trucks_.Count; i++)
            {
                float distance = (firstDestination - trucks_[i].transform.localPosition).magnitude;
                float travelTime = distance / 300f;
                int truckCounter = i;
                flow_.insert(
                    startTime,
                    new GoTween(trucks_[truckCounter].transform,
                    travelTime,
                    new GoTweenConfig().vector3Prop("localPosition", firstDestination).onBegin(t =>
                    {
                        trucks_[truckCounter].rectTransform.eulerAngles = new Vector3(0f, 0f, -15);
                    }
                )));
                flow_.insert(
                    startTime + travelTime,
                    new GoTween(trucks_[truckCounter].transform,
                    2f,
                    new GoTweenConfig().vector3Prop("localPosition", secondDestination).onComplete(t =>
                    {
                        gameManager_.NewNodeAction(GameManager.ACTION_PREFIX + "improve-house");
                    }
                )));
                flow_.insert(
                    startTime + travelTime + 2f,
                    new GoTween(trucks_[truckCounter].transform,
                    0.15f,
                    new GoTweenConfig().vector3Prop("localScale", Vector3.zero)
                ));
                startTime += 0.5f;
            }

            flow_.play();
        }
    }

    public override void Reset()
    {
        base.Reset();

        if (flow_ != null)
        {
            flow_.destroy();
            flow_ = null;
        }

        foreach(Image i in truckParent_.GetComponentsInChildren<Image>())
        {
            Destroy(i.gameObject);
        }

        GameStorage.Integer totalTrucks = gameManager_.Storage.GetValue<GameStorage.Integer>(GameStorage.Key.Counter);

        flow_ = new GoTweenFlow(new GoTweenCollectionConfig().setIterations(-1));
        for (int i = 0; i <= totalTrucks.value; i++)
        {
            Image spawnedTruck = GameObject.Instantiate<Image>(lumberTruckPrefab_, truckParent_);
            trucks_.Add(spawnedTruck);

            float startingX = 318f;
            float startingY = -6f;

            spawnedTruck.transform.localPosition = new Vector3(startingX + i * 70f, startingY + i * 80f, 0f);
            flow_.insert(0f, new GoTween(spawnedTruck.transform, 0.15f, new GoTweenConfig().vector3Prop("localScale", new Vector3(0.405f, 0.405f, 1f))));
            flow_.insert(0.15f, new GoTween(spawnedTruck.transform, 0.15f, new GoTweenConfig().vector3Prop("localScale", new Vector3(0.4f, 0.4f, 1f))));
            flow_.play();
        }
    }

    private void OnDestroy()
    {
        if (flow_ != null)
        {
            flow_.destroy();
            flow_ = null;
        }
    }
}
