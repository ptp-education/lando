using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lando.Class.Lego2
{
    public class SpawnedVolunteers : SpawnedObject
    {
        [SerializeField] private List<GameObject> volunteers_ = new List<GameObject>();

        public override void ReceivedAction(string action)
        {
            if (ArgumentHelper.ContainsCommand("-showVolunteers", action))
            {
                GoTweenFlow flow = new GoTweenFlow();

                float time = 0f;
                foreach (GameObject v in volunteers_)
                {
                    GameObject volunteer = v;
                    float timeCopy = time;
                    Debug.LogWarning(timeCopy.ToString());
                    flow.insert(time, new GoTween(this, 0.1f, new GoTweenConfig().onComplete(t =>
                    {
                        v.gameObject.SetActive(true);
                        AudioPlayer.PlayAudio("audio/sfx/bubble-pop");
                        Debug.LogWarning(timeCopy.ToString());
                    })));

                    time += 0.25f;
                }
                flow.play();
            }
        }

        private void Start()
        {

        }

        public override void Reset()
        {

        }
    }
}
