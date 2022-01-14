using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGeneratorCutToDeep : MonoBehaviour
{
    private MeshGeneratorLeg cuttingMeshGenerator;
    public GameObject cuttingMeshObj;

    Mesh deepMesh;
    private Vector3[] deepVertices;
    private Vector3[] movedCurveVertices;
    private Vector3[] deepCorrespondingVertices;
    int[] deepTriangles;
    Mesh nonFlatCurve;

    [Range(5, 20)]
    public int distToCuttingMeshCoef;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().sleepThreshold = 0.0f;
        deepMesh = new Mesh();
        GetComponent<MeshFilter>().mesh = deepMesh;
        cuttingMeshGenerator = cuttingMeshObj.GetComponent<MeshGeneratorLeg>();
        //wait for creation of cutting mesh
        StartCoroutine(DelayedMeshCreation());
    }

    private void UpdateMesh()
    {
        
        deepMesh.Clear();
        deepMesh.vertices = deepVertices;
        deepMesh.triangles = deepTriangles;
        deepMesh.Optimize();
        deepMesh.RecalculateNormals();

        Mesh invMesh = cuttingMeshGenerator.InvertMesh(deepMesh, deepTriangles);
        invMesh = cuttingMeshGenerator.DuplicateAndMoveMesh(deepMesh, deepTriangles);

        deepMesh = cuttingMeshGenerator.MergeMeshes(deepMesh, invMesh);
        
        nonFlatCurve= cuttingMeshGenerator.CreateNonFlatCurveVertices(movedCurveVertices, deepVertices, deepTriangles);
       
        Debug.Log("DEEP VERTS: " + deepVertices);
        Debug.Log(nonFlatCurve.vertices);
        /*
        deepMesh.Clear();
        deepMesh.vertices = deepVertices;
        deepMesh.triangles = deepTriangles;
        deepMesh.Optimize();
        deepMesh.RecalculateNormals();*/
        //deepMesh = cuttingMeshGenerator.MergeMeshes(deepMesh, nonFlatCurve);
        deepMesh.Clear();
        deepMesh = nonFlatCurve;
        deepMesh.RecalculateNormals();        //deepMesh = cuttingMeshGenerator.MergeMeshes(deepMesh, nonFlatCurve);
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

    IEnumerator DelayedMeshCreation(){
        yield return new WaitUntil(() => cuttingMeshGenerator.meshWasCreated);
        CreateMesh();
        UpdateMesh();
        GetComponent<MeshCollider>().sharedMesh = deepMesh;
    }
}
