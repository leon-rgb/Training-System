using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawAnimationGenerator : MonoBehaviour
{

    public Transform holoSaw;
    [SerializeField]
    private Transform[] SawPoints;
    [SerializeField]
    private float Treshhold;
    private float dist;
    private Vector3 initialSawPos;
    private Vector3 initialSawRot;

    private void Awake()
    {
        CalcSawPointDistance();
    }

    //for calculating the width of the saw blade
    private void CalcSawPointDistance()
    {
        dist = Vector3.Distance(SawPoints[0].position, SawPoints[1].position);
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

        Vector3[] startAndEndPoint = {holoSaw.position, (pointPairs[0]+pointPairs[1])/2};
        //CurveGenerator.SmoothLine(startAndEndPoint, 0.1f);
        //Keyframe[] keyframes = new Keyframe[];
        curve = AnimationCurve.Linear(0.0F, holoSaw.position.x, 2.0F, ((pointPairs[0] + pointPairs[1]) / 2).x);
        anim.SetCurve("", typeof(Transform), "xPosition",curve);
        holoSaw.GetComponent<Animation>().AddClip(anim, anim.name);
        holoSaw.GetComponent<Animation>().Play(anim.name);
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
        holoSaw.GetComponentInChildren<RotateHoloSawBasedOnSawPosition>().DisableHoloSawRotation();
        initialSawPos = holoSaw.position;
        initialSawRot = holoSaw.eulerAngles;
        Debug.Log("OK LETS GO");
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
        for (int i = 0; i < pointsToLookAt.Length - 1; i++)
        {
            yield return new WaitForSeconds(1);
            holoSaw.LookAt(pointsToLookAt[i]);
            while(Vector3.Distance(holoSaw.position, pointsToLookAt[i]) > Treshhold)
            {
                yield return new WaitForSeconds(0.04f);
                holoSaw.position = Vector3.MoveTowards(holoSaw.position, pointsToLookAt[i], 0.01f);
            }
            yield return new WaitForSeconds(1f);
            holoSaw.position = initialSawPos;
            Debug.Log("pointPairs[" + i + "] = " + pointsToLookAt[i]);
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
