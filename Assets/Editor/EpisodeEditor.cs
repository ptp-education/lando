using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

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

        List<string> allNodes = new List<string>();
        foreach(EpisodeNode n in myTarget.AllNodes)
        {
            allNodes.Add(n.name);
        }
        if (allNodes.Distinct().Count() != allNodes.Count)
        {
            GUIStyle s = new GUIStyle(EditorStyles.textField);
            s.normal.textColor = Color.red;

            EditorGUILayout.LabelField("Error! You cannot have duplicate node names in an episode.", s);
        }
    }
}
