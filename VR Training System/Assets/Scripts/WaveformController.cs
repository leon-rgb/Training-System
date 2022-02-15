using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SG;
using System;

public class WaveformController : MonoBehaviour
{
    /// <summary> The Gloves to send the effect to  </summary>
    public SG_HapticGlove[] hapticGloves = new SG_HapticGlove[0];

    /// <summary> The effect to send to the glove(s). </summary>
    public SG_Waveform currentWaveform;

    /// <summary> Optional multiple waveforms to send. </summary>
    public List<SG_Waveform> allWaveForms = new List<SG_Waveform>();

    // Start is called before the first frame update
    void Start()
    {
        currentWaveform = allWaveForms[0];
    }

    /*/ Update is called once per frame
    void Update()
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
    /// 
    /// </summary>
    /// <param name="waveName"></param>
    /// <param name="durationInS"></param>
    public void ApplyWaveform(string waveName, float durationInS = -1)
    {
        currentWaveform = GetWaveform(waveName);

        if (durationInS != -1) //check if duration was set
        {
            float oldDuration = currentWaveform.duration_s;
            currentWaveform.duration_s = durationInS;
            SendCurrentWaveform();
            currentWaveform.duration_s = oldDuration;
        }  
        else
        {
            SendCurrentWaveform();
        }
    }

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

    public void SendCurrentWaveform()
    {
        foreach (SG_HapticGlove glove in hapticGloves)
        {
            if (glove != null)
            {
                glove.SendCmd(currentWaveform);
            }
        }
    }
}
