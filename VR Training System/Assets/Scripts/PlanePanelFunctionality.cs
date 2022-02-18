using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Functionality for init and delete panels in the scroll view
/// Is attached to every entry in the loading/deletion scroll view
/// </summary>
public class PlanePanelFunctionality : MonoBehaviour
{
    public Transform Name;
    public Transform IsFlatToggle;
    public Transform Image;

    /// <summary>
    /// initializes values of the entry
    /// </summary>
    /// <param name="name"></param>
    /// <param name="isFlat"></param>
    /// <param name="image"></param>
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
        //Debug.Log("toggle val: " + isFlat);
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

    /// <summary>
    /// sets up the cutting plane with the name that is attached to this panel
    /// </summary>
    public void Load()
    {
        string planeName = Name.GetComponent<TextMeshProUGUI>().text;
        // save plane as current plane in playerprefs
        PlayerPrefs.SetString(JSON_Serializer.StringNamePlayerPrefs, planeName);

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

    /// <summary>
    /// delete the cutting plane with the name that is attached to this panel
    /// </summary>
    public void Delete()
    {
        // if deleted plane was current plane, reset player pref
        if(PlayerPrefs.GetString(JSON_Serializer.StringNamePlayerPrefs) == Name.GetComponent<TextMeshProUGUI>().text)
        {
            PlayerPrefs.SetString(JSON_Serializer.StringNamePlayerPrefs, "");
        }
        DeletePanel.planeName = Name.GetComponent<TextMeshProUGUI>().text;
        GameObject ui = GameObject.FindGameObjectWithTag("ui");
        ui.GetComponent<UI_Manager>().OpenSubMenu("DeletePanel");

    }

    private IEnumerator DestroyPanels(GameObject loadDeleteMenu)
    {
        yield return new WaitForSeconds(0.1f);
    }
}