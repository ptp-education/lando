using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lando.SmartObjects;

public class StationManager : GameManager
{
    [SerializeField] protected SmartObjectType StationType;
    [SerializeField] protected Image background_;
    [SerializeField] protected Sprite inactiveSprite_;
    [SerializeField] protected Sprite activeSprite_;

    public string StationName
    {
        get
        {
            return StationType.ToString();
        }
    }

    public virtual float NewVoiceover(string file)
    {
        return AudioPlayer.PlayVoiceover(file, episode_.VORoot);
    }

    public virtual float NewVoiceover(List<string> files)
    {
        return AudioPlayer.PlayVoiceover(files, episode_.VORoot);
    }

    public virtual void NewPrint(string file)
    {
        
    }

    protected override void NewNodeEventInternal(EpisodeNode n)
    {
        base.NewNodeEventInternal(n);

        Reset();
        background_.sprite = IsStationActive ? activeSprite_ : inactiveSprite_;
    }

    protected override void NewActionInternal(string a)
    {
        base.NewActionInternal(a);

        List<string> args = ArgumentHelper.ArgumentsFromCommand("-station", a);
        if (args.Count > 0)
        {
            if (string.Equals(StationType.ToString(), args[0]))
            {
                NewRelevantAction(new List<string>(args.GetRange(1, args.Count - 1)));
            }
        }
    }

    protected virtual bool IsStationActive
    {
        get
        {
            return currentNode_.TestingActive;
        }
    }

    protected virtual void NewRelevantAction(List<string> arguments)
    {
        //stub
    }

    protected virtual void Reset()
    {
        //stub
    }
}
