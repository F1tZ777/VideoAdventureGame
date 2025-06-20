using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateMachineManager : MonoBehaviour
{
    public static StateMachineManager instance;

    BaseState currentState;
    public BaseState IdleState = new IdleState();
    public BaseState LoadState = new LoadState();
    public BaseState PlayState = new PlayState();
    public BaseState PauseState = new PauseState();
    public BaseState FinishedState = new FinishedState();

    Slider timer;
    int timerDuration;

    // Start is called before the first frame update
    void Start()
    {
        //EnterIdleState();
        SwitchState(IdleState);
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(Time.deltaTime, timer, timerDuration);
    }

    public void SwitchState(BaseState state)
    {
        currentState = state;

        currentState.EnterState();
    }

    public void SetSlider(GameObject passedSlider, int time) { timer = passedSlider.GetComponent<Slider>(); timerDuration = time; }

    public static StateMachineManager GetInstance() => instance;
}
