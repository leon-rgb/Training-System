using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class GrabPositionSetter : MonoBehaviour
{
    //public Transform[] hands = new Transform[0];

    public Transform leftHandTransform;
    public Transform rightHandTransform;

    private Hand leftHand;
    private Hand rightHand;

    // Start is called before the first frame update
    void Start()
    {
        Hand leftHand = leftHandTransform.GetComponent<Hand>();
        Hand rightHand = rightHandTransform.GetComponent<Hand>();

    }

    public void SetCorrectPositionOfGrabable()
    {
        Debug.Log("left hand attached : " + rightHand.currentAttachedObject);
        Debug.Log("right hand attached : " + leftHand.currentAttachedObject);
    }

}
