using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RotateHoloSawBasedOnSawPosition;

public class CuttingPlane_Interaction : MonoBehaviour
{

    public GameObject CuttingPlane;
    public Material DefaultMaterial;
    public Material CorrectInteractionMaterial;
    private Vector3 OnTriggerEnterPosition;
    private Vector3 CuttingPlanePosition;
    private Quaternion OnTriggerEnterRotation;
    private Vector3 OnTriggerStayPosition;
    private Quaternion OnTriggerStayRotation;
    private Collider ThisCollider;
    private Collider CuttingPlaneCollider;
    [SerializeField]
    private float AngleOfPosition;
    private float AngleOfRotation;
    [SerializeField]
    private float OnTriggerEnterEulerAngleX;
    [SerializeField]
    private float OnTriggerEnterEulerAngleY;
    [SerializeField]
    private float OnTriggerEnterEulerAngleZ;
    [SerializeField]
    private float OnTriggerStayEulerAngleX;
    [SerializeField]
    private float OnTriggerStayEulerAngleY;
    [SerializeField]
    private float OnTriggerStayEulerAngleZ;
    [SerializeField]
    private float Distance;

    [SerializeField]
    private float AngleX;
    [SerializeField]
    private float AngleY;
    [SerializeField]
    private float AngleZ;


    public GameObject SawHolo;
    public Material SawRed;
    public Material SawGreen;
    private Vector3 SawHoloPos;
    private float SawHoloPosX;
    private float SawHoloPosY;
    private float SawHoloPosZ;
    private Vector3 SawHoloRot;
    private float SawHoloRotX;
    private float SawHoloRotY;
    private float SawHoloRotZ;
    private Vector3 Pos;
    private float PosX;
    private float PosY;
    private float PosZ;
    private Vector3 Rot;
    private float RotX;
    private float RotY;
    private float RotZ;
    private float EulerRange;
    private float PosRange;

    public GameObject Line;
    public GameObject LineStartPoint;

    private bool IsStartTimeAlreadySet;
    private bool SawHoldingTimeIsOK;
    private float NeededSawHoldingTime;
    private float CorrectSawHoldingStartTime;
    private float CorrectSawHoldingTime;

    // Start is called before the first frame update
    void Start()
    {
        PosRange = 0.025f;
        EulerRange = 5f;
        NeededSawHoldingTime = 3f;
        CuttingPlaneCollider = CuttingPlane.GetComponent<Collider>();
        // we need the "normal" euler Angles (not local)
        /*
        Debug.Log("euler: " + SawHoloRot);
        SawHoloRot = SawHolo.transform.localEulerAngles;
       
        Debug.Log("localeuler: " + SawHoloRot);
        Debug.Log("euler: " + this.transform.eulerAngles);
        Debug.Log("localeuler: " + this.transform.localEulerAngles);
        */
        Debug.Log("pos: " + Pos);
        Debug.Log("posHolo: " + SawHoloPos);
        Debug.Log(this.GetComponent<Collider>());
    }

    // Update is called once per frame
    void Update()
    {
        //Position = this.transform.position;
        //CuttingPlanePosition = CuttingPlane.transform.position;
        //ThisCollider = this.GetComponent<Collider>();

        GetPositionsAndAnglesOfSaws();
        bool SawTransformIsOK = CheckPositionAndAngleOfSaw();
        /*
        if (SawTransformIsOK && !IsInvoking("MoveHoloSaw"))
        {
            Invoke("MoveHoloSaw", 3f);
        }
        */
        SawHoldingTimeIsOK = CorrectSawHoldingTime >= NeededSawHoldingTime;
        if (SawTransformIsOK && SawHoldingTimeIsOK)
        {
            SawHolo.GetComponent<RotateHoloSawBasedOnSawPosition>().DisableHoloSawRotation();
            //GetComponentInParent<Animator>().enabled = true;
            //transform.parent.gameObject.GetComponent<Animator>().enabled = true;
            SawHolo.GetComponentInParent<Animator>().enabled = true;
        }
        SetSawColorAndActivateLineRenderer(SawTransformIsOK);

        //Debug.Log("Angles : " + this.transform.eulerAngles + "  " + SawHolo.transform.eulerAngles);

        /**
         * TODO: 
         * ERLEDIGT - verwendete Hand einbauen, 
         * movement der holoSaw einbauen,
         * randomisierte Postion des Cutting Planes einbauen --> bzw vor allem wie tief man cutten soll
         * Text Instructions hinter dem Opertationschtisch einfügen
         * Cut Animation / Vibrationsfeedback --> positives und negatives feedback
         */

    }

