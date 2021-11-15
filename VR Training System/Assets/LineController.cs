using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    private LineRenderer linerenderer;
    private Transform[] points;

    private void Awake()
    {
        linerenderer = GetComponent<LineRenderer>();
    }

    public void UpdateLine(Transform[] points)
    {
        linerenderer.positionCount = points.Length;
        this.points = points;
    }

    private void Update()
    {
        for(int i = 0; i < points.Length; i++)
        {
            linerenderer.SetPosition(i, points[i].position);
        }

    }
}
