using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragGameObject : MonoBehaviour
{
    private Vector3 mouseOffset;
    private float mouseZ;

    //private Material mainMaterial;
    public Material highlightMaterial;

    // highlighting works only if GameObject has a prefab
    public GameObject clonePrefab;
    private GameObject clone;

    private void OnEnable()
    {
        // instantiate clone 
        clone = Instantiate(clonePrefab, transform.position, transform.rotation);
        clone.GetComponent<Collider>().enabled = false;
        clone.transform.parent = transform;
        
        // set clone up with a highlight material (bigger than original object)
        clone.GetComponent<Renderer>().material = highlightMaterial;
        clone.GetComponent<Renderer>().enabled = false;
    }

    /// <summary>
    /// Enable highlight and calculate mouse offset from object
    /// </summary>
    void OnMouseDown()
    {
        clone.GetComponent<Renderer>().enabled = true;

        mouseZ = Camera.main.WorldToScreenPoint(transform.position).z;

        mouseOffset = transform.position - GetMouseWorldPosition();
    }

    /// <summary>
    /// set mouse z position to z position of grabbed object 
    /// </summary>
    /// <returns></returns>
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;

        mousePosition.z = mouseZ;

        return Camera.main.ScreenToWorldPoint(mousePosition);
    }

    /// <summary>
    /// set grabbed object position relative to mouse position
    /// </summary>
    private void OnMouseDrag()
    {
        transform.position = GetMouseWorldPosition() + mouseOffset;
    }

    /// <summary>
    /// disable highlight
    /// </summary>
    private void OnMouseUp()
    {
        clone.GetComponent<Renderer>().enabled = false;
    }

}
