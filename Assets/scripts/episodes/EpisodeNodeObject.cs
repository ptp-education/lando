using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EpisodeNodeObject : MonoBehaviour
{
    public delegate void ReadyToStartLoop();

    protected EpisodeNode episodeNode_;
    protected GameManager gameManager_;

    private List<SpawnedObject> spawnedPrefabs_ = new List<SpawnedObject>();
    private float timer_ = 0f;

    private RectTransform spawnedObjectParent_;

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
        episodeNode_ = node;
    }

    public virtual bool IsPlaying
    {
        get
        {
            return false;
        }
    }

    public virtual void Play()
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
    }

    public virtual float ProgressPercentage
    {
        get
        {
            return -1f;
        }
    }

    private void Update()
    {
        spawnedObjectParent_.SetAsLastSibling();

        //dont forget to remove objects and reset timer

        timer_ += Time.deltaTime;

        foreach(EpisodeNode.PrefabSpawnObject o in episodeNode_.PrefabSpawnObjects)
        {
            if (timer_ > o.TimeStamp)
            {
                if (!o.Spawned)
                {
                    SpawnObject(o);
                }
            }
        }

        foreach (EpisodeNode.CommandLine c in episodeNode_.CommandLines)
        {
            if (timer_ > c.TimeStamp)
            {
                if (!c.Ran)
                {
                    gameManager_.NewNodeAction(GameManager.ACTION_PREFIX + c.Command);
                    c.Ran = true;
                }
            }
        }
    }

    private void SpawnObject(EpisodeNode.PrefabSpawnObject prefabSpawnObject)
    {
        SpawnedObject o = Resources.Load<SpawnedObject>(ShareManager.PREFAB_PATH + prefabSpawnObject.Path);
        SpawnedObject spawnedObject = GameObject.Instantiate<SpawnedObject>(o, spawnedObjectParent_);
        spawnedObject.transform.localPosition = prefabSpawnObject.Position;
        spawnedObject.Init(gameManager_);

        spawnedPrefabs_.Add(spawnedObject);

        prefabSpawnObject.Spawned = true;
    }

    private void ResetSpawnedObjects()
    {
        timer_ = 0f;
        foreach (EpisodeNode.PrefabSpawnObject o in episodeNode_.PrefabSpawnObjects)
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
        foreach(EpisodeNode.CommandLine c in episodeNode_.CommandLines)
        {
            c.Ran = false;
        }
    }
}
