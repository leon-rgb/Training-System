using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// only used for testing purposes
/// </summary>
public class VR_Scene_Info : MonoBehaviour
{
    public bool IsGloveScene = false;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (SceneManager.GetActiveScene().name == "Gloves_SurgeryRoom") IsGloveScene = true;
    }
}
