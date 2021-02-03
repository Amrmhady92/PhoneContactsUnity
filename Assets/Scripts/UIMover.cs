using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMover : MonoBehaviour
{
    [SerializeField] private Vector3 offPos;
    [SerializeField] private float speed = 0.2f;
    [SerializeField] bool offIsDown = true;
    float startPosPerc = 1;
    private Vector3 startPos;
    private RectTransform rectTransform;
    private void Awake()
    {
        rectTransform = this.GetComponent<RectTransform>();
        startPos = rectTransform.position;
        startPosPerc = startPos.y / Display.displays[0].renderingHeight;
    }

    public void HideObject(float setSpeed = -1)
    {
        float spd = speed;
        if (setSpeed >= 0) spd = setSpeed;
        offPos = rectTransform.position;
        offPos.y = Display.displays[0].renderingHeight + rectTransform.sizeDelta.y;
        if (offIsDown) offPos.y *= -1;

        rectTransform.LeanCancel();
        rectTransform.LeanMove(offPos, spd);//.setOnComplete(()=> { this.gameObject.SetActive(false); });
    }

    
    public void UnHideObject(float setSpeed = -1)
    {
        float spd = speed;
        if (setSpeed >= 0) spd = setSpeed;
        Vector3 tar = startPos;
        tar.y = Display.displays[0].renderingHeight * startPosPerc;
        //this.gameObject.SetActive(true);
        rectTransform.LeanCancel();
        rectTransform.LeanMove(tar, spd);
    }
}
