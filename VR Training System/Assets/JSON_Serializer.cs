using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JSON_Serializer : MonoBehaviour
{
    public Vector3[] cuttingPoints;
    public string savePath;

    private void Start()
    {
        savePath = Application.dataPath + "/json.txt";      
    }

    [Serializable]
    public class CuttingPlane
    {
        public String name;
        public Vector3[] positions;
        public bool isAnimatable;
    }

    [Serializable]
    public class CuttingPlaneList
    {
        public List<CuttingPlane> cuttingPlanes;
    }


    public bool SaveCuttingPlane(string _name, bool _isAnimatable)
    {
        // get cutting points
        cuttingPoints = new Vector3[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            cuttingPoints[i] = transform.GetChild(i).position;
        }

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

    public CuttingPlane LoadCuttingPlane(string planeName, List<CuttingPlane> planeList)
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

    public CuttingPlaneList LoadCuttingPlaneList()
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

    public void SetupCuttingPlane(string planeName)
    {
        List<CuttingPlane> planeList = LoadCuttingPlaneList().cuttingPlanes;
        CuttingPlane plane = LoadCuttingPlane(planeName, planeList);
        for(int i = 0; i < plane.positions.Length; i++)
        {
            transform.GetChild(i).position = plane.positions[i];
        }
    }

}
