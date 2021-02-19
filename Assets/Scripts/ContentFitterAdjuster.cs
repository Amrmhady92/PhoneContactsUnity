using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(ContentSizeFitter))]
public class ContentFitterAdjuster : MonoBehaviour
{
    ContentSizeFitter fitter;
    void Start()
    {
        fitter = this.GetComponent<ContentSizeFitter>();
       // OnScreenChange();
        //UIScreenListener.OnScreenSizeChange.AddListener(OnScreenChange);
    }

    private void OnScreenChange()
    {
        //Debug.Log(Screen.orientation);
        //if (UIScreenListener.ScreenOrientation == ScreenOrientation.Landscape)
        if (Screen.orientation == ScreenOrientation.Landscape || 
            Screen.orientation == ScreenOrientation.LandscapeLeft ||
            Screen.orientation == ScreenOrientation.LandscapeRight)
        {
            fitter.verticalFit = ContentSizeFitter.FitMode.MinSize;
            fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        }
        else
        {
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            fitter.horizontalFit = ContentSizeFitter.FitMode.MinSize;
        }
    }

    private void OnDestroy()
    {
        UIScreenListener.OnScreenSizeChange.RemoveListener(OnScreenChange);
    }
}
