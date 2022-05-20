using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintStationManager : StationManager
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
                bool showHint = string.Equals(arguments[1], "show-hint");
                HandleChallengeFailed(showHint);
            }
        }
    }

    private void HandleChallengeCompleted(string challengeName)
    {

    }

    private void HandleChallengeFailed(bool showHint)
    {

    }
}
