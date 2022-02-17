using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagement : MonoBehaviour
{
    public void Start()
    {
        string version = JSON_Serializer.GetVersion();
        if (version == null)
        {
            Debug.LogError("no version found -> automatically using controllers");
            GetComponent<Button>().onClick.AddListener(LoadControllerScene);
            return;
        }

        Debug.Log("version : " + version);
        // load glove scene if version string contains g
        if(version.Contains("g"))
        {
            GetComponent<Button>().onClick.AddListener(LoadGlovesScene);
            return;
        }

        // load controller scene if no g was found in second line of verson file
        GetComponent<Button>().onClick.AddListener(LoadControllerScene);

        /*
        if (!GameObject.Find("VR_Scene_Info").GetComponent<VR_Scene_Info>()) return;

        VR_Scene_Info vr_info = GameObject.Find("VR_Scene_Info").GetComponent<VR_Scene_Info>();
        if (!vr_info.IsGloveScene)
        {
            GetComponent<Button>().onClick.AddListener(LoadControllerScene);
            return;
        }

        GetComponent<Button>().onClick.AddListener(LoadGlovesScene);
        */
    }

    public void LoadCuttingPlaneCreation()
    {
        SceneManager.LoadScene("CuttingPlaneCreation");
    }

    public void LoadControllerScene()
    {
        Debug.Log("LOADING... Controller_SurgeryRoom");
        SceneManager.LoadScene("Controller_SurgeryRoom");
    }
    
    public void LoadGlovesScene()
    {
        Debug.Log("LOADING... Gloves_SurgeryRoom");
        SceneManager.LoadScene("Gloves_SurgeryRoom");
    }
}
