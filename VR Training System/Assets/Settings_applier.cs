using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Settings_applier: MonoBehaviour
{

    private static Settings.SettingsObject settings;
    public GameObject[] SpherePrefabs;
    private GameObject meshGenerator;
    private GameObject cuttingAccuracy;
    private GameObject sawHolo;
    private GameObject waveformCol; 

    private void OnEnable()
    {
        //Todo path setzen von den settings, da dieser nicht existiert wenn script nicht aktiv ist
        //persistent data path in Settings und json verwenden....
        Settings.savePath = Application.dataPath + "/settings.txt";
        settings = Settings.Load();
        Debug.Log("test " + settings);

        cuttingAccuracy = GameObject.Find("CutToDeepMeshGenerator");
        meshGenerator = GameObject.Find("PlaneMeshGenerator");
        waveformCol = GameObject.FindGameObjectWithTag("Sawblade");
        sawHolo = GameObject.FindGameObjectWithTag("SawHolo");

        if (SceneManager.GetActiveScene().name == "CuttingPlaneCreation")
        {
            ApplySettingsNonVR(true);
            return;
        }
        ApplySettingsVR();
    }

    public void ApplySettingsNonVR(bool IsInitialCall)
    {
        settings = Settings.Load();
        SetSpheresFrequency(settings.FrequencySpheres);
        SetDistance(settings.DistanceCutTooDeep);
        SetSpheresVisibility(settings.ShowSpheres);
        Debug.Log(settings.ShowSpheres);
        
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
        Debug.Log("isvisible : " + isVisible);
        sawHolo.SetActive(isVisible);
    }

    public void SetSpheresVisibility(bool isVisible)
    {
        foreach(GameObject go in SpherePrefabs)
        {
            go.GetComponent<Renderer>().enabled = isVisible;
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
        // check if controller is used
        if (waveformCol.GetComponent<WaveformTriggerForController>())
        {
            Debug.Log("haptic timeout = " + freq * 1.25f);
            waveformCol.GetComponent<WaveformTriggerForController>().timeoutLength = freq * 1.25f;
            if (freq == 0)
            {
                waveformCol.GetComponent<WaveformTriggerForController>().HapticsEnabled = false;
                return;
            }
            waveformCol.GetComponent<WaveformTriggerForController>().HapticsEnabled = true;
            return;
        }
        // if we are here gloves are used
        Debug.LogWarning("settings not implemented for gloves");
    }

}
