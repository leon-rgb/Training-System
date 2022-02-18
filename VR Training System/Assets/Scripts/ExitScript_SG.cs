using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SG;
using TMPro;
using UnityEngine.SceneManagement;
using System;

/// <summary>
/// Makes exiting in the VR scene with Glvoes possible
/// Works similar to basiv vr button
/// </summary>
public class ExitScript_SG : MonoBehaviour
{
    public SG_PhysicsGrab[] hapticGloves = new SG_PhysicsGrab[0];
    private SG_Interactable interactable;
    public Transform TextTransform;
    private TextMeshPro tmpro;
    float timeLeft;
    // duration user has to hold his hand into the exit area
    float timerDuration = 4;
    float startTime;
    bool isHovering = false;
    public MainScript mainScript;
    private bool exited = false;

    // Start is called before the first frame update
    void Start()
    {
        //interactable = GetComponent<SG_Interactable>();
        tmpro = TextTransform.GetComponent<TextMeshPro>();
        GetComponent<MeshRenderer>().enabled = false;
        TextTransform.gameObject.SetActive(false);
    }

    /*
    private void Update()
    {
        OnHover();
    }*/

   /*
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

        startTime = Time.time;
        TextTransform.gameObject.SetActive(true);
        GetComponent<MeshRenderer>().enabled = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Hand")) return;

        timeLeft = startTime + timerDuration - Time.time;
        tmpro.text = "Exiting in \n" + Math.Round(timeLeft) + " seconds";
        if (timeLeft <= 0 && !exited)
        {
            exited = true;             // prevents saving multiple experiment datas in editor mode
            timeLeft = float.MaxValue; // prevents saving multiple experiment datas in editor mode
            tmpro.text = "EXITED!";
            mainScript.ResetEverything();
            Application.Quit();
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
