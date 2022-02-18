using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Management;


/// <summary>
/// provides most of the neccesary steps to switch between vr and pc scenes
/// </summary>
public class SwitchBetweenVRAndPC : MonoBehaviour
{

    public GameObject SteamVRObjects;
    float aspect;
    private void Awake()
    {
        
        if (SceneManager.GetActiveScene().name != "CuttingPlaneCreation")
        {
            
            StartXR();
            GetComponent<Camera>().ResetAspect();
            //SteamVR.Initialize();
        }
        else
        {
            
            StopXR();
            //SteamVR.enabled = false;
            Destroy(GameObject.Find("Player"));
            Destroy(GameObject.Find("SawForController"));
            GetComponent<Camera>().ResetAspect();
            GetComponent<Camera>().ResetAspect();
        }           
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
        //XRSettings.enabled = true;       
        StartCoroutine(StartXRCoroutine());
        //SteamVR.enabled = true;
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
            //SteamVR.Initialize();
            //SteamVRObjects.SetActive(true);
            //SteamVR.Initialize();
        }
    }

    public void StopXR()
    {
        //XRSettings.enabled = false;
        //SteamVR.enabled = false;
        //SteamVRObjects.SetActive(false);
        Debug.Log(XRGeneralSettings.Instance.Manager.activeLoader);      
        if (XRGeneralSettings.Instance.Manager.activeLoader == null) return;

        Debug.Log("Stopping XR...");
        XRGeneralSettings.Instance.Manager.StopSubsystems();
        XRGeneralSettings.Instance.Manager.DeinitializeLoader();
        //SteamVR.enabled = false;
        Debug.Log("XR stopped completely. " + XRGeneralSettings.Instance.Manager.activeLoader);        
    }
}
