using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateHoloSawBasedOnSawPosition : MonoBehaviour
{
    public Transform CuttingPlane;
    public Transform Saw;
    [SerializeField]
    private Transform topCuttingPointTransform;
    [SerializeField]
    private Transform botCuttingPointTransform;
    private Vector3 PosRight;
    private Vector3 PosLeft;
    private Vector3 AngleRight;
    private Vector3 AngleLeft;
    private float DistRight;
    private float DistLeft;
    private bool IsEnabled;
    [Tooltip("Any child of the holoSaw -> to calculate distance between saw and holoSaw")]
    public Transform battery;

    /// <summary>
    /// Start is called once upon starting the script. Here the HoloSaw will be rotated around the cutting plane once in x direction. 
    /// Both Positions and rotations will be safed for later comparism in th Update function.
    /// </summary>
    void Start()
    {    
        EnableHoloSawRotation();

        //place saw in front of cutting plane
        Vector3 topCuttingPoint = topCuttingPointTransform.position;
        Vector3 botCuttingPoint = botCuttingPointTransform.position;
        Vector3 initialPos = (topCuttingPoint + botCuttingPoint) / 2;
        initialPos.x = transform.parent.position.x;
        transform.parent.position = initialPos;

        // tried to rotate saw to angle between top and bot point, but doesnt work.
        /* Vector3 topNorm = topCuttingPoint.normalized;
        Vector3 botNorm = botCuttingPoint.normalized;
        Vector3 cross = Vector3.Cross(topNorm,botNorm);
        float dot = Vector3.Dot(topNorm, botNorm);
        Quaternion facingRot = transform.parent.rotation;
        Vector3 dir = (topCuttingPoint - botCuttingPoint).normalized;
        transform.parent.rotation = Quaternion.LookRotation(dir);
        Vector3 euler = transform.parent.localEulerAngles;
        euler.x += 90;
        transform.parent.localEulerAngles = euler;*/
        


        //safe left and right rotated holoSaw
        PosRight = this.transform.position;
        AngleRight = this.transform.eulerAngles;
        Vector3 RotPoint = CuttingPlane.position;
        //this.transform.RotateAround(RotPoint, Vector3.left, 180f);
        //mit der neuen Rotation, wird um einen Mittelpunkt im Saegeblatt rotiert, was viel genauer ist
        this.transform.RotateAround(transform.parent.position, Vector3.left, 180f);
        PosLeft = this.transform.position;
        AngleLeft = this.transform.eulerAngles;
        this.transform.RotateAround(transform.parent.position, Vector3.left, 180f);

    }

 
    
    /// <summary>
    ///  Update is called once per frame.
    ///  Decide where to place the Holo Saw depending on positon of Saw that the user will carry
    /// </summary>
    void Update()
    {
        if (IsEnabled)
        {
            DistRight = Vector3.Distance(Saw.position, PosRight);
            DistLeft = Vector3.Distance(Saw.position, PosLeft);
            if (DistLeft < DistRight)
            {
                this.transform.position = PosLeft;
                this.transform.eulerAngles = AngleLeft;
            }
            else
            {
                this.transform.position = PosRight;
                this.transform.eulerAngles = AngleRight;
            }
        }    
    }

    public void EnableHoloSawRotation()
    {
        IsEnabled = true;
    }

    public void DisableHoloSawRotation()
    {
        IsEnabled = false;
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(PosRight, 0.05f);
        Gizmos.DrawSphere(PosLeft, 0.05f);
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(Saw.position, 0.05f);
    }
}
