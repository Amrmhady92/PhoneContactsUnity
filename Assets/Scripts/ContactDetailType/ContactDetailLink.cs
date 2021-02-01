using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ContactDetailLink : ContactDetail
{
    [SerializeField] private GameObject editButton;
    [SerializeField] private TMPro.TMP_InputField inputFieldObject;
    public override void OnContactDetailClicked()
    {
        //go to link
    }

    public void OnEditClicked()
    {
        editButton.SetActive(false);
        inputFieldObject.gameObject.SetActive(true);
    }

    public void OnEditEnd()
    {
        editButton.SetActive(true);
        inputFieldObject.gameObject.SetActive(false);
        detailValueText.text = inputFieldObject.text;
    }
}
