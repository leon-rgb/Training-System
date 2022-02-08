using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JSON_Serializer : MonoBehaviour
{
    // Note that cutting points need to be in the right order in the hierarchy. 
    // At least the mid point has to be in the middle 
    private static Vector3[] cuttingPoints;
    private static string savePath;
    private static string countSavePath;
    private static Transform MeshGeneratorLeg;
    private static MeshGeneratorLeg meshGenerator;
    private static Transform CutToDeepMeshGenerator;
    private static CuttingAccuracy cuttingAccuracy;

    private void Start()
    {
        savePath = Application.dataPath + "/json.txt";
        countSavePath = Application.dataPath + "/count.txt";
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

            // plane name is new --> add it to list and update json file
            cuttingPlaneList.cuttingPlanes.Add(plane);
            json = JsonUtility.ToJson(cuttingPlaneList, true);
            File.Delete(savePath);
            File.WriteAllText(savePath, json);
            return true;
        }
        else
        {
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

    public static void SetupCuttingPlane(string planeName)
    {
        List<CuttingPlane> planeList = LoadCuttingPlaneList().cuttingPlanes;
        CuttingPlane plane = LoadCuttingPlane(planeName, planeList);
        Transform t = GameObject.FindGameObjectWithTag("CuttingPoints").transform;
        for (int i = 0; i < plane.positions.Length; i++)
        {
            //transform.GetChild(i).position = plane.positions[i];           
            t.GetChild(i).position = plane.positions[i];
        }
    }

    public static void SetupCuttingPlaneCompletly(string name)
    {
        // setup cutting points
        SetupCuttingPlane(name);

        // get mesh generators
        MeshGeneratorLeg = GameObject.Find("PlaneMeshGenerator").transform;
        meshGenerator = MeshGeneratorLeg.GetComponent<MeshGeneratorLeg>();
        CutToDeepMeshGenerator = GameObject.Find("CutToDeepMeshGenerator").transform;
        cuttingAccuracy = CutToDeepMeshGenerator.GetComponent<CuttingAccuracy>();

        // recalculate meshes
        meshGenerator.CreateNewMesh();
        cuttingAccuracy.CreateNewMesh();
    }

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
}
