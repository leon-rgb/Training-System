using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Only used for testing purposes
/// </summary>
public class angleCalculator : MonoBehaviour
{

    public Transform point1;
    public Transform point2;
    private Vector3 midPoint;
    private float xPos;
    private float yScale;

    // Start is called before the first frame update
    void Start()
    {
        xPos = (point1.position.x + point2.position.x) / 1.985f;
        transform.position = transform.position + new Vector3(-transform.position.x + xPos ,0,0);

        yScale = Vector3.Distance(point1.position, point2.position) * 100;
        Debug.Log(yScale +"  YSCALE");
        transform.localScale = new Vector3 (transform.localScale.x, yScale, transform.localScale.z);

        
        midPoint = (point1.position + point2.position)/2;
        transform.LookAt(midPoint);
        Debug.Log("TEST " + midPoint);
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(midPoint, 0.001f);
    }
}
