using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// used for testing purposes only
/// </summary>
public class SpawnEntry : MonoBehaviour
{

    public GameObject prefab;
    private PlanePanelFunctionality plane;
    public Texture texture;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Spawn()
    {
        yield return new WaitForSeconds(1);
        for(int i = 0; i < 10; i++)
        {
            GameObject go = Instantiate(prefab, transform, false);
            plane = go.GetComponent<PlanePanelFunctionality>();
            //Texture texture = ScreenshotMaker.GetScreenshot("test");
            plane.SetElements("plane" + i, false, texture);
            yield return new WaitForSeconds(1);
        }       
    }
}
