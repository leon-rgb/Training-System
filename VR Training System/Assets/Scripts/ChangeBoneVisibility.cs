using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Is used for changing bone visibility depending on how close the saw is to the bones
/// </summary>
public class ChangeBoneVisibility : MonoBehaviour
{
    [SerializeField]
    private Transform saw; //I attached a sphere to the saw, since the original saw blade seems to have weird outputs for its transform.position
   
    // For checking in which visibility state we are
    private bool wasAlreadyPlayed;

    [Tooltip("is used for setting the range in which visibility is changed")]
    public float radiusMultiplier;

    private Animator animator;

    private void Awake()
    {
        // init animator and bool
        animator = GetComponent<Animator>();
        wasAlreadyPlayed = false;
    }
    private void Update()
    {
        // check distance of saw to cutting plane
        if (Vector3.Distance(saw.position, MeshGeneratorLeg.meshMiddlePoint) <= MeshGeneratorLeg.meshRadius * radiusMultiplier)
        {
            if (!wasAlreadyPlayed)
            {
                //play animation if not already played and reset animation bool
                wasAlreadyPlayed = true;
                //animator.SetFloat("direction", 1f);
                //animator.speed = 1f;
                animator.SetBool("playReversed", false);
                //animator.playbackTime = 0f;
                animator.enabled = true;                
                //Debug.Log("THIS IS A IF TEST");
               
            }
        }
        else if (wasAlreadyPlayed)
        {
            // play reversed animation and reset bools.
            //Debug.Log("THIS IS A ELSE TEST");
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