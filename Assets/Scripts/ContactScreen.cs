using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ContactScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI contactNameText;
    [SerializeField] private Pooler contactDetailsPool;

    private List<GameObject> activeObjects;
    private ContactDetail cDetail = null;
    private GameObject detailObject = null;
    private Contact contact;
    public bool DisplayContact(Contact contact)
    {
        if(contactDetailsPool == null)
        {
            Debug.LogError("No pooler referenced");
            return false;
        }
        if (contact == null) return false;

        if (activeObjects == null) activeObjects = new List<GameObject>();
        else
        {
            for (int i = 0; i < activeObjects.Count; i++)
            {
                activeObjects[i].SetActive(false);
            }
            activeObjects.Clear();
        }
        this.contact = contact;

        //Name
        contactNameText.text = contact.name + " " + contact.lastname;
        //if(MakeContact(DetailType.Name) == false)
        //{
        //    Debug.LogError("Failed to Create Detail");
        //    return false;
        //}

        //Phones
        for (int i = 0; i < contact.phoneNumbers.Count; i++)
        {
            if (MakeContact(DetailType.Phone, i) == false)
            {
                Debug.LogError("Failed to Create Detail");
                return false;
            }
        }

        //Emails
        for (int i = 0; i < contact.emails.Count; i++)
        {
            if (MakeContact(DetailType.Email, i) == false)
            {
                Debug.LogError("Failed to Create Detail");
                return false;
            }
        }

        //Links
        for (int i = 0; i < contact.links.Count; i++)
        {
            if (MakeContact(DetailType.Link, i) == false)
            {
                Debug.LogError("Failed to Create Detail");
                return false;
            }
        }

        //Description
        if (MakeContact(DetailType.Description) == false)
        {
            Debug.LogError("Failed to Create Detail");
            return false;
        }

        //Date Added
        if (MakeContact(DetailType.DateAdded) == false)
        {
            Debug.LogError("Failed to Create Detail");
            return false;
        }

        return true;

    }

    private bool MakeContact(DetailType type, int index = 0)
    {
        detailObject = contactDetailsPool.Get(true);
        if (detailObject == null)
        {
            Debug.LogError("Couldnt Get GameObject from Pool, check refrence");
            return false;
        }
        activeObjects.Add(detailObject);
        cDetail = detailObject.GetComponent<ContactDetail>();
        if (cDetail != null)
        {
            if (cDetail.SetContactDetail(contact, type, index) == false)
            {
                detailObject.SetActive(false); // so we wouldnt have empty spaces, chances of getting a false is dim if not sent correctly
            }
        }
        else
        {
            Debug.LogError("Couldnt Get ContactDetail from Pool, check refrence");
            return false;
        }

        return true;
    }

    public void OnDeleteContactButtonPressed()
    {
        
    }

    public void OnEditContactButtonPressed()
    {
        Handler.Instance.CurrentState = ScreenState.ContactCreationScreen;
        if (Handler.Instance.CreateContactScreen.CreateContactWindow(contact) == false)
        {
            Handler.Instance.CurrentState = ScreenState.MainMenu;
        }
    }
}
