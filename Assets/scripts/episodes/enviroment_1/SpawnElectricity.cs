using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SpawnElectricity : SpawnedObject
{
    [SerializeField] private Image meter_;

    [SerializeField] private Image zoneBackground1_;
    [SerializeField] private Image zoneBackground2_;
    [SerializeField] private Image zoneBackground3_;
    [SerializeField] private Image zoneBackground4_;

    public float voltage = 0;
    private float totalVoltage;
    private int currentLevel = 0;
    private int currentZone = 0;

    private void Start()
    {
        CheckCurrentZone();
    }

    public override void ReceivedAction(string action)
    {
        List<string> args_ = ArgumentHelper.ArgumentsFromCommand("-electricity", action);
        if (args_.Count == 0) return;
        if (args_.Count > 1) 
        {
            int.TryParse(args_[1], out currentLevel);
        }

        if (ArgumentHelper.ContainsCommand(("success"), args_[0])) 
        {
            AddVoltage();
            gameManager_.SendNewActionInternal("-update-options empty");
        }
    }

    private void AddVoltage()
    {
        float waitTime = 2f;

        switch (currentLevel) 
        {
            case 0:
                voltage += 2000;
                break;
            case 1:
                voltage += 3000;
                break;
            case 2:
                voltage += 10000;
                break;
        }
        
        waitTime = addingVoltageAudio();
        Go.to(this, waitTime, new GoTweenConfig().onComplete(t => { 
            VoltageSequence();
        }));
    }

    private float addingVoltageAudio() 
    {
        int randomAudio = Random.Range(0, 3);
        AudioPlayer.PlayAudio($"audio/enviroment_1/electricity-{currentLevel+1}-{randomAudio}");
        return AudioPlayer.AudioLength($"audio/enviroment_1/electricity-{currentLevel+1}-{randomAudio}") + 0.5f;
    }

    private void VoltageSequence()
    {
        float currentScale = voltage / totalVoltage;
        GoTweenFlow flow = new GoTweenFlow();
        flow.insert(0f, new GoTween(meter_.transform, 0.5f, new GoTweenConfig().scale(new Vector3(1, currentScale, 1))));
        flow.play();
        AudioPlayer.PlayAudio($"audio/sfx/levelup");
        if (voltage >= totalVoltage)
        {
            CheckTotalVoltage();
        }
        else 
        {
            gameManager_.SendNewActionInternal("-update-options default");
        }
    }

    private void CheckTotalVoltage()
    {
        float waitTime = 2f;
        currentZone++;
        CheckCurrentZone();
        voltage = 0;
        AudioPlayer.PlayAudio($"audio/enviroment_1/electricity-{currentZone}-complete");
        waitTime = AudioPlayer.AudioLength($"audio/enviroment_1/electricity-{currentZone}-complete");
        AudioPlayer.PlayAudio($"audio/sfx/buzz-hologram");
        Go.to(this, 1f, new GoTweenConfig().onComplete(t => {
            GoTweenFlow flow = new GoTweenFlow();
            flow.insert(0f, new GoTween(meter_.transform, 0.5f, new GoTweenConfig().scale(new Vector3(1, 0, 1))));
            flow.play();
            Go.to(this, waitTime - 1, new GoTweenConfig().onComplete(t => {
                gameManager_.SendNewActionInternal("-update-options default");
            }));
        }));
    }

    private void CheckCurrentZone() 
    {
        switch (currentZone) 
        {
            case 0:
                totalVoltage = 15000;
                break;
            case 1:
                totalVoltage = 30000;
                zoneBackground1_.color = Color.white;
                break;
            case 2:
                totalVoltage = 45000;
                zoneBackground2_.color = Color.white;
                break;
            case 3:
                totalVoltage = 60000;
                zoneBackground3_.color = Color.white;
                break;
            case 4:
                totalVoltage = 75000;
                zoneBackground4_.color = Color.white;
                break;
        }
    }
}
