using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using Spine.Unity;

public class SequenceEpisodeNodeObject : EpisodeNodeObject
{
    [SerializeField] private Transform contentParent_;

    //<object_name, type>, <object>
    private Dictionary<KeyValuePair<string, string>, Transform> objects = new Dictionary<KeyValuePair<string, string>, Transform>();

    private SequenceData sequenceData
    {
        get
        {
            return episodeNode_.ProcessedSequenceData;
        }
    }

    public override void Play()
    {
        base.Play();
    }

    public override void Loop()
    {
        base.Loop();
    }

    public override void Preload(EpisodeNode node)
    {
        base.Preload(node);

        foreach(SequenceData.Object obj in sequenceData.Objects)
        {
            Transform createdObject = null;
            if (string.Equals(obj.ObjectType, SequenceData.Object.Type.Image.ToString()))
            {
                Sprite s = Resources.Load<Sprite>(obj.ModelPath);
                Image i = Resources.Load<Image>(ShareManager.PREFAB_PATH + "single_image_player");

                Image image = GameObject.Instantiate<Image>(i);
                image.sprite = s;
                image.SetNativeSize();

                createdObject = image.GetComponent<Transform>();
            } else if (string.Equals(obj.ObjectType, SequenceData.Object.Type.Prefab.ToString()))
            {
                Debug.LogWarning("Loading prefab as scene is not yet implemented");
            } else if (string.Equals(obj.ObjectType, SequenceData.Object.Type.Video.ToString()))
            {
                Debug.LogWarning("Loading video as scene is not yet implemented");
            } else if (string.Equals(obj.ObjectType, SequenceData.Object.Type.Spine.ToString()))
            {
                SkeletonGraphic l = Resources.Load<SkeletonGraphic>(obj.ModelPath);

                if (l == null) {
                    Debug.LogError("Could not find SkeletonGraphic with path: " + obj.ModelPath);
                }
                SkeletonGraphic sa = GameObject.Instantiate<SkeletonGraphic>(l);

                createdObject = sa.GetComponent<Transform>();
            } else
            {
                Debug.LogError("Unhandled scene type: " + obj.ObjectType);
            }

            if (createdObject != null)
            {
                createdObject.transform.SetParent(contentParent_);
                createdObject.transform.localPosition = obj.StartingPosition;
                createdObject.transform.localScale = obj.StartingScale;

                objects.Add(new KeyValuePair<string, string>(obj.Name, obj.ObjectType), createdObject);
            }
        }
    }
}
