using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelData : MonoBehaviour
{
    [SerializeField] public List<GameStorage.ResourceType> StartingResources = new List<GameStorage.ResourceType>();
    [SerializeField] public List<string> StartingHints = new List<string>();
    [SerializeField] public Hint HowToPlay; //we show this hint to everyone at the start of class, to set up the class
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
        public string ScanRfidCommand;  //Run this when RFID is scanned
        public string FailCommand;      //Fail after testing
        public string RewardCommand;    //Add a wood tower!
        public string RequirementsCommand;  //Your bridge needs to support Nessy and 5 pounds!
        public string NextChallengeCommand; //"the next challenge is 5 pounds!"
        public string EncourageCommand; //override a voice command like "Let's do this!"
    }

    [Serializable]
    public class Hint
    {
        public string Name;
        public Sprite Thumbnail;
        public string Command;
    }

    [Serializable]
    public class BeforeTestFail
    {
        public string Name;
        public string ButtonName;   //text to display on button
        public string Command;
    }
}