    private void MoveHoloSaw()
    {
        SawHolo.GetComponent<RotateHoloSawBasedOnSawPosition>().DisableHoloSawRotation();
        while (SawHolo.transform.position != CuttingPlane.transform.position)
        {
            Debug.Log("MOVING");
            float step = 5f * Time.deltaTime;
            SawHolo.transform.position = Vector3.MoveTowards(SawHolo.transform.position, CuttingPlane.transform.position, step);
        }  
    }

    /// <summary>
    /// Sets Color of Holo Saw to green and activates the LineRenderer if the Transform of the Saw carried by the user is ok. Else deactives it.
    /// </summary>
    /// <param name="SawTransformIsOK"></param>
    private void SetSawColorAndActivateLineRenderer(bool SawTransformIsOK)
    {
        if (SawTransformIsOK)
        {
            ChangeHoloSawColor(SawGreen);
            ActivateLineRenderer(true);
        }
        else if (!SawHoldingTimeIsOK)
        {
            ChangeHoloSawColor(SawRed);
            ActivateLineRenderer(false);
        }
    }

    /// <summary>
    /// Checks if the position and Angle of the Saw carried by the user is similar to those of the Holo Saw.
    /// </summary>
    /// <returns> True if Position and Angles of Saw carried by user are ok.</returns>
    private bool CheckPositionAndAngleOfSaw()
    {
        bool xPosIsOk = SawHoloPosX + PosRange > PosX && SawHoloPosX - PosRange < PosX;
        bool yPosIsOk = SawHoloPosY + PosRange > PosY && SawHoloPosY - PosRange < PosY;
        bool zPosIsOk = SawHoloPosZ + PosRange > PosZ && SawHoloPosZ - PosRange < PosZ;

        bool xRotIsOK = CalculateEulerAngleRange(RotX, SawHoloRotX); // SawHoloRotX + 6f > RotX && SawHoloRotX - 6f + 360f < RotX;
        bool yRotIsOK = CalculateEulerAngleRange(RotY, SawHoloRotY); //SawHoloRotY + 6f > RotY && SawHoloRotY - 6f + 360f < RotY;
        bool zRotIsOK = CalculateEulerAngleRange(RotZ, SawHoloRotZ); //SawHoloRotZ + 6f > RotZ && SawHoloRotZ - 6f + 360f < RotZ;

        if (xPosIsOk && yPosIsOk && zPosIsOk && xRotIsOK && yRotIsOK && zRotIsOK)
        {
            if (!IsStartTimeAlreadySet)
            {
                IsStartTimeAlreadySet = true;
                CorrectSawHoldingStartTime = Time.time;
            }
            else
            {
                CorrectSawHoldingTime = Time.time - CorrectSawHoldingStartTime;
            }
            return true;
        }
        else
        {
            IsStartTimeAlreadySet = false;
            return false;
        }
    }

    /// <summary>
    /// Gets the Positions and Euler angles of the Holo Saw and the Saw that the user should carry.
    /// </summary>
    private void GetPositionsAndAnglesOfSaws()
    {
        SawHoloPos = SawHolo.transform.position;
        SawHoloPosX = SawHoloPos.x;
        SawHoloPosY = SawHoloPos.y;
        SawHoloPosZ = SawHoloPos.z;

        SawHoloRot = SawHolo.transform.eulerAngles;
        SawHoloRotX = SawHoloRot.x;
        SawHoloRotY = SawHoloRot.y;
        SawHoloRotZ = SawHoloRot.z;

        Pos = this.transform.position;
        PosX = Pos.x;
        PosY = Pos.y;
        PosZ = Pos.z;

        Rot = this.transform.eulerAngles;
        RotX = Rot.x;
        RotY = Rot.y;
        RotZ = Rot.z;
    }

    /// <summary>
    /// Activates Line Randerer that draws a Line between the Holo Saw and the Cutting Plane (if setActive == true).
    /// </summary>
    /// <param name="setActive"></param>
    private void ActivateLineRenderer(bool setActive)
    {
        if (setActive)
        {
            LineStartPoint.GetComponent<MeshRenderer>().enabled = true;
            Line.GetComponent<LineRenderer>().enabled = true;
        }
        else
        {
            LineStartPoint.GetComponent<MeshRenderer>().enabled = false;
            Line.GetComponent<LineRenderer>().enabled = false;
        }
    }

