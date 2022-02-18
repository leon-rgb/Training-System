using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Class that creates the "CutTooDeep"-Spheres and calculates the accuracy of the cutting
/// </summary>
public class CuttingAccuracy : MonoBehaviour
{
    // the cutting plane
    private MeshGeneratorLeg cuttingMeshGenerator;
    public GameObject cuttingMeshObj;

    // variables of this this mesh 
    // (actually isn't a mesh anymore, only the moved vertices are used)
    Mesh deepMesh;
    private Vector3[] deepVertices;
    private Vector3[] movedCurveVertices;
    private Vector3[] deepCorrespondingVertices;
    int[] deepTriangles;
    Mesh nonFlatCurve;

    // distance to the cutting plane of the "CutTooDeep"-Spheres
    public int distToCuttingMeshCoef { get; set; } //Range 0-20

    [Tooltip("Prefab of the \"CutTooDeep\"-Spheres")]
    public GameObject spherePrefab;

    // variables for calculating the accuracy
    private List<CuttingPlane_Sphere> allCuttingPlaneAccuracySpheres;
    public float CuttingPlaneAccuracy { get; set; } = 0;
    public float TotalAccuracy { get; set; } = 0;
    [Range(0.3f, 1)]
    public float UpdateAccuracyInterval;
    public float Difficulty { get; set; }

    [Tooltip("Select the collider of the saw blade in the scene")]
    public Transform mainTransform;
    private MainScript main;
    private bool isMainMissing;
    private bool difficultyIs0;

    // used to choose whether spheres are visible or not
    public Settings_applier settings_applier;

    private void Awake()
    {
        // init mesh and cutting plane
        deepMesh = new Mesh();
        cuttingMeshGenerator = cuttingMeshObj.GetComponent<MeshGeneratorLeg>();
        
        // check if main script is attached
        if (mainTransform)
        {
            // get main
            main = mainTransform.GetComponent<MainScript>();
            isMainMissing = false;
            return;
        }
        // used to prevent exceptions
        isMainMissing = true;
        difficultyIs0 = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        /*//GetComponent<Rigidbody>().sleepThreshold = 0.0f;
        deepMesh = new Mesh();
        //GetComponent<MeshFilter>().mesh = deepMesh;
        cuttingMeshGenerator = cuttingMeshObj.GetComponent<MeshGeneratorLeg>();
        //wait for creation of cutting mesh
        //StartCoroutine(DelayedMeshCreation());
        if (mainTransform)
        {
            main = mainTransform.GetComponent<MainScript>();
            isMainMissing = false;
            return;
        }
        isMainMissing = true;
        difficultyIs0 = false;*/
    }

    private void Update()
    {
        //calculate accuracy if main script is attached
        if (!isMainMissing && Time.time > 2)
        {
            StartCoroutine(CalculateCuttingPlaneAccuracy());
            StartCoroutine(CalculateTotalAccuracy());
        }   
    }

    /// <summary>
    /// Clears array that stores the spheres for calculating the accuracy and resets main script
    /// </summary>
    public void ClearAccuracyData()
    {
        cuttingMeshGenerator.AllCuttingPlaneAccuracySpheres.Clear();
        //CuttingPlaneAccuracy = 0;
        //TotalAccuracy = 0;
        main.ResetEverything();
    }

    /// <summary>
    /// Calculates the accuracy inside the cutting plane
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// calculates the total accuracy
    /// </summary>
    /// <returns></returns>
    private IEnumerator CalculateTotalAccuracy()
    {
        yield return new WaitForSeconds(UpdateAccuracyInterval);
        float depth = main.Depth * 1000; // *1000 to get size in mm 
        float cutToDeepCount = main.CutTooDeepCount;
        // you could scale x to make the accuracy score easier/harder -> e.g. x*0.5 is much easier.
        // or you could not take the size in mm --> e.g. depth = main.Depth * 100 (is in cm then).
        // of course you could also scale it in a non linear way.
        float x = cutToDeepCount * depth;
        x *= Difficulty; // scales x to 0.6 for default difficulty 
        //-> means that gradient of function is 0.6 of normal gradient

        // check to prevent errors
        if (Difficulty == 0 && !difficultyIs0)
        {
            difficultyIs0 = true;
            Debug.LogWarning("Difficulty was not set!");
        }

        //calculates value of the function used to calculate the accuracy
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
        
        // multiply error with accuracy in cutting plane and round the result
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

    /// <summary>
    /// Instantiate the "CutTooDeep"-Spheres as child of this object
    /// </summary>
    private void InstantiateSpheres()
    {
        foreach(Vector3 vec in movedCurveVertices)
        {
            GameObject go = Instantiate(spherePrefab, vec, Quaternion.identity);
            go.transform.parent = transform;
        }
        settings_applier.SetSpheresVisibility(Settings_applier.settings.ShowSpheres);
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

    /// <summary>
    /// duplicates and moves the curve of the cutting plane
    /// </summary>
    public void CreateMovedCurve()
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

        CreateMovedCurve();
        InstantiateSpheres();
    }

    /// <summary>
    /// Deletes current mesh and spheres and calculates new one based on vertices of the cutting plane
    /// </summary>
    public void CreateNewMesh()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        CreateMovedCurve();
        InstantiateSpheres();
    }
}
