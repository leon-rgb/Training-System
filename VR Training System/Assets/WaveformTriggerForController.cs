using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;

public class WaveformTriggerForController : MonoBehaviour
{
    //public GameObject controllerLeft;
    //public GameObject controllerRight;
    private Hand handLeft;
    private Hand handRight;
    public SteamVR_Action_Vibration hapticAction;
    //public SteamVR_Action_Boolean trackpadAction;

    // Start is called before the first frame update
    void Start()
    {
        //handLeft = controllerLeft.GetComponent<Hand>();
        //handRight = controllerRight.GetComponent<Hand>();
        GetComponent<Rigidbody>().sleepThreshold = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {    
        if (other.CompareTag("CuttingPlane"))
        {
            Debug.Log("WAVEFORMCOLLISION");
            sendPulse(0.3f, 3, 75);
            
            //sendDelayedPulse(0.35f, 0.2f, 100, 75);
            //(0.7f, 0.2f, 200, 75);
        }
        else if(other.CompareTag("Bones"))
        {
            Debug.Log("WAVEFORMCOLLISION bone");
            sendPulse(0.5f, 200, 120);
        }
    }

    private void sendPulse(float duration, float frequency, float amplitude, SteamVR_Input_Sources source = SteamVR_Input_Sources.Any)
    {
        if(source == SteamVR_Input_Sources.Any)
        {
            hapticAction.Execute(0, duration, frequency, amplitude, SteamVR_Input_Sources.LeftHand);
            hapticAction.Execute(0, duration, frequency, amplitude, SteamVR_Input_Sources.RightHand);
        }
        else
        {
            hapticAction.Execute(0, duration, frequency, amplitude, source);
        }  
    }

    private void sendDelayedPulse(float delay, float duration, float frequency, float amplitude, SteamVR_Input_Sources source = SteamVR_Input_Sources.Any)
    {
        if (source == SteamVR_Input_Sources.Any)
        {
            hapticAction.Execute(delay, duration, frequency, amplitude, SteamVR_Input_Sources.LeftHand);
            hapticAction.Execute(delay, duration, frequency, amplitude, SteamVR_Input_Sources.RightHand);
        }
        else
        {
            hapticAction.Execute(delay, duration, frequency, amplitude, source);
        }
    }
}
