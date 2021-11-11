using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EpisodeSpawnData
{
    public class NodeOption
    {
        public string Name;
        public Node Node;
    }
    public class Node
    {
        public string VideoFile;
        public string LoopVideoFile;
        public string Script;
        public List<NodeOption> Options = new List<NodeOption>();
    }

    public string EpisodeName;
    public string VideoRoot;
    public List<Node> Nodes = new List<Node>();
}