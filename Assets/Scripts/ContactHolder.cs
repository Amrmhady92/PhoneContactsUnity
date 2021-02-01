using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ContactHolder : MonoBehaviour
{

    Contact contact;

    [SerializeField] private TextMeshProUGUI contactNameText;
    [SerializeField] private TextMeshProUGUI contactMainNumberText;

    public bool SetConact(Contact c)
    {
        if (c == null)
        {
            Debug.LogError("NoContactSent");
            return false;
        }
        contact = c;

        contactNameText.text = contact.name;
        if (contact.phoneNumbers != null && contact.phoneNumbers.Count > 0)
        {
            contactMainNumberText.text = contact.phoneNumbers[0].number.ToString();
        }
        else
        {
            contactMainNumberText.text = "";
        }


        return true;
    }


    private void OnMouseDown() 
    {
        Handler.Instance.OnContactClicked(contact);
    }
}
