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

    const string kAssetPrefix = "Assets/StreamingAssets/";

    void OnEnable()
    {
        videoFilePath = serializedObject.FindProperty("VideoFilePath");
        videoFileAsset = serializedObject.FindProperty("Video");
        loopingFilePath = serializedObject.FindProperty("VideoLoopFilePath");
        loopingFileAsset = serializedObject.FindProperty("VideoLoop");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(videoFilePath);
        EditorGUILayout.PropertyField(videoFileAsset);
        EditorGUILayout.PropertyField(loopingFilePath);
        EditorGUILayout.PropertyField(loopingFileAsset);

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