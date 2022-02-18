using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// used for testing purposes only
/// </summary>
public class SawTransformation : MonoBehaviour
{
    float start;
    public Transform objectToLookAt;
    // Start is called before the first frame update
    void Start()
    {
        start = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float end = Time.time - start;
        if(end >= 3)
        {
            GameObject.Find("SawHolo").GetComponent<RotateHoloSawBasedOnSawPosition>().DisableHoloSawRotation();
            transform.LookAt(objectToLookAt);
        }
        
    }
}
