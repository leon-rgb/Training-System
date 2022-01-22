using UnityEngine;
using Random = UnityEngine.Random;
using System;
using System.Collections;

public class MainScript : MonoBehaviour
{

    public float Depth { get; set; }
    public int CutTooDeepCount { get; set; }
    private Vector3 depthStart;

    [Header("UI Manager and Accuracy GO")]
    public Transform uiManagerTransform;
    private UI_Manager uiManager;

    public Transform cuttingAccuracy;
    private CuttingAccuracy cuttingAccuracyScript;


    private void OnEnable()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        //initialize variables
        Depth = 0;
        CutTooDeepCount = 0;

        //initialize variables
        uiManager = uiManagerTransform.GetComponent<UI_Manager>();
        cuttingAccuracyScript = cuttingAccuracy.GetComponent<CuttingAccuracy>();

        //initialize textfields
        StartCoroutine(initializeUITexts());
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUIText(Infotext.ACCURACY_PLANE);
        UpdateUIText(Infotext.ACCURACY_TOTAL);
    }

    IEnumerator initializeUITexts()
    {
        yield return new WaitUntil(() => uiManager.AreInGameTextsInit);
        uiManager.DepthText.text = "Max Length you cut to deep:";
        uiManager.CutToDeepText.text = "Times you cut to deep:";
        uiManager.AccuracyText.text = "Cutting accuracy (in plane):";
        uiManager.TotalAccuracyText.text = "Cutting accuracy (total):";
        UpdateUIText(Infotext.DEPTH);
        UpdateUIText(Infotext.TO_DEEP_COUNT);
    }

    public void UpdateUIText(Infotext infotext)
    {
        switch (infotext)
        {
            case Infotext.DEPTH:
                uiManager.DepthValue.text =  Math.Round(Depth * 100, 2) + "cm";              
                break;

            case Infotext.TO_DEEP_COUNT:
                uiManager.CutToDeepValue.text = "" + CutTooDeepCount;
                break;

            case Infotext.ACCURACY_PLANE:
                uiManager.AccuracyValue.text = cuttingAccuracyScript.CuttingPlaneAccuracy + "%";
                break;

            case Infotext.ACCURACY_TOTAL:
                uiManager.TotalAccuracyValue.text = cuttingAccuracyScript.TotalAccuracy + "%";
                break;
        }
    }

    public void OnCutTooDeepEnter(Vector3 triggerPos)
    {
        depthStart = triggerPos;
        CutTooDeepCount++;
        UpdateUIText(Infotext.TO_DEEP_COUNT);
    }
    public void OnCutTooDeepStay(Vector3 triggerPos)
    {
        float dist = Vector3.Distance(depthStart, triggerPos);
        if (dist > Depth)
        {
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
