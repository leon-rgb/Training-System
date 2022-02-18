using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// used for testing purposes only
/// </summary>
public class SawAndHandleMovement : MonoBehaviour
{
    public GameObject ObjectToMoveAlong;
    private Vector3 Offset;
    private Vector3 RotOffset;
   
    // Start is called before the first frame update
    void Start()
    {
        //ObjectToMoveAlong = this.transform.parent.gameObject;
        Debug.Log("parent is   " + ObjectToMoveAlong);
        Offset = this.transform.position - ObjectToMoveAlong.transform.position;
        RotOffset = this.transform.eulerAngles - ObjectToMoveAlong.transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        ObjectToMoveAlong.transform.position = this.transform.position - Offset;
        ObjectToMoveAlong.transform.eulerAngles = this.transform.eulerAngles - RotOffset;
    }
}
