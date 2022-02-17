using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.ScrollRect;
using UnityEngine.SceneManagement;

public class ScrollVR_collision : MonoBehaviour
{
    Vector3 startPoint;
    Vector3 currentPoint;
    public ScrollRect scrollRect;

    private float speedCoef = 0.004f;
    public float xSpeed = 0;
    public float ySpeed = 0;
    private float horizontalPos;
    private float verticalPos;

    public bool collionEnabled;

    private void Start()
    {
        if(SceneManager.GetActiveScene().name == "Gloves_SurgeryRoom")
        {
            speedCoef = 0.02f;
            //GetComponent<Collider>().isTrigger = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // get start point of hand of collision
        startPoint = collision.transform.position;
        //Debug.Log("collider hit at " + startPoint);
    }

    void OnCollisionStay(Collision collision)
    {
        //Debug.Log("STAY");
        // get current hand position and calculate distance to start point
        currentPoint = collision.transform.position;

        // calculate scrolling speed
        ySpeed = startPoint.y - currentPoint.y;

        // calculate scrolled positions by getting current scroll position and adding the speed value to it
        horizontalPos = scrollRect.horizontalNormalizedPosition + xSpeed * speedCoef;
        verticalPos = scrollRect.verticalNormalizedPosition + ySpeed * speedCoef;

        xSpeed = Mathf.Lerp(xSpeed, 0, 0.1f);
        ySpeed = Mathf.Lerp(ySpeed, 0, 0.1f);

        
        // check if scrollrect is clamped
        if (scrollRect.movementType == MovementType.Clamped)
        {
            horizontalPos = Mathf.Clamp01(horizontalPos);
            verticalPos = Mathf.Clamp01(verticalPos);
        }

        // set scrolling position
        scrollRect.normalizedPosition = new Vector2(horizontalPos, verticalPos);
        //startPoint = currentPoint; lol das war so dumm das zu machen
    }

}
