using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class ImageEpisodeNodeObject : EpisodeNodeObject
{
    [SerializeField] private Image mainImage_;

    public override bool IsPlaying
    {
        get
        {
            return true;
        }
    }

    public override void Init(GameManager gameManager, EpisodeNode node)
    {
        base.Init(gameManager, node);

        mainImage_.sprite = Resources.Load<Sprite>(Node.ImageFilePath);
    }
}
