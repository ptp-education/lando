using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStationManager : StationManager
{
    protected override void NewRelevantAction(List<string> arguments)
    {
        base.NewRelevantAction(arguments);

        if (string.Equals("completed-challenge", arguments[0]))
        {
            if (arguments.Count > 0)
            {
                HandleChallengeCompleted(arguments[1]);
            }
        }

        if (string.Equals("failed-challenge", arguments[0]))
        {
            if (arguments.Count > 0)
            {
                bool showHint = false;
                if (string.Equals(arguments[1], "show-hint"))
                {
                    showHint = true;
                }
                else if (string.Equals(arguments[1], "dont-show-hint"))
                {
                    showHint = false;
                }
                HandleChallengeFailed(showHint);
            }
        }
    }

    private void HandleChallengeCompleted(string challengeName)
    {
        Debug.Log("challenge completed: " + challengeName);
    }

    private void HandleChallengeFailed(bool showHint)
    {
        Debug.Log("challenge failed show hint: " + showHint);
    }
}

