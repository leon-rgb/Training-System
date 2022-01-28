using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CuttingAccuracy : MonoBehaviour
{
    private MeshGeneratorLeg cuttingMeshGenerator;
    public GameObject cuttingMeshObj;

    Mesh deepMesh;
    private Vector3[] deepVertices;
    private Vector3[] movedCurveVertices;
    private Vector3[] deepCorrespondingVertices;
    int[] deepTriangles;
    Mesh nonFlatCurve;

    [Range(1, 20)]
    public int distToCuttingMeshCoef;

    public GameObject spherePrefab;

    private List<CuttingPlane_Sphere> allCuttingPlaneAccuracySpheres;
    public float CuttingPlaneAccuracy { get; set; } = 0;
    public float TotalAccuracy { get; set; } = 0;
    [Range(0.3f, 1)]
    public float UpdateAccuracyInterval;

    [Tooltip("Select the collider of the saw blade in the scene")]
    public Transform mainTransform;
    private MainScript main;

    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<Rigidbody>().sleepThreshold = 0.0f;
        deepMesh = new Mesh();
        GetComponent<MeshFilter>().mesh = deepMesh;
        cuttingMeshGenerator = cuttingMeshObj.GetComponent<MeshGeneratorLeg>();
        //wait for creation of cutting mesh
        StartCoroutine(DelayedMeshCreation());
        main = mainTransform.GetComponent<MainScript>();
    }

    private void Update()
    {
       StartCoroutine(CalculateCuttingPlaneAccuracy());
       StartCoroutine(CalculateTotalAccuracy());
    }

    private IEnumerator CalculateCuttingPlaneAccuracy()
    {
        yield return new WaitForSeconds(UpdateAccuracyInterval);
        allCuttingPlaneAccuracySpheres = cuttingMeshGenerator.AllCuttingPlaneAccuracySpheres;
        int totalQuantity = allCuttingPlaneAccuracySpheres.Count;
        int realQuantity = 0;
        foreach (CuttingPlane_Sphere sphere in allCuttingPlaneAccuracySpheres)
        {
            if (sphere.wasHit) realQuantity++;
        }
        CuttingPlaneAccuracy = (100 * realQuantity) / totalQuantity;      
    }

    private IEnumerator CalculateTotalAccuracy()
    {
        yield return new WaitForSeconds(UpdateAccuracyInterval);
        float depth = main.Depth * 1000; // *1000 to get size in mm 
        float cutToDeepCount = main.CutTooDeepCount;
        // you could scale x to make the accuracy score easier/harder -> e.g. x*0.5 is much easier.
        // or you could not take the size in mm --> e.g. depth = main.Depth * 100 (is in cm then).
        // of course you could also scale it in a non linear way.
        float x = cutToDeepCount * depth;
        x *= 0.5f;
        //Debug.Log("x = " + x);

        //function used: 5.08219*10^-22x^4 + 0.00003*x^3 - 0.00194*x^2 + 0.00091x + 1
        //every line is one exponent (for easier reading)
        double errCoefficient;
        if (x > 32.8)
        {
            errCoefficient = 0; //needed since approximation gets negative after x = 32.8 and than gets positive again
        }
        else
        {
            errCoefficient = 5.08219 * Math.Pow(10, -22) * Math.Pow(x, 4);
            errCoefficient += 0.00003 * Math.Pow(x, 3);
            errCoefficient -= 0.00194 * Math.Pow(x, 2);
            errCoefficient += 0.00091 * x + 1;

            if (errCoefficient > 1) errCoefficient = 1;
            //if (errCoefficient < 0) errCoefficient = 0;
        }
       
        //TotalAccuracy = (float) errCoefficient * CuttingPlaneAccuracy;
        TotalAccuracy = (float)errCoefficient * CuttingPlaneAccuracy;
        TotalAccuracy = (float) (Math.Round(TotalAccuracy, 2));
    }

    private void UpdateMesh()
    {
        
        deepMesh.Clear();
        deepMesh.vertices = deepVertices;
        deepMesh.triangles = deepTriangles;
        deepMesh.Optimize();
        deepMesh.RecalculateNormals();

        Mesh invMesh;
        invMesh = cuttingMeshGenerator.DuplicateAndMoveMesh(deepMesh, deepTriangles);
        invMesh.triangles = invMesh.triangles.Reverse().ToArray();

        deepMesh = cuttingMeshGenerator.MergeMeshes(deepMesh, invMesh);
        
        nonFlatCurve= cuttingMeshGenerator.CreateNonFlatCurveVertices(movedCurveVertices, deepVertices, deepTriangles, deepCorrespondingVertices);
       
        Debug.Log("DEEP VERTS: " + deepVertices);
        Debug.Log(nonFlatCurve.vertices);
        /*
        deepMesh.Clear();
        deepMesh.vertices = deepVertices;
        deepMesh.triangles = deepTriangles;
        deepMesh.Optimize();
        deepMesh.RecalculateNormals();*/
        //deepMesh = cuttingMeshGenerator.MergeMeshes(deepMesh, nonFlatCurve);
        
        nonFlatCurve = cuttingMeshGenerator.MergeMeshes(deepMesh, nonFlatCurve);
        deepMesh = nonFlatCurve;
        deepMesh.RecalculateNormals();        //deepMesh = cuttingMeshGenerator.MergeMeshes(deepMesh, nonFlatCurve);
    }

    private void InstantiateSpheres()
    {
        foreach(Vector3 vec in movedCurveVertices)
        {
            GameObject go = Instantiate(spherePrefab, vec, Quaternion.identity);
            go.transform.parent = transform;
        }
    }

    private void CreateMesh()
    {
        Vector3[] curveVertices = cuttingMeshGenerator.getVertices();
        movedCurveVertices = new Vector3[curveVertices.Length];
        for(int i = 0; i < curveVertices.Length; i++)
        {
            float x = curveVertices[i].x + cuttingMeshGenerator.lengthCoef/distToCuttingMeshCoef;
            movedCurveVertices[i] = new Vector3(x, curveVertices[i].y, curveVertices[i].z);
        }

        deepCorrespondingVertices = new Vector3[curveVertices.Length];
        for (int i = 0; i < curveVertices.Length; i++)
        {
            deepCorrespondingVertices[i] = new Vector3(movedCurveVertices[0].x + cuttingMeshGenerator.lengthCoef, curveVertices[i].y, curveVertices[i].z);
        }

        List<Vector3> sortedVertices = new List<Vector3>();
        sortedVertices = cuttingMeshGenerator.SortVertices(deepCorrespondingVertices, movedCurveVertices);

        deepVertices = sortedVertices.ToArray();

        deepTriangles = new int[deepVertices.Length];
        for (int i = 0; i < deepVertices.Length; i++)
        {
            deepTriangles[i] = i;
        }
    }

    private void OnDrawGizmos()
    {
        /*
        for(int i = 0; i<movedCurveVertices.Length; i++)
        {
            Gizmos.DrawSphere(movedCurveVertices[i], 0.001f);
            Gizmos.DrawSphere(deepCorrespondingVertices[i], 0.001f);
        }
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(movedCurveVertices[0], 0.001f);*/

        if(nonFlatCurve != null)
        {
            foreach (Vector3 vert in nonFlatCurve.vertices)
            {
                Gizmos.DrawSphere(vert, 0.001f);
            }
        }
        
    }

    private void createMovedCurve()
    {
        Vector3[] curveVertices = cuttingMeshGenerator.getVertices();
        movedCurveVertices = new Vector3[curveVertices.Length];
        for (int i = 0; i < curveVertices.Length; i++)
        {
            float x = curveVertices[i].x + 0.0025f + cuttingMeshGenerator.lengthCoef * 0.01f * distToCuttingMeshCoef;
            movedCurveVertices[i] = new Vector3(x, curveVertices[i].y, curveVertices[i].z);
        }
    }

    IEnumerator DelayedMeshCreation(){
        yield return new WaitUntil(() => cuttingMeshGenerator.meshWasCreated);
        //CreateMesh();
        //UpdateMesh();
        //GetComponent<MeshCollider>().sharedMesh = deepMesh;

        createMovedCurve();
        InstantiateSpheres();
    }
}
