using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NodeVisualizer : MonoBehaviour
{
    [SerializeField] TextMeshPro title_;
    [SerializeField] TextMeshPro type_;
    [SerializeField] TextMeshPro prompt_;
    [SerializeField] TextMeshPro path_;
    [SerializeField] Transform optionsParent_;
    [SerializeField] TextMeshPro optionCopy_;

    [HideInInspector] public bool LineDrawn = false;

    private List<TextMeshPro> options_ = new List<TextMeshPro>();

    public void Init(EpisodeNode node)
    {
        title_.text = node.name;
        prompt_.text = node.Prompt;
        node.VisualNode = this;

        switch(node.Type)
        {
            case EpisodeNode.EpisodeType.Video:
                type_.text = "Type: Video";
                path_.text = node.VideoFilePath + " / " + node.VideoLoopFilePath;
                break;
            case EpisodeNode.EpisodeType.Image:
                type_.text = "Type: Image";
                path_.text = node.ImageFilePath + " / " + node.ImageLoopFilePath;
                break;
            case EpisodeNode.EpisodeType.LoopWithOptions:
                type_.text = "Type: LoopWithOptions";
                path_.text = node.VideoLoopFilePath;
                break;
        }

        for (int i = -1; i < node.Options.Count; i++)
        {
            string optionText = "";
            if (i == -1)
            {
                optionText = "Next";
            } else
            {
                optionText = node.Options[i].Action;
            }
            TextMeshPro tm = GameObject.Instantiate<TextMeshPro>(optionCopy_);
            tm.gameObject.SetActive(true);
            tm.transform.SetParent(optionsParent_);
            tm.transform.localPosition = new Vector3((i + 1) * 20, 0f, 0f);
            tm.text = optionText;

            options_.Add(tm);
        }
        float xOffset = (node.Options.Count + 1) * 15f + node.Options.Count * 5f;
        optionsParent_.transform.localPosition = new Vector3(-xOffset / 2 + 7.5f, optionsParent_.transform.localPosition.y, 0f);
    }

    public Vector3 PositionForOption(string option)
    {
        foreach(TextMeshPro o in options_)
        {
            if (string.Equals(o.text, option))
            {
                return o.transform.position;
            }
        }
        return Vector3.zero;
    }
}
