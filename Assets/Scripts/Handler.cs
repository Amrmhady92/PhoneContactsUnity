using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ScreenState
{
    MainMenu,
    ContactDetailScreen,
    ContactCreationScreen
}

public class Handler : MonoBehaviour
{
    [SerializeField] private Pooler contactDetailPool;
    [SerializeField] private float clickCoolDown = 0.2f;
    [SerializeField] private GameData phoneData;



    
    
    private static Handler instance;
    public static Handler Instance
    {
        get
        {
            return instance;
        }
    }

    public Pooler ContactDetailPool
    {
        get
        {
            return contactDetailPool;
        }

        private set
        {
            contactDetailPool = value;
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
    }

    private void Start()
    {
        PhoneData.Init();
        //PlayerPrefs.DeleteAll();

        //if (PlayerPrefs.HasKey("Saved Once") == false)
        //{
        //    Contact c = new Contact();
        //    c.name = "Amr";
        //    c.phoneNumbers = new List<PhoneNumber>();
        //    c.phoneNumbers.Add(new PhoneNumber() { number = "0700446383", type = PhoneNumberType.Mobile });
        //    c.phoneNumbers.Add(new PhoneNumber() { number = "01145471546", type = PhoneNumberType.Home });
        //    c.emails = new List<string>() { "amrmhady92@gmail.com", "night_raven1992" };
        //    c.links = new List<string>() { "amrmhady92.com", "g.com" };
        //    c.note = "Hi";


        //    Contact c2 = new Contact();
        //    c2.name = "Jack";
        //    c2.phoneNumbers = new List<PhoneNumber>();
        //    c2.phoneNumbers.Add(new PhoneNumber() { number = "070042223", type = PhoneNumberType.Mobile });
        //    c2.phoneNumbers.Add(new PhoneNumber() { number = "0011445566", type = PhoneNumberType.Home });
        //    c2.emails = new List<string>() { "jack@gmail.com", "jcc" };
        //    c2.links = new List<string>() { "jack,linkedin.com" };
        //    c2.note = "Jack is here";


        //    phoneData.AddContact(c);
        //    phoneData.AddContact(c2);
        //    PlayerPrefs.SetFloat("Saved Once", 0);
        //    PlayerPrefs.Save();
        //}
        
        List<Contact> allcontacts = phoneData.GetAllContacts();
        if (allcontacts != null)
        {
            Debug.Log(allcontacts.Count);
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

    }

    private void ClickCoolDown()
    {
        canClick = false;
        StartCoroutine(WaitThenDo(clickCoolDown, () => { canClick = true; }));
    }

    IEnumerator WaitThenDo(float waitTime, System.Action callback)
    {
        yield return new WaitForSeconds(waitTime);
        callback?.Invoke();
    }
}