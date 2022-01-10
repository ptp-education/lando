using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EpisodeNode))]
public class EpisodeNodeEditor : Editor
{
    const string kAssetPrefix = "Assets/StreamingAssets/";
    const string kResourcesPrefix = "Assets/Resources/";
    const string kEpisodeObjectPrefix = "Assets/Resources/prefabs/episode_objects/";
    const string kPrefabSuffix = ".prefab";

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        SerializedProperty backgroundLoop = serializedObject.FindProperty("BgLoop");
        SerializedProperty video = serializedObject.FindProperty("Video");
        SerializedProperty videoLoop = serializedObject.FindProperty("VideoLoop");
        SerializedProperty image = serializedObject.FindProperty("Image");
        SerializedProperty imageLoop = serializedObject.FindProperty("ImageLoop");
        SerializedProperty prefab = serializedObject.FindProperty("Prefab");

        EpisodeNode myTarget = (EpisodeNode)target;

        string audioPath = "null";
        if (backgroundLoop.objectReferenceValue != null)
        {
            audioPath = AssetDatabase.GetAssetPath(backgroundLoop.objectReferenceValue.GetInstanceID());
            audioPath = audioPath.Substring(kResourcesPrefix.Length).StripFileExtension();
            myTarget.BgLoopPath = audioPath;
        } else
        {
            myTarget.BgLoopPath = null;
        }

        EditorGUILayout.LabelField(string.Format("New BG Loop ({0})", myTarget.BgLoopPath == null ? "null" : myTarget.BgLoopPath));
        myTarget.BgLoop = (Object)EditorGUILayout.ObjectField(myTarget.BgLoop, typeof(Object), false);

        EditorGUILayout.LabelField(string.Format("Node Type"));

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

            EditorGUILayout.LabelField(string.Format("Video ({0})", myTarget.VideoFilePath));
            myTarget.Video = EditorGUILayout.ObjectField(myTarget.Video, typeof(Object), false);

            EditorGUILayout.LabelField(string.Format("Video Loop ({0})", myTarget.VideoLoopFilePath));
            myTarget.VideoLoop = EditorGUILayout.ObjectField(myTarget.VideoLoop, typeof(Object), false);
        }
        else if (myTarget.Type == EpisodeNode.EpisodeType.Image)
        {
            string imagePath = "empty";
            if (image.objectReferenceValue != null)
            {
                imagePath = AssetDatabase.GetAssetPath(image.objectReferenceValue.GetInstanceID());
                imagePath = imagePath.Substring(kResourcesPrefix.Length);
                imagePath = StripExtension(imagePath);
                myTarget.ImageFilePath = imagePath;
            }
            string imageLoopPath = "empty";
            if (imageLoop.objectReferenceValue != null)
            {
                imageLoopPath = AssetDatabase.GetAssetPath(imageLoop.objectReferenceValue.GetInstanceID());
                imageLoopPath = imageLoopPath.Substring(kResourcesPrefix.Length);
                imageLoopPath = StripExtension(imageLoopPath);
                myTarget.ImageLoopFilePath = imageLoopPath;
            }

            EditorGUILayout.LabelField(string.Format("Image ({0})", imagePath));
            myTarget.Image = EditorGUILayout.ObjectField(myTarget.Image, typeof(Object), false);

            EditorGUILayout.LabelField(string.Format("Video Loop ({0})", imageLoopPath));
            myTarget.ImageLoop = EditorGUILayout.ObjectField(myTarget.ImageLoop, typeof(Object), false);
        }
        else if (myTarget.Type == EpisodeNode.EpisodeType.LoopWithOptions)
        {
            string videoLoopPath = "empty";
            if (videoLoop.objectReferenceValue != null)
            {
                videoLoopPath = AssetDatabase.GetAssetPath(videoLoop.objectReferenceValue.GetInstanceID());
                videoLoopPath = videoLoopPath.Substring(kAssetPrefix.Length);
                myTarget.VideoLoopFilePath = videoLoopPath;
            }

            EditorGUILayout.LabelField(string.Format("Video Loop ({0})", myTarget.VideoLoopFilePath));
            myTarget.VideoLoop = EditorGUILayout.ObjectField(myTarget.VideoLoop, typeof(Object), false);

            EditorGUILayout.LabelField("Video options");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("VideoOptions"));

            foreach(EpisodeNode.VideoOption o in myTarget.VideoOptions)
            {
                foreach(EpisodeNode.VideoOption.Video v in o.Videos)
                {
                    if (v.VideoObject != null)
                    {
                        v.VideoPath = AssetDatabase.GetAssetPath(v.VideoObject).Substring(kAssetPrefix.Length);
                    }
                }
            }
        }

        EditorGUILayout.LabelField("Prompt");
        myTarget.Prompt = EditorGUILayout.TextArea(myTarget.Prompt, TextAreaStyle, GUILayout.Width(400), GUILayout.MinHeight(100), GUILayout.MaxHeight(400));

        EditorGUILayout.LabelField("Spawn Prefabs");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("PrefabSpawnObjects"));
        foreach (EpisodeNode.PrefabSpawnObject o in myTarget.PrefabSpawnObjects)
        {
            if (o.Object != null)
            {
                o.Path = AssetDatabase.GetAssetPath(o.Object);
                o.Path = o.Path.Substring(kEpisodeObjectPrefix.Length, o.Path.Length - (kEpisodeObjectPrefix.Length + kPrefabSuffix.Length));
            }
        }

        EditorGUILayout.LabelField("Command Line");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("CommandLines"));

        EditorGUILayout.LabelField("Next Node - leave empty if last episode");
        myTarget.NextNode = (EpisodeNode)EditorGUILayout.ObjectField(myTarget.NextNode, typeof(EpisodeNode), true);

        EditorGUILayout.LabelField("Additional options");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Options"));

        serializedObject.ApplyModifiedProperties();
    }

    private string StripExtension(string input)
    {
        string[] split = input.Split('.');
        return split[0];
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
