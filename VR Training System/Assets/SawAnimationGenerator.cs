using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Has methods for creating an automated movement for the holoSaw in the cutting Plane.
/// </summary>
public class SawAnimationGenerator : MonoBehaviour
{

    public Transform holoSawPivot;
    [SerializeField]
    private Transform[] SawPoints;
    [SerializeField]
    private float Treshhold;
    private float dist;
    private Vector3 initialSawPos;
    private Vector3 initialSawRot;
    private Vector3[] firstAndLastPoint;
    private float speed;
    private float distanceRange;
    private int angleRange;


    List<CustomTransform> transforms;

    public Transform Saw;
    public Transform SawPivot;
    public Transform HoloSawFrame1;
    private bool isSawClose;
    private float distanceBetweenSaws;
    private float angleBetweenSaws;
    public SawMat curMat { get;set;}
    public Material SawGreen;
    public Material SawRed;
    private bool wasMovementStarted;
    private MeshGeneratorLeg meshGenerator;
    List<Vector3> pointPairs;
    Vector3[] pointsToLookAt;

    public static bool isCuttingPlaneFlat;

    private void Awake()
    {
        // initialize variables
        CalcSawPointDistance();
        curMat = SawMat.RED;
        wasMovementStarted = false;
        meshGenerator = GameObject.Find("PlaneMeshGenerator").GetComponent<MeshGeneratorLeg>();
        speed = 0.06f; //default = 0.05f
        distanceRange = 0.085f; //default = 0.075f
        angleRange = 7; //default = 5
    }

    public void ResetEverything()
    {
        StopAllCoroutines();
        ChangeVisibilityOfSawParts(true);
        holoSawPivot.GetComponent<Animator>().enabled = false;
        wasMovementStarted = false;
        Awake();
    }

    private void Update()
    {
        // if plane is not flat do nothing
        if (!isCuttingPlaneFlat)
        {
            return;
        }

        // use the frame (or any other child of the saws) for calculating the distance
        // because the position center is not the same in the parent
        distanceBetweenSaws = Vector3.Distance(SawPivot.position, holoSawPivot.GetChild(0).position);

        //get angle between saw and holoSaw (not pivot)
        angleBetweenSaws = Quaternion.Angle(Saw.rotation, holoSawPivot.GetChild(0).rotation);
        // Debug.Log(distanceBetweenSaws + "   " + angleBetweenSaws + "   " + holoSawPivot.GetChild(0));
        
        // check if distance and angle between holded saw and holoSaw is in range
        if(distanceBetweenSaws < distanceRange && angleBetweenSaws < angleRange && angleBetweenSaws > -angleRange)
        {
            isSawClose = true;
            // change material if it's not already the right one
            if(curMat != SawMat.GREEN)
            {
                ChangeHoloSawColor(SawGreen);
                curMat = SawMat.GREEN;
            }
            // start saw movement if it wasn't already and mesh of cutting plane was already created
            if (!wasMovementStarted /*&& meshGenerator.meshWasCreated*/)
            {
                StartSawMovement();
                wasMovementStarted = true;
            }

        }
        else
        {
            isSawClose = false;
            // change material if it's not already the right one
            if (curMat != SawMat.RED)
            {
                ChangeHoloSawColor(SawRed);
                curMat = SawMat.RED;
            }
        }
    }
    private void OnDrawGizmos()
    {
        //Gizmos.DrawSphere(SawPivot.position, 0.05f);
        //Gizmos.DrawSphere(holoSawPivot.position, 0.05f);
        Gizmos.color = Color.black;

        Gizmos.DrawSphere(SawPoints[0].position, 0.01f);
        Gizmos.DrawSphere(SawPoints[1].position, 0.01f);
        Gizmos.DrawSphere(holoSawPivot.position, 0.01f);

        Gizmos.color = Color.red;
        if (pointPairs != null)
        {
            /*foreach(Vector3 point in pointsToLookAt)
            {
                Gizmos.DrawSphere(point, 0.01f);
            }*/
            Gizmos.DrawSphere(firstAndLastPoint[0], 0.01f);
            Gizmos.DrawSphere(firstAndLastPoint[1], 0.01f);
        }
    }

