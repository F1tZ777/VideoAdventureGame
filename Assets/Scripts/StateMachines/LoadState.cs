using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadState : BaseState
{
    public override void EnterState()
    {
        Debug.Log("Entering Load State");
    }

    public override void UpdateState(float deltaTime, Slider timer, int timerDuration)
    {

    }
}
