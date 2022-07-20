using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class EventObject : MonoBehaviour
{
    [SerializeField] private List<Event> events_ = new List<Event>();
    [SerializeField] private Image image_;

    [System.Serializable]
    public class Event
    {
        [SerializeField] public float TimeStamp;
        [SerializeField] public string VoiceoverFile;
        [SerializeField] public string CommandLine;
        [SerializeField] public string PrintFile;
        [SerializeField] public Sprite ImageToSet;
        [SerializeField] public bool SelfDestruct;
        [HideInInspector] public bool Ran = false;
    }

    public enum Type
    {
        Projector,
        iPad
    }

    private static float kMaxRunLength = 600f;

    private Type type_;
    private float timer_;
    private GameManager gameManager_;
    private System.Action onComplete_;
    private float lastVoiceoverCompleteTime_ = 0f;
    private bool autoSelfDestruct_;
    private bool waitingToSelfDestruct_;

    private bool dontDestroy_ = false;

    private void Start()
    {
        if (image_ != null)
        {
            image_.color = Color.clear;
        }
    }

    public void Init(Type type, GameManager gameManager, System.Action onComplete, bool dontDestroy = false)
    {
        type_ = type;
        gameManager_ = gameManager;
        onComplete_ = onComplete;
        dontDestroy_ = dontDestroy;
    }

    private void SelfDestruct()
    {
        if (onComplete_ != null)
        {
            onComplete_();
        }

        if (!dontDestroy_)
        {
            Destroy(this.gameObject);
        }

        waitingToSelfDestruct_ = true;
    }

    private void Update()
    {
        if (timer_ > kMaxRunLength) return;

        if (waitingToSelfDestruct_) return;

        timer_ += Time.deltaTime;

        if (autoSelfDestruct_ && !waitingToSelfDestruct_ && timer_ > lastVoiceoverCompleteTime_) 
        {
            SelfDestruct();
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
                        float duration = gameManager_.NewVoiceover(e.VoiceoverFile);
                        lastVoiceoverCompleteTime_ = Mathf.Max(lastVoiceoverCompleteTime_, timer_ + duration);
                    }
                    if (e.PrintFile != null && e.PrintFile.Length > 0)
                    {
                        gameManager_.SendNewActionNetworked(GameManager.PRINT_COMMAND + " " + e.PrintFile);
                        Go.addTween(new GoTween(transform, 1f, new GoTweenConfig().onComplete(t =>
                        {
                            AudioPlayer.PlayPrint();
                        })));
                    }
                    if (e.ImageToSet != null)
                    {
                        image_.color = Color.white;
                        image_.sprite = e.ImageToSet;
                        AudioPlayer.PlaySfx("bubble-pop");

                        switch(type_)
                        {
                            case Type.iPad:
                                image_.rectTransform.sizeDelta = new Vector2(1024, 768);
                                break;
                            case Type.Projector:
                                image_.rectTransform.sizeDelta = new Vector2(1920, 1080);
                                break;
                        }
                    }
                    if (e.CommandLine != null && e.CommandLine.Length > 0)
                    {
                        gameManager_.SendNewActionInternal(e.CommandLine);
                    }
                    if (e.SelfDestruct)
                    {
                        SelfDestruct();
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