    public enum SawMat
    {
        GREEN,
        RED
    }

    //for calculating the width of the saw blade
    private void CalcSawPointDistance()
    {
        dist = Vector3.Distance(SawPoints[0].position, SawPoints[1].position); //* 0.8f;
    }

    /// <summary>
    /// Safes every point in points, that have aproximately the distance of the width of the saw
    /// </summary>
    /// <param name="points">points from which to chose from</param>
    /// <returns>List of points, that have aproximately the distance of the width of the saw </returns>
    private List<Vector3> GeneratePointPairs(Vector3[] points)
    {
        List<Vector3> pointPairs = new List<Vector3>();
        Vector3 curPoint = points[0];
        pointPairs.Add(curPoint);
        for (int i = 1; i < points.Length; i++)
        {
            Debug.Log("this was executed");
            if (Mathf.Abs(Vector3.Distance(curPoint, points[i]) - dist * 0.75f) <= Treshhold)
            {
                pointPairs.Add(points[i]);
                curPoint = points[i];
            }
        }
        //check if last point is in pointPairs and add it with another point if not. (since we want to cut every part of the plane)
        if (!pointPairs.Contains(points[points.Length - 1]))
        {
            pointPairs.Add(points[points.Length - 1]);
            /**
            for (int i = points.Length - 2; i >= 0; i--)
            {
                if (Mathf.Abs(Vector3.Distance(curPoint, points[i]) - dist) <= Treshhold)
                {
                    pointPairs.Add(curPoint);
                    pointPairs.Add(points[i]);
                    break;
                }
            }*/

        }
        pointPairs.Add(points[points.Length - 1]);
        return pointPairs;    
    }

    /// <summary>
    /// executes everything needed to automatically move the saw and moves it afterwards.
    /// </summary>
    /// <returns></returns>
    public List<Vector3> StartSawMovement()
    {
        Vector3[] points = GameObject.Find("PlaneMeshGenerator").GetComponent<MeshGeneratorLeg>().getVertices();
        firstAndLastPoint = new Vector3[]
        {
            points[0],
            points[points.Length-1]
        };

        pointPairs = GeneratePointPairs(points);

        holoSawPivot.GetComponentInChildren<RotateHoloSawBasedOnSawPosition>().DisableHoloSawRotation();
        initialSawPos = holoSawPivot.position;
        initialSawRot = holoSawPivot.eulerAngles;
        //Debug.Log("OK LETS GO");
        pointsToLookAt= CalcPointsToLookAt(pointPairs);
        StartCoroutine(Look(pointsToLookAt));
        return pointPairs;
    }

    /// <summary>
    /// Creates an automated animation of the holo saw to give user an idea how to cut the cutting plan
    /// </summary>
    /// <param name="pointsToLookAt">points that the saw will be moved and rotated to</param>
    /// <returns></returns>
    IEnumerator Look(Vector3[] pointsToLookAt)
    {
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => isSawClose);
        // this curstom transform was created to be able to record the transform of the saw and rewind the recording
        // methods for this are InsertTransform() and RewindTransform() 
        transforms = new List<CustomTransform>();
        Treshhold /= 2;

