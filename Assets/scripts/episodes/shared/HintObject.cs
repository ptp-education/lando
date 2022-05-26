using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class HintObject : MonoBehaviour
{
    [SerializeField] private List<Event> events_ = new List<Event>();
    [SerializeField] private Image image_;

    [System.Serializable]
    public class Event
    {
        [SerializeField] public float TimeStamp;
        [SerializeField] public string VoiceoverFile;
        [SerializeField] public string PrintFile;
        [SerializeField] public Sprite ImageToSet;
        [SerializeField] public bool SelfDestruct;
        [HideInInspector] public bool Ran = false;
    }

    private static float kMaxRunLength = 600f;

    private float timer_;
    private StationManager stationManager_;
    private System.Action onComplete_;
    private float lastVoiceoverCompleteTime_ = 0f;
    private bool autoSelfDestruct_;
    private bool waitingToSelfDestruct_;

    private void Start()
    {
        if (image_ != null)
        {
            image_.color = Color.clear;
        }
    }

    public void Init(StationManager stationManager, System.Action onComplete)
    {
        stationManager_ = stationManager;
        onComplete_ = onComplete;
    }

    private void Update()
    {
        if (timer_ > kMaxRunLength) return;

        timer_ += Time.deltaTime;

        if (autoSelfDestruct_ && !waitingToSelfDestruct_ && timer_ > lastVoiceoverCompleteTime_) 
        {
            onComplete_();
            waitingToSelfDestruct_ = true;
        }

        foreach(Event e in events_)
        {
            if (!e.Ran)
            {
                if (timer_ >= e.TimeStamp)
                {
                    //we should complete destruct the hint at least 5 seconds after the last event
                    lastVoiceoverCompleteTime_ = Mathf.Max(lastVoiceoverCompleteTime_, timer_ + 5f);

                    if (e.VoiceoverFile != null && e.VoiceoverFile.Length > 0)
                    {
                        float duration = stationManager_.NewVoiceover(e.VoiceoverFile);
                        lastVoiceoverCompleteTime_ = Mathf.Max(lastVoiceoverCompleteTime_, timer_ + duration);
                    }
                    if (e.PrintFile != null && e.PrintFile.Length > 0)
                    {
                        stationManager_.NewPrint(e.PrintFile);
                    }
                    if (e.ImageToSet != null)
                    {
                        image_.color = Color.white;
                        image_.sprite = e.ImageToSet;
                    }
                    if (e.SelfDestruct)
                    {
                        onComplete_();
                    }

                    e.Ran = true;

                    if (e == events_.Last())
                    {
                        autoSelfDestruct_ = events_.Find(e => e.SelfDestruct) == null;
                    }
                }
            }
        }
    }
}
