using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// used for testing purposes only
/// </summary>
public class HMD_Manager : MonoBehaviour
{
    public GameObject realVRRig;
    public GameObject gloves;

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
            if (XRSettings.loadedDeviceName == "MockHMD Display")
            { 
                Debug.Log("the real vr Rig and the glvoes will be disabled due to MockHMD");
                realVRRig.SetActive(false);
                gloves.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
