using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;

public class WaveformTriggerForController : MonoBehaviour
{
    public GameObject controllerLeft;
    public GameObject controllerRight;
    private Hand handLeft;
    private Hand handRight;
    private Hand hand;
    public SteamVR_Action_Vibration hapticAction;
    public GameObject sawForController;
    private Interactable sawForControllerI;
    //public SteamVR_Action_Boolean trackpadAction;

    // Start is called before the first frame update
    void Start()
    {
        handLeft = controllerLeft.GetComponent<Hand>();
        handRight = controllerRight.GetComponent<Hand>();
        GetComponent<Rigidbody>().sleepThreshold = 0.0f;
        sawForControllerI = sawForController.GetComponent<Interactable>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(sawForControllerI.attachedToHand != null)
        {
            if (sawForControllerI.attachedToHand == handRight)
            {
                Debug.Log(sawForControllerI.attachedToHand);
            }      
        }*/
    }

    private void OnTriggerEnter(Collider other)
    {    
        if (other.CompareTag("CuttingPlane"))
        {
            hand = getAttachedHand();
            if (hand != null)
            {
                sendPulse(0.3f, 10, 75, hand);
                Debug.Log("WAVEFORMCOLLISION  " + hand);
            }   
            
        }
        else if(other.CompareTag("Bones"))
        {
            hand = getAttachedHand();
            if (hand != null)
            {
                sendPulse(0.5f, 200, 120, hand);
                Debug.Log("WAVEFORMCOLLISION bone  " + hand);
            }
            Debug.Log("WAVEFORMCOLLISION bone");           
        }
    }

    private Hand getAttachedHand()
    {
        if(sawForControllerI.attachedToHand != null)
        {
            return sawForControllerI.attachedToHand;
        }
        else
        {
            Debug.Log("No Hand was connected when trying to send haptic feedback");
            return null;
        }
    }

    private void sendPulse(float duration, float frequency, float amplitude, Hand hand = null)
    {
        if(hand == null)
        {
            hapticAction.Execute(0, duration, frequency, amplitude, SteamVR_Input_Sources.LeftHand);
            hapticAction.Execute(0, duration, frequency, amplitude, SteamVR_Input_Sources.RightHand);
        }
        else if (hand == handRight)
        {
            hapticAction.Execute(0, duration, frequency, amplitude, SteamVR_Input_Sources.LeftHand);
        }
        else
        {
            hapticAction.Execute(0, duration, frequency, amplitude, SteamVR_Input_Sources.RightHand);
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
