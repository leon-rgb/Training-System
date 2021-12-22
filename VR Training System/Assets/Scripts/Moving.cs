using UnityEngine;
using System.Collections;

public class Moving : MonoBehaviour
{
    //Amount to travel (in metres) per second
    public float Speed = 1f;

    //Direction to travel
    public Transform Direction;

    // Update is called once per frame
    void Update()
    {
        //Transform component on this object
        Transform ThisTransform = GetComponent<Transform>();

        //Update position in specified direction by speed
        //ThisTransform.position += Direction.position.normalized * Speed * Time.deltaTime;
        if (Vector3.Distance(transform.position, Direction.position) > 0.001f)
        {
            float step = Speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, Direction.position, step);
        }
    }
}