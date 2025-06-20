using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishedState : BaseState
{
    public override void EnterState()
    {
        Debug.Log("Entering Finished State");
    }

    public override void UpdateState(float deltaTime, Slider timer, int timerDuration)
    {
        if (timer != null)
        {
            if (timer.value > 0)
                timer.value -= deltaTime/(float)timerDuration;
            else
            {
                timer.value = 0;
                // Figure out how to send the appropriate ID to VideoHandler
                VideoManager.GetInstance().TimerFailSendClip();
            }
        }
    }
}
