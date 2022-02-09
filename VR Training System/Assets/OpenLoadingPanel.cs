using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenLoadingPanel : MonoBehaviour
{
    public float speed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, speed * Time.deltaTime);
    }
}
