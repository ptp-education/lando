using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class ShareManager : GameManager
{
    public const string PREFAB_PATH = "prefabs/episode_objects/";

    [SerializeField] private Transform nodeObjectParent_;

    private string internalState_ = "";
    private Dictionary<string, EpisodeNodeObject> cachedNodeObjects_ = new Dictionary<string, EpisodeNodeObject>();

    protected override void NewNodeEventInternal(EpisodeNode node)
    {
        base.NewNodeEventInternal(node);

        internalState_ = NodeState.Playing;
        UpdateNodeState(NodeState.Playing);

        StartCoroutine(UpdateEpisodeNode(internalState_, currentNode_));
    }

    protected override void NewActionInternal(string a)
    {
        base.NewActionInternal(a);

        cachedNodeObjects_[Key(currentNode_)].ReceiveAction(a);
    }

    private IEnumerator UpdateEpisodeNode(string nodeState, EpisodeNode currentNode)
    {
        List<string> preloadedAssets = new List<string>();

        PreloadObject(currentNode);
        preloadedAssets.Add(Key(currentNode));

        if (currentNode.NextNode != null)
        {
            PreloadObject(currentNode.NextNode);
            preloadedAssets.Add(Key(currentNode.NextNode));
        }

        foreach (EpisodeNode.Option o in currentNode_.Options)
        {
            if (o.Node != null)
            {
                PreloadObject(o.Node);
                preloadedAssets.Add(Key(o.Node));
            }
        }

        yield return 0;

        bool isLooping = string.Equals(nodeState, GameManager.NodeState.Looping);
        if (isLooping)
        {
            cachedNodeObjects_[Key(currentNode)].Loop();
        } else
        {
            cachedNodeObjects_[Key(currentNode)].Play();
        }

        for (int i = 0; i < 12; i ++)
        {
            yield return 0;
        }

        cachedNodeObjects_[Key(currentNode)].transform.SetAsLastSibling();

        yield return 0;

        List<string> allKeys = new List<string>(cachedNodeObjects_.Keys);
        foreach (string k in allKeys)
        {
            if (!string.Equals(k, Key(currentNode)))
            {
                cachedNodeObjects_[k].Hide();
            }
            if (!preloadedAssets.Any(p => string.Equals(p, k)))
            {
                GameObject.Destroy(cachedNodeObjects_[k].gameObject);
                cachedNodeObjects_.Remove(k);
            }
        }
    }

    private void PreloadObject(EpisodeNode node)
    {
        if (cachedNodeObjects_.ContainsKey(Key(node))) return;

        EpisodeNodeObject nodeObject = null;
        string prefabPath = PREFAB_PATH;
        switch (node.Type)
        {
            case EpisodeNode.EpisodeType.Video:
                prefabPath += "video_player";
                break;

            case EpisodeNode.EpisodeType.Prefab:
                prefabPath += "prefab_player";
                break;
        }
        EpisodeNodeObject o = Resources.Load<EpisodeNodeObject>(prefabPath);
        nodeObject = GameObject.Instantiate<EpisodeNodeObject>(o);

        nodeObject.gameObject.name = node.gameObject.name;
        nodeObject.transform.SetParent(nodeObjectParent_);
        nodeObject.transform.localPosition = Vector3.zero;

        nodeObject.Init(currentNode_, EpisodeNodeFinished);
        nodeObject.Preload(node);
        nodeObject.Hide();
        cachedNodeObjects_[Key(node)] = nodeObject;
    }

    private void EpisodeNodeFinished()
    {
        if (string.Equals(internalState_, NodeState.Playing))
        {
            internalState_ = NodeState.Looping;
            UpdateNodeState(NodeState.Looping);

            StartCoroutine(UpdateEpisodeNode(internalState_, currentNode_));
        }
    }

    private string Key(EpisodeNode node)
    {
        return node.PrefabPath + node.VideoFilePath + node.VideoLoopFilePath;
    }
}
