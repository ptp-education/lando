using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lando.Class.Lego2
{
    public class SpawnedVolunteers : SpawnedObject
    {
        [SerializeField] private List<GameObject> volunteers_ = new List<GameObject>();
        private void Start()
        {
            foreach(GameObject v in volunteers_)
            {
                v.gameObject.SetActive(false);
            }

            GoTweenFlow flow = new GoTweenFlow();

            float time = 0.25f;
            foreach(GameObject v in volunteers_)
            {
                GameObject volunteer = v;
                flow.insert(time, new GoTween(this, 0.01f, new GoTweenConfig().onComplete(t =>
                {
                    v.gameObject.SetActive(true);
                })));

                time += 0.25f;
            }
        }

        public override void ReceivedAction(string action)
        {

        }

        public override void Reset()
        {

        }
    }
}
