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

    /// <summary>
    /// Opening or closing the scroll view. Depending on it's current state.
    /// </summary>
    public void OpenLoadingScrollView()
    {
        LoadingMenuScroller.SetActive(!LoadingMenuScroller.activeInHierarchy);
    }

    /// <summary>
    /// Setup a cutting plane by it's name
    /// </summary>
    /// <param name="planeName"></param>
    public void LoadCuttingPlane(string planeName)
    {
        // clear accuracy and setup plane
        cuttingAccuracy.ClearAccuracyData();
        JSON_Serializer.SetupCuttingPlaneCompletly(planeName);

        // Reset HoloSaw
        SawHolo.SetActive(true);
        SawHolo.GetComponent<RotateHoloSawBasedOnSawPosition>().Start();
        SawHolo.GetComponentInChildren<SawAnimationGenerator>().ResetEverything();
    }

    /// <summary>
    /// Reload the current setup plane
    /// </summary>
    public void ReloadCuttingPlane()
    {
        // Get name of current plane
        string planeName = PlayerPrefs.GetString(JSON_Serializer.StringNamePlayerPrefs);

        // Setup the plane
        cuttingAccuracy.ClearAccuracyData();
        JSON_Serializer.SetupCuttingPlaneCompletly(planeName);

        // Reset HoloSaw
        SawHolo.SetActive(true);
        SawHolo.GetComponent<RotateHoloSawBasedOnSawPosition>().Start();
        SawHolo.GetComponentInChildren<SawAnimationGenerator>().ResetEverything();   
    }

    /// <summary>
    /// Sets up a random cutting plane (but never the same as the current one except only 1 exists)
    /// </summary>
    public void NextCuttingPlane()
    {

        string currentPlane = PlayerPrefs.GetString(JSON_Serializer.StringNamePlayerPrefs);
        List <JSON_Serializer.CuttingPlane> planeList = JSON_Serializer.LoadCuttingPlaneList().cuttingPlanes;

        // check if only one plane exists --> load default and return if true
        if(planeList == null || planeList.Count == 1)
        {
            JSON_Serializer.SetupCuttingPlaneCompletly(currentPlane);
            Debug.Log("Loaded Same Cutting Plane since only 1 exists");
            return;
        }

        // Get random plane until it is a new one
        int rand = 0;
        JSON_Serializer.CuttingPlane plane = null;
        plane = GetRandomPlane(planeList, rand, plane);

        while(currentPlane == plane.name)
        {
            plane = GetRandomPlane(planeList, rand, plane);
        }
        Debug.Log("Loaded " + plane.name);

        // Setup the new plane
        cuttingAccuracy.ClearAccuracyData();
        JSON_Serializer.SetupCuttingPlaneCompletly(plane.name);
        // Reset HoloSaw
        SawHolo.SetActive(true);
        SawHolo.GetComponent<RotateHoloSawBasedOnSawPosition>().Start();
        SawHolo.GetComponentInChildren<SawAnimationGenerator>().ResetEverything();    
    }

    /// <summary>
    /// gets a random plane from saved planes
    /// </summary>
    /// <param name="planeList"></param>
    /// <param name="rand"></param>
    /// <param name="plane"></param>
    /// <returns></returns>
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
