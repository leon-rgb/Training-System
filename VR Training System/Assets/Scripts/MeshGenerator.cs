using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// used for test purposes only
/// </summary>
[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{

    Mesh mesh;

    Vector3[] vertices;
    Vector3[] startVertices;
    Vector3[] curveVertices;
    Vector3[] correspondingVertices;
    int[] triangles;


    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateMesh();
        UpdateMesh();
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    private void CreateMesh()
    {
        /*
        vertices = new Vector3[]
        {
            new Vector3(0,0,0),
            new Vector3(0,0,1),
            new Vector3(1,0,0),
            new Vector3(1,0,1),
            new Vector3(0,1,0),
            new Vector3(0,1,1),
            new Vector3(1,1,0),
            new Vector3(1,1,1)
        };

        triangles = new int[]
        {
            0, 1, 2,
            1, 3, 2,

            //4, 5, 6,
            //5, 7, 6
        };
        */

        startVertices = new Vector3[]
        {
            new Vector3(0,0,2),
            //new Vector3(0.45f,0,1.65f),
            //new Vector3(0.5f,0,1.3f),
            new Vector3(0.6f,0,0.7f),
            //new Vector3(0.5f,0,0.35f),
            new Vector3(0,0,0),
        };
        //create curve out of the vertices
        curveVertices = CurveGenerator.CreateCurve(startVertices, 0.05f);
        foreach (Vector3 vec in curveVertices)
        {
            Debug.Log(vec);
        }
        Debug.Log(curveVertices.Length);

        //create vertices in a straight line. Each vertex corresponds to a vertex in the curve.
        correspondingVertices = new Vector3[curveVertices.Length];
        for (int i = 0; i < curveVertices.Length; i++)
        {
            correspondingVertices[i] = new Vector3(-1, 0, curveVertices[i].z);
        }

        //Algorithm to connect all Vertices correctly (so that they can just be added to the triangles in order)
        List<Vector3> sortedVertices = new List<Vector3>();
        for (int i = 0; i < correspondingVertices.Length; i++)
        {
            if(i == 0)
            {
                sortedVertices.Add(correspondingVertices[i]);
                sortedVertices.Add(curveVertices[i]);
                sortedVertices.Add(curveVertices[i+1]);
            }
            else if (i == correspondingVertices.Length - 1)
            {
                sortedVertices.Add(correspondingVertices[i]);
                sortedVertices.Add(correspondingVertices[i - 1]);
                sortedVertices.Add(curveVertices[i]);
            }
            else
            {
                sortedVertices.Add(correspondingVertices[i]);
                sortedVertices.Add(correspondingVertices[i - 1]);
                sortedVertices.Add(curveVertices[i]);
                sortedVertices.Add(correspondingVertices[i]);
                sortedVertices.Add(curveVertices[i]);
                sortedVertices.Add(curveVertices[i + 1]);
            }
        }

        vertices = sortedVertices.ToArray();

        triangles = new int[vertices.Length];
        for(int i = 0; i < vertices.Length; i++)
        {
            triangles[i] = i;
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.Optimize();
        mesh.RecalculateNormals();
    }

    private void OnDrawGizmos()
    {
        if(vertices == null)
        {
            return;

        }
        Gizmos.color = Color.white;
        foreach(Vector3 vert in curveVertices)
        {
            Gizmos.DrawSphere(vert, 0.01f);
        }

        
        Gizmos.color = Color.red;
        foreach (Vector3 vert in startVertices)
        {
            Gizmos.DrawSphere(vert, 0.01f);
        }


        Gizmos.color = Color.blue;
        foreach(Vector3 vert in correspondingVertices)
        {
            Gizmos.DrawSphere(vert, 0.01f);
        }

        //Gizmos.DrawSphere(new Vector3(-1, 0, 0), 0.01f);
    }
}
