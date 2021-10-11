using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Episode : MonoBehaviour
{
    [SerializeField] public EpisodeNode StartingNode;
    [SerializeField] private NodeVisualizer nodeVisualizer_;

    private const string kVisualizerTag = "visualizer";
    private Transform visualizerParent_;

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
        if (visualizerParent_ == null)
        {
            GameObject o = new GameObject("Visualizer Parent");
            o.tag = kVisualizerTag;
            o.AddComponent<Transform>();
            o.transform.SetParent(transform);
            visualizerParent_ = o.GetComponent<Transform>();
        }
        
        Visualize(StartingNode, Vector3.zero);
        DrawLines(StartingNode);
    }

    public void RemoveVisualize()
    {
        if (visualizerParent_ == null)
        {
            GameObject o = GameObject.FindGameObjectWithTag(kVisualizerTag);
            if (o != null)
            {
                visualizerParent_ = o.GetComponent<Transform>();
            }
        }
        if (visualizerParent_ != null)
        {
            if (Application.isPlaying)
            {
                Destroy(visualizerParent_.gameObject);
            } else
            {
                DestroyImmediate(visualizerParent_.gameObject);
            }
        }
        visualizerParent_ = null;
    }

    private void Visualize(EpisodeNode node, Vector3 spawnLocation)
    {
        if (node.VisualNode != null) return;

        NodeVisualizer nv = GameObject.Instantiate<NodeVisualizer>(nodeVisualizer_);
        nv.Init(node);
        nv.gameObject.SetActive(true);
        nv.transform.SetParent(visualizerParent_);
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
            DrawLine(node.VisualNode.PositionForOption("Next"), node.NextNode.VisualNode.transform.position);
            DrawLines(node.NextNode);
        }
        foreach (EpisodeNode.Option o in node.Options)
        {
            if (o.Node != null)
            {
                DrawLine(node.VisualNode.PositionForOption(o.Prompt), o.Node.VisualNode.transform.position);
                DrawLines(o.Node);
            }
        }
    }

    private void DrawLine(Vector3 start, Vector3 end)
    {
        start = new Vector3(start.x, start.y, -100);
        end = new Vector3(end.x, end.y + 22, 0);

        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.transform.SetParent(visualizerParent_);
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