    /// <summary>
    /// Calclulates if the Angle of the hold Saw in the hand of the user is positioned correctly according to it's angle relative to the angle of the HoloSaw.
    /// Since Unity uses positive Euler Angles we have to map the angles to this range (0 to 360) if the predefined Euler Range added or subtracted to the angle of the HoloSaw exceed the range.
    /// </summary>
    /// <param name="angle"></param>
    /// <param name="angleHolo"></param>
    /// <returns></returns>
    private bool CalculateEulerAngleRange(float angle, float angleHolo)
    {
        float tmpAngle;
        float tmpAngle2;
        bool IsInRange;
        if (angleHolo + EulerRange > 360f)
        {
            tmpAngle = angleHolo + EulerRange - 360;
            tmpAngle2 = angleHolo - EulerRange;
            IsInRange = angle < tmpAngle || angle > tmpAngle2;
        }
        else if (angleHolo - EulerRange < 0f)
        {
            tmpAngle = angleHolo + EulerRange;
            tmpAngle2 = angleHolo - EulerRange + 360f;
            IsInRange = angle < tmpAngle || angle > tmpAngle2;
        }
        else
        {
            tmpAngle = angleHolo + EulerRange;
            tmpAngle2 = angleHolo - EulerRange;
            IsInRange = angle < tmpAngle && angle > tmpAngle2;
        }
        return IsInRange;
    }

    private void OnTriggerEnter(Collider other)
    {
        /*
        if (other == CuttingPlaneCollider)
        {
            CuttingPlane.GetComponent<MeshRenderer>().material = CorrectInteractionMaterial;
            OnTriggerEnterPosition = this.transform.position;
            OnTriggerEnterEulerAngleX = this.transform.localEulerAngles.x;
            OnTriggerEnterEulerAngleY = this.transform.localEulerAngles.y;
            OnTriggerEnterEulerAngleZ = this.transform.localEulerAngles.z;
            Debug.Log("TRIGGER ENTERED");
        }
        */
        Debug.Log("This is: " + this.name + "   and : " + other.gameObject.name);
        if (other.tag == "SawHolo")
        {
           
            Debug.Log("SAW HOLO TRIGGER ENTERED");
            foreach (Transform child in other.transform)
            {
                //Debug.Log("this is a child: " + child.name);
                child.GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        /*
        if (other == CuttingPlaneCollider)
        {
            OnTriggerStayPosition = this.transform.position;
            OnTriggerStayEulerAngleX = this.transform.localEulerAngles.x;
            OnTriggerStayEulerAngleY = this.transform.localEulerAngles.y;
            OnTriggerStayEulerAngleZ = this.transform.localEulerAngles.z;

            AngleOfPosition = Vector3.Angle(OnTriggerEnterPosition, OnTriggerStayPosition);
            Distance = Vector3.Distance(OnTriggerEnterPosition, OnTriggerStayPosition);
            AngleX = Mathf.Abs(OnTriggerEnterEulerAngleX - OnTriggerStayEulerAngleX);
            AngleY = Mathf.Abs(OnTriggerEnterEulerAngleY - OnTriggerStayEulerAngleY);
            AngleZ = Mathf.Abs(OnTriggerEnterEulerAngleZ - OnTriggerStayEulerAngleZ);

            if (AngleOfPosition > 0.4)
            {
                CuttingPlane.GetComponent<MeshRenderer>().material = DefaultMaterial;
            }
        }
        */

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "SawHolo")
        {
            foreach (Transform child in other.transform)
            {
                //Debug.Log("this is a child: " + child.name);
                child.GetComponent<MeshRenderer>().enabled = false;
            }
            //Debug.Log("EXITED");
        }
    }

    /*
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("COLLISION WITH: " + collision.gameObject.name);
        if(collision.collider.tag == "CuttingPlane")
        {
            collision.rigidbody.isKinematic = true;
            Debug.Log("CUTTINGPLANE HIT");
            CuttingPlane.GetComponent<MeshRenderer>().material = CorrectInteractionMaterial;
        }

    }

    private void OnCollisionStay(Collision collision)
    {
        //Debug.Log("COLLISION WITH: " + collision.gameObject.name);
    }

    private void OnCollisionExit(Collision collision)
    {
        collision.rigidbody.isKinematic = false;
    }
    */

    /// <summary>
    /// Changes the color of the holo Saw depending on the material parameter. 
    /// </summary>
    /// <param name="material"></param>
    private void ChangeHoloSawColor(Material material)
    {
        foreach (Transform child in SawHolo.transform)
        {
            child.GetComponent<MeshRenderer>().material = material;
        }
    }
}