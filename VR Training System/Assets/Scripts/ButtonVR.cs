using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonVR : Button
{
    public UnityEvent onPressed;
    bool isPressed = false;
    public float timeout = 1.5f;
    float startTime;
    float timeLeft = float.MaxValue;

    protected override void OnEnable()
    {
        startTime = Time.time;
    }

    private void Update()
    {
        timeLeft = startTime + timeout - Time.time;
        // reset to default
        if (isPressed && timeLeft < 0)
        {
            isPressed = false;
            timeLeft = float.MaxValue;
            DoStateTransition(SelectionState.Normal, true);
            Debug.Log("exited button");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            // give the button a timeout, load plane and close menu
            Debug.Log(other.tag + "  " + other.name);
            if (!isPressed)
            {
                isPressed = true;
                startTime = Time.time;
                DoStateTransition(SelectionState.Pressed, true);
                onPressed.Invoke();
                Debug.Log("pressed button");
            }
        }
    }
}
