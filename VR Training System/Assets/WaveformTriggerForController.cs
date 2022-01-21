using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;
using System;

public class WaveformTriggerForController : MonoBehaviour
{
    [Header("InGame TextFields")]
    public GameObject controllerLeft;
    public GameObject controllerRight;
    private Hand handLeft;
    private Hand handRight;
    private Hand hand;
    public SteamVR_Action_Vibration hapticAction;
    public GameObject sawForController;
    private Interactable sawForControllerI;

    [Tooltip("This is a tooltip")]
    public float timeoutLength;

    private bool cuttingPlaneTimeout;
    private float cuttingPlaneStartTime;

    private bool cutTooDeepTimeout;
    private float cutTooDeepStartTime;
    public float Depth { get; set; }
    public int CutToDeepCount { get; set; }
    private Vector3 depthStart;

    float curTime;

    public Transform mainTransform;
    private MainScript main;

    // Start is called before the first frame update
    void Start()
    {
        handLeft = controllerLeft.GetComponent<Hand>();
        handRight = controllerRight.GetComponent<Hand>();
        //GetComponent<Rigidbody>().sleepThreshold = 0.0f;
        sawForControllerI = sawForController.GetComponent<Interactable>();
        
        cuttingPlaneTimeout = false;
        cutTooDeepTimeout = false;

        main = mainTransform.GetComponent<MainScript>();
    }

    // Update is called once per frame
    void Update()
    {
        curTime = Time.time;
        if (cutTooDeepTimeout)
        {
            //Debug.Log("timeout left : " + (timeoutLength - (curTime-cutToDeepStartTime)));
            if(curTime -  timeoutLength/10 > cutTooDeepStartTime)
            {
                cutTooDeepTimeout = false;
                //Debug.Log("timeout ended");
            }
        }
        if (cuttingPlaneTimeout)
        {
            if (curTime - timeoutLength > cuttingPlaneStartTime)
            {
                cuttingPlaneTimeout = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CuttingPlane") && !cuttingPlaneTimeout)
        {
            cuttingPlaneTimeout = true;
            cuttingPlaneStartTime = Time.time;
            hand = getAttachedHand();
            Debug.Log("hallo plane");
            if (hand != null)
            {
                StartCoroutine(sendThreePulses(hand));
                Debug.Log("WAVEFORMCOLLISION  " + hand);
            }

        }
        else if (other.CompareTag("Bones"))
        {
            hand = getAttachedHand();
            if (hand != null)
            {
                sendPulse(0.5f, 70, 90, hand);
                Debug.Log("WAVEFORMCOLLISION bone  " + hand);
            }
        }
        else if (other.CompareTag("CutToDeep") && !cutTooDeepTimeout)
        {
            cutTooDeepTimeout = true;
            cutTooDeepStartTime = Time.time;
            main.OnCutTooDeepEnter(transform.position);
            Debug.Log("hallo " + other.name);
            hand = getAttachedHand();
            if (hand != null)
            {
                sendPulse(0.5f, 160, 200, hand);
                Debug.Log("WAVEFORMCOLLISION ToDeep  " + hand);
            }
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("CutToDeep"))
        {
            main.OnCutTooDeepStay(transform.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
       
    }

    /*
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collision detected with : " + collision.rigidbody.gameObject);
        if (collision.gameObject.CompareTag("CutToDeep"))
        {
            hand = getAttachedHand();
            if (hand != null)
            {
                sendPulse(0.5f, 120, 150, hand);
                Debug.Log("WAVEFORMCOLLISION ToDeep  " + hand);
            }
        }
    }*/

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

    private void sendDelayedPulse(float delay, float duration, float frequency, float amplitude, Hand hand = null)
    {
        if (hand == null)
        {
            hapticAction.Execute(delay, duration, frequency, amplitude, SteamVR_Input_Sources.LeftHand);
            hapticAction.Execute(delay, duration, frequency, amplitude, SteamVR_Input_Sources.RightHand);
        }
        else if (hand == handRight)
        {
            hapticAction.Execute(delay, duration, frequency, amplitude, SteamVR_Input_Sources.LeftHand);
        }
        else
        {
            hapticAction.Execute(delay, duration, frequency, amplitude, SteamVR_Input_Sources.RightHand);
        }
    }

    IEnumerator sendThreePulses(Hand hand)
    {
        sendPulse(0.25f, 30, 40, hand);
        yield return new WaitForSeconds(0.45f);
        sendPulse(0.25f, 30, 40, hand);
        yield return new WaitForSeconds(0.45f);
        sendPulse(0.25f, 30, 40, hand);
    }
}
