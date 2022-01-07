using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SpawnedFarm : SpawnedObject
{
    [SerializeField] GameObject cows_;
    [SerializeField] GameObject chickens_;
    [SerializeField] GameObject pigs_;
    [SerializeField] GameObject appleTrees_;
    [SerializeField] GameObject pearTrees_;
    [SerializeField] GameObject orangeTrees_;
    [SerializeField] Image cornCrops_;
    [SerializeField] Image wheatCrops_;
    [SerializeField] Image cottonCrops_;
    [SerializeField] Image archBridge_;
    [SerializeField] Image tractor_;


    public override void ReceivedAction(string action)
    {
        base.ReceivedAction(action);

        if (action.Contains("add-farm-"))
        {
            string[] split = action.Split('-');

            string newObject = split[split.Length - 1];
            if (newObject.Length > 0)
            {
                List<string> farmObjects = gameManager_.Storage.GetValue<List<string>>(GameStorage.Key.FarmObjects);
                if (farmObjects == null)
                {
                    farmObjects = new List<string>();
                }
                farmObjects.Add(newObject);
                gameManager_.Storage.Add<List<string>>(GameStorage.Key.FarmObjects, farmObjects);

                RefreshFarm(newObject);
            }
        }
    }

    private void RefreshFarm(string newObject = null)
    {
        if (newObject != null)
        {
            switch (newObject)
            {
                case "orangetree":
                    AudioPlayer.PlayAudio("audio/sfx/tree-rustle");
                    break;
                case "appletree":
                    AudioPlayer.PlayAudio("audio/sfx/tree-plant");
                    break;
                case "peartree":
                    AudioPlayer.PlayAudio("audio/sfx/tree-plant");
                    break;
                case "corn":
                    AudioPlayer.PlayAudio("audio/sfx/crop-rustle");
                    break;
                case "cotton":
                    AudioPlayer.PlayAudio("audio/sfx/crop-rustle");
                    break;
                case "wheat":
                    AudioPlayer.PlayAudio("audio/sfx/crop-rustle");
                    break;
                case "pig":
                    AudioPlayer.PlayAudio("audio/sfx/pig");
                    break;
                case "cow":
                    AudioPlayer.PlayAudio("audio/sfx/cow");
                    break;
                case "chicken":
                    AudioPlayer.PlayAudio("audio/sfx/chicken");
                    break;
                case "arch":
                    AudioPlayer.PlayAudio("audio/sfx/arch");
                    break;
                case "tractor":
                    AudioPlayer.PlayAudio("audio/sfx/tractor");
                    break;
            }
        }
        //orangetree, corn, cotton, wheat, appletree, peartree, arch, pig, cow, chicken, tractor

        List<string> farmObjects = gameManager_.Storage.GetValue<List<string>>(GameStorage.Key.FarmObjects);
        if (farmObjects == null || farmObjects.Count == 0) return;

        int orangeTrees = farmObjects.FindAll(s => string.Equals("orangetree", s)).Count;
        int appleTrees = farmObjects.FindAll(s => string.Equals("appletree", s)).Count;
        int pearTrees = farmObjects.FindAll(s => string.Equals("peartree", s)).Count;
        int pigs = farmObjects.FindAll(s => string.Equals("pig", s)).Count;
        int cows = farmObjects.FindAll(s => string.Equals("cow", s)).Count;
        int corn = farmObjects.FindAll(s => string.Equals("corn", s)).Count;
        int chickens = farmObjects.FindAll(s => string.Equals("chicken", s)).Count;
        int wheat = farmObjects.FindAll(s => string.Equals("wheat", s)).Count;
        int cotton = farmObjects.FindAll(s => string.Equals("cotton", s)).Count;
        int tractor = farmObjects.FindAll(s => string.Equals("tractor", s)).Count;
        int archBridge = farmObjects.FindAll(s => string.Equals("archbridge", s)).Count;

        cornCrops_.gameObject.SetActive(corn > 0);
        wheatCrops_.gameObject.SetActive(wheat > 0);
        cottonCrops_.gameObject.SetActive(cotton > 0);
        tractor_.gameObject.SetActive(tractor > 0);
        archBridge_.gameObject.SetActive(archBridge > 0);
        orangeTrees_.gameObject.SetActive(orangeTrees > 0);
        appleTrees_.gameObject.SetActive(appleTrees > 0);
        pearTrees_.gameObject.SetActive(pearTrees > 0);
        pigs_.gameObject.SetActive(pigs > 0);
        cows_.gameObject.SetActive(cows > 0);
        chickens_.gameObject.SetActive(chickens > 0);

    }

    public override void Reset()
    {
        base.Reset();

        RefreshFarm();
    }
}
