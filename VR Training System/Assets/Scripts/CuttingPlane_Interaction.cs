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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Position = this.transform.position;
        //CuttingPlanePosition = CuttingPlane.transform.position;
        //ThisCollider = this.GetComponent<Collider>();

        CuttingPlaneCollider = CuttingPlane.GetComponent<Collider>();
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other == CuttingPlaneCollider)
        {
            CuttingPlane.GetComponent<MeshRenderer>().material = CorrectInteractionMaterial;
            OnTriggerEnterPosition = this.transform.position;
            OnTriggerEnterEulerAngleX = this.transform.localEulerAngles.x;
            OnTriggerEnterEulerAngleY = this.transform.localEulerAngles.y;
            OnTriggerEnterEulerAngleZ = this.transform.localEulerAngles.z;
            Debug.Log("TRIGGER ENTERED");
        }
    }

    private void OnTriggerStay(Collider other)
    {
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
    }

}
