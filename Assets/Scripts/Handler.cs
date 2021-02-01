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
    [SerializeField] private ContactScreen contactScreen;


    [SerializeField] private Canvas gameCanvas;

    //Searching
    private bool searching = false;
    [SerializeField] private RectTransform searchBar;
    private SortMethod currentSortMethod = SortMethod.DateD;
    //
    //
    [SerializeField] private TextMeshProUGUI sortingButtonText;

    private List<Contact> contacts;
    private List<GameObject> activeContactHolders;

    private CanvasScaler gameCanvasScaler;

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

    private bool canClick = true;
    private void Awake()
    {
        if (instance == null) instance = this;

        contacts = new List<Contact>();
        gameCanvasScaler = gameCanvas.GetComponent<CanvasScaler>();
        PhoneData.Init();
    }

    private void Start()
    {
        //PlayerPrefs.DeleteAll();
        //if (testerbool)
        //{
        //    string[] names = new string[] { "Amr", "Kareem", "Hossam", "Nada", "Engy", "Mohamed", "Hala" };
        //    string[] namesL = new string[] { "Saleh", "Mohamed", "Abdelhady", "Mohamed", "Saleh", "Radwan", "ElSafy" };
        //    for (int i = 0; i < names.Length; i++)
        //    {
        //        Contact c = new Contact();
        //        c.name = names[i];
        //        c.lastname = namesL[i];
        //        c.phoneNumbers = new List<PhoneNumber>();
        //        c.phoneNumbers.Add(new PhoneNumber() { number = Random.Range(10000000, 99999999).ToString(), type = PhoneNumberType.Mobile });
        //        c.emails = new List<string>() { Random.Range(-1000, 1000).ToString() };
        //        c.links = new List<string>() { Random.Range(-1000, 1000).ToString() };
        //        c.description = Random.Range(-1000, 1000).ToString();
        //        c.dateAdded = new System.DateTime(Random.Range(2000,2020), Random.Range(1, 12), Random.Range(1, 30));
        //        Debug.Log(c.dateAdded);
        //        phoneData.AddContact(c);

        //    }
        //}





        LoadContacts();
        //UpdateContactsList();
        OnSortButtonPressed();
    }



    public void SortContacts(SortMethod sortMethod)
    {

        if(contacts != null && contacts.Count > 0)
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
    private void LoadContacts()
    {
        contacts = new List<Contact>(phoneData.GetAllContacts());
    }

    public void OnContactClicked(Contact contact)
    {
        if (canClick == false) return;
        ClickCoolDown();
        CloseAllScreens();
        contactScreen.gameObject.SetActive(true);
        if (contactScreen.DisplayContact(contact) == false)
        {
            CloseAllScreens();
            createContactScreenGO.SetActive(true);
        }

        Debug.Log("ContactClicked");
    }

    private void CloseAllScreens()
    {
        contactsViewScreenGO.SetActive(false);
        contactScreen.gameObject.SetActive(false);
        createContactScreenGO.SetActive(false);
        if (searching) OnSearchButtonPressed();
    }

    private void ClickCoolDown()
    {
        canClick = false;
        StartCoroutine(WaitThenDo(clickCoolDown, () => { canClick = true; }));
    }

    public void OnSearchButtonPressed()
    {
        if (searching)
        {
            searching = false;
            searchBar.LeanCancel();
            Vector2 offSize = new Vector2(0, searchBar.rect.height);
            searchBar.LeanSize(offSize, 0.2f);
            LoadContacts();
            UpdateContactsList();
        }
        else
        {
            searching = true;
            searchBar.LeanCancel();
            Debug.Log(/*Screen.currentResolution.height*/gameCanvasScaler.referenceResolution.x);
            
            Vector2 offSize = new Vector2(gameCanvasScaler.referenceResolution.x * 0.7f, searchBar.rect.height); // 70% of screen width.
            searchBar.LeanSize(offSize, 0.2f);
        }
    }

    public void OnSearchBarValueChanged(string value)
    {
        if (!searching) return;

        if(value == "")
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

    private void UpdateContactsList()
    {
        if (contacts == null) return;
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
    IEnumerator WaitThenDo(float waitTime, System.Action callback)
    {
        yield return new WaitForSeconds(waitTime);
        callback?.Invoke();
    }
}
