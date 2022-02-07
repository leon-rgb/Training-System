using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public void LoadCuttingPlaneCreation()
    {
        SceneManager.LoadScene("CuttingPlaneCreation");
    }


    public void LoadControllerScene()
    {
        SceneManager.LoadScene("Controller_SurgeryRoom");
    }
    
    public void LoadGlovesScene()
    {
        Debug.Log("Gloves_SurgeryRoom");
    }
}
