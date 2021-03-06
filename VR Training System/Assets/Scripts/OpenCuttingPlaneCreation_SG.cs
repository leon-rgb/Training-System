using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SG;
using TMPro;
using UnityEngine.SceneManagement;
using System;

/// <summary>
/// makes it possible to switch between glove vr scene and cutting plane creation
/// works similar to a basic vr button
/// </summary>
public class OpenCuttingPlaneCreation_SG : MonoBehaviour
{
    public SG_PhysicsGrab[] hapticGloves = new SG_PhysicsGrab[0];
    public Transform TextTransform;
    private TextMeshPro tmpro;
    float timeLeft;
    // duration user has to hold his hand into the exit area
    float timerDuration = 4;
    float startTime;
    bool isHovering = false;
    public MainScript mainScript;
    //public SwitchBetweenVRAndPC switchBetweenVR;
    public DestroyOnSceneChange player;

    private void Start()
    {
        //GetComponent<SG_Interactable>().SetHighLight(true);
        tmpro = TextTransform.GetComponent<TextMeshPro>();
        GetComponent<MeshRenderer>().enabled = false;
        TextTransform.gameObject.SetActive(false);
    }

    /*private void Update()
    {
        OnHover();
    }

    public void OnHover()
    {
        if (IsHovering())
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

    public bool IsHovering()
    {
        foreach (SG_PhysicsGrab grabber in hapticGloves)
        {
            if (grabber.IsHovering()) return true;
        }
        return false;
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Hand")) return;

        // start timer 
        startTime = Time.time;
        TextTransform.gameObject.SetActive(true);
        GetComponent<MeshRenderer>().enabled = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Hand")) return;

        timeLeft = startTime + timerDuration - Time.time;
        tmpro.text = "Switch to Cutting Plane Creation in " + Math.Round(timeLeft) + " seconds";
        if (timeLeft <= 0)
        {
            tmpro.text = "Loaded Scene!";
            mainScript.ResetEverything();
            //switchBetweenVR.StopXR();
            if(player != null) player.Destroy();
            SceneManager.LoadScene("CuttingPlaneCreation");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Hand")) return;

        TextTransform.gameObject.SetActive(false);
        GetComponent<MeshRenderer>().enabled = false;
        isHovering = false;
        startTime = int.MaxValue;
    }
}
