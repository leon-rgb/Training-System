using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGeneratorLeg : MonoBehaviour
{

    public Transform TopCuttingPoint;
    public Transform MidCuttingPoint;
    public Transform BotCuttingPoint;
    public float lengthCoef;
    public float widthCoef;
    public float curveSmoothCoef;

    Mesh mesh;

    Vector3[] vertices;
    Vector3[] startVertices;
    Vector3[] curveVertices;
    Vector3[] correspondingVertices;
    int[] triangles;

    public static Vector3 meshMiddlePoint;
    public static float meshRadius;

    public GameObject saw;
    public bool meshWasCreated = false;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().sleepThreshold = 0.0f;
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateMesh();
        meshWasCreated = true;
        UpdateMesh();
        UpdateCollider();
        meshMiddlePoint = mesh.bounds.center;
        meshRadius = Vector3.Magnitude(mesh.bounds.max - mesh.bounds.center);
        //meshMiddlePoint = (TopCuttingPoint.position + correspondingVertices[correspondingVertices.Length / 2])/2;
    }

    private void UpdateCollider()
    {
        GetComponent<MeshCollider>().sharedMesh = mesh;
        //GetComponent<MeshCollider>().convex = false;

        //GetComponent<Rigidbody>().WakeUp();
    }

    private void CreateMesh()
    {
        //create Array out of input Points 
        startVertices = new Vector3[]
        {
            TopCuttingPoint.position,
            MidCuttingPoint.position,
            BotCuttingPoint.position,
        };

        //create curve out of the vertices
        curveVertices = CurveGenerator.SmoothLine(startVertices, curveSmoothCoef);
        foreach (Vector3 vec in curveVertices)
        {
            Debug.Log("NEW SCRIPT: " + vec);
        }
        Debug.Log(curveVertices.Length);

        //create vertices in a straight line. Each vertex corresponds to a vertex in the curve.
        correspondingVertices = new Vector3[curveVertices.Length];
        for (int i = 0; i < curveVertices.Length; i++)
        {
            //correspondingVertices[i] = new Vector3(TopCuttingPoint.position.x - 0.1f, TopCuttingPoint.position.y, curveVertices[i].z);
            correspondingVertices[i] = new Vector3(TopCuttingPoint.position.x - lengthCoef, curveVertices[i].y, curveVertices[i].z);
        }

        //Algorithm to connect all Vertices correctly (so that they can just be added to the triangles in order)
        List<Vector3> sortedVertices = new List<Vector3>();
        sortedVertices = SortVertices(correspondingVertices, curveVertices);

        vertices = sortedVertices.ToArray();

        triangles = new int[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            triangles[i] = i;
        }

        /*
        Mesh tmp = new Mesh();
        tmp.Clear();
        tmp.vertices = vertices;
        mesh.triangles = triangles;
        */
    }

    public List<Vector3> SortVertices(Vector3[] correspondingVertices, Vector3[] curveVertices)
    {
        List<Vector3> sortedVertices = new List<Vector3>();
        for (int i = 0; i < correspondingVertices.Length; i++)
        {
            if (i == 0)
            {
                sortedVertices.Add(correspondingVertices[i]);
                sortedVertices.Add(curveVertices[i]);
                sortedVertices.Add(curveVertices[i + 1]);
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
        return sortedVertices;
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.Optimize();
        mesh.RecalculateNormals();
        /*
        Mesh[] meshesToCombine = new Mesh[]
        {
            mesh,
            InvertMesh()
        };*/
        //Mesh.CombineMeshes(new Mesh[]{mesh, InvertMesh()}, true);
        //Mesh invMesh = InvertMesh();
        //Mesh combMesh = MergeMeshes(invMesh);
        //mesh = combMesh;
        Mesh invMesh = InvertMesh(mesh, triangles);
        invMesh = DuplicateAndMoveMesh(invMesh, triangles);
        //invMesh.triangles.Reverse().ToArray();

        mesh = MergeMeshes(mesh, invMesh);
        Debug.Log("ARRAY: " + mesh.vertices);

        saw.GetComponent<SawAnimationGenerator>().test();
    }

    public Mesh DuplicateAndMoveMesh(Mesh mesh, int[] triangles)
    {
        Mesh dupMesh = new Mesh();
        Vector3[] verts = mesh.vertices;
        
        for (int i = 0; i < verts.Length; i++)
        {
            verts[i].z = verts[i].z - widthCoef;
        }
         
        dupMesh.vertices = verts;
        dupMesh.triangles = triangles;
        return dupMesh;
    }
    public Mesh MergeMeshes(Mesh mesh, Mesh invMesh)
    {
        CombineInstance[] combInst = new CombineInstance[2]
        {
            new CombineInstance(){mesh = invMesh, transform = Matrix4x4.identity},
            new CombineInstance(){mesh = mesh, transform = Matrix4x4.identity}
        };

        Mesh combMesh = new Mesh();
        combMesh.CombineMeshes(combInst);
        return combMesh;
    }
    public Mesh InvertMesh(Mesh mesh, int[] triangles)
    {
        triangles = mesh.triangles.Reverse().ToArray();
        Vector3[] normals = mesh.normals;
        Vector3[] invertedNormals = new Vector3[normals.Length];
        for (int i = 0; i < invertedNormals.Length; i++)
        {
            invertedNormals[i] = -normals[i];
        }
        Vector4[] tangents = mesh.tangents;
        Vector4[] invertedTangents = new Vector4[tangents.Length];
        for (int i = 0; i < invertedTangents.Length; i++)
        {
            invertedTangents[i] = tangents[i];
            invertedTangents[i].w = -invertedTangents[i].w;
        }
        Mesh invMesh = new Mesh();
        invMesh.vertices = mesh.vertices;
        invMesh.normals = invertedNormals;
        invMesh.tangents = invertedTangents;
        invMesh.triangles = triangles;
        return invMesh;
    }

    public Vector3[] getVertices()
    {
        Debug.Log("Curve Verts: " + curveVertices);
        return curveVertices;
    }

    public Mesh CreateNonFlatCurveVertices(Vector3[] curveVertices, Vector3[]vertices, int[]triangles)
    {
        List<Vector3> tmp = new List<Vector3>();
        for(int i = 0; i < curveVertices.Length-1; i++)
        {
            tmp.Add(curveVertices[i]);

            Vector3 newLeft = new Vector3(curveVertices[i].x, curveVertices[i].y, curveVertices[i].z - widthCoef);
            tmp.Add(newLeft);

            tmp.Add(curveVertices[i+1]);

            tmp.Add(curveVertices[i + 1]);

            tmp.Add(newLeft);

            Vector3 newRight = new Vector3(curveVertices[i+1].x, curveVertices[i+1].y, curveVertices[i+1].z - widthCoef);
            tmp.Add(newRight);
        }

        Vector3[] tmpArray = tmp.ToArray();
        Vector3[] combinedVertices = new Vector3[tmpArray.Length];

        int j = 0;/*
        foreach(Vector3 vert in vertices)
        {
            combinedVertices[j] = vert;
            j++;
        }*/
        foreach(Vector3 vert in tmpArray)
        {
            combinedVertices[j] = vert;
            j++;
        }

        


        int[] tmp2 = new int[combinedVertices.Length];
        for (int i = 0; i < combinedVertices.Length; i++)
        {
            tmp2[i] = i;
        }

        Mesh nonFlatCurve = new Mesh();
        nonFlatCurve.vertices = combinedVertices;
        nonFlatCurve.triangles = tmp2;
        //nonFlatCurve.triangles = nonFlatCurve.triangles.Reverse().ToArray();
        return nonFlatCurve;
    }
    
    private void OnDrawGizmos()
    {
        if (vertices == null)
        {
            return;

        }
        /*
        Gizmos.color = Color.white;
        foreach (Vector3 vert in curveVertices)
        {
            Gizmos.DrawSphere(vert, 0.001f);
        }


        Gizmos.color = Color.red;
        foreach (Vector3 vert in startVertices)
        {
            Gizmos.DrawSphere(vert, 0.01f);
        }


        Gizmos.color = Color.blue;
        foreach (Vector3 vert in correspondingVertices)
        {
            Gizmos.DrawSphere(vert, 0.01f);
        }
        
        foreach ( Vector3 vert in CreateNonFlatCurveVertices(curveVertices))
        {
            Gizmos.DrawSphere(vert, 0.001f);
        }*/


        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(mesh.bounds.center, 0.01f);
        //Debug.Log(GameObject.Find("MeshObjectLeg").GetComponent<Mesh>().bounds.center);
    }
}
