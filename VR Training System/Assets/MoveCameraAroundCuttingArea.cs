using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraAroundCuttingArea : MonoBehaviour
{
    public float speed;
    private float rotateSpeed;
    [Tooltip("the point the camera should be moved around")]
    public Transform RotationPoint;
    private float distanceToRotPoint;
    private Rigidbody body;
    public Transform MeshGeneratorLeg;
    private MeshGeneratorLeg meshGenerator;
    public Transform CutToDeepMeshGenerator;
    private CuttingAccuracy cuttingAccuracy;
    public GameObject leg;
    private Animator anim;
    public Transform JSON_SerializerTransform;
    private JSON_Serializer serializer;

    private void Awake()
    {
        transform.LookAt(RotationPoint);
        body = GetComponent<Rigidbody>();
        meshGenerator = MeshGeneratorLeg.GetComponent<MeshGeneratorLeg>();
        cuttingAccuracy = CutToDeepMeshGenerator.GetComponent<CuttingAccuracy>();
        anim = leg.GetComponent<Animator>();
        serializer = JSON_SerializerTransform.GetComponent<JSON_Serializer>();
        rotateSpeed = speed / 3;
        anim.SetBool("playReversed", true);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            meshGenerator.CreateNewMesh();
            cuttingAccuracy.CreateNewMesh();
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            ChangeVisibility();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            serializer.SetupCuttingPlane("hallo");
            meshGenerator.CreateNewMesh();
            cuttingAccuracy.CreateNewMesh();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {           
            Debug.Log("Save was succesful? --> " + serializer.SaveCuttingPlane("plane2", false));
        }
    }
    private void FixedUpdate()
    {
        distanceToRotPoint = Vector3.Distance(transform.position, RotationPoint.position);
        if (Input.GetKey(KeyCode.RightArrow))
        {
            //need y-direction to rotate camera left and right
            transform.RotateAround(RotationPoint.position, Vector3.down, rotateSpeed);
            body.velocity = Vector3.zero;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.RotateAround(RotationPoint.position, Vector3.up, rotateSpeed);
            body.velocity = Vector3.zero;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (distanceToRotPoint < 0.5f)
            {
                //transform.position -= (transform.forward * Time.deltaTime * speed);
                body.velocity = -transform.forward * speed * Time.fixedDeltaTime;
            }
            else
            {
                body.velocity = Vector3.zero;
            }
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (distanceToRotPoint > 0.1f)
            {
                //transform.position += (transform.forward * Time.deltaTime * speed);
                body.velocity = transform.forward * speed * Time.fixedDeltaTime;
            }
            else
            {
                body.velocity = Vector3.zero;
            }
        }

        
        if(!(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow)))
        {
            body.velocity = Vector3.zero;
        }

        if((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow)) && (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow)))
        {
            body.velocity = Vector3.zero;
        }
        //Debug.Log(distanceToRotPoint);
    }

    private void ChangeVisibility()
    {
        anim.enabled = true;
        if (anim.GetBool("playReversed"))
        {            
            anim.SetBool("playReversed", false);
        }
        else 
        {            
            anim.SetBool("playReversed", true);
        }
    }


    private void SaveProcess()
    {

    }
}
