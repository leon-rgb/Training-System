using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveformTrigger : MonoBehaviour
{
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
    }
}
