using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrefabContentCodeRehearse : PrefabContent
{
    [SerializeField] GameObject light_;
    [SerializeField] GameObject codeTextCopy_;
    [SerializeField] Image outline_;
    [SerializeField] Transform codeTextHolder_;
    [SerializeField] List<DanceCharacter> characters_;

    private Vector3 cachedCodeStartingPosition_;
    private DanceCharacter activeCharacter_;
    private DanceCode code_;
    private List<GameObject> codeTexts_;
    private GoTweenFlow flow_;

    private void Start()
    {
        cachedCodeStartingPosition_ = codeTextCopy_.transform.localPosition;
        codeTextCopy_.transform.localScale = Vector3.zero;
    }

    public override void Play()
    {
        base.Play();

        code_ = gameManager_.Storage.GetValue<DanceCode>(GameStorage.Key.DanceCode);
        SpawnCode();

        string selectedCharacter = gameManager_.Storage.GetValue<string>(GameStorage.Key.SelectedCharacter);
        foreach (DanceCharacter c in characters_)
        {
            bool rightCharacter = string.Equals(c.gameObject.name, selectedCharacter);
            c.gameObject.SetActive(rightCharacter);
            if (rightCharacter)
            {
                activeCharacter_ = c;
            }
        }

        Camera.transform.SetParent(activeCharacter_.transform);
        light_.transform.SetParent(activeCharacter_.transform);
    }

    public override void ReceiveAction(string action)
    {
        base.ReceiveAction(action);

        if (string.Equals(action, "Play"))
        {
            PlayCode();
        }
    }

    private void SpawnCode()
    {
        if (codeTexts_ != null)
        {
            foreach(GameObject t in codeTexts_)
            {
                Destroy(t.gameObject);
            }
        }
        codeTexts_ = new List<GameObject>();

        for (int i = 0; i < code_.Commands.Count; i++)
        {
            DanceCode.Command c = code_.Commands[i];
            GameObject t = GameObject.Instantiate<GameObject>(codeTextCopy_);
            t.GetComponentInChildren<Text>().text = c.AnimationName;
            t.transform.SetParent(codeTextHolder_);
            t.transform.localScale = Vector3.one;
            t.transform.localPosition = new Vector3(cachedCodeStartingPosition_.x, cachedCodeStartingPosition_.y - 125f * i, 0);
            t.transform.localRotation = Quaternion.identity;
            codeTexts_.Add(t);
        }
    }

    private void PlayCode()
    {
        if (flow_ != null)
        {
            flow_.complete();
            flow_.destroy();
        }
        activeCharacter_.transform.localPosition = Vector3.zero;

        flow_ = new GoTweenFlow();
        float time = 0f;

        for (int i = 0; i < code_.Commands.Count; i++)
        {
            DanceCode.Command c = code_.Commands[i];
            GameObject codeText = codeTexts_[i];

            string animationName = c.AnimationName;
            float animationTime = activeCharacter_.TimeForAnimation(animationName);

            GoTween t = new GoTween(
                activeCharacter_,
                animationTime,
                new GoTweenConfig().onBegin(t =>
                {
                    activeCharacter_.PlayAnimation(animationName);
                    outline_.transform.localPosition = codeText.transform.localPosition;
                })
            );
            flow_.insert(time, t);

            time += animationTime;
        }

        flow_.setOnCompleteHandler(t =>
        {
            activeCharacter_.Idle();
        });

        flow_.play();
    }
}