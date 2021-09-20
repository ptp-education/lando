using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EpisodeNode))]
public class EpisodeNodeEditor : Editor
{
    const string kAssetPrefix = "Assets/StreamingAssets/";

    void OnEnable()
    {

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        SerializedProperty video = serializedObject.FindProperty("Video");
        SerializedProperty videoLoop = serializedObject.FindProperty("VideoLoop");

        EpisodeNode myTarget = (EpisodeNode)target;

        string videoPath = "empty";
        if (video.objectReferenceValue != null)
        {
            videoPath = AssetDatabase.GetAssetPath(video.objectReferenceValue.GetInstanceID());
            myTarget.VideoFilePath = videoPath;
        }
        string videoLoopPath = "empty";
        if (videoLoop.objectReferenceValue != null)
        {
            videoLoopPath = AssetDatabase.GetAssetPath(videoLoop.objectReferenceValue.GetInstanceID());
            myTarget.VideoFilePath = videoLoopPath;
        }

        EditorGUILayout.LabelField(string.Format("Video ({0})", videoPath));
        myTarget.Video = EditorGUILayout.ObjectField(myTarget.Video, typeof(Object), false);

        EditorGUILayout.LabelField(string.Format("Video Loop ({0})", videoLoopPath));
        myTarget.VideoLoop = EditorGUILayout.ObjectField(myTarget.VideoLoop, typeof(Object), false);

        EditorGUILayout.LabelField("Prompt");
        myTarget.Prompt = EditorGUILayout.TextArea(myTarget.Prompt, TextAreaStyle, GUILayout.Width(400), GUILayout.MinHeight(100), GUILayout.MaxHeight(400));

        EditorGUILayout.LabelField("Next Node - leave empty if last episode");
        myTarget.NextNode = (EpisodeNode)EditorGUILayout.ObjectField(myTarget.NextNode, typeof(EpisodeNode), true);

        EditorGUILayout.LabelField("Additional options");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Options"));

        serializedObject.ApplyModifiedProperties();
    }

    private GUIStyle TextAreaStyle
    {
        get
        {
            return new GUIStyle(EditorStyles.textArea)
            {
                wordWrap = true
            };
        }
    }
}
