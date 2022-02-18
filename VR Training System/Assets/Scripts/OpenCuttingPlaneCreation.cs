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
    // duration user has to hold his hand into the exit area
    float timerDuration = 4;
    float startTime;
    bool isHovering = false;
    public MainScript mainScript;
    //public SwitchBetweenVRAndPC switchBetweenVR;
    public  DestroyOnSceneChange player;

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
                //switchBetweenVR.StopXR();
                player.Destroy();
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
