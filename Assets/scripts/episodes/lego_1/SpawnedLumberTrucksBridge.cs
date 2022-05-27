using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnedLumberTrucksBridge : SpawnedObject
{
    [SerializeField] private Image lumberTruckPrefab_;
    [SerializeField] private Transform truckParent_;

    [SerializeField] private Vector3 challengeOneLumberStart_;
    [SerializeField] private Vector3 challengeOneLumberEnd_;
    [SerializeField] private Vector3 challengeTwoLumberStart_;
    [SerializeField] private Vector3 challengeTwoLumberEnd_;

    private bool nextStage_ = false;

    public override void ReceivedAction(string action)
    {
        base.ReceivedAction(action);

        if (ArgumentHelper.ContainsCommand("-next-stage", action))
        {
            nextStage_ = true;
        }

        List<string> args = ArgumentHelper.ArgumentsFromCommand("-bridge", action);

        if (args.Count > 0)
        {
            switch (args[0])
            {
                case "send-truck":
                    if (args.Count > 1)
                    {
                        SendMultipleTrucks(int.Parse(args[1]));
                    }
                    break;
            }
        }
    }

    private void SendMultipleTrucks(int trucks)
    {
        DestroyAllTrucks();

        GoTweenFlow flow = new GoTweenFlow();

        float time = 2f;
        for (int i = 0; i < trucks; i++)
        {
            flow.insert(time, new GoTween(transform, Random.Range(0.3f, 0.6f), new GoTweenConfig().onComplete(t =>
            {
                SendTruck();
            })));
            time += 1f;
        }
        flow.insert(time + 1f, new GoTween(transform, 0.5f, new GoTweenConfig().onComplete(t =>
        {
            Debug.Log("rewards finished");
        })));
        flow.play();
    }

    private void SendTruck()
    {
        Image truck = GameObject.Instantiate<Image>(lumberTruckPrefab_, truckParent_);
        truck.transform.localPosition = nextStage_ ? challengeTwoLumberStart_ :  challengeOneLumberStart_;
        AudioPlayer.PlayAudio("audio/sfx/car-starting");

        GoTweenFlow flow = new GoTweenFlow();
        flow.insert(0f, new GoTween(truck.transform, 1.5f, new GoTweenConfig().localPosition(nextStage_ ? challengeTwoLumberEnd_ : challengeOneLumberEnd_).setEaseType(GoEaseType.QuintInOut).onComplete(t =>
        {
            gameManager_.SendNewActionInternal("-farmhouse-increase");
        })));
        flow.insert(1.5f, new GoTween(truck, 0.25f, new GoTweenConfig().colorProp("color", Color.clear).onComplete(t =>
        {
            Destroy(truck.gameObject);
        })));
        flow.play();
    }

    private void DestroyAllTrucks()
    {
        foreach (Image i in truckParent_.GetComponentsInChildren<Image>())
        {
            Destroy(i.gameObject);
        }
    }

}
