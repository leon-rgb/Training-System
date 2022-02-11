using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BasicVRButton : MonoBehaviour
{
    
    bool isPressed = false;
    float timeout = 1f;
    float startTime;
    float timeLeft = float.MaxValue;
    Material material;
    public Material highlightMaterial;
    public UnityEvent onPressed;

    // Update is called once per frame
    void Update()
    {
        timeLeft = startTime + timeout - Time.time;
        if (isPressed && timeLeft < 0)
        {
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
            // give the button a timeout
            //Debug.Log(other.tag + "  " + other.name);
            if (!isPressed)
            {
                isPressed = true;
                startTime = Time.time;
                onPressed.Invoke();
                material = GetComponent<MeshRenderer>().material;
                GetComponent<MeshRenderer>().material = highlightMaterial;
                //Debug.Log("pressed button");
            }
        }
    }
}
