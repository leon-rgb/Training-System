using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Statistics = MathNet.Numerics.Statistics.Statistics;

/// <summary>
/// Used for serializing different things. 
/// Most importantly to Cutting Plane and a count that decides the default name of a cutting plane.
/// But also experiment data and the version of the application (version means glove or controller).
/// </summary>
public class JSON_Serializer : MonoBehaviour
{
    // Note that cutting points need to be in the right order in the hierarchy. 
    // At least the mid point has to be in the middle 
    private static Vector3[] cuttingPoints;

    // paths for saving serialized data
    private static string savePath;
    private static string countSavePath;
    private static string experimentDataSavePath;
    private static string experimentStatisticsSavePath;

    // mesh scripts
    private static Transform MeshGeneratorLeg;
    private static MeshGeneratorLeg meshGenerator;
    private static Transform CutToDeepMeshGenerator;
    private static CuttingAccuracy cuttingAccuracy;

    // current cutting plane player pref
    public const string StringNamePlayerPrefs = "currentCuttingPlaneName";

    private void Start()
    {     
        // setup all save paths
        savePath = Application.dataPath + "/json.txt";
        countSavePath = Application.dataPath + "/count.txt";
        experimentDataSavePath = Application.dataPath + "/ExperimentData.txt";
        experimentStatisticsSavePath = Application.dataPath + "/ExperimentStatistics.txt";

        // create version file if it not exists
        CreateVersionFile();

        // create settings file if it not exists
        if (!File.Exists(Application.dataPath + "/settings.txt"))
        {
            Settings.SaveOnFirstLaunch();
        }

        // create current cutting plane player pref if it not exists and set default cutting plane up then
        if (PlayerPrefs.GetString(StringNamePlayerPrefs) == null)
        {
            PlayerPrefs.SetString(StringNamePlayerPrefs, "");
            //SetupCuttingPlaneCompletly("");
        }      

        // setup last used cutting plane
        SetupCuttingPlaneCompletly(PlayerPrefs.GetString(StringNamePlayerPrefs));
    }

    [Serializable]
    public class CuttingPlane
    {
        public string name;
        public Vector3[] positions;
        public bool isAnimatable;
    }

    [Serializable]
    public class CuttingPlaneList
    {
        public List<CuttingPlane> cuttingPlanes;
    }

    [Serializable]
    public class CuttingPlaneCount
    {
        public int value;
    }

    /// <summary>
    /// Get's the Cutting Points in the scene
    /// </summary>
    /// <returns></returns>
    public static Vector3[] GetCuttingPoints()
    {
        Transform t = GameObject.FindGameObjectWithTag("CuttingPoints").transform;
        cuttingPoints = new Vector3[t.childCount];
        for (int i = 0; i < t.childCount; i++)
        {
            cuttingPoints[i] = t.GetChild(i).position;
        }
        return cuttingPoints;
    }

    /// <summary>
    /// save a cutting plane with a name and a bool that says if its animatable aka flat
    /// </summary>
    /// <param name="_name"></param>
    /// <param name="_isAnimatable"></param>
    /// <returns></returns>
    public static bool SaveCuttingPlane(string _name, bool _isAnimatable)
    {
        // get cutting points
        GetCuttingPoints();

        /*for (int i = 0; i < transform.childCount; i++)
        {
            cuttingPoints[i] = transform.GetChild(i).position;
        }*/

        // create CuttingPlan object
        CuttingPlane plane = new CuttingPlane
        {
            name = _name,
            positions = cuttingPoints,
            isAnimatable = _isAnimatable
        };

        // declare json data
        string json;

        // check if file already exists.
        if(File.Exists(savePath))
        {
            // check if name already exists in json file
            CuttingPlaneList cuttingPlaneList = LoadCuttingPlaneList();
            List<CuttingPlane> planeList = cuttingPlaneList.cuttingPlanes;
            foreach(CuttingPlane tmp_plane in planeList)
            {
                if (_name == tmp_plane.name) return false;
            }
            // save name (if new) as current cutting plane 
            PlayerPrefs.SetString(StringNamePlayerPrefs, _name);

            // plane name is new --> add it to list and update json file
            cuttingPlaneList.cuttingPlanes.Add(plane);
            json = JsonUtility.ToJson(cuttingPlaneList, true);
            File.Delete(savePath);
            File.WriteAllText(savePath, json);
            return true;
        }
        else
        {
            // save name as current cutting plane
            PlayerPrefs.SetString(StringNamePlayerPrefs, _name);

            // create tmp List to initialize json file with a list of cutting planes
            List<CuttingPlane> tmp = new List<CuttingPlane>();
            tmp.Add(plane);
            CuttingPlaneList planeList = new CuttingPlaneList();
            planeList.cuttingPlanes = tmp;

            // create json file
            json = JsonUtility.ToJson(planeList, true); // true is for printing the json better readable
            File.WriteAllText(savePath, json);
            return true;
        }       
    }

