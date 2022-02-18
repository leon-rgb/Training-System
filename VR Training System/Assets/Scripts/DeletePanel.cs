using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// functionality of the pop up apperaing when clicking "delete" in the Load/Delete menu
/// </summary>
public class DeletePanel : MonoBehaviour
{
    public Transform Text;
    private TextMeshProUGUI tmpro;
    public GameObject UI_Manager_go;

    public static string planeName { get; set; }

    private void OnEnable()
    {
        // display planeName in deletion question
        tmpro = Text.GetComponent<TextMeshProUGUI>();
        SetText("Do you really want to delete " + planeName + "?");
    }

    public void SetText(string text)
    {
        tmpro.text = text;
    }

    public void ClickedNo()
    {
        gameObject.SetActive(false);
    }

    public void ClickedYes()
    {
        // Delete corresponding cutting plane and screenshot
        Debug.Log("deletion value: " + JSON_Serializer.DeleteCuttingPlane(planeName));
        ScreenshotMaker.DeleteScreenshot(planeName);

        // reload the whole scroll view and set this menu inactive   
        GameObject content = GameObject.FindGameObjectWithTag("ScrollViewContent");
        content.SetActive(false);
        content.SetActive(true);
        gameObject.SetActive(false);

        // display that plane was deleted
        UI_Manager_go.GetComponent<UI_Manager>().ShowPopUpText("\"" + planeName + "\"  was deleted!");

        // set name in text
        // ...
        planeName = null;
    }

    /*public IEnumerator DeletionProcess()
    {
        yield return new WaitUntil(() => planeName != null);
        SetText("Do you really want to delete " + planeName + "?");

        JSON_Serializer.DeleteCuttingPlane(planeName);
        gameObject.SetActive(false);

        // set name in text
        // ...
       

        //reset name
        planeName = null;
    }*/
}
