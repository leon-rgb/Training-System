using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    [Header("InGame TextFields")]
    [Space]
    public RectTransform CutToDeep;
    public RectTransform Depth;
    public RectTransform Accuracy;
    public TextMeshPro CutToDeepText { get; set; }
    public TextMeshPro DepthText { get; set; }
    public TextMeshPro AccuracyText { get; set; }

    
    [Header("Other UI Elements")]
    public Transform test;

    // Start is called before the first frame update
    void Start()
    {
        if(CutToDeep != null)
        {
            CutToDeepText = CutToDeep.GetComponent<TextMeshPro>();
            DepthText = Depth.GetComponent<TextMeshPro>();
            AccuracyText = Accuracy.GetComponent<TextMeshPro>();
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
