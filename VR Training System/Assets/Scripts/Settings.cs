using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.IO;

/// <summary>
/// class for serializing settings
/// </summary>
public class Settings : MonoBehaviour
{
    public static string savePath;
    public static string savePathDefault;
    public static bool ShowAnimation { get; set; }
    public static int Difficulty { get; set; }
    public static int FrequencyHaptic { get; set; }
    public static bool ShowSpheres { get; set; }
    public static float FrequencySpheres { get; set; }
    public static int DistanceCutTooDeep { get; set; }

    private static SettingsObject SettingsOnMenuOpened;
    private static SettingsObject SettingsOnMenuClosed;

    public Transform AnimationTrafo;
    public Transform DifficultyTrafo;
    public Transform FrequencyHapticTrafo;
    public Transform ShowSpheresTrafo;
    public Transform FrequencySpheresTrafo;
    public Transform DistanceCutTooDeepTrafo;

    public GameObject Ui_manager_go;
    private UI_Manager Ui_Manager;

    public GameObject Settings_appl;

    [Serializable]
    public class SettingsObject
    {
        public bool ShowAnimation;
        public int Difficulty;
        public int FrequencyHaptic;
        public bool ShowSpheres;
        public float FrequencySpheres;
        public int DistanceCutTooDeep;
    }

    private void OnEnable()
    {
        savePath = Application.dataPath + "/settings.txt";
        savePathDefault = Application.dataPath + "/settings_default.txt";
        // if applicaiton is launched for the first time create initial settings
        if (!File.Exists(savePath))
        {
            SaveOnFirstLaunch();
        }

        if (Ui_manager_go.GetComponent<UI_Manager>())
        {
            Ui_Manager = Ui_manager_go.GetComponent<UI_Manager>();
        }
        
        OnMenuOpened();
    }

    private void OnDisable()
    {
        OnMenuClosed();
    }

    public void OnMenuOpened()
    {
        Debug.Log("Menu opened");
        // Load saved settings
        SettingsOnMenuOpened = Load();
        // set them as values for the objects in the menu
        SetValues(SettingsOnMenuOpened);
    }

    public void OnMenuClosed()
    {
        // update static properties
        GetValues();
        // create settings out of that properties
        SettingsOnMenuClosed = CreateSettingsObject();
        // check if settings have changed since menu opened
        if(AreSettingsEqual(SettingsOnMenuClosed, SettingsOnMenuOpened))
        {
            Debug.Log("Settings haven't changed");
            // disable ui panels and display that settigns were saved
            Ui_Manager.DisablePanels();
            Ui_Manager.ShowPopUpText("Settings haven't been changed.");
            return;
        }
        // settings have changed
        // disable ui panels and display that settings were saved
        Save();       
        Ui_Manager.DisablePanels();
        Ui_Manager.ShowPopUpText("Settings were saved!");
        Settings_appl.GetComponent<Settings_applier>().ApplySettingsNonVR(false);
    }

    /// <summary>
    /// Check if two SettingsObjects have the same values
    /// </summary>
    /// <param name="settings1"></param>
    /// <param name="settings2"></param>
    /// <returns></returns>
    public bool AreSettingsEqual(SettingsObject settings1, SettingsObject settings2)
    {
        return
            settings1.ShowAnimation == settings2.ShowAnimation &&
            settings1.ShowSpheres == settings2.ShowSpheres &&
            settings1.FrequencySpheres == settings2.FrequencySpheres &&
            settings1.FrequencyHaptic == settings2.FrequencyHaptic &&
            settings1.DistanceCutTooDeep == settings2.DistanceCutTooDeep &&
            settings1.Difficulty == settings2.Difficulty;
    }

    /*private void FixedUpdate()
    {
        GetValues();
    }*/

    /// <summary>
    /// Updates the static properties of this class
    /// </summary>
    public void GetValues()
    {
        ShowAnimation = AnimationTrafo.GetComponent<Toggle>().isOn;
        Difficulty = (int) DifficultyTrafo.GetComponent<Slider>().value;
        FrequencyHaptic = (int) FrequencyHapticTrafo.GetComponent<Slider>().value;
        ShowSpheres = ShowSpheresTrafo.GetComponent<Toggle>().isOn;
        FrequencySpheres = FrequencySpheresTrafo.GetComponent<Slider>().value;
        DistanceCutTooDeep = (int) DistanceCutTooDeepTrafo.GetComponent<Slider>().value;
    }