        //note that distance is a new variable and has not the same semantic meaning as dist 
        //(dist is the distance between most left and most right point of the saw blade)
        //move saw to a middle position
        float distance = Vector3.Distance(holoSawPivot.position, (firstAndLastPoint[0] + firstAndLastPoint[1]) / 2);
        Debug.Log("distance  " + distance);
        while (Vector3.Distance(holoSawPivot.position, (firstAndLastPoint[0]+firstAndLastPoint[1])/2) > distance / 5)
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitUntil(() => isSawClose);
            holoSawPivot.position += holoSawPivot.forward * Time.deltaTime * speed;
        }
        InsertTransform();
        yield return new WaitForSeconds(1.5f);
        yield return new WaitUntil(() => isSawClose);

        //move top point down by half the saw blade width and move saw to the same heigth
        firstAndLastPoint[0].y -= dist / 2;
        float stepSize = Math.Abs(holoSawPivot.position.y - firstAndLastPoint[0].y) / 120;
        while (Math.Abs(holoSawPivot.position.y - firstAndLastPoint[0].y) > Treshhold)
        {
            holoSawPivot.position = new Vector3(holoSawPivot.position.x, holoSawPivot.position.y + stepSize, holoSawPivot.position.z);
            InsertTransform();
            yield return new WaitForEndOfFrame();
            yield return new WaitUntil(() => isSawClose);
        }
        holoSawPivot.position = new Vector3(holoSawPivot.position.x, firstAndLastPoint[0].y, holoSawPivot.position.z);
        InsertTransform();

        // move saw forward to the first point
        while (Vector3.Distance(holoSawPivot.position, firstAndLastPoint[0]) > Treshhold)
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitUntil(() => isSawClose);
            holoSawPivot.position += holoSawPivot.forward * Time.deltaTime * speed;
            InsertTransform();
        } 
        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => isSawClose);
        // make the saw move back to mid position
        while (RewindTransform()) { yield return new WaitForEndOfFrame(); }
        yield return new WaitUntil(() => isSawClose);

        Quaternion wantedRot;
        // for every pointToLook at rotate saw towards it and then move saw to that point
        for (int i = 0; i < pointsToLookAt.Length - 2; i++)
        {
            // sa
            float curDist = Vector3.Distance(holoSawPivot.position, pointsToLookAt[i]);
            // bool to check if saw was already rotated towards a point
            bool executed = false;
            while (curDist > Treshhold)
            {
                yield return new WaitForEndOfFrame();
                yield return new WaitUntil(() => isSawClose);
                //rotate saw towards point if it's the first iterationf or the point
                if (!executed)
                {
                    executed = true;
                    Vector3 dir = pointsToLookAt[i] - holoSawPivot.position;
                    wantedRot = Quaternion.LookRotation(dir);
                    while (true)
                    {
                        yield return new WaitForEndOfFrame();
                        yield return new WaitUntil(() => isSawClose);
                        dir = pointsToLookAt[i] - holoSawPivot.position;
                        Quaternion rot = Quaternion.LookRotation(dir);
                        holoSawPivot.rotation = Quaternion.Lerp(holoSawPivot.rotation, rot, 1.5f * Time.deltaTime);
                        if (-0.4f < Quaternion.Angle(holoSawPivot.rotation, wantedRot)  && Quaternion.Angle(holoSawPivot.rotation, wantedRot) < 0.4f) break;
                    }
                    holoSawPivot.LookAt(pointsToLookAt[i]);
                    // lerp to the wanted rotation over time
                }
                // move saw a bit and save transform
                InsertTransform();
                holoSawPivot.position += holoSawPivot.forward * Time.deltaTime * speed;
                curDist = Vector3.Distance(holoSawPivot.position, pointsToLookAt[i]);
            }
            yield return new WaitForSeconds(1f);
            yield return new WaitUntil(() => isSawClose);
            // make the saw move back to mid position
            while (RewindTransform()) { yield return new WaitForEndOfFrame(); }
            yield return new WaitUntil(() => isSawClose);
        }
        yield return new WaitForSeconds(1f);

        // lerp back to initial position
        wantedRot = Quaternion.Euler(initialSawRot);
        while (true)
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitUntil(() => isSawClose);
            holoSawPivot.rotation = Quaternion.Lerp(holoSawPivot.rotation, wantedRot, 1.5f * Time.deltaTime);
            if (-0.4f < Quaternion.Angle(holoSawPivot.rotation, wantedRot) && Quaternion.Angle(holoSawPivot.rotation, wantedRot) < 0.4f) break;
        }

        // move bot point up by half the saw blade width and move saw to the same heigth
        firstAndLastPoint[1].y += dist / 2;
        stepSize = Math.Abs(holoSawPivot.position.y - firstAndLastPoint[1].y) / 120;
        while (Math.Abs(holoSawPivot.position.y - firstAndLastPoint[1].y) > Treshhold)
        {
            holoSawPivot.position = new Vector3(holoSawPivot.position.x, holoSawPivot.position.y - stepSize, holoSawPivot.position.z);
            InsertTransform();
            yield return new WaitForEndOfFrame();
            yield return new WaitUntil(() => isSawClose);
        }
        holoSawPivot.position = new Vector3(holoSawPivot.position.x, firstAndLastPoint[1].y, holoSawPivot.position.z);
        InsertTransform();

        // move saw towards the last point
        while (Vector3.Distance(holoSawPivot.position, firstAndLastPoint[1]) > Treshhold)
        {
            InsertTransform();
            yield return new WaitForEndOfFrame();
            yield return new WaitUntil(() => isSawClose);
            holoSawPivot.position += holoSawPivot.forward * Time.deltaTime * speed * 0.25f;
        }
        InsertTransform();
        while (RewindTransform()) { yield return new WaitForEndOfFrame(); }

        // fade out the saw 
        yield return new WaitForSeconds(1);
        //holoSawPivot.GetComponent<Animator>().enabled = true;
        ChangeVisibilityOfSawParts(false);
        yield return new WaitForSeconds(1);

        // reset saw transform and treshhold
        holoSawPivot.position = initialSawPos;
        holoSawPivot.localEulerAngles = initialSawRot;
        Treshhold *= 2;      
    }


    private void ChangeVisibilityOfSawParts(bool visible)
    {
        foreach( Transform t in holoSawPivot.GetChild(0))
        {
            t.GetComponent<Renderer>().enabled = visible;
        }
    }

    /// <summary>
    /// Safes the middle of every two points next to each other in the list
    /// </summary>
    /// <param name="pointPairs">
    /// points of the curve of the cutting plane which have approximately the distance of the width of the saw blade
    /// </param>
    /// <returns>A List with the final points to look at</returns>
    private Vector3[] CalcPointsToLookAt(List<Vector3> pointPairs)
    {
        Vector3[] pointsToLookAt = new Vector3[pointPairs.Count - 1];
        for (int i = 1; i < pointPairs.Count - 1; i++)
        {
            Vector3 middle = (pointPairs[i - 1] + pointPairs[i]) / 2;
            pointsToLookAt[i - 1] = middle;
        }
        return pointsToLookAt;
    }

    /// <summary>
    /// safe the current position an roation of the saw
    /// </summary>
    private void InsertTransform()
    {
        transforms.Insert(0, new CustomTransform(holoSawPivot.position, holoSawPivot.rotation));
    }

    /// <summary>
    /// Sets the position and rotation of the Saw back to the last saved one
    /// </summary>
    /// <returns>True if Transform list is not empty</returns>
    private bool RewindTransform()
    {
        if(transforms.Count != 0)
        {
            CustomTransform rewindedTransform = transforms[0];
            holoSawPivot.position = rewindedTransform.pos;
            holoSawPivot.rotation = rewindedTransform.rot;
            transforms.RemoveAt(0);
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// A simplified Transform that can be easily used for recording position and rotation of a Transform.
    /// </summary>
    public class CustomTransform
    {
        public Vector3 pos;
        public Quaternion rot;

        public CustomTransform(Vector3 _pos, Quaternion _rot)
        {
            pos = _pos;
            rot = _rot;
        }

    }

    /// <summary>
    /// Changes the color of the holo Saw depending on the material parameter. 
    /// </summary>
    /// <param name="material"></param>
    private void ChangeHoloSawColor(Material material)
    {
        foreach (Transform child in holoSawPivot.GetChild(0).transform)
        {
            child.GetComponent<MeshRenderer>().material = material;
        }
    }

    /* This moves the saw around like shit, since it calls the test function again and again
    private void OnDrawGizmos()
    {
        Debug.Log("TEST");
        if(GameObject.Find("PlaneMeshGenerator").GetComponent<MeshGeneratorLeg>().getVertices().Length != 0)
        {
            List<Vector3> testList = test();
            Debug.Log("TESTLIST: " + testList);
            Gizmos.color = Color.black;
            foreach (Vector3 point in testList) { Gizmos.DrawSphere(point, 0.001f); Debug.Log("GIZMOS: " + point); }
        }
    }*/
}
