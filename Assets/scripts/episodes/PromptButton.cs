using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PromptButton : MonoBehaviour
{
    [SerializeField] private Text prompt_; 
    [SerializeField] private Text command_;

    public delegate void ActionDetected(string linkedEpisode);

    private string action_;
    private ActionDetected callback_;
    private string commandKey_;

    public void Init(string prompt, string action, string commandKey, ActionDetected callback)
    {
        transform.localScale = Vector3.one;

        prompt_.text = prompt;
        action_ = action;
        callback_ = callback;
        commandKey_ = commandKey;

        command_.text = commandKey_;
    }

    public void OnClick()
    {
        callback_.Invoke(action_);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(commandKey_))
        {
            callback_.Invoke(action_);
        }
    }
}