    public static void SaveOnFirstLaunch()
    {
        SettingsObject settings = SettingsOnFirstLaunch();
        string json = JsonUtility.ToJson(settings);
        File.WriteAllText(Application.dataPath + "/settings.txt", json);
        File.WriteAllText(Application.dataPath + "/settings_default.txt", json);
    }

    public static SettingsObject SettingsOnFirstLaunch()
    {
        SettingsObject settings = new SettingsObject
        {
            ShowAnimation = true,
            Difficulty = 3,
            FrequencyHaptic = 2,
            ShowSpheres = false,
            FrequencySpheres = 0.075f,
            DistanceCutTooDeep = 6
        };
        return settings;
    }

    /// <summary>
    /// creates a settingsObject out of the static properties of this class
    /// </summary>
    /// <returns></returns>
    public SettingsObject CreateSettingsObject()
    {
        SettingsObject settings = new SettingsObject
        {
            ShowAnimation = ShowAnimation,
            Difficulty = Difficulty,
            FrequencyHaptic = FrequencyHaptic,
            ShowSpheres = ShowSpheres,
            FrequencySpheres = FrequencySpheres,
            DistanceCutTooDeep = DistanceCutTooDeep
        };
        return settings;
    }

    public void Save()
    {
        Debug.Log("Settings were saved!");
        // update static properties
        GetValues();

        // create settings with those values
        SettingsObject settings = CreateSettingsObject();

        // declare json data
        string json;

        // check if file already exists.
        if (File.Exists(savePath))
        {
            // if file exists, update it
            json = JsonUtility.ToJson(settings, true);
            File.Delete(savePath);
            File.WriteAllText(savePath, json);
        }
        else
        {
            // create json file
            json = JsonUtility.ToJson(settings, true); // true is for printing the json better readable
            File.WriteAllText(savePath, json);
        }
    }

    public static SettingsObject Load()
    {
        if (File.Exists(savePath))
        {
            string fileContent = File.ReadAllText(savePath);
            if (fileContent != null)
            {
                SettingsObject count = JsonUtility.FromJson<SettingsObject>(fileContent);
                return count;
            }
        }
        return null;
    }

    public static SettingsObject LoadDefault()
    {
        if (File.Exists(savePathDefault))
        {
            string fileContent = File.ReadAllText(savePathDefault);
            if (fileContent != null)
            {
                SettingsObject count = JsonUtility.FromJson<SettingsObject>(fileContent);
                return count;
            }
        }
        return null;
    }

    /// <summary>
    /// Sets the values of the ui elements to the values of a settingsobject
    /// </summary>
    /// <param name="settings"></param>
    public void SetValues (SettingsObject settings)
    {
        AnimationTrafo.GetComponent<Toggle>().isOn = settings.ShowAnimation;
        DifficultyTrafo.GetComponent<Slider>().value = settings.Difficulty;
        FrequencyHapticTrafo.GetComponent<Slider>().value = settings.FrequencyHaptic;
        ShowSpheresTrafo.GetComponent<Toggle>().isOn = settings.ShowSpheres;
        FrequencySpheresTrafo.GetComponent<Slider>().value = settings.FrequencySpheres;
        DistanceCutTooDeepTrafo.GetComponent<Slider>().value = settings.DistanceCutTooDeep;
    }

    public void RestoreDefault()
    {
        // display that seetings were restored
        Ui_Manager.ShowPopUpText("Settings were restored to default!");

        // get default settings
        SettingsObject settings = LoadDefault();

        // load them into corresponding fields
        AnimationTrafo.GetComponent<Toggle>().isOn = settings.ShowAnimation;
        DifficultyTrafo.GetComponent<Slider>().value = settings.Difficulty;
        FrequencyHapticTrafo.GetComponent<Slider>().value = settings.FrequencyHaptic;
        ShowSpheresTrafo.GetComponent<Toggle>().isOn = settings.ShowSpheres;
        FrequencySpheresTrafo.GetComponent<Slider>().value = settings.FrequencySpheres;
        DistanceCutTooDeepTrafo.GetComponent<Slider>().value = settings.DistanceCutTooDeep;
    }
}
