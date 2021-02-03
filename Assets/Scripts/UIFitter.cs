using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIFitter : MonoBehaviour
{
    //For Stretch 
    [SerializeField] float widthPercentage = 0;
    [SerializeField] bool widthResize = false;
    [SerializeField] float heightPercentage = 0;
    [SerializeField] bool heightResize = false;

    //[SerializeField] float xPercentage = 0;
    //[SerializeField] bool xPosition = false;
    //[SerializeField] float yPercentage = 0;
    //[SerializeField] bool yPosition = false;

    private float left, right, top, bottom;

    Vector2 v2;
    Vector2 v3;
    Rect rec;
    private RectTransform rectTransform;
    private LayoutElement element;
    private void Start()
    {
        UIFitterHandler.AddToFitter(this);
        rectTransform = this.GetComponent<RectTransform>();
        element = this.GetComponent<LayoutElement>();
    }
    public void UpdateFitter()
    {
        v2 = rectTransform.sizeDelta;
        if (widthResize)
        {
            v2.x = UIFitterHandler.Width * widthPercentage;
            if(element != null)
            {
                element.minWidth = v2.x;
            }
        }
        if (heightResize)
        {
            v2.y = UIFitterHandler.Height * heightPercentage;
            if (element != null)
            {
                element.minHeight = v2.y;
            }
        }
        rectTransform.sizeDelta = v2;

        //v3 = rectTransform.localPosition;
        //if (xPosition)
        //{
        //    v3.x = UIFitterHandler.Width * xPercentage;
        //}
        //if (yPosition)
        //{
        //    v3.y = UIFitterHandler.Height * yPercentage;
        //}
        //rectTransform.localPosition = v3;


        //rectTransform.rect.Set(v3.x, v3.y, v2.x, v2.y);

    }

    private void OnDestroy()
    {
        UIFitterHandler.RemoveFromFitter(this);
    }
}
