using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnTurbine : SpawnedObject
{
    [System.Serializable]
    public class Zone 
    {
        public Image background_;
        public List<GameObject> buildings_;
        public Sprite upgradedBackground_;
    }

    [SerializeField] private Zone zone1_;
    [SerializeField] private Zone zone2_;
    [SerializeField] private Zone zone3_;
    [SerializeField] private Zone zone4_;

    [SerializeField] private Transform electricMeter_;
    [SerializeField] private List<Vector2> zonePositions_;

    private float currentVoltage;
    private float totalVoltage;

    private int currentLevel_;

    private int currentZone_ = 0;

    private void Start()
    {
        VoltageNeeded();
    }

    public override void ReceivedAction(string action)
    {
        List<string> args_ = ArgumentHelper.ArgumentsFromCommand("-turbine", action);

        if (args_.Count == 0) return;

        int.TryParse(args_[1], out currentLevel_);

        if (args_.Contains("success")) 
        {
            gameManager_.SendNewActionInternal("-update-options empty");
            FIllVoltage();
        }
    }

    private void FIllVoltage()
    {
        switch (currentLevel_) 
        {
            case 0:
                currentVoltage += 2400;
                break;
            case 1:
                currentVoltage += 2800;
                break;
            case 2:
                currentVoltage += 3200;
                break;
            case 3:
                currentVoltage += 3600;
                break;
            case 4:
                currentVoltage += 4000;
                break;
        }
        FillMeter();
        if (currentVoltage >= totalVoltage)
        {
            UpgradeZone();
        }
        else 
        {
            gameManager_.SendNewActionInternal("-update-options default");
        }
    }

    private void FillMeter() 
    {
        float voltage = currentVoltage / totalVoltage;

        GoTweenFlow flow = new GoTweenFlow();
        flow.insert(0f, new GoTween(electricMeter_, 0.25f, new GoTweenConfig().scale(new Vector3(1, voltage,1))));
        flow.play();

        AudioPlayer.PlayAudio("audio/sfx/levelup");

    }

    private void UpgradeZone()
    {
        switch (currentZone_) 
        {
            case 0:
                //Zoom in
                AudioPlayer.PlayAudio("audio/sfx/whoosh");
                zone1_.background_.sprite = zone1_.upgradedBackground_;
                GoTweenFlow zone1Flow = new GoTweenFlow();
                zone1Flow.insert(0f, new GoTween(this.transform, 0.5f, new GoTweenConfig().scale(1.7f)));
                zone1Flow.insert(0f, new GoTween(this.transform.GetComponent<RectTransform>(), 0.5f, new GoTweenConfig().anchoredPosition(zonePositions_[0])));
                zone1Flow.insert(0f, new GoTween(electricMeter_, 1f, new GoTweenConfig().scale(new Vector3(1, 0, 1))));
                zone1Flow.insert(0.5f, new GoTween(zone1_.buildings_[0].transform, 0.15f, new GoTweenConfig().scale(1).onComplete(t => { AudioPlayer.PlayAudio("audio/sfx/pop"); })));
                zone1Flow.insert(.65f, new GoTween(zone1_.buildings_[1].transform, 0.15f, new GoTweenConfig().scale(1).onComplete(t => { AudioPlayer.PlayAudio("audio/sfx/pop"); })));
                zone1Flow.insert(.80f, new GoTween(zone1_.buildings_[2].transform, 0.15f, new GoTweenConfig().scale(1).onComplete(t => { AudioPlayer.PlayAudio("audio/sfx/pop"); })));
                zone1Flow.insert(.95f, new GoTween(zone1_.buildings_[3].transform, 0.15f, new GoTweenConfig().scale(1).onComplete(t => { AudioPlayer.PlayAudio("audio/sfx/pop"); })));
                zone1Flow.insert(1.1f, new GoTween(zone1_.buildings_[4].transform, 0.15f, new GoTweenConfig().scale(1).onComplete(t => { AudioPlayer.PlayAudio("audio/sfx/pop"); })));
                zone1Flow.insert(1.25f, new GoTween(zone1_.buildings_[5].transform, 0.15f, new GoTweenConfig().scale(1).onComplete(t => { AudioPlayer.PlayAudio("audio/sfx/pop"); })));
                zone1Flow.play();

                AudioPlayer.PlayAudio("audio/sfx/cheer");
                Go.to(this, 3f, new GoTweenConfig().onComplete(t => {
                    AudioPlayer.PlayAudio("audio/sfx/whoosh");
                    GoTweenFlow flow = new GoTweenFlow();
                    flow.insert(0f, new GoTween(this.transform, 0.25f, new GoTweenConfig().scale(1)));
                    flow.insert(0f, new GoTween(this.gameObject.GetComponent<RectTransform>(), 0.25f, new GoTweenConfig().anchoredPosition(Vector2.zero)));
                    flow.play();
                    currentZone_++;
                    currentVoltage = 0;
                    zone2_.background_.color = Color.white;
                    VoltageNeeded();
                }));
                break;
            case 1:
                AudioPlayer.PlayAudio("audio/sfx/whoosh");
                zone2_.background_.sprite = zone2_.upgradedBackground_;
                GoTweenFlow zone2Flow = new GoTweenFlow();
                zone2Flow.insert(0f, new GoTween(this.transform, 0.5f, new GoTweenConfig().scale(1.7f)));
                zone2Flow.insert(0f, new GoTween(this.transform.GetComponent<RectTransform>(), 0.5f, new GoTweenConfig().anchoredPosition(zonePositions_[1])));
                    
                zone2Flow.insert(0.5f, new GoTween(zone2_.buildings_[0].transform, 0.15f, new GoTweenConfig().scale(1).onComplete(t => { AudioPlayer.PlayAudio("audio/sfx/pop"); })));
                zone2Flow.insert(.65f, new GoTween(zone2_.buildings_[1].transform, 0.15f, new GoTweenConfig().scale(1).onComplete(t => { AudioPlayer.PlayAudio("audio/sfx/pop"); })));
                zone2Flow.insert(.80f, new GoTween(zone2_.buildings_[2].transform, 0.15f, new GoTweenConfig().scale(1).onComplete(t => { AudioPlayer.PlayAudio("audio/sfx/pop"); })));
                zone2Flow.insert(.95f, new GoTween(zone2_.buildings_[3].transform, 0.15f, new GoTweenConfig().scale(1).onComplete(t => { AudioPlayer.PlayAudio("audio/sfx/pop"); })));
                zone2Flow.insert(1.1f, new GoTween(zone2_.buildings_[4].transform, 0.15f, new GoTweenConfig().scale(1).onComplete(t => { AudioPlayer.PlayAudio("audio/sfx/pop"); })));
                zone2Flow.insert(0f, new GoTween(electricMeter_, 1f, new GoTweenConfig().scale(new Vector3(1, 0, 1))));
                zone2Flow.play();

                AudioPlayer.PlayAudio("audio/sfx/cheer");
                Go.to(this, 3f, new GoTweenConfig().onComplete(t => {
                    AudioPlayer.PlayAudio("audio/sfx/whoosh");
                    GoTweenFlow flow = new GoTweenFlow();
                    flow.insert(0f, new GoTween(this.transform, 0.25f, new GoTweenConfig().scale(1)));
                    flow.insert(0f, new GoTween(this.gameObject.GetComponent<RectTransform>(), 0.25f, new GoTweenConfig().anchoredPosition(Vector2.zero)));
                    flow.play();
                    currentZone_++;
                    zone3_.background_.color = Color.white;
                    currentVoltage = 0;
                    VoltageNeeded();
                }));
                break;
            case 2:
                AudioPlayer.PlayAudio("audio/sfx/whoosh");
                zone3_.background_.sprite = zone3_.upgradedBackground_;
                GoTweenFlow zone3Flow = new GoTweenFlow();
                zone3Flow.insert(0f, new GoTween(this.transform, 0.5f, new GoTweenConfig().scale(1.7f)));
                zone3Flow.insert(0f, new GoTween(this.transform.GetComponent<RectTransform>(), 0.5f, new GoTweenConfig().anchoredPosition(zonePositions_[2])));
                    
                zone3Flow.insert(0.5f, new GoTween(zone3_.buildings_[0].transform, 0.15f, new GoTweenConfig().scale(1).onComplete(t => { AudioPlayer.PlayAudio("audio/sfx/pop"); })));
                zone3Flow.insert(.65f, new GoTween(zone3_.buildings_[1].transform, 0.15f, new GoTweenConfig().scale(1).onComplete(t => { AudioPlayer.PlayAudio("audio/sfx/pop"); })));
                zone3Flow.insert(.80f, new GoTween(zone3_.buildings_[2].transform, 0.15f, new GoTweenConfig().scale(1).onComplete(t => { AudioPlayer.PlayAudio("audio/sfx/pop"); })));
                zone3Flow.insert(0f, new GoTween(electricMeter_, 1f, new GoTweenConfig().scale(new Vector3(1, 0, 1))));
                zone3Flow.play();

                AudioPlayer.PlayAudio("audio/sfx/cheer");
                Go.to(this, 3f, new GoTweenConfig().onComplete(t => {
                    AudioPlayer.PlayAudio("audio/sfx/whoosh");
                    GoTweenFlow flow = new GoTweenFlow();
                    flow.insert(0f, new GoTween(this.transform, 0.25f, new GoTweenConfig().scale(1)));
                    flow.insert(0f, new GoTween(this.gameObject.GetComponent<RectTransform>(), 0.25f, new GoTweenConfig().anchoredPosition(Vector2.zero)));
                    flow.play();
                    currentZone_++;
                    zone4_.background_.color = Color.white;
                    currentVoltage = 0;
                    VoltageNeeded();
                }));
                break;
            case 3:
                AudioPlayer.PlayAudio("audio/sfx/whoosh");
                zone4_.background_.sprite = zone4_.upgradedBackground_;
                GoTweenFlow zone4Flow = new GoTweenFlow();
                zone4Flow.insert(0f, new GoTween(this.transform, 0.5f, new GoTweenConfig().scale(1.7f)));
                zone4Flow.insert(0f, new GoTween(this.transform.GetComponent<RectTransform>(), 0.5f, new GoTweenConfig().anchoredPosition(zonePositions_[3])));
                    
                zone4Flow.insert(0.5f, new GoTween(zone4_.buildings_[0].transform, 0.15f, new GoTweenConfig().scale(1).onComplete(t => { AudioPlayer.PlayAudio("audio/sfx/pop"); })));
                zone4Flow.insert(.65f, new GoTween(zone4_.buildings_[1].transform, 0.15f, new GoTweenConfig().scale(1).onComplete(t => { AudioPlayer.PlayAudio("audio/sfx/pop"); })));
                zone4Flow.insert(.80f, new GoTween(zone4_.buildings_[2].transform, 0.15f, new GoTweenConfig().scale(1).onComplete(t => { AudioPlayer.PlayAudio("audio/sfx/pop"); })));
                zone4Flow.insert(.95f, new GoTween(zone4_.buildings_[3].transform, 0.15f, new GoTweenConfig().scale(1).onComplete(t => { AudioPlayer.PlayAudio("audio/sfx/pop"); })));
                zone4Flow.insert(1.1f, new GoTween(zone4_.buildings_[4].transform, 0.15f, new GoTweenConfig().scale(1).onComplete(t => { AudioPlayer.PlayAudio("audio/sfx/pop"); })));
                zone4Flow.play();

                AudioPlayer.PlayAudio("audio/sfx/cheer");
                Go.to(this, 3f, new GoTweenConfig().onComplete(t => {
                    AudioPlayer.PlayAudio("audio/sfx/whoosh");
                    GoTweenFlow flow = new GoTweenFlow();
                    flow.insert(0f, new GoTween(this.transform, 0.25f, new GoTweenConfig().scale(1)));
                    flow.insert(0f, new GoTween(this.gameObject.GetComponent<RectTransform>(), 0.25f, new GoTweenConfig().anchoredPosition(Vector2.zero)));
                    flow.play();
                }));
                break;
        }

        gameManager_.SendNewActionInternal("-update-options default");
    }

    private void VoltageNeeded() 
    {
        switch (currentZone_) 
        {
            case 0:
                totalVoltage = 16000;
                break;
            case 1:
                totalVoltage = 32000;
                break;
            case 2:
                totalVoltage = 48000;
                break;
            case 3:
                totalVoltage = 64000;
                break;
        }
    }
}
