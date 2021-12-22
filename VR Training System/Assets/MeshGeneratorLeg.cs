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

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateMesh();
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
        Mesh invMesh = InvertMesh();
        invMesh = DuplicateAndMoveMesh(invMesh);
        //invMesh.triangles.Reverse().ToArray();

        mesh = MergeMeshes(invMesh);
        Debug.Log("ARRAY: " + mesh.vertices);

       saw.GetComponent<SawAnimationGenerator>().test();
    }

    private Mesh DuplicateAndMoveMesh(Mesh mesh)
    {
        Mesh dupMesh = new Mesh();
        Vector3[] verts = mesh.vertices;
        for(int i = 0; i < verts.Length; i++)
        {
            verts[i].z = verts[i].z - widthCoef;
        }
        dupMesh.vertices = verts;
        dupMesh.triangles = triangles;
        return dupMesh;
    }
    private Mesh MergeMeshes(Mesh invMesh)
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
    private Mesh InvertMesh()
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

    
    private void OnDrawGizmos()
    {
        if (vertices == null)
        {
            return;

        }
        Gizmos.color = Color.white;
        foreach (Vector3 vert in curveVertices)
        {
            Gizmos.DrawSphere(vert, 0.01f);
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

        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(mesh.bounds.center, 0.01f);
        //Debug.Log(GameObject.Find("MeshObjectLeg").GetComponent<Mesh>().bounds.center);
    }
}
