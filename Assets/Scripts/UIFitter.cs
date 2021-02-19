using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIFitter : MonoBehaviour
{
    [SerializeField] float landscapeWidth = 1900;
    [SerializeField] float portraitWidth = 1000;
    [SerializeField] float landscapeHeight = 120;
    [SerializeField] float portraitHeight = 220;
    [SerializeField] bool setWidth = false;
    [SerializeField] bool setHeight = false;


    //RectTransform rectT;
    //Vector2 v;
    LayoutElement layoutElement;
    private void Start()
    {
        UIScreenListener.OnScreenSizeChange.AddListener(UpdateFitter);
        //rectT = this.GetComponent<RectTransform>();
        layoutElement = this.GetComponent<LayoutElement>();
    }
    public void UpdateFitter()
    {
        //v = rectT.sizeDelta;
        bool landscape = UIScreenListener.Width / UIScreenListener.Height > 1;
        if (setWidth)
        {
            if (landscape)
            {
                // v.x = landscapeWidth;
                layoutElement.preferredWidth = landscapeWidth;
            }
            else
            {
                //v.x = portraitWidth;
                layoutElement.preferredWidth = portraitWidth;
            }
        }

        if (setHeight)
        {
            if (landscape)
            {
                //v.y = landscapeHeight;
                layoutElement.preferredHeight = landscapeHeight;
            }
            else
            {
                //v.y = portraitHeight;
                layoutElement.preferredHeight = portraitHeight;
            }
        }
        //rectT.sizeDelta = v;
    }

    private void OnDestroy()
    {
        UIScreenListener.OnScreenSizeChange.RemoveListener(UpdateFitter);

    }
}
