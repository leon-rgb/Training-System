using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePositionAndAngleOfCuttingAdvisors : MonoBehaviour
{
    private GameObject SawHolo;
    private GameObject CuttingPlane;

    // Start is called before the first frame update
    void Start()
    {
        SawHolo = GameObject.Find("SawHolo");
        CuttingPlane = GameObject.Find("CuttingPlane");
        //SawHolo.transform.RotateAround(CuttingPlane.transform.position, Vector3.right, 180);
        //SawHolo.transform.RotateAround(CuttingPlane.transform.position, Vector3.up, 90);
        //SawHolo.transform.RotateAround(CuttingPlane.transform.position, Vector3.left, 90);
        //SawHolo.transform.RotateAround(CuttingPlane.transform.position, new Vector3 (0,0,-1), 90);
        SawHolo.transform.RotateAround(CuttingPlane.transform.position, Vector3.back, 90);
        //SawHolo.transform.RotateAround(SawHolo.transform.position, Vector3.)
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
