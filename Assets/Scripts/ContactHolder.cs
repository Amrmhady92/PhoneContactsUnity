using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ContactHolder : MonoBehaviour
{

    Contact contact;

    [SerializeField] private TextMeshProUGUI contactNameText;
    [SerializeField] private TextMeshProUGUI contactMainNumberText;
    [SerializeField] private TextMeshProUGUI contactNotesText;




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
            contactMainNumberText.text = contact.phoneNumbers[0].ToString();
        }

        for (int i = 0; i < contact.phoneNumbers.Count; i++)
        {

        }
        for (int i = 0; i < contact.emails.Count; i++)
        {

        }
        for (int i = 0; i < contact.links.Count; i++)
        {

        }
        contactNotesText.text = contact.note;

        return true;
    }

    private void OnMouseDown() 
    {
        Handler.Instance.OnContactClicked(contact);
    }
}
