using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseState
{
    public abstract void EnterState();

    public abstract void UpdateState(float deltaTime, Slider timer, int timerDuration);
}