    /// <summary>
    /// deletes a saved cutting plane by it's name
    /// </summary>
    /// <param name="planeName"></param>
    /// <returns></returns>
    public static bool DeleteCuttingPlane(string planeName)
    {
        CuttingPlaneList cuttingPlaneList = LoadCuttingPlaneList();
        foreach (CuttingPlane tmp_plane in cuttingPlaneList.cuttingPlanes)
        {
            if (planeName == tmp_plane.name)
            {
                cuttingPlaneList.cuttingPlanes.Remove(tmp_plane);

                string json = JsonUtility.ToJson(cuttingPlaneList, true);
                File.Delete(savePath);
                File.WriteAllText(savePath, json);
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Loads a cutting plane by it's name out of a List of Cutting Plane
    /// </summary>
    /// <param name="planeName"></param>
    /// <param name="planeList"></param>
    /// <returns></returns>
    public static CuttingPlane LoadCuttingPlane(string planeName, List<CuttingPlane> planeList)
    {
        if (planeList != null)
        {
            foreach(CuttingPlane plane in planeList)
            {
                if (plane.name == planeName) return plane;
            }                       
        }
        return null;
    }

    /// <summary>
    /// Loads the Cutting Plane list out of the savePath
    /// </summary>
    /// <returns></returns>
    public static CuttingPlaneList LoadCuttingPlaneList()
    {
        if (File.Exists(savePath))
        {
            string fileContent = File.ReadAllText(savePath);
            if(fileContent != null)
            {
                CuttingPlaneList planeList = JsonUtility.FromJson<CuttingPlaneList>(fileContent);
                return planeList;
            }
        }
        return null;
    }

    /// <summary>
    /// Sets the cutting points into the right position when seting up a cutting plane
    /// </summary>
    /// <param name="planeName">if plane is animatable</param>
    /// <returns></returns>
    public static bool SetupCuttingPlane(string planeName)
    {
        // get cutting pints and CuttingPlaneList for null check in next if
        Transform t = GameObject.FindGameObjectWithTag("CuttingPoints").transform;
        CuttingPlaneList cuttingPlaneList = LoadCuttingPlaneList();

        // check if default plane should be loaded
        if (planeName == "" || cuttingPlaneList == null)
        {
            t.GetChild(0).position = new Vector3(-2.46281028f, 0.911469996f, 1.34659994f);
            t.GetChild(1).position = new Vector3(-2.44809985f, 0.889499962f, 1.34659994f);
            t.GetChild(2).position = new Vector3(-2.46429992f, 0.865499973f, 1.34659994f);
            return true;
        }

        List<CuttingPlane> planeList = cuttingPlaneList.cuttingPlanes;
        CuttingPlane plane = LoadCuttingPlane(planeName, planeList);       

        for (int i = 0; i < plane.positions.Length; i++)
        {
            //transform.GetChild(i).position = plane.positions[i];           
            t.GetChild(i).position = plane.positions[i];
        }
        return plane.isAnimatable;
    }

    /// <summary>
    /// Does all necceasery steps to setup a cutting plane into the scne by its name
    /// </summary>
    /// <param name="name"></param>
    public static void SetupCuttingPlaneCompletly(string name)
    {
        // setup cutting points and save current name
        bool isAnimatable = SetupCuttingPlane(name);
        PlayerPrefs.SetString(StringNamePlayerPrefs, name);

        // get mesh generators
        MeshGeneratorLeg = GameObject.Find("PlaneMeshGenerator").transform;
        meshGenerator = MeshGeneratorLeg.GetComponent<MeshGeneratorLeg>();
        CutToDeepMeshGenerator = GameObject.Find("CutToDeepMeshGenerator").transform;
        cuttingAccuracy = CutToDeepMeshGenerator.GetComponent<CuttingAccuracy>();


        // recalculate meshes
        meshGenerator.CreateNewMesh();
        cuttingAccuracy.CreateNewMesh();

        // signal SawAnimation if it should work
        SawAnimationGenerator.isCuttingPlaneFlat = isAnimatable;
    }

     
    /// <summary>
    /// saves and increases the (cutting plane) count. used for giving cutting planes a default name.
    /// </summary>
    public void SaveCount()
    {
        CuttingPlaneCount count = new CuttingPlaneCount();    

        // declare json data
        string json;

        // check if file already exists.
        if (File.Exists(countSavePath))
        {
            // load saved value and increment it
            count.value = LoadCount().value + 1;
            
            // save new value
            json = JsonUtility.ToJson(count, true);
            File.Delete(countSavePath);
            File.WriteAllText(countSavePath, json);
        }
        else
        {
            // set count to 2 since its the first plane saved (and next one saved will be no 2)
            count.value = 2;

            // create json file
            json = JsonUtility.ToJson(count, true); // true is for printing the json better readable
            File.WriteAllText(countSavePath, json);
        }
    }

    /// <summary>
    /// loads the current cutting plane count
    /// </summary>
    /// <returns></returns>
    public static CuttingPlaneCount LoadCount()
    {
        if (File.Exists(countSavePath))
        {
            string fileContent = File.ReadAllText(countSavePath);
            if (fileContent != null)
            {
                CuttingPlaneCount count = JsonUtility.FromJson<CuttingPlaneCount>(fileContent);
                return count;
            }
        }
        return null;
    }

    public static void DecreaseCount()
    {
        CuttingPlaneCount count = new CuttingPlaneCount();
        string json;
        count.value = LoadCount().value - 1;
        
        // save new value
        json = JsonUtility.ToJson(count, true);
        File.Delete(countSavePath);
        File.WriteAllText(countSavePath, json);
    }


    [Serializable]
    public class ExperimentData
    {
        public int TotalDataCount;

        public List<int> CutTooDeepCounts;

        public List<float> CutTooDeepLenghts;

        public List<float> PlaneAccuracies;

        public List<float> TotalAccuracies;
    }

    /// <summary>
    /// 
    /// 
    /// </summary>
    /// <param name="CutTooDeepCount"></param>
    /// <param name="CutTooDeepLength"></param>
    /// <param name="PlaneAccuracy"></param>
    /// <param name="TotalAccuracy"></param>
    public static void SaveExperimentData(int CutTooDeepCount, float CutTooDeepLength, float PlaneAccuracy, float TotalAccuracy)
    {
        ExperimentData experimentData = new ExperimentData();

        // declare json data
        string json;

        // check if file already exists.
        if (File.Exists(experimentDataSavePath))
        {
            experimentData = LoadExperimentData();

            experimentData.TotalDataCount++;
            experimentData.CutTooDeepCounts.Add(CutTooDeepCount);
            experimentData.CutTooDeepLenghts.Add(CutTooDeepLength);
            experimentData.PlaneAccuracies.Add(PlaneAccuracy);
            experimentData.TotalAccuracies.Add(TotalAccuracy);

            /*
            experimentData.AverageCutTooDeepCount = (experimentData.AverageCutTooDeepCount + CutTooDeepCount) / 2;
            experimentData.AverageCutTooDeepLength = (experimentData.AverageCutTooDeepLength + CutTooDeepLength) / 2;
            experimentData.AveragePlaneAccuracy = (experimentData.AveragePlaneAccuracy + PlaneAccuracy) / 2;
            experimentData.AverageTotalAccuracy = (experimentData.AverageTotalAccuracy + TotalAccuracy) / 2;
            */

            json = JsonUtility.ToJson(experimentData, true);
            File.Delete(experimentDataSavePath);
            File.WriteAllText(experimentDataSavePath, json);
        }
        else
        {
            experimentData.TotalDataCount = 1;
            experimentData.CutTooDeepCounts = new List<int>();
            experimentData.CutTooDeepCounts.Add(CutTooDeepCount);
            experimentData.CutTooDeepLenghts = new List<float>();
            experimentData.CutTooDeepLenghts.Add(CutTooDeepLength);
            experimentData.PlaneAccuracies = new List<float>();
            experimentData.PlaneAccuracies.Add(PlaneAccuracy);
            experimentData.TotalAccuracies = new List<float>();
            experimentData.TotalAccuracies.Add(TotalAccuracy);

            /*
            experimentData.AverageCutTooDeepCount = CutTooDeepCount;
            experimentData.AverageCutTooDeepLength = CutTooDeepLength;
            experimentData.AveragePlaneAccuracy = PlaneAccuracy;
            experimentData.AverageTotalAccuracy = TotalAccuracy;
            */
            
            // create json file
            json = JsonUtility.ToJson(experimentData, true); // true is for printing the json better readable
            File.WriteAllText(experimentDataSavePath, json);
        }
    }

    /// <summary>
    /// loads the experiment data file and returns it as ExperimentData
    /// </summary>
    /// <returns></returns>
    public static ExperimentData LoadExperimentData()
    {
        if (File.Exists(experimentDataSavePath))
        {
            string fileContent = File.ReadAllText(experimentDataSavePath);
            if (fileContent != null)
            {
                ExperimentData experimentData = JsonUtility.FromJson<ExperimentData>(fileContent);
                return experimentData;
            }
        }
        return null;
    }




    [Serializable]
    public class ExperimentStatistics
    {
        public ExperimentStatisticObject CutTooDeepCount;
        public ExperimentStatisticObject CutTooDeepLength;
        public ExperimentStatisticObject PlaneAccuracy;
        public ExperimentStatisticObject TotalAccuracy;
    }

    [Serializable]
    public class ExperimentStatisticObject
    {
        public float Median;
        public float Mean;
        public float Variance;
        public float StandardDeviation;
        public float Maximum;
        public float Minimum;
        public float InterquartileRange;
    }


    /// <summary>
    /// will be used to generate statistic values out of the saved experiment data
    /// </summary>
    /// <param name="experimentData"></param>
    public void GenerateStatisticValues(ExperimentData experimentData)
    {
        // TODO implement
        Debug.Log("Finished Calclulating statistic values and saved them in " + "path");
    }


    /// <summary>
    /// Creates a file with instructions how to specify which application version to use
    /// </summary>
    public static void CreateVersionFile()
    {
        
        string path = Application.dataPath + "/version.txt";
        //if path not exists...
        if (!File.Exists(path))
        {
            string[] lines = new string[]
            {
                "",
                "Write g (for gloves) or c(for controller) into the first line to specify which version you want to use.",
                "Reminder: To ensure haptic feedback works properly you should disconnect controllers/trackers you don't use from steamvr and restart steamvr."
            };
            File.WriteAllLines(path, lines);         
        }      
    }

    /// <summary>
    /// Gets the version specified by user to load correct VR scene
    /// </summary>
    /// <returns></returns>
    public static string GetVersion()
    {
        string path = Application.dataPath + "/version.txt";

        if (!File.Exists(path)) return null;

        string[] lines = File.ReadAllLines(path);
        return lines[0];
    }
}
