using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum ScreenState
{
    MainMenu,
    ContactDetailScreen,
    ContactCreationScreen
}

public enum SortMethod
{
    AlphabeticallyA,
    AlphabeticallyD,
    DateA,
    DateD,
    Count
}

public class PhoneBook : MonoBehaviour
{

    [SerializeField] private Pooler contactHolderPool;
    [SerializeField] private float clickCoolDown = 0.2f;


    [SerializeField] private TextMeshProUGUI noContactsText;

    [SerializeField] private GameObject mainMenuUpperPanelGO;
    [SerializeField] private UIMover upperPanelMover;

    [SerializeField] private UIMover[] mainMenuMovers;

    private ContactScreen contactScreen;
    private CreateContactScreen createContactScreen;

    [SerializeField] private GameObject contactScreenPrefab;
    [SerializeField] private GameObject createContactScreenPrefab;

    //private ContactManager manager;
    private Contact contact;
    private ContactHolder holder;
    private GameObject holderObject;
    private bool canClick = true;




    private ScreenState currentState = ScreenState.MainMenu;

    //Searching
    private bool searching = false;
    [SerializeField] private RectTransform searchBar;
    private SortMethod currentSortMethod = SortMethod.DateD;
    //
    //
    [SerializeField] private TextMeshProUGUI sortingButtonText;
    [SerializeField] private Image sortingButtonImage;
    [SerializeField] private Sprite sortingAlphaAscIcon;
    [SerializeField] private Sprite sortingAlphaDscIcon;
    [SerializeField] private Sprite sortingDateAscIcon;
    [SerializeField] private Sprite sortingDateDscIcon;


    private List<Contact> contacts; // a secondary list // next step dump this list and just use the Manager's list
    private List<GameObject> activeContactHolders;

    private static PhoneBook instance;
    public static PhoneBook Instance
    {
        get
        {
            return instance;
        }
    }


    //public GameData PhoneData
    //{
    //    get
    //    {
    //        return phoneData;
    //    }

    //    private set
    //    {
    //        phoneData = value;
    //    }
    //}
    #region Properties
    public ScreenState CurrentState
    {
        get
        {
            return currentState;
        }

        set
        {
            currentState = value;
            switch (currentState)
            {
                case ScreenState.MainMenu:
                    HideMovers(contactScreen.ContactViewScreenMovers);
                    HideMovers(createContactScreen.ContactCreationScreenMovers);
                    //HideMovers(contactViewScreenMovers);
                   // HideMovers(contactCreationScreenMovers);
                    StartCoroutine(WaitThenDo(0.5f, () =>
                    {
                        UnHideMovers(mainMenuMovers);
                    }
                    ));
                    contactScreen.OnDeleteCancel();
                    LoadContacts();
                    SortContacts(currentSortMethod);
                    break;
                case ScreenState.ContactDetailScreen:

                    HideMovers(mainMenuMovers);
                    HideMovers(createContactScreen.ContactCreationScreenMovers);

                    //HideMovers(contactCreationScreenMovers);
                    StartCoroutine(WaitThenDo(0.5f, () =>
                    {
                        //UnHideMovers(contactViewScreenMovers);
                        UnHideMovers(contactScreen.ContactViewScreenMovers);
                    }));
                    break;
                case ScreenState.ContactCreationScreen:
                    //HideMovers(contactViewScreenMovers);
                    HideMovers(contactScreen.ContactViewScreenMovers);
                    HideMovers(mainMenuMovers);
                    StartCoroutine(WaitThenDo(0.5f, () =>
                    {
                        //UnHideMovers(contactCreationScreenMovers);
                        UnHideMovers(createContactScreen.ContactCreationScreenMovers);
                    }));
                    break;
                default:
                    break;
            }
        }
    }

    public CreateContactScreen CreateContactScreenComponent
    {
        get
        {
            return createContactScreen;
        }

        private set
        {
            createContactScreen = value;
        }
    }

    public List<Contact> Contacts
    {
        get
        {
            return contacts;
        }

        private set
        {
            contacts = value;
        }
    }



    #endregion

    #region MonoBehavior Methods
    ///Methods////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        if (instance == null) instance = this;

