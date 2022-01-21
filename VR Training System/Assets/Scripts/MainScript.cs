using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;
using System;

public class MainScript : MonoBehaviour
{

    [Tooltip("This is a tooltip")]
    public float timeoutLength;

    public static float Depth { get; set; }
    public static int CutTooDeepCount { get; set; }
    private static Vector3 depthStart;

    float curTime;

    [Header("UI Manager and Accuracy GO")]
    public Transform uiManagerTransform;
    private static UI_Manager uiManager;

    public Transform cuttingAccuracy;
    private static CuttingAccuracy cuttingAccuracyScript;


    private void OnEnable()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        //initialize variables
        CutTooDeepCount = 0;

        //initialize variables
        uiManager = uiManagerTransform.GetComponent<UI_Manager>();
        cuttingAccuracyScript = cuttingAccuracy.GetComponent<CuttingAccuracy>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUIText(Infotext.ACCURACY_PLANE);
    }

    public static void UpdateUIText(Infotext infotext)
    {
        switch (infotext)
        {
            case Infotext.DEPTH:
                uiManager.DepthText.text = "Max Length you cut to deep: " + Math.Round(Depth * 100, 2) + "cm";              
                break;

            case Infotext.TO_DEEP_COUNT:
                uiManager.CutToDeepText.text = "Times you cut to deep: " + CutTooDeepCount;
                break;

            case Infotext.ACCURACY_PLANE:
                uiManager.AccuracyText.text = "Cutting accuracy (in plane): " + cuttingAccuracyScript.CuttingPlaneAccuracy + "%";
                break;

            case Infotext.ACCURACY_TOTAL:

                break;
        }
    }

    public static void OnCutTooDeepEnter(Vector3 triggerPos)
    {
        depthStart = triggerPos;
        CutTooDeepCount++;
        UpdateUIText(Infotext.TO_DEEP_COUNT);
    }
    public static void OnCutTooDeepStay(Vector3 triggerPos)
    {
        float dist = Vector3.Distance(depthStart, triggerPos);
        Debug.Log("test1" + dist + "   " + Depth);
        if (dist > Depth)
        {
            Debug.Log("test2");
            Depth = dist;
            UpdateUIText(Infotext.DEPTH);
        }
    }

    public enum Infotext
    {
        DEPTH,
        TO_DEEP_COUNT,
        ACCURACY_PLANE,
        ACCURACY_TOTAL
    }
}
