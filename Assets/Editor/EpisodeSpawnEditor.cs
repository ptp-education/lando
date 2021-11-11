using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;

public class EpisodeSpawnEditor : EditorWindow
{
    private string NodeData;

    [MenuItem("Lando/Episode Generator")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(EpisodeSpawnEditor));
    }

    void OnGUI()
    {
        // The actual window code goes here
        GUILayout.Label("Episode JSON");
        NodeData = GUILayout.TextArea(NodeData, TextAreaStyle, GUILayout.Width(400), GUILayout.MinHeight(100), GUILayout.MaxHeight(400));
        if (GUILayout.Button("Generate"))
        {
            GenerateNodes();
        }
    }

    //does not support nested options
    public void GenerateNodes()
    {
        EpisodeSpawnData d = JsonConvert.DeserializeObject<EpisodeSpawnData>(NodeData);

        GameObject obj = new GameObject();
        obj.AddComponent<Episode>();
        Episode episode = obj.GetComponent<Episode>();
        episode.gameObject.name = d.EpisodeName;

        List<EpisodeNode> newNodes = new List<EpisodeNode>();

        foreach (EpisodeSpawnData.Node n in d.Nodes)
        {
            newNodes.Add(EpisodeNode.CreateNewNode(episode.transform, d.VideoRoot + n.VideoFile + ".mp4", d.VideoRoot + n.LoopVideoFile + ".mp4", n.Script, n.Options));
        }
        for (int i = 0; i < newNodes.Count; i++)
        {
            EpisodeNode nextNode = null;
            if (i + 1 < newNodes.Count)
            {
                nextNode = newNodes[i + 1];
            }
            newNodes[i].NextNode = nextNode;

            List<EpisodeNode.Option> o = newNodes[i].Options;
            if (o.Count > 0)
            {
                newNodes[i].NextNode = null;    //TA first choose an option first before skipping
                for (int ii = 0; ii < o.Count; ii++)
                {
                    o[ii].Node.Options = o;
                    o[ii].Node.NextNode = nextNode;
                }
            }
        }

        episode.StartingNode = newNodes[0];
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
