using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script attatched to the spheres outside the bone in the cutting plane
/// </summary>
public class Cutting_Sphere : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sawblade"))
        {
            GetComponent<MeshRenderer>().material.color = Color.cyan;
            GetComponent<Collider>().enabled = false;
        }
    }
}
