using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class VR_Canvas_functions : MonoBehaviour
{

    public GameObject LoadingMenuScroller;
    public MeshGeneratorLeg meshGenerator;
    public CuttingAccuracy cuttingAccuracy;
    public Settings_applier settings;
    public GameObject SawHolo;

    public void OpenLoadingScrollView()
    {
        LoadingMenuScroller.SetActive(!LoadingMenuScroller.activeInHierarchy);
    }

    public static void LoadCuttingPlane(string planeName)
    {
        // used find to have a static function
        GameObject.Find("CutToDeepMeshGenerator").GetComponent<CuttingAccuracy>().ClearAccuracyData();
        JSON_Serializer.SetupCuttingPlaneCompletly(planeName);
        GameObject go = GameObject.FindGameObjectWithTag("SawHolo");
        go.SetActive(false);
        go.GetComponent<RotateHoloSawBasedOnSawPosition>().enabled = false;
        go.SetActive(true);
        go.GetComponent<RotateHoloSawBasedOnSawPosition>().enabled = true;
    }

    public void ReloadCuttingPlane()
    {
        string planeName = PlayerPrefs.GetString(JSON_Serializer.StringNamePlayerPrefs);
        cuttingAccuracy.ClearAccuracyData();
        JSON_Serializer.SetupCuttingPlaneCompletly(planeName);
        SawHolo.SetActive(false);
        SawHolo.SetActive(true);
    }

    /// <summary>
    /// Sets up a random cutting plane (but never the same as the current one except only 1 exists)
    /// </summary>
    public void NextCuttingPlane()
    {

        string currentPlane = PlayerPrefs.GetString(JSON_Serializer.StringNamePlayerPrefs);
        List <JSON_Serializer.CuttingPlane> planeList = JSON_Serializer.LoadCuttingPlaneList().cuttingPlanes;

        if(planeList == null || planeList.Count == 1)
        {
            JSON_Serializer.SetupCuttingPlaneCompletly(currentPlane);
            Debug.Log("Loaded Same Cutting Plane since only 1 exists");
            return;
        }

        int rand = 0;
        JSON_Serializer.CuttingPlane plane = null;
        plane = GetRandomPlane(planeList, rand, plane);

        while(currentPlane == plane.name)
        {
            plane = GetRandomPlane(planeList, rand, plane);
        }
        Debug.Log("Loaded " + plane.name);
        SawHolo.GetComponent<RotateHoloSawBasedOnSawPosition>().Start();
        SawHolo.GetComponentInChildren<SawAnimationGenerator>().ResetEverything();
        JSON_Serializer.SetupCuttingPlaneCompletly(plane.name);
    }

    private JSON_Serializer.CuttingPlane GetRandomPlane(List<JSON_Serializer.CuttingPlane> planeList, int rand, JSON_Serializer.CuttingPlane plane)
    {
        rand = Random.Range(0, planeList.Count);
        plane = planeList[rand];
        return plane;
    }

    public void EnableSawAnimation()
    {
        Settings_applier.settings.ShowAnimation = !Settings_applier.settings.ShowAnimation;
        settings.SetHoloSawVisibility(Settings_applier.settings.ShowAnimation);
    }

    public void Show_Hide_CuttingSpheres()
    {
        Settings_applier.settings.ShowSpheres = !Settings_applier.settings.ShowSpheres;
        settings.SetSpheresVisibility(Settings_applier.settings.ShowSpheres);
        meshGenerator.CreateNewMesh();
    }
}
