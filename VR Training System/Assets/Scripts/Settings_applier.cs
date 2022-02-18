using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// manages the settings saved
/// </summary>
public class Settings_applier: MonoBehaviour
{

    public static Settings.SettingsObject settings;
    public GameObject[] SpherePrefabs;
    private GameObject meshGenerator;
    private GameObject cuttingAccuracy;
    private GameObject sawHolo;
    private GameObject waveformCol; 

    private void OnEnable()
    {
        //Todo path setzen von den settings, da dieser nicht existiert wenn script nicht aktiv ist
        //persistent data path in Settings und json verwenden....       

        cuttingAccuracy = GameObject.Find("CutToDeepMeshGenerator");
        meshGenerator = GameObject.Find("PlaneMeshGenerator");
        waveformCol = GameObject.FindGameObjectWithTag("Sawblade");
        sawHolo = GameObject.FindGameObjectWithTag("SawHolo");

        Settings.savePath = Application.dataPath + "/settings.txt";

        // if application is launched for the first time set values manually
        if (!File.Exists(Settings.savePath))
        {
            settings = Settings.SettingsOnFirstLaunch();
        }
        else
        {
            settings = Settings.Load();
        }
        

        if (SceneManager.GetActiveScene().name == "CuttingPlaneCreation")
        {
            ApplySettingsNonVR(true);
            return;
        }
        ApplySettingsVR();
    }

    public void ApplySettingsNonVR(bool IsInitialCall)
    {
        if (File.Exists(Settings.savePath)) settings = Settings.Load();
        SetSpheresFrequency(settings.FrequencySpheres);
        SetDistance(settings.DistanceCutTooDeep);
        SetSpheresVisibility(settings.ShowSpheres);
        Debug.Log("show spheres? --> " + settings.ShowSpheres);
        
        // if it's not the first call of this method generate new mesh to make visibility changes visible.
        if (!IsInitialCall)
        {
            meshGenerator.GetComponent<MeshGeneratorLeg>().CreateNewMesh();
            cuttingAccuracy.GetComponent<CuttingAccuracy>().CreateNewMesh();
        }
    }

    public void ApplySettingsVR()
    {
        settings = Settings.Load();
        SetSpheresFrequency(settings.FrequencySpheres);
        SetDistance(settings.DistanceCutTooDeep);
        SetHoloSawVisibility(settings.ShowAnimation);
        SetSpheresVisibility(settings.ShowSpheres);
        SetDifficulty(settings.Difficulty);
        SetHapticFrequency(settings.FrequencyHaptic);
    }

    IEnumerator WaitShort()
    {
        yield return new WaitForSeconds(3);
        settings = GetSettings();
        SetHoloSawVisibility(settings.ShowAnimation);
    }

    public static Settings.SettingsObject GetSettings()
    {
        return Settings.Load();
    }

    public void SetHoloSawVisibility(bool isVisible) 
    {
        //Debug.Log("isvisible : " + isVisible);
        sawHolo.SetActive(isVisible);
    }

    public void SetSpheresVisibility(bool isVisible)
    {
        foreach(Transform t in meshGenerator.transform)
        {
            t.GetComponent<Renderer>().enabled = isVisible;
        }
        foreach(Transform t in cuttingAccuracy.transform)
        {
            t.GetComponent<Renderer>().enabled = isVisible;
        }
    }

    public void SetDifficulty(int dif)
    {
        // since integer division rounds, upcast int to decimal
        Decimal dif_decimal = dif;
        // scales x to 0.6 for default difficulty 
        //-> means that gradient of function is 0.6 of normal gradient
        cuttingAccuracy.GetComponent<CuttingAccuracy>().Difficulty = (float) dif_decimal / 5;
        Debug.Log("Difficulty is " + (float)dif_decimal / 5);
    }

    public void SetDistance(int dist)
    {
        cuttingAccuracy.GetComponent<CuttingAccuracy>().distToCuttingMeshCoef = dist;
    }

    public void SetSpheresFrequency(float freq)
    {
        meshGenerator.GetComponent<MeshGeneratorLeg>().sphereFrequencyCoef = freq;
    }

    public void SetHapticFrequency(int freq)
    {
        Debug.Log("haptic timeout = " + freq * 1.25f);
        // check if controller is used
        if (waveformCol.GetComponent<WaveformTriggerForController>())
        {
            waveformCol.GetComponent<WaveformTriggerForController>().timeoutLength = freq * 1.25f;
            if (freq == 0)
            {
                waveformCol.GetComponent<WaveformTriggerForController>().HapticsEnabled = false;
                return;
            }
            waveformCol.GetComponent<WaveformTriggerForController>().HapticsEnabled = true;
            return;
        }

        // check if gloves are used
        if (waveformCol.GetComponent<WaveformTrigger>())
        {
            waveformCol.GetComponent<WaveformTrigger>().timeoutLength = freq * 1.25f;
            if (freq == 0)
            {
                waveformCol.GetComponent<WaveformTrigger>().HapticsEnabled = false;
                return;
            }
            waveformCol.GetComponent<WaveformTrigger>().HapticsEnabled = true;
            return;
        }

        // if we are here something went wrong
        Debug.LogWarning("haptic settings are corrupted");
    }

}
