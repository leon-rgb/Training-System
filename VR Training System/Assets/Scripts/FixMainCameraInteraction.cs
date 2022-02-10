using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixMainCameraInteraction : MonoBehaviour
{
    public Camera screenshotCamera;

    void OnPreRender()
    {
        screenshotCamera.Render();
    }
}
