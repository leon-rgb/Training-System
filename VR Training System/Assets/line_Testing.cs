using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class line_Testing : MonoBehaviour
{

    [SerializeField] private Transform[] points;
    [SerializeField] private LineController line;

    // Update is called once per frame
    void Update()
    {
        line.UpdateLine(points);
    }
}
