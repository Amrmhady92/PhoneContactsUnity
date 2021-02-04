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

public class Handler : MonoBehaviour
{

    public bool testerbool = true;
    [SerializeField] private Pooler contactHolderPool;
    [SerializeField] private float clickCoolDown = 0.2f;
    [SerializeField] private GameData phoneData;

    [SerializeField] private TextMeshProUGUI noContactsText;

    [SerializeField] private GameObject contactsViewScreenGO;
    [SerializeField] private GameObject createContactScreenGO;
    [SerializeField] private GameObject mainMenuUpperPanelGO;
    [SerializeField] private UIMover upperPanelMover;

    [SerializeField] private UIMover[] mainMenuMovers;
    [SerializeField] private UIMover[] contactViewScreenMovers;
    [SerializeField] private UIMover[] contactCreationScreenMovers;

    [SerializeField] private ContactScreen contactScreen;
    [SerializeField] private CreateContactScreen createContactScreen;



    private ScreenState currentState = ScreenState.MainMenu;

    //Searching
    private bool searching = false;
    [SerializeField] private RectTransform searchBar;
    private SortMethod currentSortMethod = SortMethod.DateD;
    //
    //
    [SerializeField] private TextMeshProUGUI sortingButtonText;

    private List<Contact> contacts;
    private List<GameObject> activeContactHolders;

    private static Handler instance;
    public static Handler Instance
    {
        get
        {
            return instance;
        }
    }


