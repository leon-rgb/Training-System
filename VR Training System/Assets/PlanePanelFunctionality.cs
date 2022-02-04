using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
        JSON_Serializer.SetupCuttingPlaneCompletly(Name.GetComponent<TextMeshProUGUI>().text);
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