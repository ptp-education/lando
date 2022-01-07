using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnedColorBridge : SpawnedObject
{
    [SerializeField] private List<Image> bridges_;
    private GoTweenFlow flow_;

    public override void ReceivedAction(string action)
    {
        base.ReceivedAction(action);

        if (action.Contains("bridge-color-"))
        {
            string[] split = action.Split('-');

            int color = -1;
            int.TryParse(split[split.Length - 1], out color);
            if (color > 0)
            {
                GameStorage.Integer chosenColor = new GameStorage.Integer(color);
                gameManager_.Storage.Add<GameStorage.Integer>(GameStorage.Key.ChosenColor, chosenColor);
            }
        }
    }

    public override void Reset()
    {
        base.Reset();

        GameStorage.Integer chosenColor = gameManager_.Storage.GetValue<GameStorage.Integer>(GameStorage.Key.ChosenColor);

        if (chosenColor == null)
        {
            if (flow_ != null)
            {
                flow_.destroy();
                flow_ = null;
            }

            flow_ = new GoTweenFlow(new GoTweenCollectionConfig().setIterations(-1));

            float start = 0f;
            for (int i = 0; i < bridges_.Count; i++)
            {
                for (int ii = 0; ii < bridges_.Count; ii++)
                {
                    float adjustedStartTime = start;
                    if (ii == i) adjustedStartTime -= 0.05f;
   
                    flow_.insert(adjustedStartTime, new GoTween(
                        bridges_[ii].transform,
                        0.01f,
                        new GoTweenConfig().vector3Prop("localScale", ii == i ? Vector3.one : Vector3.zero))
                    );
                }
                start += 1f;
            }
            flow_.play();
        } else
        {
            for (int i = 0; i < bridges_.Count; i++)
            {
                bridges_[i].transform.localScale = i == chosenColor.value ? Vector3.one : Vector3.zero;
            }
        }
    }

    private void OnDestroy()
    {
        if (flow_ != null)
        {
            flow_.destroy();
            flow_ = null;
        }
    }
}
