using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Episode))]
public class EpisodeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Episode myTarget = (Episode)target;

        if (GUILayout.Button("Visualize"))
        {
            myTarget.Visualize();
        }

        if (GUILayout.Button("Remove Visualize"))
        {
            myTarget.RemoveVisualize();
        }
    }
}
