//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class OnscreenCharacter : MonoBehaviour
//{
//    private const float SOUND_THRESHOLD = 0.3f;

//    [SerializeField] private GameObject voiceBubble_;
//    [SerializeField] private float speed_;

//    private GoTweenFlow flow_;
//    private Vector3 talkPosition_;
//    private ShareManager shareManager_;

//    public void Init(Vector3 talkPosition, ShareManager sm)
//    {
//        talkPosition_ = talkPosition;
//        shareManager_ = sm;

//        PreMove();
//    }

//    private void Start()
//    {
//        transform.localPosition = new Vector3(-1500f, 750);
//    }

//    public void ToggleVoiceBubble(bool show)
//    {
//        if (show)
//        {
//            if (transform.localPosition != talkPosition_)
//            {
//                MoveToPosition(talkPosition_);
//            }
//        } else
//        {
//            if (!GameManager.ZoneActive)
//            {
//                MoveOffscreen(null);
//            }
//        }
//        voiceBubble_.gameObject.SetActive(show);
//    }

//    public void MoveOnscreen()
//    {
//        MoveToPosition(talkPosition_);
//    }

//    private void PreMove()
//    {
//        float x = talkPosition_.x > 0 ? 1500 : -1500;
//        float y = talkPosition_.y + Random.Range(-50f, 50f);
//        transform.localPosition = new Vector3(x, y, 0f);
//    }

//    public void MoveToPosition(Vector3 destination)
//    {
//        if (flow_ != null)
//        {
//            flow_.complete();
//        }

//        flow_ = new GoTweenFlow();

//        float distance = Mathf.Sqrt(Mathf.Pow(transform.localPosition.x - destination.x, 2) + Mathf.Pow(transform.localPosition.y - destination.y, 2));
//        float duration = distance / speed_;
//        flow_.insert(0f, new GoTween(transform, duration, new GoTweenConfig().setEaseType(GoEaseType.QuintInOut).localPosition(destination)));

//        if (duration > SOUND_THRESHOLD)
//        {
//            flow_.insert(0.2f, new GoTween(transform, 0.01f, new GoTweenConfig().onComplete(t =>
//            {
//                AudioPlayer.PlayAudio("audio/sfx/whoosh");
//            })));
//        }

//        flow_.play();
//    }

//    public void MoveOffscreen(System.Action callback, float overrideDuration = -1f)
//    {
//        if (flow_ != null)
//        {
//            flow_.complete();
//        }
//        flow_ = new GoTweenFlow();

//        float xDest = transform.localPosition.x > 0 ? 1500 : -1500;
//        float yDest = transform.localPosition.y + Random.Range(-50f, 50f);

//        float duration = overrideDuration;
//        if (duration == -1f)
//        {
//            float distance = Mathf.Sqrt(Mathf.Pow(transform.localPosition.x - xDest, 2) + Mathf.Pow(transform.localPosition.y - yDest, 2));
//            duration = distance / speed_;
//        }
//        flow_.insert(0f, new GoTween(transform, duration, new GoTweenConfig().setEaseType(GoEaseType.QuintInOut).localPosition(new Vector3(xDest, yDest, 0f)).onComplete(t =>
//        {
//            if (callback != null)
//            {
//                callback();
//            }
//        })));

//        if (duration > SOUND_THRESHOLD)
//        {
//            flow_.insert(0.2f, new GoTween(transform, 0.01f, new GoTweenConfig().onComplete(t =>
//            {
//                AudioPlayer.PlayAudio("audio/sfx/whoosh");
//            })));
//        }

//        flow_.play();
//    }
//}
