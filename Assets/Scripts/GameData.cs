//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using System.Runtime.Serialization.Formatters.Binary;
//using UnityEngine;

//[CreateAssetMenu(menuName = "SOs/GameData")]
//public class GameData : ScriptableObject
//{

//    [SerializeField] private List<Contact> contacts;


//    private List<Contact> tempContacts = new List<Contact>();
//    private List<string> tempEmailsAndLinks = new List<string>();
//    private int searchedStringLength;
//    private string searchedString;
//    private bool found = false;
//    private int index;
//    private Contact tempContact;

//    //SaveData() & LoadData() Code used from , edited to fit the project
//    //https://www.sitepoint.com/saving-and-loading-player-game-data-in-unity/

//    //Must be called
//    public void Init()
//    {
//        LoadData();
//    }
//    private void SaveContact(Contact contact)
//    {
//        if (contacts == null || contacts.Count == 0) return;

//        int index = contacts.Count;


//        if (!Directory.Exists(Application.persistentDataPath + "/Saves")) 
//            Directory.CreateDirectory(Application.persistentDataPath + "/Saves");

//        BinaryFormatter formatter = new BinaryFormatter();
//        string filename = contact.GetHashCode().ToString();
//        FileStream saveFile = File.Create(Application.persistentDataPath + "/Saves/" + filename + ".cntct");
//        formatter.Serialize(saveFile, contact);
//        saveFile.Close();
//    }
//    private void RemoveSaveFile(Contact contactToDelete)
//    {
//        if (!Directory.Exists(Application.persistentDataPath + "/Saves"))
//        {
//            contacts = new List<Contact>();
//            return;
//        }

//        string[] allsaves = System.IO.Directory.GetFiles(Application.persistentDataPath + "/Saves", "*.cntct"); // my own extentions
//        if (allsaves.Length > 0)
//        {
//            FileStream saveFile;
//            BinaryFormatter formatter;
//            Contact contact;
//            for (int i = 0; i < allsaves.Length; i++)
//            {
//                formatter = new BinaryFormatter();
//                saveFile = File.Open(allsaves[i], FileMode.Open);
//                contact = (Contact)formatter.Deserialize(saveFile);
//                saveFile.Close();


//                if (contact != null)
//                {
//                    if(contact.CompareContact(contactToDelete))
//                    {
//                        File.Delete(allsaves[i]);
//                        Debug.Log("Found and Deleted " + contactToDelete.name);
//                        return;
//                    }
//                }
//            }
//        }

//        Debug.Log("Couldnt Find Contact, Nothing Deleted");
//    }
//    //Only needed to load at start, all contacts are stored under contacts list
//    public void LoadData()
//    {
//        if (!Directory.Exists(Application.persistentDataPath + "/Saves"))
//        {
//            Debug.Log("Nothing to load");
//            contacts = new List<Contact>();
//            return;
//        }

//        if (contacts == null) contacts = new List<Contact>();
//        else contacts.Clear();
        
//        string[] allsaves = System.IO.Directory.GetFiles((Application.persistentDataPath + "/Saves"), "*.cntct"); // my own extentions
        
//        if (allsaves.Length > 0)
//        {
//            FileStream saveFile;
//            BinaryFormatter formatter;
//            Contact contact;
//            for (int i = 0; i < allsaves.Length; i++)
//            {
//                formatter = new BinaryFormatter();
//                saveFile = File.Open(allsaves[i], FileMode.Open);
//                contact = (Contact)formatter.Deserialize(saveFile);
//                saveFile.Close();
//                if(contact != null)
//                {
//                    contacts.Add(contact);
//                }
//            }
//        }


//    }

