using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{
    public abstract void EnterState(StateMachineManager state);

    public abstract void UpdateState(StateMachineManager state);
}
