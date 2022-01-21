using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingPlane_Sphere : MonoBehaviour
{
    public bool wasHit { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        wasHit = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sawblade"))
        {
            Debug.Log("cuttingSphere was hit");
            wasHit = true;
            GetComponent<MeshRenderer>().material.color = Color.blue;
            GetComponent<Collider>().enabled = false;
        }       
    }
}
