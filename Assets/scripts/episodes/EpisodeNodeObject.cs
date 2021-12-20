using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EpisodeNodeObject : MonoBehaviour
{
    public delegate void ReadyToStartLoop();

    protected ReadyToStartLoop startLoopCallback_;
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
        spawnedObjectParent_.localScale = new Vector3(-1, 1, 1);
    }

    public virtual void Init(GameManager manager, EpisodeNode node, ReadyToStartLoop callback)
    {
        gameManager_ = manager;
        startLoopCallback_ = callback;
        episodeNode_ = node;
    }

    public virtual void Preload(EpisodeNode node)
    {
        Hide();
    }

    public virtual void Hide()
    {
        Hidden = true;
    }

    public virtual bool Hidden
    {
        set
        {
            if (value)
            {
                transform.localScale = Vector3.zero;
            } else
            {
                transform.localScale = Vector3.one;
            }
        }
        get
        {
            return transform.localScale == Vector3.zero;
        }
    }

    public virtual void Play()
    {
        Hidden = false;
        ResetSpawnedObjects();
    }

    public virtual void Loop()
    {
        Hidden = false;
    }

    public virtual void ReceiveAction(string action)
    {
        foreach(SpawnedObject o in spawnedPrefabs_)
        {
            o.ReceivedAction(action);
        }
    }

    public virtual void OnExit() 
    {

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

        if (!Hidden)
        {
            timer_ += Time.deltaTime;
        }
        //dont forget to remove objects and reset timer

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
}
