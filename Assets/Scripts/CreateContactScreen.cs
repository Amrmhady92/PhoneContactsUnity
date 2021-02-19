using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class CreateContactScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;


    [SerializeField] private UIMover addDetailWindow;
    [SerializeField] private UIMover addNameWindow;
    [SerializeField] private UIMover addPhoneWindow;
    [SerializeField] private UIMover addEmailWindow;
    [SerializeField] private UIMover addLinkWindow;
    [SerializeField] private UIMover addDescriptionWindow;

    [SerializeField] private UIMover confirmDeleteWindow;
    [SerializeField] private UIMover warningWindow;
    [SerializeField] private TextMeshProUGUI warningWindowText;
    [SerializeField] private GameObject blocker;


    [SerializeField] private Pooler contactDetailPool;
    [SerializeField] private GameObject addContactDetailButtonGO;
    [SerializeField] private GameObject finishButtonGO;
    [SerializeField] private GameObject cancelButtonGO;

    [SerializeField] private UIMover[] contactCreationScreenMovers;


    private List<GameObject> activeObjects;
    private ContactDetail cDetail = null;
    private GameObject detailObject = null;
    private Contact contact;
    private Contact oldContact;
    private UIMover lastMover = null;
    private string insertedData_1;
    private string insertedData_2;
    private PhoneNumberType insertedPhoneNumberType;

    private Action warningClosedAction;
    private bool deleteButtonDisabled = false;

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

    public UIMover[] ContactCreationScreenMovers
    {
        get
        {
            return contactCreationScreenMovers;
        }

        private set
        {
            contactCreationScreenMovers = value;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(lastMover != null)
            {
                lastMover.HideObject();
            }
            EnableDisableButtons(true);
            warningWindow.HideObject();
            UpdateScreen();
        }
    }

    public bool CreateContactWindow(Contact c = null)
    {
        //Editing a Contact
        if (c == null)
        {
            contact = new Contact();
            oldContact = null;
        }
        else // for new Contact
        {
            oldContact = new Contact(c);
            contact = new Contact(c);
        }

        insertedData_1 = "";
        insertedData_2 = "";
        deleteButtonDisabled = false;

        return UpdateScreen();
    }
    private bool UpdateScreen()
    {
        if (contact == null) return false;

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

         nameText.text ="Name: " + contact.name + " " + contact.lastname;
        

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


    public void OnAddDetailButtonPressed()
    {
        deleteButtonDisabled = true;
        EnableDisableButtons(false);
        addDetailWindow.UnHideObject(0.4f);
    }
    public void OnCloseAddDetailWindowPressed()
    {
        deleteButtonDisabled = false;
        EnableDisableButtons(true);
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
                contact.name = insertedData_1 == "" ? contact.name : insertedData_1;
                contact.lastname = insertedData_2 == "" ? contact.lastname : insertedData_2;
                break;
            case DetailType.Phone:
                PhoneNumber pNum = new PhoneNumber();
                pNum.type = insertedPhoneNumberType;
                pNum.number = insertedData_1;
                if(contact.phoneNumbers.Contains(pNum) == false)
                {
                    contact.phoneNumbers.Add(pNum);
                    MakeContact(DetailType.Phone, contact.phoneNumbers.Count - 1);
                }
                else
                {
                    SetWarning("Number Already Exists");
                    return;
                }
                break;
            case DetailType.Email:

                if(insertedData_1.Contains("@") == false)
                {
                    SetWarning("Email has no @");
                    return;
                }
                if (insertedData_1.IndexOf("@") != insertedData_1.LastIndexOf("@"))
                {
                    SetWarning("Email has multiple @");
                    return;
                }


                if (contact.emails.Contains(insertedData_1) == false)
                {
                    contact.emails.Add(insertedData_1);
                    MakeContact(DetailType.Email, contact.emails.Count - 1);
                }
                else
                {
                    SetWarning("Email Already Exists");
                    return;
                }
                break;

            case DetailType.Link:
                if(contact.links.Contains(insertedData_1) == false)
                {
                    contact.links.Add(insertedData_1);
                    MakeContact(DetailType.Link, contact.links.Count - 1);
                }
                else
                {
                    SetWarning("Link Already Exists");
                    return;
                }
                break;
            case DetailType.Description:

                if (contact.description != insertedData_1) 
                {
                    contact.description = insertedData_1;
                }

                if(contact.description != "")
                {
                    MakeContact(DetailType.Description);
                }
                else
                {
                    SetWarning("Description Not Changed");
                    return;
                }
                break;
        }
        if (lastMover != null) lastMover.HideObject();
        UpdateScreen();
        EnableDisableButtons(true);
        deleteButtonDisabled = false;
    }
    public void OnCancelButtonPressed()
    {
        contact = null;
        PhoneBook.Instance.CurrentState = ScreenState.MainMenu;
        EnableDisableButtons(true);
    }
    public void EnableDisableButtons(bool onOff)
    {
        finishButtonGO.SetActive(onOff);
        addContactDetailButtonGO.SetActive(onOff);
        cancelButtonGO.SetActive(onOff);
    }
    public void OnFinishedEditButtonPressed()
    {
        if (contact.name + contact.lastname == "")
        {
            EnableDisableButtons(false); // just to make sure
            SetWarning("Contact Has No Name", () => {
                EnableDisableButtons(true);
            });
            return;
        }
        if (ContactManager.FindExactContact(contact) != null)
        {
            EnableDisableButtons(false); // just to make sure
            SetWarning("Contact Already Exists", () => {
                EnableDisableButtons(true);
            });
            return;
        }

        if (oldContact != null)
        {
            ContactManager.RemoveContact(oldContact);
        }
        if(contact != null && contact.name != "")
        {
            contact.dateAdded = System.DateTime.Now;
            ContactManager.AddContact(contact);

        }
        EnableDisableButtons(true);
        PhoneBook.Instance.CurrentState = ScreenState.MainMenu;
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
        cDetail.onButtonClick = SetDetailDelete; //cheap trick for now
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
    private void DeleteDetail(ContactDetail detail)
    {
        Debug.Log("Delete");
        switch (detail.Type)
        {
            case DetailType.Phone:
                PhoneNumberType type;
                for (int i = 0; i < contact.phoneNumbers.Count; i++)
                {
                    //Temp solution for the detail
                    if (detail.DetailTypeText.text.Contains("Mobile")) type = PhoneNumberType.Mobile;
                    else if (detail.DetailTypeText.text.Contains("Home")) type = PhoneNumberType.Home;
                    else type = PhoneNumberType.Work;
                    if (contact.phoneNumbers[i].number == detail.DetailValueText.text && contact.phoneNumbers[i].type == type)
                    {
                        contact.phoneNumbers.RemoveAt(i);
                        break;
                    }
                }
                break;
            case DetailType.Email:
                for (int i = 0; i < contact.emails.Count; i++)
                {
                    if (contact.emails[i] == detail.DetailValueText.text)
                    {
                        contact.emails.RemoveAt(i);
                        break;
                    }
                }
                break;
            case DetailType.Link:
                for (int i = 0; i < contact.links.Count; i++)
                {
                    if (contact.links[i] == detail.DetailValueText.text)
                    {
                        contact.links.RemoveAt(i);
                        break;
                    }
                }
                break;
            case DetailType.Description:
                contact.description = "";
                break;
        }
        UpdateScreen();
    }
    private void SetDetailDelete(ContactDetail detail)
    {
        if (deleteButtonDisabled) return;
        deleteButtonDisabled = true;

        cDetail = detail;
        confirmDeleteWindow.UnHideObject();
        EnableDisableButtons(false);
    }
    public void ConfirmDeleteButtonPressed()
    {
        deleteButtonDisabled = false;
        EnableDisableButtons(true);
        DeleteDetail(cDetail);
        cDetail = null;
    }
    public void OnDeleteCancel()
    {
        deleteButtonDisabled = false;
        EnableDisableButtons(true);
    }

    private void SetWarning(string warningMessage, Action warningAction = null)
    {
        warningWindowText.text = warningMessage;
        warningWindow.UnHideObject();
        blocker.SetActive(true);
        warningClosedAction = warningAction;
    }
    public void WarningWindowClosed()
    {
        warningClosedAction?.Invoke();
    }
}
