using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabContentTest : PrefabContent
{
    [SerializeField] private Transform objectToMove_;

    public override void ReceiveAction(string action)
    {
        base.ReceiveAction(action);

        if (string.Equals(action, "Left"))
        {
            objectToMove_.localPosition = new Vector3(objectToMove_.localPosition.x + 100, objectToMove_.localPosition.y, objectToMove_.localPosition.z);
        } else if (string.Equals(action, "Right"))
        {
            objectToMove_.localPosition = new Vector3(objectToMove_.localPosition.x - 100, objectToMove_.localPosition.y, objectToMove_.localPosition.z);
        }
    }
}
