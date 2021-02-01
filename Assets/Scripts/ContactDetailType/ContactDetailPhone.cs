using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ContactDetailPhone : ContactDetail
{

    [SerializeField] private GameObject editButton;
    [SerializeField] private TMPro.TMP_InputField inputFieldObject;
    [SerializeField] private TMP_Dropdown dropDown;

    public override void OnContactDetailClicked()
    {
        //Call
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

    public void OnDropDownSelected(string value)
    {
        detailTypeText.text = value;
    }
}
