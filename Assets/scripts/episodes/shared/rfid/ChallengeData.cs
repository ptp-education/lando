using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChallengeData : MonoBehaviour
{
    [SerializeField] public List<GameStorage.InventoryType> StartingReward = new List<GameStorage.InventoryType>();

    [SerializeField] public List<Challenge> Challenges = new List<Challenge>();
    [SerializeField] public List<Hint> Hints = new List<Hint>();

    [Serializable]
    public class Challenge
    {
        public string Name;
        public Sprite Sprite;   //descriptor of the challenge
        public Sprite CompletedSprite;  //completion badge
        public string ScanRfidCommand;  //Run this when RFID is scanned
        public string FailCommand;      //Fail after testing
        public string RewardCommand;    //Add a wood tower!
        public string RequirementsCommand;  //Your bridge needs to support Nessy and 5 pounds!
        public string EncourageCommand; //override a voice command like "Let's do this!"
        public string NextChallengeCommand; //"the next challenge is 5 pounds!"
        public List<GameStorage.InventoryType> Rewards = new List<GameStorage.InventoryType>();
    }

    [Serializable]
    public class Hint
    {
        public string Name;
        public string CommandToRun; //command to run to trigger a hint
        public string ChallengeRequirement; //note the challenge name that the child has to be at before getting this hint
    }
}
