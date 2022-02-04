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
    public GameObject UI_Manager_go;
    private UI_Manager ui;
    private bool isUiActive;
    private static Vector3 pos1;
    private static Vector3 pos2;
    private static Quaternion rot1;
    private static Quaternion rot2;
    private bool IsAtPos1;

    private void Awake()
    {
        transform.LookAt(RotationPoint);
        body = GetComponent<Rigidbody>();
        meshGenerator = MeshGeneratorLeg.GetComponent<MeshGeneratorLeg>();
        cuttingAccuracy = CutToDeepMeshGenerator.GetComponent<CuttingAccuracy>();
        anim = leg.GetComponent<Animator>();
        serializer = JSON_SerializerTransform.GetComponent<JSON_Serializer>();
        ui = UI_Manager_go.GetComponent<UI_Manager>();
        rotateSpeed = speed / 3;
        anim.SetBool("playReversed", true);

        SetUpPosAndRots();
    }
    void Update()
    {
        //check if ui is active
        isUiActive = ui.IsAnySubMenuEnabled();
        if (isUiActive)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ui.UI_SwitchEnabledState();
        }
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
            JSON_Serializer.SetupCuttingPlane("hallo");
            //meshGenerator.CreateNewMesh();
            //cuttingAccuracy.CreateNewMesh();
        }

        // change transform to a 2d like view
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (!IsAtPos1)
            {
                transform.position = pos1;
                transform.rotation = rot1;
                IsAtPos1 = true;
            }
            else
            {
                transform.position = pos2;
                transform.rotation = rot2;
                IsAtPos1 = false;
            }
        }
    }
    private void FixedUpdate()
    {
        //check if ui is active
        if (isUiActive)
        {
            return;
        }

        
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

    /// <summary>
    /// Changes the animation of the leg model accordingly to current visibilty
    /// </summary>
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

    /// <summary>
    /// Sets up 2 hardcoded transforms that give a 2d like view on the cutting plane
    /// </summary>
    private void SetUpPosAndRots()
    {
        IsAtPos1 = false;
        pos1 = JsonUtility.FromJson<Vector3>("{ \"x\":-2.6302037239074709,\"y\":0.9123166799545288,\"z\":1.347090244293213}");
        rot1 = JsonUtility.FromJson<Quaternion>("{ \"x\":0.04833417758345604,\"y\":0.7054529190063477,\"z\":-0.04833417758345604,\"w\":0.7054529190063477}");
        pos2 = new Vector3(-2.45450997f, 0.904134154f, 1.45936608f);
        rot2 = new Quaternion(-1.37835741e-07f, 0.997661114f, -0.0683548152f, -2.04332173e-06f);
    }
}
