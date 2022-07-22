using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnMomo : SpawnedObject
{
    //This class is for the momo that is on the screen
    [System.Serializable]
    public class MomoDisplay 
    {
        public Image momo_;
        public Image antenna_;
        public Image tail_;
        public Image spots_;
        public Image claws_;
        public Image mohawk_;
        public Image nose_;
        public Image stripes_;
        public Image whiskers_;
        public Image wings_;
    }

    //This are the sprite that the student have attached to their momo
    [System.Serializable]
    public class Momo 
    {
        public Sprite momo_;
        public Sprite antenna_;
        public Sprite tail_;
        public Sprite spots_;
        public Sprite claws_;
        public Sprite mohawk_;
        public Sprite nose_;
        public Sprite stripes_;
        public Sprite whiskers_;
        public Sprite wings_;
    }

    private string nfcID_;
    private int currentLevel_;
    private string command_;

    public override void ReceivedAction(string action)
    {
        List<string> args_ = ArgumentHelper.ArgumentsFromCommand("-momo", action);
        if (args_.Count == 0) return;

        //-momo success {nfcID} {currentLevel}
        //-momo left {nfcID}

        if (args_.Count > 2) 
        {
            int.TryParse(args_[2], out currentLevel_);
        }

        nfcID_ = args_[1];
        command_ = args_[0];

        if (ArgumentHelper.ContainsCommand("success", command_))
        {
            RewardSequence();
        }
        else if (ArgumentHelper.ContainsCommand("show", command_))
        {
            ShowMomoOnScreen();
        }
        else 
        {
            CustomizeMomo();
        }

    }

    private void CustomizeMomo()
    {
        //Get the options
        //Check the current level and with that see what customization have to attached
    }

    private void ShowMomoOnScreen()
    {
        //Show the neutral momo on screen
    }

    private void ShowMomoEatingBerry() 
    {
        
    }

    private void RewardSequence()
    {
        //Check the current level because level 1 have different behaviour
        //Based on that Show the eating animation or the select momo screen
    }
}
