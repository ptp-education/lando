using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(EpisodeNode))]
public class StreamingImageEditor : Editor
{

    SerializedProperty videoFilePath;
    SerializedProperty videoFileAsset;
    SerializedProperty loopingFilePath;
    SerializedProperty loopingFileAsset;

    SerializedProperty prompt;
    SerializedProperty nextVideo;
    SerializedProperty options;

    const string kAssetPrefix = "Assets/StreamingAssets/";

    void OnEnable()
    {
        videoFilePath = serializedObject.FindProperty("VideoFilePath");
        videoFileAsset = serializedObject.FindProperty("Video");
        loopingFilePath = serializedObject.FindProperty("VideoLoopFilePath");
        loopingFileAsset = serializedObject.FindProperty("VideoLoop");

        prompt = serializedObject.FindProperty("Prompt");
        nextVideo = serializedObject.FindProperty("NextNode");
        options = serializedObject.FindProperty("Options");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(videoFilePath);
        EditorGUILayout.PropertyField(videoFileAsset);
        EditorGUILayout.PropertyField(loopingFilePath);
        EditorGUILayout.PropertyField(loopingFileAsset);
        EditorGUILayout.PropertyField(prompt, GUILayout.Width(400), GUILayout.Height(400));
        EditorGUILayout.PropertyField(nextVideo);
        EditorGUILayout.PropertyField(options);

        if (videoFileAsset.objectReferenceValue != null)
        {
            string p = AssetDatabase.GetAssetPath(videoFileAsset.objectReferenceValue.GetInstanceID());
            p = p.Substring(kAssetPrefix.Length);
            videoFilePath.stringValue = p;
        }
        if (loopingFileAsset.objectReferenceValue != null)
        {
            string p = AssetDatabase.GetAssetPath(loopingFileAsset.objectReferenceValue.GetInstanceID());
            p = p.Substring(kAssetPrefix.Length);
            loopingFilePath.stringValue = p;
        }
        serializedObject.ApplyModifiedProperties();
    }
}