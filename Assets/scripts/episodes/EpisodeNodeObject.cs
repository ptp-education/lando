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
        transform.localScale = Vector3.zero;
    }

    public virtual void Play()
    {
        foreach (SpawnedObject o in spawnedPrefabs_)
        {
            o.Reset();
        }

        transform.localScale = Vector3.one;
    }

    public virtual void Loop()
    {
        transform.localScale = Vector3.one;
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

    private void Update()
    {
        timer_ += Time.deltaTime;
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
        RawImage ri = GetComponentInChildren<RawImage>();

        SpawnedObject o = Resources.Load<SpawnedObject>(ShareManager.PREFAB_PATH + prefabSpawnObject.Path);
        SpawnedObject spawnedObject = GameObject.Instantiate<SpawnedObject>(o, ri.transform);
        spawnedObject.transform.localPosition = prefabSpawnObject.Position;

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
