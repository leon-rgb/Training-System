using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Only used for testing purposes
/// </summary>
public class CalcBoneCollision : MonoBehaviour
{
    public MeshCollider BoneCollider;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Bones")) {
            print("true");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Outside TRIGGER" + other);
        if(other.CompareTag("Bones"))
        {
            print("TRIGGER ENTERED");
            
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        //Gizmos.DrawSphere(BoneCollider.bounds.min, 0.1f);
    }

}
