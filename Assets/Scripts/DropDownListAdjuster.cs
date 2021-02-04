using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DropDownListAdjuster : MonoBehaviour
{
    private TMP_Dropdown dropDown;
    private RectTransform rectTransform;
    float yToHeightPercent;

    private void Start()
    {
        dropDown = this.GetComponent<TMP_Dropdown>();
        rectTransform = this.GetComponent<RectTransform>();
        UIScreenListener.OnScreenSizeChange.AddListener(OnSizeChange);
        yToHeightPercent = rectTransform.sizeDelta.y / 1920;
    }


    public void OnSizeChange()
    {
        Vector2 size = rectTransform.sizeDelta;
        size.y = yToHeightPercent * UIScreenListener.Height;
        rectTransform.sizeDelta = size;

        size.x = dropDown.template.sizeDelta.x;
        size.y *= dropDown.options.Count;
        dropDown.template.sizeDelta = size;
    }


    private void OnDestroy()
    {
        UIScreenListener.OnScreenSizeChange.RemoveListener(OnSizeChange);
    }

}
