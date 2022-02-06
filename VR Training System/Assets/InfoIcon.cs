using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
