using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnedLumberTrucksBridge : SpawnedObject
{
    [SerializeField] private Image lumberTruck2lbPrefab_;
    [SerializeField] private Image lumberTruck5lbPrefab_;
    [SerializeField] private Image lumberTruck10lbPrefab_;
    [SerializeField] private Image lumberTruck15lbPrefab_;
    [SerializeField] private Image lumberTruckDinoPrefab_;
    [SerializeField] private Image lumberTruckGiraffePrefab_;
    [SerializeField] private Image lumberTruckElephantPrefab_;
    [SerializeField] private Transform truckParent_;

    [SerializeField] private Animator bridge_;

    public override void ReceivedAction(string action)
    {
        base.ReceivedAction(action);

        string[] split = action.Split(' ');

        foreach(string a in split)
        {
            string trimmedAction = a.Trim();

            if (string.Equals(trimmedAction, "send-truck"))
            {
                SendTruck();
            }
            else if (string.Equals(trimmedAction, "bridge-collapse"))
            {
                bridge_.Play("bridge-collapse");
                AudioPlayer.PlayAudio("audio/sfx/bridge-crash");
            }
            else if (string.Equals(trimmedAction, "bridge-appear"))
            {
                bridge_.Play("bridge-appear");
            }
            else if (string.Equals(trimmedAction, "bridge-quick-appear"))
            {
                bridge_.Play("bridge-quick-appear");
            }
            else if (string.Equals(trimmedAction, "bridge-thick"))
            {
                bridge_.Play("bridge-thick");
            }
            else
            {
                SpawnTruck(a);
            }
        }
    }

    private void SpawnTruck(string command, bool playSound = true)
    {
        Vector3 spawnLocation = new Vector3(248f, -188, 0f);

        bool validCommand = true;
        switch (command)
        {
            case "spawn-2lb-truck":
                SpawnTruck(lumberTruck2lbPrefab_, new Vector3(281f, -80f, 0f), playSound);
                break;
            case "spawn-5lb-truck":
                SpawnTruck(lumberTruck5lbPrefab_, new Vector3(199f, -115, 0f), playSound);
                break;
            case "spawn-10lb-truck":
                SpawnTruck(lumberTruck10lbPrefab_, new Vector3(146f, -134f, 0f), playSound);
                break;
            case "spawn-15lb-truck":
                SpawnTruck(lumberTruck15lbPrefab_, new Vector3(141f, -144f, 0f), playSound);
                break;
            case "spawn-dino-truck":
                SpawnTruck(lumberTruckDinoPrefab_, new Vector3(106, -123, 0), false);
                if (playSound) AudioPlayer.PlayAudio("audio/sfx/dino");
                if (playSound) AudioPlayer.PlayAudio("audio/sfx/car-starting");
                break;
            case "spawn-elephant-truck":
                SpawnTruck(lumberTruckElephantPrefab_, new Vector3(142, -100, 0), false);
                if (playSound) AudioPlayer.PlayAudio("audio/sfx/elephant");
                if (playSound) AudioPlayer.PlayAudio("audio/sfx/car-starting");
                break;
            case "spawn-giraffe-truck":
                SpawnTruck(lumberTruckGiraffePrefab_, new Vector3(92, -157, 0f), false);
                if (playSound) AudioPlayer.PlayAudio("audio/sfx/giraffe");
                if (playSound) AudioPlayer.PlayAudio("audio/sfx/car-starting");
                break;
            default:
                validCommand = false;
                break;
        }
        if (validCommand)
        {
            gameManager_.Storage.Add<string>(GameStorage.Key.SpawnedTruck, command);
        }
    }

    private void SpawnTruck(Image prefab, Vector3 spawnLocation, bool playSound)
    {
        DestroyAllTrucks();

        Image spawnedTruck = GameObject.Instantiate<Image>(prefab, truckParent_);

        spawnedTruck.transform.localPosition = spawnLocation;

        if (playSound)
        {
            AudioPlayer.PlayAudio("audio/sfx/car-starting");
        }
    }

    private void SendTruck()
    {
        Transform spawnedTruck = truckParent_.GetChild(0);
        if (spawnedTruck == null)
        {
            Debug.LogWarning("Tried to send truck without a spawned truck.");
            return;
        }

        gameManager_.Storage.Add<string>(GameStorage.Key.SpawnedTruck, null);

        string command = null;

        GoTweenFlow flow = new GoTweenFlow();

        string spawnedTruckName = spawnedTruck.name.Replace("(Clone)", "");

        if (string.Equals(spawnedTruckName, lumberTruck2lbPrefab_.name)) {
            command = "truck-add-2lb";
        }
        else if (string.Equals(spawnedTruckName, lumberTruck5lbPrefab_.name)) {
            command = "truck-add-5lb";
        }
        else if (string.Equals(spawnedTruckName, lumberTruck10lbPrefab_.name)) {
            command = "truck-add-10lb";
        }
        else if (string.Equals(spawnedTruckName, lumberTruck15lbPrefab_.name)) {
            command = "truck-add-15lb";
        }
        else if (string.Equals(spawnedTruckName, lumberTruckDinoPrefab_.name)) {
            command = "truck-add-dino";
        }
        else if (string.Equals(spawnedTruckName, lumberTruckGiraffePrefab_.name)) {
            command = "truck-add-giraffe";
        }
        else if (string.Equals(spawnedTruckName, lumberTruckElephantPrefab_.name)) {
            command = "truck-add-elephant";
        }

        flow.insert(0f, new GoTween(spawnedTruck.transform, 1.5f, new GoTweenConfig().vector3Prop("localPosition", new Vector3(-504f, 207f, 0f))));
        flow.insert(1.5f, new GoTween(spawnedTruck.transform, 0.35f, new GoTweenConfig().vector3Prop("localScale", Vector3.zero).onComplete(t =>
        {
            if (command != null)
            {
                gameManager_.SendNewAction(command);
            }
        })));
        flow.play();
    }

    public override void Reset()
    {
        base.Reset();

        string spawnedTruck = gameManager_.Storage.GetValue<string>(GameStorage.Key.SpawnedTruck);
        if (spawnedTruck != null && spawnedTruck.Length > 0)
        {
            SpawnTruck(spawnedTruck, false);
        }
    }

    private void DestroyAllTrucks()
    {
        foreach (Image i in truckParent_.GetComponentsInChildren<Image>())
        {
            Destroy(i.gameObject);
        }
    }

    private void Update()
    {
        transform.SetAsLastSibling();
    }
}
