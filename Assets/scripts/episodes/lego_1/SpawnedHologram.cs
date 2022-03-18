using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lando.Class.Lego1
{
    public class SpawnedHologram : SpawnedObject
    {
        private void Start()
        {
            transform.localScale = Vector3.zero;
        }

        public override void ReceivedAction(string action)
        {
            if (ArgumentHelper.ContainsCommand("-show-bridge", action))
            {
                transform.localScale = Vector3.one;
                AudioPlayer.PlayAudio("audio/sfx/buzz-hologram");
            }
        }
    }
}
