using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// a class that manages nearly all text fields in multiple scenes
/// </summary>
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

    public GameObject FinishedText;
    public TextMeshPro CuttingPlaneName;

    public bool AreInGameTextsInit { get; set; } = false;


    [Header("Other UI Elements")]
    [Tooltip("Meant are the panels that open when you click a button on the ui")]
    public GameObject[] OverlayPanel;
    public GameObject PopUpText_go;
    private TextMeshProUGUI PopUpText;
    public GameObject[] BindingsAndButtons;

    // Start is called before the first frame update
    void Start()
    {
        // sets up texts in vr
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

    /// <summary>
    /// Triggers coroutine for displaying the finnished text in vr
    /// </summary>
    public void TriggerFinishedText()
    {
        StartCoroutine(FinishedTextCoroutine());
    }

    /// <summary>
    /// displays the animated text when finishing cutting in vr
    /// </summary>
    /// <returns></returns>
    IEnumerator FinishedTextCoroutine()
    {
        FinishedText.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        FinishedText.SetActive(false);
    }

    public void ExitGame()
    {
        Debug.Log("Game was closed");
        Application.Quit();
    }

    /// <summary>
    /// disables all ui panels in cutting plane creation
    /// </summary>
    public void DisablePanels()
    {
        foreach(GameObject go in OverlayPanel)
        {
            go.SetActive(false);
        }
    }

    /// <summary>
    /// opens a ui planel by name in cutting plane creation
    /// </summary>
    /// <param name="name"></param>
    public void OpenSubMenu(string name)
    {
        foreach (GameObject go in OverlayPanel)
        {
            if (go.name == name) go.SetActive(true);
        }
    }

    /// <summary>
    /// gets a submenu by name in cutting plane creation
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public GameObject GetSubMenu(string name)
    {
        foreach (GameObject go in OverlayPanel)
        {
            if (go.name == name) return go;
        }
        return null;
    }

    /// <summary>
    /// checks if any submenu is enabled in cutting plane creation
    /// </summary>
    /// <returns></returns>
    public bool IsAnySubMenuEnabled()
    {
        foreach (GameObject go in OverlayPanel)
        {
            if (go.activeInHierarchy) return true;
        }
        return false;
    }

    /// <summary>
    /// enables or disables the ui in cutting plane creation
    /// </summary>
    public void UI_SwitchEnabledState()
    {
        bool enabledState = BindingsAndButtons[0].activeInHierarchy;
        foreach(GameObject go in BindingsAndButtons)
        {
            go.SetActive(!enabledState);
        }
    }

    /// <summary>
    /// manages the text that pops up when e.g. saving a cutting plane
    /// </summary>
    /// <param name="text"></param>
    public void ShowPopUpText(string text)
    {
        PopUpText = PopUpText_go.GetComponent<TextMeshProUGUI>();
        PopUpText.text = text;
        StartCoroutine(PopUpTextAnim());
    }

    /// <summary>
    /// animates the text that pops up when e.g. saving a cutting plane
    /// </summary>
    /// <returns></returns>
    private IEnumerator PopUpTextAnim()
    {
        PopUpText_go.SetActive(true);
        yield return new WaitForSeconds(1.1f); //since animation duration is around 1.1 secs
        PopUpText_go.SetActive(false);
    }
}
