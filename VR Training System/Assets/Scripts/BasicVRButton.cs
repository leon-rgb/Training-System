using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// A button that works with collision in VR. 
/// Controller has to have Tag "Hand" to collider with the button.
/// </summary>
public class BasicVRButton : MonoBehaviour
{
    // for making sure button can't be reactivated instantly
    bool isPressed = false;

    // how long the button can't be reactivated
    float timeout = 1f;

    // time values for calculating how much time is left to reactivate
    float startTime;
    float timeLeft = float.MaxValue;
    
    [Tooltip("material shown when button is pressed")]
    public Material highlightMaterial;
    // for saving default material
    Material material;

    // event that is triggered when coliding
    public UnityEvent onPressed;


    // Update is called once per frame
    void Update()
    {
        // check if timeout is over and button was pressed
        timeLeft = startTime + timeout - Time.time;
        if (isPressed && timeLeft < 0)
        {
            // reset button
            isPressed = false;
            timeLeft = float.MaxValue;
            GetComponent<MeshRenderer>().material = material;
            //Debug.Log("exited button");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            
            //Debug.Log(other.tag + "  " + other.name);
            if (!isPressed)
            {
                // give the button a timeout
                isPressed = true;
                startTime = Time.time;
                // trigger event and set highlight material
                onPressed.Invoke();
                material = GetComponent<MeshRenderer>().material;
                GetComponent<MeshRenderer>().material = highlightMaterial;
                //Debug.Log("pressed button");
            }
        }
    }
}
