using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// manages the load/delete menu in the cutting plane creation scene
/// </summary>
public class LoadDeleteMenu : MonoBehaviour
{
    // prefab of the plane panels (in the scroll view)
    public GameObject prefab;
    private PlanePanelFunctionality planePanel;

    private void OnEnable()
    {
        if (SceneManager.GetActiveScene().name == "CuttingPlaneCreation") InitPlanePanels();
    }

    private void OnDisable()
    {
        if (SceneManager.GetActiveScene().name == "CuttingPlaneCreation") DestroyPlanePanels();
    }
    private void Start()
    {
        if (SceneManager.GetActiveScene().name != "CuttingPlaneCreation")
        {
            InitPlanePanels();
            GameObject.FindGameObjectWithTag("LoadMenu").SetActive(false);
        }
    }

    /// <summary>
    /// Load all saved cutting planes and create plane panel for them
    /// </summary>
    public void InitPlanePanels()
    {
        //If list does not exist load nothing
        JSON_Serializer.CuttingPlaneList cuttingPlaneList = JSON_Serializer.LoadCuttingPlaneList();
        if (cuttingPlaneList == null) return;

        foreach (JSON_Serializer.CuttingPlane plane in cuttingPlaneList.cuttingPlanes)
        {
            // instantiate prefab as child of this transform and set WorldPositionStays to false
            GameObject go = Instantiate(prefab, transform, false);

            //get corresponding image of the cutting plane
            Texture texture = ScreenshotMaker.GetScreenshot(plane.name);

            // fill name, flatness and image of the planePanel
            planePanel = go.GetComponent<PlanePanelFunctionality>();
            planePanel.SetElements(plane.name, plane.isAnimatable, texture);
        }
    }

    /// <summary>
    /// destroy all loaded plane panels
    /// </summary>
    public void DestroyPlanePanels()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }
    }
}
