using UnityEngine;
using Valve.VR.InteractionSystem;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class OpenCuttingPlaneCreation : MonoBehaviour
{
    private Hand HoveringHand;
    public Transform TextTransform;
    private TextMeshPro tmpro;
    float timeLeft;
    float timerDuration = 4;
    float startTime;
    bool isHovering = false;
    public MainScript mainScript;
    private void Start()
    {
        tmpro = TextTransform.GetComponent<TextMeshPro>();
    }
    private void Update()
    {
        OnHover();
    }

    public void OnHover()
    {
        if (GetComponent<Interactable>().hoveringHand != null)
        {
            GetComponent<MeshRenderer>().enabled = true;
            if (!isHovering)
            {
                isHovering = true;
                startTime = Time.time;
            }
            timeLeft = startTime + timerDuration - Time.time;
            tmpro.text = "Switch to Cutting Plane Creation in " + Math.Round(timeLeft) + " seconds";
            TextTransform.gameObject.SetActive(true);
            if (timeLeft <= 0)
            {
                tmpro.text = "Loaded Scene!";
                mainScript.ResetEverything();
                SceneManager.LoadScene("CuttingPlaneCreation");
            }
        }
        else
        {
            TextTransform.gameObject.SetActive(false);
            GetComponent<MeshRenderer>().enabled = false;
            isHovering = false;
            startTime = int.MaxValue;
        }
    }
}
