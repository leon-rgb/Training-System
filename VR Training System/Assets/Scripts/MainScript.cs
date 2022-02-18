using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// This scripts connects the texts on the whiteboard in vr with the cutting accuracy script
/// It saves values about the cutting accuracy and updates the texts
/// </summary>
public class MainScript : MonoBehaviour
{
    // accuracy values
    public float Depth { get; set; }
    public int CutTooDeepCount { get; set; }
    private Vector3 depthStart;

    [Header("UI Manager and Accuracy GO")]
    public Transform uiManagerTransform;
    private UI_Manager uiManager;

    public Transform cuttingAccuracy;
    private CuttingAccuracy cuttingAccuracyScript;

    // for the finished text
    private bool WasFinishedDisplayed;


    private void OnEnable()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        //initialize scripts
        uiManager = uiManagerTransform.GetComponent<UI_Manager>();
        cuttingAccuracyScript = cuttingAccuracy.GetComponent<CuttingAccuracy>();

        //initialize variables
        //initialize textfields
        ResetEverything();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUIText(Infotext.ACCURACY_PLANE);
        UpdateUIText(Infotext.ACCURACY_TOTAL);

        // display finished text if it not already was and accuracy is 100%
        if (WasFinishedDisplayed) return;
        if (cuttingAccuracyScript.CuttingPlaneAccuracy >= 100)
        {
            WasFinishedDisplayed = true;
            uiManager.TriggerFinishedText();
        }
    }

    /// <summary>
    /// setup the texts on the whiteboard
    /// </summary>
    /// <returns></returns>
    IEnumerator initializeUITexts()
    {
        yield return new WaitUntil(() => uiManager.AreInGameTextsInit);
        uiManager.CuttingPlaneName.text = PlayerPrefs.GetString(JSON_Serializer.StringNamePlayerPrefs);
        uiManager.DepthText.text = "Max Length you cut to deep:";
        uiManager.CutToDeepText.text = "Times you cut to deep:";
        uiManager.AccuracyText.text = "Cutting accuracy (in plane):";
        uiManager.TotalAccuracyText.text = "Cutting accuracy (total):";
        UpdateUIText(Infotext.DEPTH);
        UpdateUIText(Infotext.TO_DEEP_COUNT);
    }

    /// <summary>
    /// reset all texts and values
    /// </summary>
    public void ResetEverything()
    {
        // only save experiment data if user started cutting
        if(cuttingAccuracyScript.CuttingPlaneAccuracy != 0)
        {
            JSON_Serializer.SaveExperimentData(CutTooDeepCount,
                        (float)Math.Round(Depth * 100, 2),
                        cuttingAccuracyScript.CuttingPlaneAccuracy,
                        cuttingAccuracyScript.TotalAccuracy);
        }
       
        Depth = 0;
        CutTooDeepCount = 0;
        cuttingAccuracyScript.CuttingPlaneAccuracy = 0;
        cuttingAccuracyScript.TotalAccuracy = 0;
        StartCoroutine(initializeUITexts());
        WasFinishedDisplayed = false;
    }

    /// <summary>
    /// update a specific text on the whiteboard
    /// </summary>
    /// <param name="infotext">the infotext to update</param>
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

    /// <summary>
    /// saves the start position when cutting to deep and increses cut to deep count
    /// </summary>
    /// <param name="triggerPos"></param>
    public void OnCutTooDeepEnter(Vector3 triggerPos)
    {
        depthStart = triggerPos;
        CutTooDeepCount++;
        UpdateUIText(Infotext.TO_DEEP_COUNT);
    }

    /// <summary>
    /// tracks the depth when cutting to deep
    /// </summary>
    /// <param name="triggerPos"></param>
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
