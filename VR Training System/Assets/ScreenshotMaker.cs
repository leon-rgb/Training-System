using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ScreenshotMaker : MonoBehaviour
{
    public Material muscleMat;
    public Material boneMat;

    private Camera ScreenshotCamera;
    private static string BasePath;
    private static readonly int width = 177 * 5;
    private static readonly int height = 100 * 5;

    private void Start()
    {
        ScreenshotCamera = GetComponent<Camera>();
        BasePath = Application.dataPath + "/";
        //TakeScreenshot("test");
    }

    /// <summary>
    /// Makes all necessary steps to take and save a screenshot
    /// </summary>
    /// <param name="planeName"></param>
    public void TakeScreenshot(string planeName)
    {
        var color = ChangeVisibility();

        Texture2D texture = Capture(ScreenshotCamera, width, height);

        string path = BasePath + planeName + ".png";
        SaveTexture(path, texture);

        ChangeVisibilityBack(color);
    }

    /// <summary>
    /// gets the screenshot corresponding to a plane name
    /// </summary>
    /// <param name="planeName"></param>
    /// <returns></returns>
    public static Texture GetScreenshot(string planeName)
    {
        byte[] pngBytes = File.ReadAllBytes(BasePath + planeName + ".png");
        Texture2D texture = new Texture2D(width, height);
        texture.LoadImage(pngBytes);
        return texture;
    }

    /// <summary>
    /// deletes the screenshot corresponding to a plane name
    /// </summary>
    /// <param name="planeName"></param>
    public static void DeleteScreenshot(string planeName)
    {
        File.Delete(BasePath + planeName + ".png");
    }

    /// <summary>
    /// Captures a screenshot of the current game state
    /// </summary>
    /// <param name="camera">the camera that should take the screenshot</param>
    /// <param name="width">output width of the screeshot</param>
    /// <param name="height">output height of the screenshot</param>
    /// <returns></returns>
    public static Texture2D Capture(Camera camera, int width, int height)
    {
        RenderTexture rt = new RenderTexture(width, height, 0);
        rt.depth = 24;
        rt.antiAliasing = 8;
        camera.targetTexture = rt;
        camera.RenderDontRestore();
        RenderTexture.active = rt;
        Texture2D texture = new Texture2D(width, height, TextureFormat.ARGB32, false, true);
        Rect rect = new Rect(0, 0, width, height);
        texture.ReadPixels(rect, 0, 0);
        texture.filterMode = FilterMode.Point;
        texture.Apply();
        camera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);
        return texture;
    }

    /// <summary>
    /// Save a texture as png
    /// </summary>
    /// <param name="path">save path</param>
    /// <param name="texture">texture to save</param>
    public static void SaveTexture(string path, Texture2D texture)
    {
        File.WriteAllBytes(path, texture.EncodeToPNG());
        Debug.Log("screenshot saved at " + path);
    }

    /// <summary>
    /// Changes alpha of muscle and bone material.
    /// </summary>
    /// <returns>initial bone color</returns>
    private Color ChangeVisibility()
    {
        var color = muscleMat.color;
        float old_alpha = color.a;
        color.a = 0.15f;
        muscleMat.color = color;
        color = boneMat.color;
        color.a = 0.15f;
        boneMat.color = color;
        color.a = old_alpha;
        return color;
    }

    /// <summary>
    /// Changes alpha of muscle and bone material to initial alpha
    /// </summary>
    /// <param name="color">initial color</param>
    private void ChangeVisibilityBack(Color color)
    {       
        boneMat.color = color;
        float old_alpha = color.a;
        color = muscleMat.color;
        color.a = old_alpha;
        muscleMat.color = color;
    }
}