using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnedTimer : SpawnedObject
{
    [SerializeField] private Text timerText_;

    private float timer_;
    private float limit_ = -1f;

    public override void ReceivedAction(string action)
    {
        base.ReceivedAction(action);

        if (action.Contains("minute timer"))
        {
            string[] split = action.Split(' ');

            int minutes = -1;
            int.TryParse(split[0], out minutes);
            if (minutes > 0)
            {
                limit_ = (float)minutes * 60f;
            }
        }
    }

    public override void Reset()
    {
        base.Reset();

        timer_ = 0f;
        limit_ = -1f;

        timerText_.text = "00:00";
    }

    private void Update()
    {
        if (timer_ >= limit_)
        {
            timerText_.text = "00:00";
        } else
        {
            timer_ += Time.deltaTime;

            int minutes = 0;

            int timeLeft = (int)limit_ - (int)timer_;
            while (timeLeft > 60f)
            {
                minutes++;
                timeLeft -= 60;
            }

            string minuteZero = minutes > 9 ? "" : "0";
            string secondsZero = timeLeft > 9 ? "" : "0";
            timerText_.text = string.Format("{0}{1}:{2}{3}", minuteZero, minutes.ToString(), secondsZero, timeLeft.ToString());
        }
    }
}
