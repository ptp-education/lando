using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class ImageEpisodeNodeObject : EpisodeNodeObject
{
    [SerializeField] private Image mainImage_;
    [SerializeField] private Image loopImage_;

    public override void Play()
    {
        base.Play();

        mainImage_.transform.localScale = Vector3.one;
        loopImage_.transform.localScale = Vector3.zero;
    }

    //public override void Preload(EpisodeNode node)
    //{
    //    base.Preload(node);

    //    mainImage_.sprite = Resources.Load<Sprite>(node.ImageFilePath);

    //    if (node.ImageLoopFilePath == null || node.ImageLoopFilePath.Length == 0)
    //    {
    //        loopImage_.sprite = Resources.Load<Sprite>(node.ImageFilePath);
    //    } else
    //    {
    //        loopImage_.sprite = Resources.Load<Sprite>(node.ImageLoopFilePath);
    //    }
    //}
}
