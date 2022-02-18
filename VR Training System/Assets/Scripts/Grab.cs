using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;

/// <summary>
/// for testing purposes only. Was made by a fellow student @duc
/// </summary>
public class Grab : MonoBehaviour
{
    private List<GameObject> objects = new List<GameObject>();
    public GameObject saw;
    private GameObject go;
    public HandRole handRole;

    void Update()
    {
        if (GameObject.ReferenceEquals(saw, go))
        {
            if (ViveInput.GetPress(handRole, ControllerButton.FullTrigger))
            {
                saw.transform.parent = this.transform;
                Rigidbody rb = saw.GetComponent<Rigidbody>();
                saw.transform.localPosition = new Vector3(0, -0.08f, -0.032f);
                saw.transform.localEulerAngles = new Vector3(-25f, 0, 0);
                rb.isKinematic = true;
                rb.useGravity = false;

            }
            else if (ViveInput.GetPress(handRole, ControllerButton.Grip))
            {
                saw.transform.parent = null;
                Rigidbody rb = saw.GetComponent<Rigidbody>();
                rb.isKinematic = true;
                rb.useGravity = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        go = other.gameObject;
    }

    void OnTriggerExit(Collider other)
    {
        go = null;
    }
}
