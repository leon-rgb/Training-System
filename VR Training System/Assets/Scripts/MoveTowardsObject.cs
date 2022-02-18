using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// usef for testing purposes only
/// </summary>
public class MoveTowardsObject : MonoBehaviour
{
    private Transform ThisTransform;
    public Transform Target;
    public float RotateSpeed;

    private void Awake()
    {
        ThisTransform = transform;    
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TrackRotation(Target));
    }

    IEnumerator TrackRotation(Transform target)
    {
        while (true)
        {
            if(ThisTransform != null && Target != null)
            {
                Vector3 relativePos = Target.position - ThisTransform.position;

                //Calculate rotation to target
                Quaternion NewRotation = Quaternion.LookRotation(relativePos);

                //Rotate to target by speed
                ThisTransform.rotation = Quaternion.RotateTowards(ThisTransform.rotation, NewRotation, RotateSpeed * Time.deltaTime);
            }
            //wait for next frame
            yield return null;
        }
    }

    /*
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(ThisTransform.position, ThisTransform.forward.normalized * 5f);
    }*/

    // Update is called once per frame
 
}
