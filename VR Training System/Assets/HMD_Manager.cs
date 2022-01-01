using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HMD_Manager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!XRSettings.isDeviceActive)
        {
            Debug.Log("No HMD connected");
        }
        else
        {
            Debug.Log("Connected HMD :   " + XRSettings.loadedDeviceName);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
