using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveformTrigger : MonoBehaviour
{
    /*
    public GameObject WaveformControllerObject;
    private WaveformController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = WaveformControllerObject.GetComponent<WaveformController>();
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
            controller.ApplyWaveform("CutPlane");
        }
        else if(other.CompareTag("Bones"))
        {
            Debug.Log("WAVEFORMCOLLISION bone");
            controller.ApplyWaveform("CutBones");
        }
    }*/

    public WaveformController controller;

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

    public MainScript main;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Haptics enabled = " + HapticsEnabled);
        //GetComponent<Rigidbody>().sleepThreshold = 0.0f;

        cuttingPlaneTimeout = false;
        cutTooDeepTimeout = false;
        bonesTimeout = false;
    }

    // Update is called once per frame
    void Update()
    {
        curTime = Time.time;
        if (cutTooDeepTimeout)
        {
            //Debug.Log("timeout left : " + (timeoutLength - (curTime-cutToDeepStartTime)));
            if (curTime - timeoutLength / 10 > cutTooDeepStartTime)
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
            if (curTime - timeoutLength * 0.4f > bonesStartTime)
            {
                bonesTimeout = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CuttingPlane") && !cuttingPlaneTimeout)
        {
            cuttingPlaneTimeout = true;
            cuttingPlaneStartTime = Time.time;
            if (HapticsEnabled)
            {
                controller.ApplyWaveform("CutPlane");
                Debug.Log("WAVEFORMCOLLISION  ");
            }

        }
        else if (other.CompareTag("Bones") && !bonesTimeout)
        {
            bonesTimeout = true;
            bonesStartTime = Time.time;
            if (HapticsEnabled)
            {
                controller.ApplyWaveform("CutBones");
                Debug.Log("WAVEFORMCOLLISION bone");
            }
        }
        else if (other.CompareTag("CutToDeep") && !cutTooDeepTimeout)
        {
            cutTooDeepTimeout = true;
            cutTooDeepStartTime = Time.time;
            main.OnCutTooDeepEnter(transform.position);
            if (HapticsEnabled)
            {
                controller.ApplyWaveform("CutTooDeep");
                Debug.Log("WAVEFORMCOLLISION ToDeep ");
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
}
