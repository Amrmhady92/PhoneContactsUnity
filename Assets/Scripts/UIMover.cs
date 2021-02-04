using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMover : MonoBehaviour
{
    [SerializeField] private Vector3 offPos;
    [SerializeField] private float speed = 0.2f;
    [SerializeField] private bool offIsDown = true;
    [SerializeField] private bool hideOnStart = true;
    float startPosPerc = 1;
    private Vector3 startPos;
    private RectTransform rectTransform;
    private bool hidden = false;
    private void Start()
    {
        rectTransform = this.GetComponent<RectTransform>();
        startPos = rectTransform.position;
        startPosPerc = startPos.y / Display.displays[0].renderingHeight;
        UIScreenListener.OnScreenSizeChange.AddListener(ScreenChangedEvent);
        hidden = false;

        if(hideOnStart)
        {
            HideObject(0);
        }
    }

    public void HideObject(float setSpeed = -1)
    {
        if (hidden) return; 
        if(rectTransform == null) rectTransform = this.GetComponent<RectTransform>();

        float spd = speed;
        if (setSpeed >= 0) spd = setSpeed;

        offPos = rectTransform.position;
        offPos.y = Display.displays[0].renderingHeight + rectTransform.sizeDelta.y;
        if (offIsDown) offPos.y *= -1;

        rectTransform.LeanCancel();
        rectTransform.LeanMove(offPos, spd);//.setOnComplete(()=> { this.gameObject.SetActive(false); });
        hidden = true;
    }

    
    public void UnHideObject(float setSpeed = -1)
    {
        if (!hidden) return;
        if (rectTransform == null) rectTransform = this.GetComponent<RectTransform>();

        float spd = speed;
        if (setSpeed >= 0) spd = setSpeed;

        Vector3 tar = startPos;
        tar.y = Display.displays[0].renderingHeight * startPosPerc;

        rectTransform.LeanCancel();
        rectTransform.LeanMove(tar, spd);
        hidden = false;
    }

    public void ScreenChangedEvent()
    {
        startPos.y = Display.displays[0].renderingHeight * startPosPerc;
        if (hidden)
        {
            offPos = startPos;
            offPos.y = Display.displays[0].renderingHeight + rectTransform.sizeDelta.y;
            if (offIsDown) offPos.y *= -1;
            rectTransform.LeanCancel();
            rectTransform.LeanMove(offPos, 0);
        }
    }

    private void OnDestroy()
    {
        UIScreenListener.OnScreenSizeChange.RemoveListener(ScreenChangedEvent);
    }
}
