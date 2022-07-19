using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelData : MonoBehaviour
{
    [SerializeField] public List<GameStorage.ResourceType> StartingResources = new List<GameStorage.ResourceType>();
    [SerializeField] public List<string> StartingHints = new List<string>();
    [SerializeField] public List<Challenge> Challenges = new List<Challenge>();
    [SerializeField] public List<Hint> Hints = new List<Hint>();
    [SerializeField] public List<BeforeTestFail> WaysToFail = new List<BeforeTestFail>();

    [Serializable]
    public class Challenge
    {
        public string Name;
        public Sprite Sprite;   //descriptor of the challenge
        public List<GameStorage.ResourceType> ResourceRewards = new List<GameStorage.ResourceType>();   //resources you get for completing this challenge
        public List<string> HintRewards = new List<string>();   //hints you get for completing this challenge
        public string FailCommand;      //Fail after testing
        public string RewardCommand;    //Add a wood tower!
        public string OnScanCommand;    //Run this on NFC scan   
        public string RequirementsVO;  //Your bridge needs to support Nessy and 5 pounds!
        public string NextChallengeVO; //"the next challenge is 5 pounds!"
    }

    [Serializable]
    public class Hint
    {
        public string Name;
        public Sprite Thumbnail;
        public EventObject ObjectToLoad;
    }

    [Serializable]
    public class BeforeTestFail
    {
        [HideInInspector] public string Name {
            get
            {
                return ButtonName.Replace(' ', '-');
            }
        }
        public string ButtonName;   //text to display on button
        public EventObject ObjectToLoad;
    }
}