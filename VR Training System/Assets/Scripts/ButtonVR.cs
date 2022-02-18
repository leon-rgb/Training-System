using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// very similar to BasicVRButton, but this class should be used on UI
/// </summary>
public class ButtonVR : Button
{
    public UnityEvent onPressed;
    bool isPressed = false;
    public float timeout = 1.5f;
    float startTime;
    float timeLeft = float.MaxValue;

    protected override void OnEnable()
    {
        // init time since it's always called in update
        startTime = Time.time;
    }

    private void Update()
    {
        timeLeft = startTime + timeout - Time.time;
        // reset to default if timeout is over
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
            Debug.Log(other.tag + "  " + other.name);
            // make sure button can't be pressed again instantly
            if (!isPressed)
            {
                // give the button a timeout, load plane and close menu
                isPressed = true;
                startTime = Time.time;
                DoStateTransition(SelectionState.Pressed, true);
                onPressed.Invoke();
                Debug.Log("pressed button");
            }
        }
    }
}
