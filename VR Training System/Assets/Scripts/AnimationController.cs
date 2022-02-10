using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

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
