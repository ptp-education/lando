    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EpisodeNodeObject : MonoBehaviour
{
    public static string kSpawnCommand = "-spawn-";
    public delegate void ReadyToStartLoop();

    public EpisodeNode Node;
    protected GameManager gameManager_;

    private List<SpawnedObject> spawnedPrefabs_ = new List<SpawnedObject>();
    private float timer_ = 0f;

    private RectTransform spawnedObjectParent_;
    public Transform OverlayParent;

    private void Start()
    {
        GameObject o = new GameObject("spawned prefab parent");
        o.AddComponent<RectTransform>();
        o.transform.SetParent(transform);
        spawnedObjectParent_ = o.GetComponent<RectTransform>();
        spawnedObjectParent_.transform.localScale = Vector3.one;
        spawnedObjectParent_.transform.localPosition = Vector3.zero;
    }

    public virtual void Init(GameManager manager, EpisodeNode node)
    {
        gameManager_ = manager;
        Node = node;
    }

    public virtual bool IsPlaying
    {
        get
        {
            return false;
        }
    }

    public virtual void Reset()
    {
        ResetSpawnedObjects();
        ResetCommandLines();
    }

    public virtual void ReceiveAction(string action)
    {
        foreach(SpawnedObject o in spawnedPrefabs_)
        {
            o.ReceivedAction(action);
        }

        if (action.StartsWith(kSpawnCommand))
        {
            SpawnObject(action.Substring(kSpawnCommand.Length).Trim());
        }

        foreach(EpisodeNode.CommandContainer c in Node.CommandLineContainers)
        {
            if (ArgumentHelper.ContainsCommand(c.CommandToCall, action))
            {
                RunCommandContainer(c);
            }
        }
    }

    public virtual void Hide()
    {
        foreach(SpawnedObject o in spawnedPrefabs_)
        {
            o.Hide();
        }
    }

    public virtual float ProgressPercentage
    {
        get
        {
            return -1f;
        }
    }

    private void RunCommandContainer(EpisodeNode.CommandContainer commandContainer)
    {
        GoTweenFlow flow = new GoTweenFlow();
        foreach(EpisodeNode.CommandLine c in commandContainer.StoredCommands)
        {
            string command = c.Command;
            float timeStamp = c.TimeStamp;
            if (timeStamp < 0.1)
            {
                timeStamp = 0.1f;
            }
            flow.insert(0, new GoTween(this.transform,  timeStamp, new GoTweenConfig().onComplete(t =>
            {
                gameManager_.SendNewAction(command);
            })));
        }
        flow.play();
    }

    private void SpawnObject(string command)
    {
        foreach (EpisodeNode.PrefabSpawnObject o in Node.PrefabSpawnObjects)
        {
            if (string.Equals(o.SpawnKey, command))
            {
                SpawnObject(o);
            }
        }
    }

    private void Update()
    {
        spawnedObjectParent_.SetAsLastSibling();
           
        if (IsPlaying)
        {
            timer_ += Time.deltaTime;
        }

        foreach(EpisodeNode.PrefabSpawnObject o in Node.PrefabSpawnObjects)
        {
            if (timer_ > o.TimeStamp && o.TimeStamp != -1f)
            {
                if (!o.Spawned)
                {
                    SpawnObject(o);
                }
            }
        }

        foreach (EpisodeNode.CommandLine c in Node.CommandLines)
        {
            if (timer_ > c.TimeStamp)
            {
                if (!c.Ran)
                {
                    gameManager_.SendNewAction(c.Command);
                    c.Ran = true;
                }
            }
        }
    }

    private void SpawnObject(EpisodeNode.PrefabSpawnObject prefabSpawnObject)
    {
        SpawnedObject o = Resources.Load<SpawnedObject>(prefabSpawnObject.Path);
        SpawnedObject spawnedObject = GameObject.Instantiate<SpawnedObject>(o, spawnedObjectParent_);
        spawnedObject.transform.localPosition = prefabSpawnObject.Position;
        spawnedObject.transform.localScale = prefabSpawnObject.Scale;
        spawnedObject.Init(gameManager_);

        spawnedPrefabs_.Add(spawnedObject);

        prefabSpawnObject.Spawned = true;
    }

    private void ResetSpawnedObjects()
    {
        timer_ = 0f;
        foreach (EpisodeNode.PrefabSpawnObject o in Node.PrefabSpawnObjects)
        {
            o.Spawned = false;
        }
        for (int i = 0; i < spawnedPrefabs_.Count; i++)
        {
            SpawnedObject o = spawnedPrefabs_[i];
            Destroy(o.gameObject);
        }
        spawnedPrefabs_ = new List<SpawnedObject>();
    }

    private void ResetCommandLines()
    {
        timer_ = 0f;
        foreach(EpisodeNode.CommandLine c in Node.CommandLines)
        {
            c.Ran = false;
        }
    }
}
