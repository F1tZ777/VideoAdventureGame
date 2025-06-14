using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineManager : MonoBehaviour
{
    public static StateMachineManager instance;

    BaseState currentState;
    public BaseState IdleState = new IdleState();
    public BaseState LoadState = new LoadState();
    public BaseState PlayState = new PlayState();
    public BaseState PauseState = new PauseState();
    public BaseState FinishedState = new FinishedState();

    // Start is called before the first frame update
    void Start()
    {
        EnterIdleState();
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    public void EnterIdleState()
    {
        currentState = IdleState;

        currentState.EnterState(this);
    }

    public void EnterLoadState()
    {
        currentState = LoadState;

        currentState.EnterState(this);
    }

    public void EnterPlayState()
    {
        currentState = PlayState;

        currentState.EnterState(this);
    }

    public void EnterPauseState()
    {
        currentState = PauseState;

        currentState.EnterState(this);
    }

    public void EnterFinishedState()
    {
        currentState = FinishedState;

        currentState.EnterState(this);
    }

    public static StateMachineManager GetInstance() => instance;
}
