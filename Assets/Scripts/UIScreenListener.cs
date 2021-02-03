using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class UIScreenListener : MonoBehaviour
{
    private static UnityEvent onScreenSizeChange;
    private static float width = 1920;
    private static float height = 1080;
    private bool change = false;

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
    }
}
