using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Management;

public class SwitchBetweenVRAndPC : MonoBehaviour
{
    private void Awake()
    {
        if (SceneManager.GetActiveScene().name == "CuttingPlaneCreation")
        {
            StopXR();
            return;
        }
        StartXR();        
    }

    /*public static void EnableVR(bool enable)
    {
        if (enable)
        {
            Debug.Log("VR enabled");
            //XRGeneralSettings.Instance.Manager.InitializeLoader();
            //XRGeneralSettings.Instance.Manager.InitializeLoader();
            XRGeneralSettings.Instance.Manager.StartSubsystems();
        }
        else
        {
            Debug.Log("VR disabled");
            XRGeneralSettings.Instance.Manager.StopSubsystems();
            //XRGeneralSettings.Instance.Manager.DeinitializeLoader();
        }
    }*/

    public void StartXR()
    {
        SteamVR.enabled = true;
        StartCoroutine(StartXRCoroutine());
    }

    public IEnumerator StartXRCoroutine()
    {
        Debug.Log("Initializing XR...");
        yield return XRGeneralSettings.Instance.Manager.InitializeLoader();

        if (XRGeneralSettings.Instance.Manager.activeLoader == null)
        {
            Debug.LogError("Initializing XR Failed. Check Editor or Player log for details.");
        }
        else
        {
            Debug.Log("Starting XR..." + XRGeneralSettings.Instance.Manager.activeLoader);
            XRGeneralSettings.Instance.Manager.StartSubsystems();
        }
    }

    void StopXR()
    {
        Debug.Log("Stopping XR...");

        XRGeneralSettings.Instance.Manager.StopSubsystems();
        XRGeneralSettings.Instance.Manager.DeinitializeLoader();
        SteamVR.enabled = false;
        Debug.Log("XR stopped completely. " + XRGeneralSettings.Instance.Manager.activeLoader);
    }
}
