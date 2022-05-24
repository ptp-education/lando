using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintObject : MonoBehaviour
{
    [SerializeField] private List<Event> events_ = new List<Event>();

    [System.Serializable]
    public class Event
    {
        public enum Type
        {
            Voiceover,
            Print,
            SelfDestruct
        }
        [SerializeField] public float TimeStamp;
        [SerializeField] public Type EventType;
        [SerializeField] public string File;
        [HideInInspector] public bool Ran = false;
    }

    private float timer_;
    private StationManager stationManager_;
    private System.Action onComplete_;

    public void Init(StationManager stationManager, System.Action onComplete)
    {
        stationManager_ = stationManager;
        onComplete_ = onComplete;
    }

    private void Update()
    {
        timer_ += Time.deltaTime;

        foreach(Event e in events_)
        {
            if (!e.Ran)
            {
                if (timer_ >= e.TimeStamp)
                {
                    switch(e.EventType)
                    {
                        case Event.Type.Voiceover:
                            stationManager_.NewVoiceover(e.File);
                            break;
                        case Event.Type.Print:
                            stationManager_.NewPrint(e.File);
                            break;
                        case Event.Type.SelfDestruct:
                            if (onComplete_ != null)
                            {
                                onComplete_();
                            }
                            break;
                    }
                    e.Ran = true;
                }
            }
        }
    }
}
