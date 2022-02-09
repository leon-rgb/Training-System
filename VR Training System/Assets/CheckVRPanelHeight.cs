using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckVRPanelHeight : MonoBehaviour
{

    public Collider LoadButton;


    private void FixedUpdate()
    {
        if(transform.localPosition.y < -200)
        {
            LoadButton.enabled = false;
        }
    }
}
