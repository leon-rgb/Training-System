using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SaveMenu : MonoBehaviour
{

    public GameObject Json_serializer_go;
    private JSON_Serializer serializer;

    public GameObject EmptyName_go;
    private TextMeshProUGUI EmptyName;

    public GameObject NameExists_go;
    private TextMeshProUGUI NameExists;

    public GameObject NameInput_go;
    private TMP_InputField NameInput;

    public GameObject Toggle_go;
    private Toggle toggle;

    public GameObject Ui_manager_go;
    private UI_Manager Ui_Manager;

    public GameObject ScreenshotMaker_go;
    private ScreenshotMaker screenshotMaker;

    private string initValue;

    // Start is called before the first frame update
    void Start()
    {
        serializer = Json_serializer_go.GetComponent<JSON_Serializer>();
        EmptyName = EmptyName_go.GetComponent<TextMeshProUGUI>();
        NameInput = NameInput_go.GetComponent<TMP_InputField>();
        Ui_Manager = Ui_manager_go.GetComponent<UI_Manager>();
        screenshotMaker = ScreenshotMaker_go.GetComponent<ScreenshotMaker>();
        NameExists = NameExists_go.GetComponent<TextMeshProUGUI>();
        toggle = Toggle_go.GetComponent<Toggle>();
    }

    private void OnEnable()
    {
        Start();
        InitName();
    }

    private void OnDisable()
    {
        NameExists.enabled = false;
        EmptyName.enabled = false;
    }

    public void InitName()
    {
        Debug.Log(serializer);
        // note that this is null when game is started in the save menu (but this case should never happen)
        if(JSON_Serializer.LoadCount() == null)
        {
            initValue = "CuttingPlane" + 1;
            Debug.Log(NameInput);
            NameInput.text = initValue;
            return;
        }
        initValue = "CuttingPlane" + JSON_Serializer.LoadCount().value;
        NameInput.text = initValue;       
    }

    public void ClearInput()
    {
        NameExists.enabled = false;
        if (NameInput.text.Contains("CuttingPlane"))
        {
            NameInput.text = "";
            EmptyName.enabled = false;
        }
    }

    public void OnDeselectInput()
    {
        if(NameInput.text == "")
        {
            NameInput.text = initValue;
            EmptyName.enabled = true;
        }
    }

    public void ClickSave()
    {
        if(NameInput.text == "")
        {
            EmptyName.enabled = true;
            return;
        }

        //Debug.Log(serializer.SaveCuttingPlane(NameInput.text, toggle.isOn));
        Debug.Log(NameInput);
        Debug.Log(toggle);
        
        // if plane was not saved a plane with the input name does already exist
        if(!JSON_Serializer.SaveCuttingPlane(NameInput.text, toggle.isOn))
        {
            NameExists.enabled = true;
            return;
        }
        if (initValue == NameInput.text)
        {
            serializer.SaveCount();
        }

        // take screenshot and save it with planes name
        screenshotMaker.TakeScreenshot(NameInput.text);
        
        // disable ui panels and display that plane was saved
        Ui_Manager.DisablePanels();
        Ui_Manager.ShowPopUpText("\""+ NameInput.text + "\"  was saved!");
    }
}