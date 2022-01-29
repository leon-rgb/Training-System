using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGeneratorLeg : MonoBehaviour
{
    [Header("Cutting Points")]
    public Transform TopCuttingPoint;
    public Transform MidCuttingPoint;
    public Transform BotCuttingPoint;

    [Header("Plane coefficients")]
    public float lengthCoef;
    public float widthCoef;
    public float curveSmoothCoef;
    [Tooltip("Defines how close to each other the spheres are and therefore how many exist. Value is relative to length of cutting plane. ")]
    [Range(0.04f, 1)]
    public float sphereFrequencyCoef;  //default between 0.1 and 0.05

    Mesh mesh;

    Vector3[] vertices;
    Vector3[] startVertices;
    Vector3[] curveVertices;
    Vector3[] correspondingVertices;
    int[] triangles;

    public static Vector3 meshMiddlePoint;
    public static float meshRadius;

    public bool meshWasCreated { get; set; } = false;
    
    [Header("Prefabs for in plane created spheres")]
    public GameObject cuttingSphereAccuracyPrefab;
    public GameObject cuttingSphereWaveformPrefab;
    public List<CuttingPlane_Sphere> AllCuttingPlaneAccuracySpheres { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().sleepThreshold = 0.0f;
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateMesh();
        AllCuttingPlaneAccuracySpheres = new List<CuttingPlane_Sphere>();
        InstantiateCuttingSpheres();
        meshWasCreated = true;
        UpdateMesh();
        UpdateCollider();
        meshMiddlePoint = mesh.bounds.center;
        meshRadius = Vector3.Magnitude(mesh.bounds.max - mesh.bounds.center);
        //meshMiddlePoint = (TopCuttingPoint.position + correspondingVertices[correspondingVertices.Length / 2])/2;        
    }

    public void CreateNewMesh()
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        mesh = new Mesh();
        CreateMesh();
        InstantiateCuttingSpheres();
        UpdateMesh();
        UpdateCollider();
        meshMiddlePoint = mesh.bounds.center;
        meshRadius = Vector3.Magnitude(mesh.bounds.max - mesh.bounds.center);

        //alle kinder destroyen
        //maybe lsite fpr accuracy leeren, aber wahrscheinlich nicht notwendig, da sie eh nur im hauptprogramm verwendet wird
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

        //Mesh invMesh = InvertMesh(mesh, triangles);
        //invMesh = DuplicateAndMoveMesh(invMesh, triangles);
        
        //no need to reverse
        //invMesh.triangles.Reverse().ToArray();
        //mesh.triangles = triangles.Reverse().ToArray();

        //mesh = MergeMeshes(mesh, invMesh);
        GetComponent<MeshFilter>().mesh = mesh;
        //saw.GetComponent<SawAnimationGenerator>().StartSawMovement();
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
        //Debug.Log("Curve Verts: " + curveVertices);
        return curveVertices;
    }

    public Mesh CreateNonFlatCurveVertices(Vector3[] curveVertices, Vector3[]vertices, int[]triangles, Vector3[] correspondingVertices)
    {
        List<Vector3> tmp = new List<Vector3>();
        Vector3 firstBackRight = Vector3.zero;
        Vector3 firstBackLeft = Vector3.zero;
        for (int i = 0; i < curveVertices.Length-1; i++)
        {      
            tmp.Add(curveVertices[i]);

            Vector3 newLeft = new Vector3(curveVertices[i].x, curveVertices[i].y, curveVertices[i].z - widthCoef);
            tmp.Add(newLeft);

            tmp.Add(curveVertices[i+1]);

            tmp.Add(curveVertices[i + 1]);

            tmp.Add(newLeft);

            Vector3 newRight = new Vector3(curveVertices[i+1].x, curveVertices[i+1].y, curveVertices[i+1].z - widthCoef);
            tmp.Add(newRight);

            if (i == 0)
            {    
                firstBackRight = new Vector3(correspondingVertices[i].x, curveVertices[i].y, curveVertices[i].z - widthCoef);
                tmp.Add(firstBackRight);
                tmp.Add(newLeft);               
                tmp.Add(curveVertices[i]);

                firstBackLeft = new Vector3(correspondingVertices[i].x, curveVertices[i].y, curveVertices[i].z);
                tmp.Add(firstBackRight);
                tmp.Add(curveVertices[i]);
                tmp.Add(firstBackLeft);
            }

            if(i == curveVertices.Length - 2)
            {
                i++;
                Vector3 backRight = new Vector3(correspondingVertices[i].x, curveVertices[i].y, curveVertices[i].z - widthCoef);
                tmp.Add(curveVertices[i]);
                tmp.Add(newRight);
                tmp.Add(backRight);

                
                Vector3 backLeft = new Vector3(correspondingVertices[i].x, curveVertices[i].y, curveVertices[i].z);
                tmp.Add(backLeft);
                tmp.Add(curveVertices[i]);
                tmp.Add(backRight);

                tmp.Add(backRight);
                tmp.Add(firstBackRight);
                tmp.Add(backLeft);

                tmp.Add(backLeft);
                tmp.Add(firstBackRight);
                tmp.Add(firstBackLeft);
            }
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
    
    private void InstantiateCuttingSpheres()
    {
        Vector3 pos = Vector3.zero;
        for(int i = 0; i < curveVertices.Length; i++)
        {
            pos = curveVertices[i];
            float spheresPerRow = (Vector3.Distance(curveVertices[i], correspondingVertices[i]) / (lengthCoef*sphereFrequencyCoef));
            float accuracyPortion = spheresPerRow/3;
            //Debug.Log("spheres per row : " + spheresPerRow);
            for (int j = 0; j < spheresPerRow; j++)
            {
                pos.x = curveVertices[i].x - j * lengthCoef * sphereFrequencyCoef;
                GameObject go;
                if (j < accuracyPortion)
                {
                    go = Instantiate(cuttingSphereAccuracyPrefab, pos, Quaternion.identity);
                    AllCuttingPlaneAccuracySpheres.Add(go.GetComponent<CuttingPlane_Sphere>());
                }
                else
                {
                    go = Instantiate(cuttingSphereWaveformPrefab, pos, Quaternion.identity);
                }               
                go.transform.parent = transform;
            }
        }
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