        //contacts = new List<Contact>();
        // manager = new ContactManager();
        //ContactManager.Init();
    }

    private void Start()
    {
        RectTransform rectT = this.GetComponentInChildren<RectTransform>();
        //PhoneData.Init();
        contactScreen = GameObject.Instantiate(contactScreenPrefab, rectT).GetComponent<ContactScreen>();
        CreateContactScreenComponent = GameObject.Instantiate(createContactScreenPrefab, rectT).GetComponent<CreateContactScreen>();
        

        LoadContacts();
        OnSortButtonPressed();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && CurrentState == ScreenState.ContactDetailScreen)
        {
            CurrentState = ScreenState.MainMenu;
        }
    }
    #endregion
    #region Contacts Methods
    public void SortContacts(SortMethod sortMethod)
    {

        if (Contacts != null && Contacts.Count > 0)
        {
            switch (sortMethod)
            {
                case SortMethod.AlphabeticallyA:
                    Contacts.Sort((a, b) => a.name.CompareTo(b.name));
                    break;
                case SortMethod.AlphabeticallyD:
                    Contacts.Sort((a, b) => b.name.CompareTo(a.name));
                    break;
                case SortMethod.DateA:
                    Contacts.Sort((a, b) => a.dateAdded.CompareTo(b.dateAdded));
                    break;
                case SortMethod.DateD:
                    Contacts.Sort((a, b) => b.dateAdded.CompareTo(a.dateAdded));
                    break;
            }
        }

        UpdateContactsList();
    }
    private void LoadContacts()
    {
        Contacts = new List<Contact>(ContactManager.Contacts);// replicating the list so we wont dmg the already build list
    }
    #endregion

    #region UI Methods;
    public bool ClickCoolDown()
    {
        if (canClick)
        {
            canClick = false;
            StartCoroutine(WaitThenDo(clickCoolDown, () => { canClick = true; }));
            return true;
        }
        else
        {
            return canClick;
        }
    }
    private void UpdateContactsList()
    {
        if (Contacts == null) return;

        noContactsText.text = Contacts.Count == 0 ? "Phone Book Empty" : "";
        DeactivateHolders();

        for (int i = 0; i < Contacts.Count; i++)
        {
            contact = Contacts[i];
            if (contact == null) continue;

            //Safety Checks
            holderObject = contactHolderPool.Get();
            if (holderObject == null)
            {
                Debug.LogError("Error getting pooled object");
                return;
            }
            holder = holderObject.GetComponent<ContactHolder>();
            if (holder == null)
            {
                Debug.LogError("Error getting pooled object component");
                return;
            }
            //
            //Debug.Log(holder.name);
            holder.SetConact(contact);
            holderObject.SetActive(true);
            activeContactHolders.Add(holderObject);
        }
    }
    private void DeactivateHolders()
    {
        if (activeContactHolders == null)
        {
            activeContactHolders = new List<GameObject>();
            return;
        }
        for (int i = 0; i < activeContactHolders.Count; i++)
        {
            activeContactHolders[i].SetActive(false);
        }
        activeContactHolders.Clear();
    }

    private void HideMovers(UIMover[] movers, float speed = -1)
    {
        for (int i = 0; i < movers.Length; i++)
        {
            movers[i].HideObject(speed);
        }
    }
    private void UnHideMovers(UIMover[] movers, float speed = -1)
    {
        for (int i = 0; i < movers.Length; i++)
        {
            movers[i].UnHideObject(speed);
        }
    }

    #endregion

    #region UI Events

    public void OnSortButtonPressed()
    {
        int sortIndex = (int)currentSortMethod;
        sortIndex++;
        if (sortIndex == (int)SortMethod.Count) sortIndex = 0;
        currentSortMethod = (SortMethod)sortIndex;
        switch (currentSortMethod)
        {
            case SortMethod.AlphabeticallyA:
                //sortingButtonText.text = "Alph a";
                sortingButtonImage.sprite = sortingAlphaAscIcon;
                break;
            case SortMethod.AlphabeticallyD:
                //sortingButtonText.text = "Alph D";
                sortingButtonImage.sprite = sortingAlphaDscIcon;
                break;
            case SortMethod.DateA:
                //sortingButtonText.text = "Date a";
                sortingButtonImage.sprite = sortingDateAscIcon;
                break;
            case SortMethod.DateD:
                //sortingButtonText.text = "Date D";
                sortingButtonImage.sprite = sortingDateDscIcon;
                break;
        }
        SortContacts(currentSortMethod);
    }
    public void OnContactClicked(Contact contact)
    {
        if (ClickCoolDown() == false) return;
        if (searching) OnSearchButtonPressed();

        CurrentState = ScreenState.ContactDetailScreen;
        //CloseAllScreens();
       // contactScreen.gameObject.SetActive(true);
        if (contactScreen.DisplayContact(contact) == false)
        {
            CurrentState = ScreenState.MainMenu;
            //CloseAllScreens();
           // createContactScreenGO.SetActive(true);
        }
    }

    public void OnCreateContactClicked()
    {
        if (ClickCoolDown() == false) return;
        if (searching) OnSearchButtonPressed();
        CurrentState = ScreenState.ContactCreationScreen;
        CreateContactScreenComponent.CreateContactWindow();
    }

    public void OnSearchButtonPressed()
    {
        if (ClickCoolDown() == false) return;

        if (searching)
        {
            searching = false;
            searchBar.LeanCancel();
            //Vector2 offSize = new Vector2(0, searchBar.sizeDelta.y);
            //searchBar.LeanSize(offSize, 0.2f);
            searchBar.LeanScaleX(0, 0.2f);

            LoadContacts();
            UpdateContactsList();
        }
        else
        {
            searching = true;
            searchBar.LeanCancel();
            //Vector2 offSize = new Vector2(gameCanvasScaler.referenceResolution.x * 0.7f, searchBar.sizeDelta.y); // 70% of screen width.
            //searchBar.LeanSize(offSize, 0.2f);
            searchBar.LeanScaleX(1, 0.2f);
        }
    }
    public void OnSearchBarValueChanged(string value)
    {
        if (!searching) return;

        if (value == "")
        {
            LoadContacts();
        }
        else
        {
            //can use this list.. (is actually tempContacts in phoneData
            Contacts = ContactManager.GetContactsByText(value);
        }

        SortContacts(currentSortMethod); // Sorts and updates
    }

    #endregion






    IEnumerator WaitThenDo(float waitTime, System.Action callback)
    {
        yield return new WaitForSeconds(waitTime);
        callback?.Invoke();
    }
}
