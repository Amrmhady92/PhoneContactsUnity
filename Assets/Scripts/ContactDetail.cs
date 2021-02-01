using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public abstract class ContactDetail : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI detailTypeText;
    [SerializeField] protected TextMeshProUGUI detailValueText;

    public void SetContactDetail(string contactDetail, string detailType, int index = 0)
    {
        string ind = "";
        if(index > 0)
        {
            ind = index + " ";
        }
        detailTypeText.text = detailType+ " " + ind + ":";
        detailValueText.text = contactDetail;
    }
    public abstract void OnContactDetailClicked();
}
