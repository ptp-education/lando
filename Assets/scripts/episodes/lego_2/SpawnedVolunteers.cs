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

                float time = 0.25f;
                foreach (GameObject v in volunteers_)
                {
                    GameObject volunteer = v;
                    flow.insert(time, new GoTween(this, 0.01f, new GoTweenConfig().onComplete(t =>
                    {
                        v.gameObject.SetActive(true);
                        AudioPlayer.PlayAudio("audio/sfx/bubble-pop");
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
