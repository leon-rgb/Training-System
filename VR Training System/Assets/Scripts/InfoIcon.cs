using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// used in the settings menu to display more information about some settings
/// </summary>
public class InfoIcon : MonoBehaviour
{
    public GameObject Content;
    public void EnableContent()
    {
        Content.SetActive(true);
    }

    public void DisableContent()
    {
        Content.SetActive(false);
    }
}
