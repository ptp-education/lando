using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnedHint : SpawnedObject
{
    [SerializeField] private Image hint_;
    [SerializeField] private Sprite platformSprite_;
    [SerializeField] private Sprite piersSprite_;
    [SerializeField] private Sprite reinforceSprite_;

    public override void ReceivedAction(string action)
    {
        base.ReceivedAction(action);

        if (action.Contains("hint-platform"))
        {
            hint_.sprite = platformSprite_;
        } else if (action.Contains("hint-piers"))
        {
            hint_.sprite = piersSprite_;
        }
        else if (action.Contains("hint-reinforce"))
        {
            hint_.sprite = reinforceSprite_;
        }
    }

    public override void Reset()
    {
        base.Reset();

        ShareManager sm = (ShareManager)gameManager_;
        if (sm != null)
        {
            transform.SetParent(sm.OverlayParent);
        }
    }

    private void Update()
    {
        transform.SetAsLastSibling();
    }
}
