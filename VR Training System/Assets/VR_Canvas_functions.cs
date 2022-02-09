using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VR_Canvas_functions : MonoBehaviour
{

    public GameObject LoadingMenuScroller;
    public CuttingAccuracy cuttingAccuracy;

    public void OpenLoadingScrollView()
    {
        LoadingMenuScroller.SetActive(true);
    }

    public static void LoadCuttingPlane(string planeName)
    {
        GameObject.Find("CutToDeepMeshGenerator").GetComponent<CuttingAccuracy>().ClearAccuracyData();
        JSON_Serializer.SetupCuttingPlaneCompletly(planeName);
    }

    public void ReloadCuttingPlane()
    {
        string planeName = PlayerPrefs.GetString(JSON_Serializer.StringNamePlayerPrefs);
        cuttingAccuracy.ClearAccuracyData();
        JSON_Serializer.SetupCuttingPlaneCompletly("");
    }

    /// <summary>
    /// Sets up a random cutting plane (but never the same as the current one)
    /// </summary>
    public void NextCuttingPlane()
    {

    }
}
