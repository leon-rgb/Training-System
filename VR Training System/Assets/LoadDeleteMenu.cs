using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadDeleteMenu : MonoBehaviour
{
    public GameObject prefab;
    private PlanePanelFunctionality planePanel;


    private void OnEnable()
    {
        InitPlanePanels();    
    }

    private void OnDisable()
    {
        DestroyPlanePanels();
    }

    public void InitPlanePanels()
    {
        foreach (JSON_Serializer.CuttingPlane plane in JSON_Serializer.LoadCuttingPlaneList().cuttingPlanes)
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
