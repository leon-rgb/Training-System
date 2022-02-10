using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlanePanelFunctionality : MonoBehaviour
{
    public Transform Name;
    public Transform IsFlatToggle;
    public Transform Image;

    public void SetElements(string name, bool isFlat, Texture image)
    {
        SetName(name);
        SetFlatness(isFlat);
        SetImage(image);
    }

    public void SetName(string name)
    {
        Name.GetComponent<TextMeshProUGUI>().text = name;
    }

    public void SetFlatness(bool isFlat)
    {
        Debug.Log("toggle val: " + isFlat);
        if (isFlat)
        {
            IsFlatToggle.GetComponent<Toggle>().isOn = true;
            return;
        }
        IsFlatToggle.GetComponent<Toggle>().isOn = false;
    }

    public void SetImage(Texture image)
    {
        Image.GetComponent<RawImage>().texture = image;
    }

    public void Load()
    {
        string planeName = Name.GetComponent<TextMeshProUGUI>().text;
        Debug.Log("LOADED " + planeName);
        
        // display pop up text if in cutting plane creation
        if (SceneManager.GetActiveScene().name == "CuttingPlaneCreation") {
            JSON_Serializer.SetupCuttingPlaneCompletly(planeName);
            GameObject.FindGameObjectWithTag("ui").GetComponent<UI_Manager>().ShowPopUpText("\"" + planeName + "\"  was loaded!");
        } 
        else
        {
            GameObject.Find("Canvases").GetComponent<VR_Canvas_functions>().LoadCuttingPlane(planeName);
        }
        // disable load menu
        GameObject.FindGameObjectWithTag("LoadMenu").SetActive(false);
    }

    public void Delete()
    {
        DeletePanel.planeName = Name.GetComponent<TextMeshProUGUI>().text;
        GameObject ui = GameObject.FindGameObjectWithTag("ui");
        ui.GetComponent<UI_Manager>().OpenSubMenu("DeletePanel");

    }

    private IEnumerator DestroyPanels(GameObject loadDeleteMenu)
    {
        yield return new WaitForSeconds(0.1f);
    }
}