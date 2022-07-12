using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimulatorNodeObject : EpisodeNodeObject
{
    public override bool IsPlaying
    {
        get
        {
            return true;
        }
    }

    [SerializeField] private Transform buttonHolder_;

    private EventObject activeEventObject_;
    private int counter_ = 0;
    private State state_;
    private bool takingInput_
    {
        set
        {
            buttonHolder_.gameObject.SetActive(value);
        }
        get
        {
            return buttonHolder_.gameObject.activeSelf;
        }
    }

    private enum State
    {
        ShowingAnswer,
        ShowingQuestion
    }

    protected override void Setup()
    {
        base.Setup();

        ShareManager sm = (ShareManager)gameManager_;
        buttonHolder_.transform.SetParent(sm.CharacterParent);
    }

    public override void Init(GameManager gameManager, EpisodeNode node)
    {
        base.Init(gameManager, node);

        LoadNextQuestion(0);
    }

    public override void ReceiveAction(string action)
    {
        base.ReceiveAction(action);

        if (takingInput_)
        {
            List<string> args = ArgumentHelper.ArgumentsFromCommand(GameManager.OPTION_SELECT, action);
            if (args.Count > 0)
            {
                if (state_ == State.ShowingQuestion)
                {
                    if (Node.SimulatorDetails.ShowAnswer)
                    {
                        LoadNextAnswer(counter_);
                    } else
                    {
                        LoadNextQuestion(counter_ + 1);
                    }
                }
            }
        }
    }



    private void LoadNextAnswer(int counter)
    {
        counter_ = counter;
        state_ = State.ShowingAnswer;
        takingInput_ = false;

        LoadEventObject(Node.SimulatorDetails.Steps[counter_].Answer, false, () =>
        {
            LoadNextQuestion(counter_ + 1);
        });
    }

    private void LoadNextQuestion(int counter)
    {
        counter_ = counter;
        state_ = State.ShowingQuestion;
        takingInput_ = false;

        if (counter_ >= Node.SimulatorDetails.Steps.Count)
        {
            gameManager_.SendNewActionInternal("-node next");
            return;
        }

        float voTime = 0f;
        if (counter != 0)
        {
            List<string> startVoOptions = new List<string>();
            for (int i = 0; i <= 12; i++)
            {
                startVoOptions.Add("simulator-next-" + i.ToString());
            }
            voTime = AudioPlayer.PlayVoiceover(startVoOptions) + 0.4f;
        }

        Go.to(transform, voTime, new GoTweenConfig().onComplete(t =>
        {
            LoadEventObject(Node.SimulatorDetails.Steps[counter_].Question, true, () =>
            {
                takingInput_ = true;
                AudioPlayer.PlaySfx("turn-off");
            });
        }));
    }

    private void LoadEventObject(EventObject eventObject, bool dontDestroy, System.Action callback)
    {
        ShareManager sm = (ShareManager)gameManager_;

        EventObject eo = Instantiate(eventObject);
        eo.transform.SetParent(sm.OverlayParent);
        eo.transform.localScale = Vector3.one;
        eo.transform.localPosition = Vector3.zero;
        eo.Init(EventObject.Type.Projector, gameManager_, () =>
        {
            callback();
        }, dontDestroy);

        if (activeEventObject_ != null)
        {
            Destroy(activeEventObject_.gameObject);
        }

        activeEventObject_ = eo;
    }

    public override void Reset()
    {
        base.Reset();

        if (activeEventObject_ != null)
        {
            Destroy(activeEventObject_.gameObject);
            activeEventObject_ = null;
        }
        if (buttonHolder_ != null)
        {
            Destroy(buttonHolder_.gameObject);
            buttonHolder_ = null;
        }
    }
}
