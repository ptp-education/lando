using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChallengeData : MonoBehaviour
{
    //<challenge-name, extra-allowance-type>
    [SerializeField] public int TotalHints = 0;
    [SerializeField] public string FirstChallenge;
    [SerializeField] public List<Challenge> Challenges = new List<Challenge>();

    [Serializable]
    public class Challenge
    {
        public string Name;
        public Sprite Sprite;
        public string RewardCommand;
        public string ExtraBlocksAllowed;
        public string NextChallengeCommand;
    }
}
