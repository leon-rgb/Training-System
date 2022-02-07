using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings_applier: MonoBehaviour
{

    private static Settings.SettingsObject settings;

    private void Start()
    {
        //Todo path setzen von den settings, da dieser nicht existiert wenn script nicht aktiv ist
        //persistent data path in Settings und json verwenden....
        Settings.savePath = Application.dataPath + "/settings.txt";
        settings = Settings.Load();
        Debug.Log("test " + settings);
        SetHoloSawVisibility(settings.ShowAnimation);
    }

    IEnumerator WaitShort()
    {
        yield return new WaitForSeconds(3);
        settings = GetSettings();
        SetHoloSawVisibility(settings.ShowAnimation);
    }

    public static Settings.SettingsObject GetSettings()
    {
        return Settings.Load();
    }

    public static void SetHoloSawVisibility(bool isVisible) 
    {
        Debug.Log("isvisible : " + isVisible);
        GameObject.FindGameObjectWithTag("SawHolo").SetActive(isVisible);
    }

    public static void SetSpheresVisibility()
    {

    }

    public static void SetDifficulty()
    {

    }

    public static void SetDistance()
    {

    }

    public static void SetSpheresFrequency()
    {

    }

    public static void SetHapticFrequency()
    {

    }

}
