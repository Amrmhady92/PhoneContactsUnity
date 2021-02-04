using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;

public class UIScreenListener : MonoBehaviour
{
    private static UnityEvent onScreenSizeChange;
    private static float width = 1920;
    private static float height = 1080;
    private bool change = false;
    private static ScreenOrientation screenOrientation;
    private static CanvasScaler scaler;
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

    public static UnityEvent OnScreenSizeChange
    {
        get
        {
            if (onScreenSizeChange == null) onScreenSizeChange = new UnityEvent();
            return onScreenSizeChange;
        }

        private set
        {
            onScreenSizeChange = value;
        }
    }

    public static ScreenOrientation ScreenOrientation
    {
        get
        {
            return screenOrientation;
        }

        private set
        {
            screenOrientation = value;
            if (screenOrientation == ScreenOrientation.Landscape)
            {
                scaler.referenceResolution = new Vector2(1920, 1080);
            }
            else if(screenOrientation == ScreenOrientation.Portrait)
            {
                scaler.referenceResolution = new Vector2(1080, 1920);
            }
        }
    }

    private void Start()
    {
        scaler = GameObject.FindObjectOfType<CanvasScaler>();
        ScreenOrientation = Screen.orientation;

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
            OnScreenSizeChange?.Invoke();
        }

        if (screenOrientation != Screen.orientation)
        {
            ScreenOrientation = Screen.orientation;
        }


    }
}
