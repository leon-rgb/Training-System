using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.ScrollRect;

public class ScrollVR_collision : MonoBehaviour
{
    Vector3 startPoint;
    Vector3 currentPoint;
    public ScrollRect scrollRect;

    private const float speedMultiplier = 0.004f;
    public float xSpeed = 0;
    public float ySpeed = 0;
    private float hPos, vPos;

    public bool collionEnabled;

    void OnCollisionEnter(Collision collision)
    {
        // get start point of hand of collision
        startPoint = collision.transform.position;
        Debug.Log("collider hit at " + startPoint);
    }

    void OnCollisionStay(Collision collision)
    {
        Debug.Log("STAY");
        // get current hand position and calculate distance to start point
        currentPoint = collision.transform.position;

        //use this to create speed of scrolling
        ySpeed = startPoint.y - currentPoint.y;

        // calculate scrolled positions
        hPos = scrollRect.horizontalNormalizedPosition + xSpeed * speedMultiplier;
        vPos = scrollRect.verticalNormalizedPosition + ySpeed * speedMultiplier;

        xSpeed = Mathf.Lerp(xSpeed, 0, 0.1f);
        ySpeed = Mathf.Lerp(ySpeed, 0, 0.1f);

        // set ne scrolling position
        if (scrollRect.movementType == MovementType.Clamped)
        {
            hPos = Mathf.Clamp01(hPos);
            vPos = Mathf.Clamp01(vPos);
        }

        scrollRect.normalizedPosition = new Vector2(hPos, vPos);
        //startPoint = currentPoint; lol das war so dumm das zu machen
    }
}