using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using TMPro;
using System;

[RequireComponent(typeof(Interactable))]
public class ExitScript : MonoBehaviour
{
    private Hand HoveringHand;
    public Transform TextTransform;
    private TextMeshPro tmpro;
    float timeLeft;
    float timerDuration = 4;
    float startTime;
    bool isHovering = false;

    private void Start()
    {
        tmpro = TextTransform.GetComponent<TextMeshPro>();
    }
    private void Update()
    {
        anything();
    }

    void anything()
    {
        if(GetComponent<Interactable>().hoveringHand != null)
        {
            GetComponent<MeshRenderer>().enabled = true;
            if (!isHovering)
            {
                isHovering = true;
                startTime = Time.time;
            }
            timeLeft = startTime + timerDuration - Time.time;
            tmpro.text = "Exiting in " + Math.Round(timeLeft) + " seconds";
            TextTransform.gameObject.SetActive(true);
            if (timeLeft <= 0)
            {
                tmpro.text = "EXITED!";
                Application.Quit();
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
