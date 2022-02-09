using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// only needed for vr scenes. Makes it possible to load all cutting planes before scene starts and closes plane planel after that
/// </summary>
public class CloseLoadMenuAfterStart : MonoBehaviour
{
    public GameObject LoadMenu;
    // Start is called before the first frame update
    void Start()
    {
        LoadMenu.SetActive(false);
    }
}
