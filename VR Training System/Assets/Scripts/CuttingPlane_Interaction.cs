using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private Vector3 Pos;
    private float PosX;
    private float PosY;
    private float PosZ;

    // Start is called before the first frame update
    void Start()
    {
        SawHoloPos = SawHolo.transform.position;
        SawHoloPosX = SawHoloPos.x;
        SawHoloPosY = SawHoloPos.y;
        SawHoloPosZ = SawHoloPos.z;

        SawHoloRot = SawHolo.transform.eulerAngles;

        // we need the "normal" euler Angles (not local)
        Debug.Log("euler: " + SawHoloRot);
        SawHoloRot = SawHolo.transform.localEulerAngles;
        /*
        Debug.Log("localeuler: " + SawHoloRot);
        Debug.Log("euler: " + this.transform.eulerAngles);
        Debug.Log("localeuler: " + this.transform.localEulerAngles);
        */
        Debug.Log("pos: " +Pos);
        Debug.Log("posHolo: " + SawHoloPos);
    }

    // Update is called once per frame
    void Update()
    {
        //Position = this.transform.position;
        //CuttingPlanePosition = CuttingPlane.transform.position;
        //ThisCollider = this.GetComponent<Collider>();

        CuttingPlaneCollider = CuttingPlane.GetComponent<Collider>();

        Pos = this.transform.position;
        PosX = Pos.x;
        PosY = Pos.y;
        PosZ = Pos.z;
         
        bool xPosIsOk = SawHoloPosX+0.025f > PosX && SawHoloPosX-0.025f <PosX;
        bool yPosIsOk = SawHoloPosY + 0.025f > PosY && SawHoloPosY - 0.025f < PosY; 
        bool zPosIsOk = SawHoloPosZ + 0.025f > PosZ && SawHoloPosZ - 0.025f < PosZ;

        if (xPosIsOk && yPosIsOk && zPosIsOk){
            ChangeHoloSawColor(SawGreen);
        }
        else
        {
            ChangeHoloSawColor(SawRed);
        }


        /**
         * TODO: auch Winkel miteinabauen!
         */
         
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
       
        if(other.tag == "SawHolo")
        {
            Debug.Log("SAW HOLO TRIGGER ENTERED");
            foreach (Transform child in other.transform)
            {
                Debug.Log("this is a child: " + child.name);
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
                Debug.Log("this is a child: " + child.name);
                child.GetComponent<MeshRenderer>().enabled = false;
            }
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

    private void ChangeHoloSawColor(Material material)
    {
        foreach (Transform child in SawHolo.transform)
        {
            child.GetComponent<MeshRenderer>().material = material;
        }
    }
}
