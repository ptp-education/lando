using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lando.SmartObjects;

public class ValidatorManager : GameManager
{
    [SerializeField] private Image challengeImage_;
    [SerializeField] private Transform testingButtonHolder_;
    [SerializeField] private Button testingButtonPrefab_;

    private void Start()
    {
        challengeImage_.color = Color.clear;
    }

    protected override void NewActionInternal(string a)
    {
        base.NewActionInternal(a);

        List<string> args = ArgumentHelper.ArgumentsFromCommand("-validator-controller", a);
        if (args.Count > 2)
        {
            string station = args[0];
            string command = args[1];

            switch(command)
            {
                case "load":
                    RefreshChallenge(args[2]);
                    break;
            }
        }
    }

    protected override void NewEpisodeEventInternal(Episode e)
    {
        base.NewEpisodeEventInternal(e);

        RemoveTestingButtons();
    }

    protected override void NewNodeEventInternal(EpisodeNode node)
    {
        base.NewNodeEventInternal(node);

        RemoveTestingButtons();

        if (currentNode_.TestingActive)
        {
            AddTestingButtons();
        }
    }

    private void RefreshChallenge(string challenge)
    {
        LevelData.Challenge c = ChallengeData.Challenges.Find(c => string.Equals(challenge, c.Name));
        if (c == null)
        {
            Debug.LogWarning("Couldn't find challenge with name: " + challenge);
            return;
        }

        challengeImage_.color = Color.white;
        challengeImage_.sprite = c.Sprite;
    }

    private void RemoveTestingButtons()
    {
        Button[] buttons = testingButtonHolder_.GetComponentsInChildren<Button>();
        for (int i = 0; i < buttons.Length; i++)
        {
            Destroy(buttons[i].gameObject);
        }
    }

    private void AddTestingButtons()
    {
        if (ChallengeData == null)
        {
            return;
        }

        List<KeyValuePair<string, string>> buttons = new List<KeyValuePair<string, string>>();

        buttons.Add(new KeyValuePair<string, string>(
            "Scan your wristband",
            string.Format(
                "-validator {0} {1}",
                SmartObjectType.TestingStation.ToString(),
                CommandDispatch.ValidatorResponse.ScanWristband.ToString()
            )
        ));
        buttons.Add(new KeyValuePair<string, string>(
            "Tested successfully",
            string.Format(
                "-validator {0} {1}",
                SmartObjectType.TestingStation.ToString(),
                CommandDispatch.ValidatorResponse.Success.ToString()
            )
        ));
        buttons.Add(new KeyValuePair<string, string>(
            "Tested and failed",
            string.Format(
                "-validator {0} {1}",
                SmartObjectType.TestingStation.ToString(),
                CommandDispatch.ValidatorResponse.Failure.ToString()
            )
        ));

        foreach (LevelData.BeforeTestFail failOption in ChallengeData.WaysToFail)
        {
            buttons.Add(new KeyValuePair<string, string>(
                failOption.ButtonName,
                string.Format(
                    "-validator {0} {1} {2}",
                    SmartObjectType.TestingStation.ToString(),
                    CommandDispatch.ValidatorResponse.BeforeTest.ToString(),
                    failOption.Name)));
        }

        foreach (KeyValuePair<string, string> b in buttons)
        {
            Button newButton = GameObject.Instantiate<Button>(testingButtonPrefab_);
            newButton.GetComponentInChildren<Text>().text = b.Key;
            newButton.onClick.AddListener(() =>
            {
                SendNewActionNetworked(b.Value);
            });
            newButton.transform.SetParent(testingButtonHolder_);
        }
    }
}
