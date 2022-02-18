using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Only used for testing purposes
/// </summary>
public class AnimationController : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (gameObject.GetComponent<Animator>().enabled)
            {
                gameObject.GetComponent<Animator>().enabled = false;
            }
            else
            {
                gameObject.GetComponent<Animator>().enabled = true;
            }         
        }
    }
}
