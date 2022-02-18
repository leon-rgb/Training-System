using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// experimental feature that wasn't used in the end
/// </summary>
public class CreateLineToCuttingPlane : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        /*
        GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        cylinder.transform.position = this.transform.position;
        cylinder.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        */
        LineRenderer lineRenderer = GameObject.Find("CuttingLine").GetComponent<LineRenderer>();
        lineRenderer.SetPositions(new Vector3[] { this.transform.position, GameObject.Find("CuttingPlane").transform.position });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
