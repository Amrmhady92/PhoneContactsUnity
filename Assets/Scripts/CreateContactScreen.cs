using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class CreateContactScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;



    [SerializeField] private UIMover addNameWindow;
    [SerializeField] private UIMover addPhoneWindow;
    [SerializeField] private UIMover addEmailWindow;
    [SerializeField] private UIMover addLinkWindow;
    [SerializeField] private UIMover addDescriptionWindow;

    [SerializeField] private Pooler contactDetailPool;

    private List<GameObject> activeObjects;
    private ContactDetail cDetail = null;
    private GameObject detailObject = null;
    private Contact contact;
    private string oldContactName = "";
    private UIMover lastMover = null;
    private string insertedData_1;
    private string insertedData_2;
    private PhoneNumberType insertedPhoneNumberType;


    private DetailType selectedType;

    public string InsertedData_1
    {
        get
        {
            return insertedData_1;
        }

        set
        {
            insertedData_1 = value;
        }
    }
    public string InsertedData_2
    {
        get
        {
            return insertedData_2;
        }

        set
        {
            insertedData_2 = value;
        }
    }


    public bool CreateContactWindow(Contact c = null)
    {
        if (c == null) contact = new Contact();
        else
        {
            oldContactName = c.name;
            contact = new Contact(c);
        }
        return UpdateScreen();
    }

    private bool UpdateScreen()
    {
        if (activeObjects == null)
        {
            activeObjects = new List<GameObject>();
        }
        else
        {
            for (int i = 0; i < activeObjects.Count; i++)
            {
                activeObjects[i].SetActive(false);
            }
            activeObjects.Clear();
        }

        //Name
        if (contact.name != "")
        {
            nameText.text = contact.name + " " + contact.lastname;
        }

        //Phones
        for (int i = 0; i < contact.phoneNumbers.Count; i++)
        {
            Debug.Log(contact.phoneNumbers.Count);
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
        if (contact.description != "")
        {
            if (MakeContact(DetailType.Description) == false)
            {
                Debug.LogError("Failed to Create Detail");
                return false;
            }
        }
        return true;
    }

    public void OnDropDownListSelect(int index)
    {
        selectedType = (DetailType)index;
    }

    public void OnDropDownListPhoneSelect(int index)
    {
        insertedPhoneNumberType = (PhoneNumberType)index;
    }
    public void OnConfirmDetailTypeButtonPressed()
    {
        switch (selectedType)
        {
            case DetailType.Name:
                addNameWindow.UnHideObject();
                lastMover = addNameWindow;
                break;
            case DetailType.Phone:
                addPhoneWindow.UnHideObject();
                lastMover = addPhoneWindow;
                break;
            case DetailType.Email:
                addEmailWindow.UnHideObject();
                lastMover = addEmailWindow;
                break;
            case DetailType.Link:
                addLinkWindow.UnHideObject();
                lastMover = addLinkWindow;
                break;
            case DetailType.Description:
                addDescriptionWindow.UnHideObject();
                lastMover = addDescriptionWindow;
                break;
        }
    }

    public void OnConfirmAddedDetail()
    {
        switch (selectedType)
        {
            case DetailType.Name:
                nameText.text = "Name: " + insertedData_1 + " " + insertedData_2;
                contact.name = insertedData_1;
                contact.lastname = insertedData_2;
                break;
            case DetailType.Phone:
                PhoneNumber pNum = new PhoneNumber();
                pNum.type = insertedPhoneNumberType;
                pNum.number = insertedData_1;
                contact.phoneNumbers.Add(pNum);
                MakeContact(DetailType.Phone, contact.phoneNumbers.Count - 1);
                break;
            case DetailType.Email:
                contact.emails.Add(insertedData_1);
                MakeContact(DetailType.Email, contact.emails.Count);
                break;
            case DetailType.Link:
                contact.links.Add(insertedData_1);
                MakeContact(DetailType.Link, contact.links.Count);
                break;
            case DetailType.Description:
                contact.description = insertedData_1;
                MakeContact(DetailType.Description);
                break;
        }
        if (lastMover != null) lastMover.HideObject();
        UpdateScreen();
    }
    public void OnCancelButtonPressed()
    {

    }
    public void OnFinishedEditButtonPressed()
    {

    }

    private bool MakeContact(DetailType type, int index = 0)
    {
        detailObject = contactDetailPool.Get(true);
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
}
