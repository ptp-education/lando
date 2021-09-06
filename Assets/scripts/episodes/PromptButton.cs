using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PromptButton : MonoBehaviour
{
    [SerializeField] private Text prompt_; 
    [SerializeField] private Text command_;

    public delegate void ActionDetected(string linkedEpisode);

    private string linkedEpisode_;
    private ActionDetected callback_;
    private int index_;

    public void Init(string prompt, string linkedEpisode, int index, ActionDetected callback)
    {
        prompt_.text = prompt;
        linkedEpisode_ = linkedEpisode;
        callback_ = callback;
        index_ = index;

        if (linkedEpisode != null && linkedEpisode.Length > 0)
        {
            command_.text = (index_).ToString();
        } else
        {
            command_.text = "";
        }
    }

    public void OnClick()
    {
        callback_.Invoke(linkedEpisode_);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown((index_).ToString()))
        {
            callback_.Invoke(linkedEpisode_);
        }
    }
}
