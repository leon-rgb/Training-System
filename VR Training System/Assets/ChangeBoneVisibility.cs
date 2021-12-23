using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBoneVisibility : MonoBehaviour
{
    [SerializeField]
    private Transform saw; //I attached a sphere to the saw, since the original saw blade seems to have weird outputs for its transform.position
    private bool wasAlreadyPlayed;
    public float radiusMultiplier;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        wasAlreadyPlayed = false;
    }
    private void Update()
    {
        if (Vector3.Distance(saw.position, MeshGeneratorLeg.meshMiddlePoint) <= MeshGeneratorLeg.meshRadius * radiusMultiplier)
        {
            if (!wasAlreadyPlayed)
            {
                wasAlreadyPlayed = true;
                //animator.SetFloat("direction", 1f);
                //animator.speed = 1f;
                animator.SetBool("playReversed", false);
                //animator.playbackTime = 0f;
                animator.enabled = true;                
                Debug.Log("THIS IS A IF TEST");
               
            }
        }
        else if (wasAlreadyPlayed)
        {
            Debug.Log("THIS IS A ELSE TEST");
            wasAlreadyPlayed = false;
            animator.SetBool("playReversed", true);
            //animator.SetFloat("direction", -1f);
            //animator.speed = -1f;
            //animator.playbackTime = animator.recorderStopTime;
            
        }
        //Debug.Log("MESH MID: " + MeshGeneratorLeg.meshMiddlePoint + "  " + MeshGeneratorLeg.meshRadius * radiusMultiplier + "  " + Vector3.Distance(saw.position, MeshGeneratorLeg.meshMiddlePoint) + "  sawpos: " + saw.position);
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(1f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(MeshGeneratorLeg.meshMiddlePoint, MeshGeneratorLeg.meshRadius * radiusMultiplier);
    }
}