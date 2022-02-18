using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SG;
using System;

/// <summary>
/// provides functionality to send haptic feedback for the gloves
/// </summary>
public class WaveformController : MonoBehaviour
{
    [Tooltip("assign right hand first")]
    /// <summary> The Gloves to send the effect to  </summary>
    public SG_HapticGlove[] hapticGloves = new SG_HapticGlove[0];

    /// <summary> The effect to send to the glove(s). </summary>
    public SG_Waveform currentWaveform;

    /// <summary> Optional multiple waveforms to send. </summary>
    public List<SG_Waveform> allWaveForms = new List<SG_Waveform>();

    public SG_Grabable Saw;
    private SG_GrabScript _GrabScriptRight;
    private SG_GrabScript _GrabScriptLeft;  


    // Start is called before the first frame update
    void Start()
    {
        currentWaveform = allWaveForms[0];
        _GrabScriptRight = hapticGloves[0].GetComponentInChildren<SG_GrabScript>();
        _GrabScriptLeft = hapticGloves[1].GetComponentInChildren<SG_GrabScript>();
    }

    // Update is called once per frame
    /*void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ApplyWaveform("CutTooDeep");
        }
        if (Input.GetKeyDown(KeyCode.B)){
            ApplyWaveform("Three Pulses", 5);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            ApplyWaveform("CutBones");
        }
    }*/

    IEnumerator waitShort()
    {
        //for()
        //if (allWaveForms.Find(currentWaveform) { }

        yield return new WaitForSeconds(3);
        foreach (SG_HapticGlove glove in hapticGloves)
        {
            if (glove != null)
            {
                glove.SendCmd(currentWaveform);
            }
        }
        yield return new WaitForSeconds(3);
        Debug.Log("wait");
    }

    /// <summary>
    /// sends a waveform by name
    /// </summary>
    /// <param name="waveName"></param>
    /// <param name="durationInS"> optional parameter for changing the duration of the wavform</param>
    public void ApplyWaveform(string waveName, float durationInS = -1)
    {
        currentWaveform = GetWaveform(waveName);

        if (durationInS != -1) //check if duration was set
        {
            // send waveform with specified duration and reset waveform to default aftwards
            float oldDuration = currentWaveform.duration_s;
            currentWaveform.duration_s = durationInS;
            SendCurrentWaveform();
            currentWaveform.duration_s = oldDuration;
        }  
        else
        {
            // send waveformw with default lenght
            SendCurrentWaveform();
        }
    }

    /// <summary>
    /// get a waveform by name
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public SG_Waveform GetWaveform(string name)
    {
        foreach(SG_Waveform wave in allWaveForms)
        {
            if(wave.name == name)
            {
                return wave;
            }
        }
        Debug.LogError("Waveform was not found");
        return null;
    }

    // sends current waveform of this script
    public void SendCurrentWaveform()
    {
        /* old version where feedback was send to both hands
        foreach (SG_HapticGlove glove in hapticGloves)
        {
            if (glove != null)
            {
                glove.SendCmd(currentWaveform);
            }
        }*/

        // makes sure that if only 1 glove is connected, feedback is sent to that glove
        if(!(hapticGloves[0].IsConnected() && hapticGloves[1].IsConnected())){
            hapticGloves[0].SendCmd(currentWaveform);
            hapticGloves[1].SendCmd(currentWaveform);
            return;
        }
        // if we are here, both gloves are connected
        // if saw grabbed by right hand send waveform to left hand
        if (Saw.GrabbedBy(_GrabScriptRight))
        {
            hapticGloves[1].SendCmd(currentWaveform);
            return;
        }
        // other way around
        hapticGloves[0].SendCmd(currentWaveform);
    }
}
