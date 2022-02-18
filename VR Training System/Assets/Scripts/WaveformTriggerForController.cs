using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;
using System;

/// <summary>
/// provides haptic feedback for the controllers
/// </summary>
public class WaveformTriggerForController : MonoBehaviour
{   
    public GameObject controllerLeft;
    public GameObject controllerRight;
    private Hand handLeft;
    private Hand handRight;
    private Hand hand;
    [Header("Select haptic actionset")]
    public SteamVR_Action_Vibration hapticAction;
    public GameObject sawForController;
    private Interactable sawForControllerI;

    //Timeout for sending haptic feedback in cutting Plane. Scaled appropriately for bones and cutToDeep
    public float timeoutLength { get; set; }
    public bool HapticsEnabled { get; set; }

    private bool cuttingPlaneTimeout;
    private float cuttingPlaneStartTime;

    private bool cutTooDeepTimeout;
    private float cutTooDeepStartTime;

    private bool bonesTimeout;
    private float bonesStartTime;

    float curTime;

    public Transform mainTransform;
    private MainScript main;

    // Start is called before the first frame update
    void Start()
    {
        // init variables
        Debug.Log("Haptics enabled = " + HapticsEnabled);
        handLeft = controllerLeft.GetComponent<Hand>();
        handRight = controllerRight.GetComponent<Hand>();
        //GetComponent<Rigidbody>().sleepThreshold = 0.0f;
        sawForControllerI = sawForController.GetComponent<Interactable>();
        
        cuttingPlaneTimeout = false;
        cutTooDeepTimeout = false;
        bonesTimeout = false;

        main = mainTransform.GetComponent<MainScript>();
    }

    // Update is called once per frame
    void Update()
    {
        // check if any timout has to be reseted
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
        if (bonesTimeout)
        {
            if (curTime - timeoutLength*0.4f > bonesStartTime)
            {
                bonesTimeout = false;
            }
        }
    }

    /// <summary>
    /// sends haptic feedback and accuracy information when spheres are hit by the saw
    /// feedback and information is sent corresponding to the type of sphere that was hit
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CuttingPlane") && !cuttingPlaneTimeout)
        {
            cuttingPlaneTimeout = true;
            cuttingPlaneStartTime = Time.time;
            hand = getAttachedHand();
            Debug.Log("hallo plane");
            if (hand != null && HapticsEnabled)
            {
                StartCoroutine(sendThreePulses(hand));
                Debug.Log("WAVEFORMCOLLISION  " + hand);
            }

        }
        else if (other.CompareTag("Bones") && !bonesTimeout)
        {
            bonesTimeout = true;
            bonesStartTime = Time.time;
            hand = getAttachedHand();
            if (hand != null && HapticsEnabled)
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
            if (hand != null && HapticsEnabled)
            {
                sendPulse(0.5f, 160, 200, hand);
                Debug.Log("WAVEFORMCOLLISION ToDeep  " + hand);
            }
        }

    }

    /// <summary>
    /// updates cutTooDeep length 
    /// </summary>
    /// <param name="other"></param>
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

    /// <summary>
    /// checks which hand holds the saw
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// sends haptic feedback to both controllers or to the other hand than which is sent as parameter
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="frequency"></param>
    /// <param name="amplitude"></param>
    /// <param name="hand"></param>
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

    /// <summary>
    /// sends three short haptic pulses to a hand or both hands (see sendPulse function)
    /// </summary>
    /// <param name="hand"></param>
    /// <returns></returns>
    IEnumerator sendThreePulses(Hand hand)
    {
        sendPulse(0.25f, 30, 40, hand);
        yield return new WaitForSeconds(0.45f);
        sendPulse(0.25f, 30, 40, hand);
        yield return new WaitForSeconds(0.45f);
        sendPulse(0.25f, 30, 40, hand);
    }
}
