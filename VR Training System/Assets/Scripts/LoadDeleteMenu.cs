using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadDeleteMenu : MonoBehaviour
{
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

    public void InitPlanePanels()
    {
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

    public void DestroyPlanePanels()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }
    }
}
