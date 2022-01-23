using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private float speed = 0.1f; //default = 0.05f

    private void Awake()
    {
        CalcSawPointDistance();
    }

    //for calculating the width of the saw blade
    private void CalcSawPointDistance()
    {
        dist = Vector3.Distance(SawPoints[0].position, SawPoints[1].position); //* 0.8f;
    }

    private void Start()
    {
        //Look(Vector3.zero);
        //test();
    }


    public AnimationClip GenerateClips(Vector3[] points)
    {
        // Get the positions of the points at the saw
        Vector3[] SawPointPositions;
        SawPointPositions = GetSawPointPositions();

        List<Vector3> pointPairs = GeneratePointPairs(points);

        AnimationClip anim = new AnimationClip();
        AnimationCurve curve = new AnimationCurve();

        Vector3[] startAndEndPoint = {holoSawPivot.position, (pointPairs[0]+pointPairs[1])/2};
        //CurveGenerator.SmoothLine(startAndEndPoint, 0.1f);
        //Keyframe[] keyframes = new Keyframe[];
        curve = AnimationCurve.Linear(0.0F, holoSawPivot.position.x, 2.0F, ((pointPairs[0] + pointPairs[1]) / 2).x);
        anim.SetCurve("", typeof(Transform), "xPosition",curve);
        holoSawPivot.GetComponent<Animation>().AddClip(anim, anim.name);
        holoSawPivot.GetComponent<Animation>().Play(anim.name);
        return null;
    }

    private List<Vector3> GeneratePointPairs(Vector3[] points)
    {
        Vector3[] SawPointPositions;
        SawPointPositions = GetSawPointPositions();

        List<Vector3> pointPairs = new List<Vector3>();

        Vector3 curPoint = points[0];
        pointPairs.Add(curPoint);
        for (int i = 1; i < points.Length; i++)
        {
            if (Mathf.Abs(Vector3.Distance(curPoint, points[i]) - dist) <= Treshhold)
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

    public List<Vector3> test()
    {
        Vector3[] points = GameObject.Find("PlaneMeshGenerator").GetComponent<MeshGeneratorLeg>().getVertices();
        Vector3[] SawPointPositions;    
        SawPointPositions = GetSawPointPositions();
        firstAndLastPoint = new Vector3[]
        {
            points[0],
            points[points.Length-1]
        };

        List<Vector3> pointPairs = new List<Vector3>();
        
        Vector3 curPoint = points[0];
        pointPairs.Add(curPoint);
        for (int i = 1; i < points.Length; i++)
        {
            Debug.Log("this was executed");
            if (Mathf.Abs(Vector3.Distance(curPoint, points[i]) - dist*0.75f) <= Treshhold)
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
        pointPairs.Add(points[points.Length-1]);

        /*
        AnimationClip anim = new AnimationClip();
        AnimationCurve curve = new AnimationCurve();

        Vector3[] startAndEndPoint = { holoSaw.position, (pointPairs[0] + pointPairs[1]) / 2 };
        //CurveGenerator.SmoothLine(startAndEndPoint, 0.1f);
        //Keyframe[] keyframes = new Keyframe[];
        curve = AnimationCurve.Linear(0.0F, holoSaw.position.x, 2.0F, ((pointPairs[0] + pointPairs[1]) / 2).x);
        anim.ClearCurves();
        anim.SetCurve("", typeof(Transform), "xPosition", curve);
        holoSaw.GetComponentInChildren<RotateHoloSawBasedOnSawPosition>().DisableHoloSawRotation();
        holoSaw.GetComponent<Animation>().AddClip(anim, "test");
        holoSaw.GetComponent<Animation>().Play(anim.name);
        */
        holoSawPivot.GetComponentInChildren<RotateHoloSawBasedOnSawPosition>().DisableHoloSawRotation();
        initialSawPos = holoSawPivot.position;
        initialSawRot = holoSawPivot.eulerAngles;
        //Debug.Log("OK LETS GO");
        Vector3[] pointsToLookAt= CalcPointsToLookAt(pointPairs);
        StartCoroutine(Look(pointsToLookAt));
        /*
        for (int i = 0; i < pointPairs.Count-1; i++)
        {
            StartCoroutine(Look(pointPairs[i]));
            Debug.Log("pointPairs[" + i + "] = " + pointPairs[i]);
        }*/
        //Look(Vector3.zero);
       // StartCoroutine(Look(pointPairs[0]));
        return pointPairs;
    }

    IEnumerator Look(Vector3[] pointsToLookAt)
    {
        yield return new WaitForSeconds(1);

        //holoSawPivot.position = new Vector3(holoSawPivot.position.x,  pointsToLookAt[0].y, holoSawPivot.position.z);
        //pointsToLookAt[0].x -= Treshhold*5;
        //StartCoroutine(Move(pointsToLookAt[0]));
        yield return new WaitForSeconds(1f);
        //yield return new WaitUntil(() => true);

        /*
        float dist = Vector3.Distance(holoSawPivot.position, pointsToLookAt[0]);
        float curDist = dist;
        bool executed = false;
        while (curDist > Treshhold)
        {
            //yield return new WaitForSeconds(0.04f);
            //holoSawPivot.position = Vector3.MoveTowards(holoSawPivot.position, targetPosition, 0.01f
            yield return new WaitForEndOfFrame();
            if((curDist < dist / 4) && !executed)
            {
                holoSawPivot.LookAt(pointsToLookAt[0]);
                executed = true;
            }
            holoSawPivot.position += holoSawPivot.forward * Time.deltaTime * 0.025f;
            //Vector3 dir = Vector3.RotateTowards(holoSawPivot.forward, pointsToLookAt[0], Time.deltaTime * 0.005f, 0);
            //holoSawPivot.LookAt(dir);
            //Debug.DrawRay(holoSawPivot.position, dir, Color.red);
            curDist = Vector3.Distance(holoSawPivot.position, pointsToLookAt[0]);
        }*/
        //holoSawPivot.position = initialSawPos;
        firstAndLastPoint[0].y -= dist / 2;
        

        holoSawPivot.position = new Vector3(holoSawPivot.position.x, firstAndLastPoint[0].y, holoSawPivot.position.z);
      
        while (Vector3.Distance(holoSawPivot.position, firstAndLastPoint[0]) > Treshhold)
        {
            //yield return new WaitForSeconds(0.04f);
            yield return new WaitForEndOfFrame();
            holoSawPivot.position += holoSawPivot.forward * Time.deltaTime * speed;
        }
        yield return new WaitForSeconds(1f);
        holoSawPivot.position = initialSawPos;
        holoSawPivot.localEulerAngles = initialSawRot;
        for (int i = 0; i < pointsToLookAt.Length - 2; i++)
        {
            
            //yield return new WaitUntil(() => true);          
            //Debug.Log("pointPairs[" + i + "] = " + pointsToLookAt[i]);
            float dist = Vector3.Distance(holoSawPivot.position, pointsToLookAt[i]);
            float curDist = dist;
            bool executed = false;
            while (curDist > Treshhold)
            {
                yield return new WaitForEndOfFrame();
                if ((curDist < dist / 4) && !executed)
                {
                    //holoSawPivot.LookAt(pointsToLookAt[i]);
                    executed = true;

                    Vector3 dir = pointsToLookAt[i] - holoSawPivot.position;
                    Quaternion wantedRot = Quaternion.LookRotation(dir);
                    while (true)
                    {
                        yield return new WaitForEndOfFrame();
                        dir = pointsToLookAt[i] - holoSawPivot.position;
                        //dir.y = 0;
                        //dir.z = 0;
                        Quaternion rot = Quaternion.LookRotation(dir);
                        holoSawPivot.rotation = Quaternion.Lerp(holoSawPivot.rotation, rot, 1.5f * Time.deltaTime);
                        //Debug.Log(from0to1);
                        if (-0.2f < Quaternion.Angle(holoSawPivot.rotation, wantedRot)  && Quaternion.Angle(holoSawPivot.rotation, wantedRot) < 0.2f) break;
                    }
                    holoSawPivot.LookAt(pointsToLookAt[i]);
                    // slerp to the desired rotation over time
                }
                holoSawPivot.position += holoSawPivot.forward * Time.deltaTime * speed;
                curDist = Vector3.Distance(holoSawPivot.position, pointsToLookAt[i]);
            }
            yield return new WaitForSeconds(1f);
            holoSawPivot.position = initialSawPos;
            holoSawPivot.localEulerAngles = initialSawRot;

            Debug.Log("pointPairs[" + i + "] = " + pointsToLookAt[i]);
        }
        firstAndLastPoint[1].y += dist / 2;
        holoSawPivot.position = new Vector3(holoSawPivot.position.x, firstAndLastPoint[1].y, holoSawPivot.position.z);

        while (Vector3.Distance(holoSawPivot.position, firstAndLastPoint[1]) > Treshhold)
        {
            //yield return new WaitForSeconds(0.04f);
            yield return new WaitForEndOfFrame();
            holoSawPivot.position += holoSawPivot.forward * Time.deltaTime * speed;
        }
        // LETZTEN PUNKT NOCH GERADE ABARBEITEN
        // nehme distanz von oberen saw punkt zu unterem / 2. Bewege ersten punkt temporär nach um diesen wert nach unten und unteren punkt umgekehrt.
    }

    IEnumerator Move(Vector3 targetPosition)
    {
        while (Vector3.Distance(holoSawPivot.position, targetPosition) > Treshhold)
        {
            //yield return new WaitForSeconds(0.04f);
            yield return new WaitForEndOfFrame();
            holoSawPivot.position = Vector3.MoveTowards(holoSawPivot.position, targetPosition, 0.01f);
        }
    }

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

    private Vector3[] GetSawPointPositions()
    {
        Vector3[] tmp = new Vector3[SawPoints.Length];
        for(int i = 0; i < SawPoints.Length; i++)
        {
            tmp[i] = SawPoints[i].position;
        }
        return tmp;
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