//    public string AddContact(Contact contact)
//    {
//        if (contact == null) return "Contact is Null";
//        if (FindExactContact(contact) != null)
//        {
//            Debug.LogError("Contact already exist");
//            return "Contact already exist";
//        }
//        else
//        {
//            contacts.Add(contact);
//            SaveContact(contact);
//            return "Contact added";
//        }
//    }
//    public void EditContact(Contact editedContact)
//    {
//        RemoveContact(editedContact);
//        AddContact(editedContact);
//    }
//    public bool RemoveContact(Contact contact)
//    {
//        tempContact = FindExactContact(contact);
//        if (tempContact != null)
//        {
//            if (contacts.Remove(tempContact))// remove from list
//            {
//                RemoveSaveFile(contact); //Delete the save file
//                return true;
//            }
//        }
//        return false;
//    }
//    public Contact FindExactContact(Contact contact)
//    {
//        return contacts.Find(c => c.CompareContact(contact) == true);
//    }
//    public List<Contact> GetAllContacts()
//    {
//        return contacts;
//    }
//    public List<Contact> Contacts
//    {
//        get
//        {
//            return contacts;
//        }

//        private set
//        {
//            contacts = value;
//        }
//    }
//    public List<Contact> GetContactsByText(string enteredText)
//    {
//        tempContacts.Clear();
//        tempEmailsAndLinks.Clear();
//        enteredText = enteredText.ToLower();
//        searchedStringLength = enteredText.Length;

//        for (int i = 0; i < contacts.Count; i++)
//        {
//            tempContact = contacts[i];

//            if (tempContact == null) continue;

//            found = false;

//            //First Name
//            if(tempContact.name != "")
//            {
//                if (tempContact.name.Length > searchedStringLength)
//                {
//                    index = tempContact.name.ToLower().IndexOf(enteredText);
//                }
//                else
//                {
//                    index = enteredText.IndexOf(tempContact.name.ToLower());
//                }

//                //If name matches
//                if (index >= 0)
//                {
//                    tempContacts.Add(tempContact);
//                    continue;
//                }
//            }
            


//            //Last Name
//            if(tempContact.lastname != "")
//            {
//                if (tempContact.lastname.Length > searchedStringLength)
//                {
//                    index = tempContact.lastname.ToLower().IndexOf(enteredText);
//                }
//                else
//                {
//                    index = enteredText.IndexOf(tempContact.lastname.ToLower());
//                }
//                //If name matches
//                if (index >= 0)
//                {
//                    tempContacts.Add(tempContact);
//                    continue;
//                }
//            }

//            //else Check Notes (less work than next items)
//            if(tempContact.description != "")
//            {
//                if (tempContact.description.Length > searchedStringLength)
//                {
//                    index = tempContact.description.ToLower().IndexOf(enteredText);
//                }
//                else
//                {
//                    index = enteredText.IndexOf(tempContact.description.ToLower());
//                }
//                if (index >= 0)
//                {
//                    tempContacts.Add(tempContact);
//                    continue;
//                }
//            }
            

//            //else check Number 
//            for (int j = 0; j < tempContact.phoneNumbers.Count; j++)
//            {
//                searchedString = tempContact.phoneNumbers[j].number.ToLower();
//                if (searchedString.Length > searchedStringLength)
//                {
//                    index = searchedString.IndexOf(enteredText);
//                }
//                else
//                {
//                    index = enteredText.IndexOf(searchedString);
//                }

//                if (index >= 0)
//                {
//                    tempContacts.Add(tempContact);
//                    found = true;
//                    break;
//                }
//            }

//            //For the break from prev forloop
//            if (found)
//            {
//                found = false;
//                continue;
//            }

//            //Else check emails and links
//            tempEmailsAndLinks.Clear();
//            tempEmailsAndLinks.AddRange(tempContact.emails);
//            tempEmailsAndLinks.AddRange(tempContact.links);

//            for (int j = 0; j < tempEmailsAndLinks.Count; j++)
//            {
//                searchedString = tempEmailsAndLinks[j].ToLower();
//                if (searchedString.Length > searchedStringLength)
//                {
//                    index = searchedString.IndexOf(enteredText);
//                }
//                else
//                {
//                    index = enteredText.IndexOf(searchedString);
//                }

//                if (index >= 0)
//                {
//                    tempContacts.Add(tempContact);
//                    break;
//                }
//            }

//        }

//        return tempContacts;


//    }


//}

