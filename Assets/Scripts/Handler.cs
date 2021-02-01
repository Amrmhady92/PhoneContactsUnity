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
    //

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
    }

    private void Start()
    {
        //PhoneData.Init();
        //PlayerPrefs.DeleteAll();
        if (testerbool)
        {
            for (int i = 0; i < 8; i++)
            {
                Contact c = new Contact();
                c.name = Random.Range(-1000, 1000).ToString();
                c.phoneNumbers = new List<PhoneNumber>();
                c.phoneNumbers.Add(new PhoneNumber() { number = "Random.Range(-1000,1000).ToString()", type = PhoneNumberType.Mobile });
                c.phoneNumbers.Add(new PhoneNumber() { number = "Random.Range(-1000,1000).ToString()", type = PhoneNumberType.Home });
                c.emails = new List<string>() { Random.Range(-1000, 1000).ToString() };
                c.links = new List<string>() { Random.Range(-1000, 1000).ToString() };
                c.note = Random.Range(-1000, 1000).ToString();

                phoneData.AddContact(c);

            }

        }




        LoadContacts();
        UpdateContactsList();

    }


    private void LoadContacts()
    {
        contacts = phoneData.GetAllContacts();
        if (contacts != null)
        {
            Debug.Log(contacts.Count);
        }
        else
        {
            Debug.LogError("No Contacts");
        }
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
            contacts = phoneData.GetAllContacts();
        }
        else
        {
            contacts = PhoneData.GetContactsByText(value);
        }

        UpdateContactsList();
    }

    private void UpdateContactsList()
    {
        if (contacts == null) return;
        DeactivateHolders();
        Contact contact;
        ContactHolder holder;
        GameObject holderObject;
        Debug.Log("contacts count "+ contacts.Count);


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
            Debug.Log(holder.name);
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
