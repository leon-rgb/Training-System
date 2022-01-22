using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    [Header("InGame TextFields")]
    [Space]
    public RectTransform CutToDeepTxtTrafo;
    public RectTransform CutToDeepValTrafo;
    public RectTransform DepthTxtTrafo;
    public RectTransform DepthValTrafo;
    public RectTransform AccuracyTxtTrafo;
    public RectTransform AccuracyValTrafo;
    public RectTransform TotalAccuracyTxtTrafo;
    public RectTransform TotalAccuracyValTrafo;
    public TextMeshPro CutToDeepText { get; set; }
    public TextMeshPro CutToDeepValue { get; set; }
    public TextMeshPro DepthText { get; set; }
    public TextMeshPro DepthValue { get; set; }
    public TextMeshPro AccuracyText { get; set; }
    public TextMeshPro AccuracyValue { get; set; }
    public TextMeshPro TotalAccuracyText { get; set; }
    public TextMeshPro TotalAccuracyValue { get; set; }
    public bool AreInGameTextsInit { get; set; } = false;


    [Header("Other UI Elements")]
    public Transform test;

    // Start is called before the first frame update
    void Start()
    {
        if(CutToDeepTxtTrafo != null)
        {
            CutToDeepText = CutToDeepTxtTrafo.GetComponent<TextMeshPro>();
            CutToDeepValue = CutToDeepValTrafo.GetComponent<TextMeshPro>();
            DepthText = DepthTxtTrafo.GetComponent<TextMeshPro>();
            DepthValue= DepthValTrafo.GetComponent<TextMeshPro>();
            AccuracyText = AccuracyTxtTrafo.GetComponent<TextMeshPro>();
            AccuracyValue = AccuracyValTrafo.GetComponent<TextMeshPro>();
            TotalAccuracyText = TotalAccuracyTxtTrafo.GetComponent<TextMeshPro>();
            TotalAccuracyValue = TotalAccuracyValTrafo.GetComponent<TextMeshPro>();
            AreInGameTextsInit = true;
        }
        else
        {
            Debug.LogWarning("InGame Text Elements not set");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
