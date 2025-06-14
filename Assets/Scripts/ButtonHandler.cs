using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ButtonHandler : MonoBehaviour
{
    GameObject timerObject;
    Slider timer;

    void Awake()
    {
        timerObject = gameObject.transform.Find("Timer").gameObject;
        timer = timerObject.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (VideoManager.GetInstance().timerStart)
        {
            // Do timer stuff here later
        }
    }


}