    public GameData PhoneData
    {
        get
        {
            return phoneData;
        }

        private set
        {
            phoneData = value;
        }
    }

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
                    HideMovers(contactViewScreenMovers);
                    HideMovers(contactCreationScreenMovers);
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
                    HideMovers(contactCreationScreenMovers);
                    StartCoroutine(WaitThenDo(0.5f, () =>
                    {
                        UnHideMovers(contactViewScreenMovers);
                    }));
                    break;
                case ScreenState.ContactCreationScreen:
                    HideMovers(contactViewScreenMovers);
                    HideMovers(mainMenuMovers);
                    StartCoroutine(WaitThenDo(0.5f, () =>
                    {
                        UnHideMovers(contactCreationScreenMovers);
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

    private bool canClick = true;


    #region MonoBehavior Methods
    ///Methods////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        if (instance == null) instance = this;

        contacts = new List<Contact>();
    }

    private void Start()
    {
        PhoneData.Init();

        //CurrentState = ScreenState.MainMenu;
        //for (int i = 0; i < contactViewScreenMovers.Length; i++)
        //{
        //    contactViewScreenMovers[i].gameObject.SetActive(false);
        //}
        //for (int i = 0; i < contactCreationScreenMovers.Length; i++)
        //{
        //    contactCreationScreenMovers[i].gameObject.SetActive(false);
        //}
        //HideMovers(contactViewScreenMovers, 0);
        //HideMovers(contactCreationScreenMovers, 0);


        //LoadContacts();

        //PlayerPrefs.DeleteAll();
        //if (PlayerPrefs.HasKey("FirstAdd1") == false) 
        //{
        //List<Contact> deleteContacts = new List<Contact>(contacts);

        //for (int i = 0; i < deleteContacts.Count; i++)
        //{
        //    PhoneData.RemoveContact(deleteContacts[i]);
        //}
        //string[] names = new string[] { "Adam", "Kareem", "Hossam", "Nada", "Engy", "Mohamed", "Hala" };
        //string[] namesL = new string[] { "Saleh", "Mohamed", "Abdelhady", "Mohamed", "Saleh", "Radwan", "ElSafy" };
        //Contact c;
        //for (int i = 0; i < names.Length; i++)
        //{
        //    c = new Contact();
        //    c.name = names[i];
        //    c.lastname = namesL[i];
        //    c.phoneNumbers = new List<PhoneNumber>();
        //    c.phoneNumbers.Add(new PhoneNumber() { number = Random.Range(10000000, 99999999).ToString(), type = PhoneNumberType.Mobile });
        //    c.emails = new List<string>() { Random.Range(-1000, 1000).ToString() };
        //    c.links = new List<string>() { Random.Range(-1000, 1000).ToString() };
        //    c.description = Random.Range(-1000, 1000).ToString();
        //    c.dateAdded = new System.DateTime(Random.Range(2000, 2020), Random.Range(1, 12), Random.Range(1, 30));
        //    //Debug.Log(c.dateAdded);
        //    phoneData.AddContact(c);

        //}

        //c = new Contact();
        //c.name = "Amr";
        //c.lastname = "Bond";
        //c.phoneNumbers = new List<PhoneNumber>();
        //c.phoneNumbers.Add(new PhoneNumber() { number = "0700446383", type = PhoneNumberType.Mobile });
        //c.phoneNumbers.Add(new PhoneNumber() { number = "01145471546", type = PhoneNumberType.Mobile });
        //c.phoneNumbers.Add(new PhoneNumber() { number = "7227703", type = PhoneNumberType.Home });
        //c.phoneNumbers.Add(new PhoneNumber() { number = "7227694", type = PhoneNumberType.Home });
        //c.phoneNumbers.Add(new PhoneNumber() { number = "8855222", type = PhoneNumberType.Work });
        //c.emails = new List<string>() { Random.Range(-1000, 1000).ToString(), "Amrmhady92@gmail.com" };
        //c.links = new List<string>() { Random.Range(-1000, 1000).ToString(), "L2", "LINKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKK" };
        //c.description = "This contact is used to test the contacts detail screen, bond james bond ";
        //c.dateAdded = new System.DateTime(Random.Range(2000, 2020), Random.Range(1, 12), Random.Range(1, 30));
        //Debug.Log(c.dateAdded);
        //phoneData.AddContact(c);

        //    PlayerPrefs.SetInt("First", 0);
        //}

        LoadContacts();
        OnSortButtonPressed();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && CurrentState == ScreenState.ContactDetailScreen)
        {
            CurrentState = ScreenState.MainMenu;
        }
        //if (Input.GetKeyDown(KeyCode.D))
        //{
        //    PlayerPrefs.DeleteAll();
        //}
    }
    #endregion
    #region Contacts Methods
    public void SortContacts(SortMethod sortMethod)
    {

        if (contacts != null && contacts.Count > 0)
        {
            switch (sortMethod)
            {
                case SortMethod.AlphabeticallyA:
                    contacts.Sort((a, b) => a.name.CompareTo(b.name));
                    break;
                case SortMethod.AlphabeticallyD:
                    contacts.Sort((a, b) => b.name.CompareTo(a.name));
                    break;
                case SortMethod.DateA:
                    contacts.Sort((a, b) => a.dateAdded.CompareTo(b.dateAdded));
                    break;
                case SortMethod.DateD:
                    contacts.Sort((a, b) => b.dateAdded.CompareTo(a.dateAdded));
                    break;
            }
        }

        UpdateContactsList();
    }
    private void LoadContacts()
    {
        contacts = new List<Contact>(phoneData.GetAllContacts());
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
        if (contacts == null) return;

        noContactsText.text = contacts.Count == 0 ? "Phone Book Empty" : "";

        DeactivateHolders();
        Contact contact;
        ContactHolder holder;
        GameObject holderObject;

        for (int i = 0; i < contacts.Count; i++)
        {
            contact = contacts[i];
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
    //private void CloseAllScreens()
    //{
    //    contactsViewScreenGO.SetActive(false);
    //    contactScreen.gameObject.SetActive(false);
    //    createContactScreenGO.SetActive(false);
    //    // mainMenuUpperPanelGO.SetActive(false);
    //    if (searching) OnSearchButtonPressed();
    //}

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
                sortingButtonText.text = "Alph a";
                break;
            case SortMethod.AlphabeticallyD:
                sortingButtonText.text = "Alph D";
                break;
            case SortMethod.DateA:
                sortingButtonText.text = "Date a";
                break;
            case SortMethod.DateD:
                sortingButtonText.text = "Date D";
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
        //createContactScreen.


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
            //show normal contacts
            LoadContacts();
            // SortContacts(currentSortMethod); // this already Updates List
        }
        else
        {
            //can use this list.. (is actually tempContacts in phoneData
            contacts = PhoneData.GetContactsByText(value);
            // UpdateContactsList();
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
