using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFitterHandler : MonoBehaviour
{
    private static List<UIFitter> fitterObjects;

    private Canvas gameCanvas;
    private static float width = 1920;
    private static float height = 1080;
    private bool change = false;
    UIFitter tempFitter;
    public static List<UIFitter> FitterObjects
    {
        get
        {
            if (fitterObjects == null) fitterObjects = new List<UIFitter>();
            return fitterObjects;
        }

        private set
        {
            if (fitterObjects == null) fitterObjects = new List<UIFitter>();
            fitterObjects = value;
        }
    }

    public static float Width
    {
        get
        {
            return width;
        }

        private set
        {
            width = value;
        }
    }
    public static float Height
    {
        get
        {
            return height;
        }

        private set
        {
            height = value;
        }
    }

    public static void AddToFitter(UIFitter fitter)
    {
        if (!FitterObjects.Contains(fitter))
        {
            FitterObjects.Add(fitter);
        }
    }

    public static void RemoveFromFitter(UIFitter fitter)
    {
        if (FitterObjects.Contains(fitter))
        {
            FitterObjects.Remove(fitter);
        }
    }



    private void Update()
    {
        if (Display.displays[0].renderingWidth != width)
        {
            width = Display.displays[0].renderingWidth;
            change = true;
        }
        if (Display.displays[0].renderingHeight != height)
        {
            height = Display.displays[0].renderingHeight;
            change = true;
        }
        if (change)
        {
            change = false;
            UpdateFitters();
        }
    }

    private void UpdateFitters()
    {

        for (int i = 0; i < FitterObjects.Count; i++)
        {
            tempFitter = FitterObjects[i];
            if(tempFitter != null)
            {
                tempFitter.UpdateFitter();
            }
        }
    }
}
