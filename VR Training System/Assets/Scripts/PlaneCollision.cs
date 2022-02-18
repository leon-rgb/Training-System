using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// [Deprecated] old version of changing the visibility of the bone
/// </summary>
public class PlaneCollision : MonoBehaviour
{
    public GameObject leg;
    private int enterCount;
    private int exitCount;
    private bool wasAlreadyPlayed=false;
    public float radiusMultiplier;
    private Animator animator;

    private void Awake()
    {
        //GetComponent<Animator>().parameters[0];
        animator = leg.GetComponent<Animator>();
    }
    private void Update()
    {
       if (Vector3.Distance(transform.position, MeshGeneratorLeg.meshMiddlePoint) <= MeshGeneratorLeg.meshRadius*radiusMultiplier)
        {
            if (!wasAlreadyPlayed)
            {
                wasAlreadyPlayed = true;
                //leg.GetComponent<Animation>().Play();
                animator.SetFloat("direction", 1f);
                animator.enabled = true;
                Debug.Log("THIS IS A IF TEST");
            }           
        }
        else if (wasAlreadyPlayed)
        {
            Debug.Log("THIS IS A ELSE TEST");
            wasAlreadyPlayed = false;
            animator.SetFloat("direction", -1f);
            /*Animation animation = leg.GetComponent<Animation>();    
            animation.Rewind();
            animation.Play();
            animation.Sample();
            animation.Stop();*/
        }
    }

    /**
    private void Awake()
    {
        enterCount = 0;
        exitCount = 0;
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Outside TRIGGER" + other);
        if (other.CompareTag("Bones"))
        {
            enterCount++;
            if(enterCount - exitCount > 0)
            {
                Debug.Log("TRIGGER ENTERED");
                leg.GetComponent<Animation>().Play();
            }     
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Bones"))
        {
            exitCount++;
            if(exitCount - exitCount == 0)
            {
                Animation animation = leg.GetComponent<Animation>();
                animation.Rewind();
                animation.Play();
                animation.Sample();
                animation.Stop();
                Debug.Log("LEFT TRIGGER");
            }           
        }
    }*/
}
