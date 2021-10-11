using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;

public class Episode : MonoBehaviour
{
    [SerializeField] public EpisodeNode StartingNode;

    private const string kVisualizerTag = "Visualizer";

    public EpisodeNode[] AllNodes
    {
        get
        {
            return GetComponentsInChildren<EpisodeNode>();
        }
    }

    private void Start()
    {
        if (Application.isPlaying)
        {
            RemoveVisualize();
        }
    }

    public void Visualize()
    {
        RemoveVisualize();
        Visualize(StartingNode, Vector3.zero);
        DrawLines(StartingNode);
    }

    public void RemoveVisualize()
    {
        foreach (Transform o in StageUtility.GetCurrentStageHandle().FindComponentsOfType<Transform>())
        {
            if (string.Equals(o.tag, kVisualizerTag))
            {
                DestroyImmediate(o.gameObject);
            }
        }
    }

    private void Visualize(EpisodeNode node, Vector3 spawnLocation)
    {
        if (node.VisualNode != null) return;

        NodeVisualizer l = Resources.Load<NodeVisualizer>("prefabs/NodeVisualizer");
        NodeVisualizer nv = GameObject.Instantiate<NodeVisualizer>(l);
        nv.Init(node);
        nv.gameObject.SetActive(true);
        nv.tag = kVisualizerTag;
        nv.transform.SetParent(node.transform);
        nv.transform.localPosition = spawnLocation;

        List<EpisodeNode> nextNodes = new List<EpisodeNode>();
        if (node.NextNode != null)
        {
            nextNodes.Add(node.NextNode);
        }
        foreach(EpisodeNode.Option o in node.Options)
        {
            if (o.Node != null)
            {
                nextNodes.Add(o.Node);
            }
        }

        float xOffset = 0f;
        foreach(EpisodeNode n in nextNodes)
        {
            Visualize(n, new Vector3(spawnLocation.x + xOffset, spawnLocation.y - 100f, 0f));
        }
    }

    private void DrawLines(EpisodeNode node)
    {
        if (node.VisualNode.LineDrawn) return;

        node.VisualNode.LineDrawn = true;

        if (node.NextNode != null)
        {
            DrawLine(node.VisualNode.PositionForOption("Next"), node.NextNode.VisualNode.transform.position, node.transform);
            DrawLines(node.NextNode);
        }
        foreach (EpisodeNode.Option o in node.Options)
        {
            if (o.Node != null)
            {
                DrawLine(node.VisualNode.PositionForOption(o.Prompt), o.Node.VisualNode.transform.position, node.transform);
                DrawLines(o.Node);
            }
        }
    }

    private void DrawLine(Vector3 start, Vector3 end, Transform parentTransform)
    {
        start = new Vector3(start.x, start.y, -100);
        end = new Vector3(end.x, end.y + 22, 0);

        GameObject myLine = new GameObject();
        myLine.transform.SetParent(parentTransform);
        myLine.transform.position = start;
        myLine.name = "connector";
        myLine.tag = kVisualizerTag;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.startColor = Color.white;
        lr.endColor = Color.white;
        lr.startWidth = 1f;
        lr.endWidth = 1f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }
}