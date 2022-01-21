using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
