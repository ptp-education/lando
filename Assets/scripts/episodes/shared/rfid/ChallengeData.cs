using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChallengeData : MonoBehaviour
{
    [SerializeField] public List<Challenge> Challenges = new List<Challenge>();

    [Serializable]
    public class Challenge
    {
        public string Name;
        public Sprite Sprite;   //descriptor of the challenge
        public Sprite CompletedSprite;  //completion badge
        public string LoadCommand;      //Load before testing
        public string FailCommand;      //Fail after testing
        public string RewardCommand;    //Add a wood tower!
        public string RequirementsCommand;  //Your bridge needs to support Nessy and 5 pounds!
        public string EncourageCommand; //override a voice command like "Let's do this!"
        public string NextChallengeCommand; //"the next challenge is 5 pounds!"
    }
}
