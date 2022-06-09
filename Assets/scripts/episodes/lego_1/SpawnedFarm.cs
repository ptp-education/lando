using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SpawnedFarm : SpawnedObject
{
    [SerializeField] List<Image> cows_;
    [SerializeField] List<Image> chickens_;
    [SerializeField] List<Image> pigs_;
    [SerializeField] List<Image> goats_;
    [SerializeField] List<Image> horses_;
    [SerializeField] List<Image> ducks_;
    [SerializeField] List<Image> appleTrees_;
    [SerializeField] List<Image> pearTrees_;
    [SerializeField] List<Image> orangeTrees_;
    [SerializeField] Image cornCrops_;
    [SerializeField] Image wheatCrops_;
    [SerializeField] Image cottonCrops_;
    [SerializeField] Image archBridge_;
    [SerializeField] Image tractor_;
    [SerializeField] Image waterTower_;
    [SerializeField] Image well_;
    [SerializeField] Image dinosaur_;
    [SerializeField] Image elephant_;
    [SerializeField] Image giraffe_;

    private Color unfilledColor_ = Color.clear;
    private Color filledColor_ = Color.white;

    public override void ReceivedAction(string action)
    {
        base.ReceivedAction(action);

        string[] actions = action.Split(' ');

        foreach(string a in actions)
        {
            if (a.Contains("add-farm-"))
            {
                string[] split = a.Split('-');

                string newObject = split[split.Length - 1];
                if (newObject.Length > 0)
                {
                    gameManager_.Storage.AddObjectToList<string>(GameStorage.Key.FarmObjects, newObject);

                    RefreshFarm(newObject);
                }
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
                    AudioPlayer.PlayAudio("audio/sfx/tree-plant");
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
                    AudioPlayer.PlayAudio("audio/sfx/car-starting");
                    break;
                case "horse":
                    AudioPlayer.PlayAudio("audio/sfx/farm-horse");
                    break;
                case "duck":
                    AudioPlayer.PlayAudio("audio/sfx/farm-duck");
                    break;
                case "watertower":
                    AudioPlayer.PlayAudio("audio/sfx/farm-watertower");
                    break;
                case "well":
                    AudioPlayer.PlayAudio("audio/sfx/farm-well");
                    break;
                case "goat":
                    AudioPlayer.PlayAudio("audio/sfx/farm-goat");
                    break;
                case "dinosaur":
                    AudioPlayer.PlayAudio("audio/sfx/dino");
                    break;
                case "giraffe":
                    AudioPlayer.PlayAudio("audio/sfx/giraffe");
                    break;
                case "elephant":
                    AudioPlayer.PlayAudio("audio/sfx/elephant");
                    break;

            }
        }
        //orangetree, corn, cotton, wheat, appletree, peartree, arch, pig, cow, chicken, tractor

        List<string> farmObjects = gameManager_.Storage.GetValue<List<string>>(GameStorage.Key.FarmObjects);
        if (farmObjects == null || farmObjects.Count == 0) return;

        int orangeTrees = farmObjects.FindAll(s => string.Equals("orangetree", s)).Count;   //
        int appleTrees = farmObjects.FindAll(s => string.Equals("appletree", s)).Count;     //
        int pearTrees = farmObjects.FindAll(s => string.Equals("peartree", s)).Count;       //
        int pigs = farmObjects.FindAll(s => string.Equals("pig", s)).Count;     //
        int cows = farmObjects.FindAll(s => string.Equals("cow", s)).Count;     //
        int corn = farmObjects.FindAll(s => string.Equals("corn", s)).Count;    //
        int chickens = farmObjects.FindAll(s => string.Equals("chicken", s)).Count;     //
        int wheat = farmObjects.FindAll(s => string.Equals("wheat", s)).Count;      //
        int cotton = farmObjects.FindAll(s => string.Equals("cotton", s)).Count;    //
        int tractor = farmObjects.FindAll(s => string.Equals("tractor", s)).Count;  //
        int archBridge = farmObjects.FindAll(s => string.Equals("archbridge", s)).Count;    //
        int horses = farmObjects.FindAll(s => string.Equals("horse", s)).Count; //
        int ducks = farmObjects.FindAll(s => string.Equals("duck", s)).Count;   //
        int waterTower = farmObjects.FindAll(s => string.Equals("watertower", s)).Count;    //
        int well = farmObjects.FindAll(s => string.Equals("well", s)).Count;    //
        int goats = farmObjects.FindAll(s => string.Equals("goat", s)).Count;   //
        int dinosaur = farmObjects.FindAll(s => string.Equals("dinosaur", s)).Count;
        int elephant = farmObjects.FindAll(s => string.Equals("elephant", s)).Count;
        int giraffe = farmObjects.FindAll(s => string.Equals("giraffe", s)).Count;

        cornCrops_.color = corn > 0 ? filledColor_ : unfilledColor_;
        wheatCrops_.color = wheat > 0 ? filledColor_ : unfilledColor_;
        cottonCrops_.color = cotton > 0 ? filledColor_ : unfilledColor_;
        tractor_.color = tractor > 0 ? filledColor_ : unfilledColor_;
        archBridge_.color = archBridge > 0 ? filledColor_ : unfilledColor_;
        waterTower_.color = waterTower > 0 ? filledColor_ : unfilledColor_;
        well_.color = well > 0 ? filledColor_ : unfilledColor_;
        dinosaur_.color = dinosaur > 0 ? filledColor_ : unfilledColor_;
        elephant_.color = elephant > 0 ? filledColor_ : unfilledColor_;
        giraffe_.color = giraffe > 0 ? filledColor_ : unfilledColor_;

        for (int i = 0; i < orangeTrees_.Count; i++) orangeTrees_[i].color = orangeTrees > 0 ? filledColor_ : unfilledColor_;
        for (int i = 0; i < appleTrees_.Count; i++) appleTrees_[i].color = appleTrees > 0 ? filledColor_ : unfilledColor_;
        for (int i = 0; i < pearTrees_.Count; i++) pearTrees_[i].color = pearTrees > 0 ? filledColor_ : unfilledColor_;
        for (int i = 0; i < pigs_.Count; i++) pigs_[i].color = pigs > 0 ? filledColor_ : unfilledColor_;
        for (int i = 0; i < cows_.Count; i++) cows_[i].color = cows > 0 ? filledColor_ : unfilledColor_;
        for (int i = 0; i < chickens_.Count; i++) chickens_[i].color = chickens > 0 ? filledColor_ : unfilledColor_;
        for (int i = 0; i < horses_.Count; i++) horses_[i].color = horses > 0 ? filledColor_ : unfilledColor_;
        for (int i = 0; i < ducks_.Count; i++) ducks_[i].color = ducks > 0 ? filledColor_ : unfilledColor_;
        for (int i = 0; i < goats_.Count; i++) goats_[i].color = goats > 0 ? filledColor_ : unfilledColor_;
    }

    public override void Reset()
    {
        base.Reset();

        RefreshFarm();
    }
}
