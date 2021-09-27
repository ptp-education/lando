using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EpisodeNode))]
public class EpisodeNodeEditor : Editor
{
    const string kAssetPrefix = "Assets/StreamingAssets/";
    const string kEpisodeObjectPrefix = "Assets/Resources/prefabs/episode_objects/";
    const string kPrefabSuffix = ".prefab";

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        SerializedProperty video = serializedObject.FindProperty("Video");
        SerializedProperty videoLoop = serializedObject.FindProperty("VideoLoop");
        SerializedProperty prefab = serializedObject.FindProperty("Prefab");

        EditorGUILayout.LabelField(string.Format("Node Type"));
        EpisodeNode myTarget = (EpisodeNode)target;

        myTarget.Type = (EpisodeNode.EpisodeType)EditorGUILayout.EnumPopup(myTarget.Type);

        if (myTarget.Type == EpisodeNode.EpisodeType.Video)
        {
            string videoPath = "empty";
            if (video.objectReferenceValue != null)
            {
                videoPath = AssetDatabase.GetAssetPath(video.objectReferenceValue.GetInstanceID());
                videoPath = videoPath.Substring(kAssetPrefix.Length);
                myTarget.VideoFilePath = videoPath;
            }
            string videoLoopPath = "empty";
            if (videoLoop.objectReferenceValue != null)
            {
                videoLoopPath = AssetDatabase.GetAssetPath(videoLoop.objectReferenceValue.GetInstanceID());
                videoLoopPath = videoLoopPath.Substring(kAssetPrefix.Length);
                myTarget.VideoLoopFilePath = videoLoopPath;
            }

            EditorGUILayout.LabelField(string.Format("Video ({0})", videoPath));
            myTarget.Video = EditorGUILayout.ObjectField(myTarget.Video, typeof(Object), false);

            EditorGUILayout.LabelField(string.Format("Video Loop ({0})", videoLoopPath));
            myTarget.VideoLoop = EditorGUILayout.ObjectField(myTarget.VideoLoop, typeof(Object), false);
        } else if (myTarget.Type == EpisodeNode.EpisodeType.Prefab)
        {
            string prefabPath = "empty";
            if (prefab.objectReferenceValue != null)
            {
                prefabPath = AssetDatabase.GetAssetPath(prefab.objectReferenceValue.GetInstanceID());
                prefabPath = prefabPath.Substring(kEpisodeObjectPrefix.Length, prefabPath.Length - (kEpisodeObjectPrefix.Length + kPrefabSuffix.Length));
                myTarget.PrefabPath = prefabPath;
            }
            EditorGUILayout.LabelField(string.Format("Prefab ({0})", myTarget.PrefabPath));
            myTarget.Prefab = (GameObject)EditorGUILayout.ObjectField(myTarget.Prefab, typeof(GameObject), false);
        }

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
