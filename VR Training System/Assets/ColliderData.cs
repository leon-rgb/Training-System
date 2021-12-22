using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderData : MonoBehaviour
{
    void Start()
    {
        Collider col = GetComponent<Collider>();
        Debug.Log("Bounds: " + col.bounds.max);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
