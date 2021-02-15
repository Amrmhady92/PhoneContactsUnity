using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
public static class ContactManager
{
    [SerializeField] private static List<Contact> contacts;

    private static List<Contact> tempContacts = new List<Contact>();
    private static List<string> tempEmailsAndLinks = new List<string>();
    private static Contact tempContact;

    private static int searchedStringLength;
    private static string searchedString;
    private static bool found = false;
    private static int index;

    private static bool initialized = false;

    public static List<Contact> Contacts
    {
        get
        {
            if (!initialized)
            {
                Init();
            }
            return contacts;
        }

        private set
        {
            contacts = value;
        }
    }

    //public static ContactManager()
    //{
    //    Init();
    //}

    //SaveData() & LoadData() Code used from for Serialization and BinaryFormat, edited to fit the project
    //https://www.sitepoint.com/saving-and-loading-player-game-data-in-unity/

    //Must be called
    public static void Init()
    {
        initialized = true; // change of order of this line will cause a stackoverflow so should fix this later...
        LoadData();
    }
    private static void SaveContact(Contact contact)
    {
        if (Contacts == null || Contacts.Count == 0) return;

        int index = Contacts.Count;


        if (!Directory.Exists(Application.persistentDataPath + "/Saves"))
            Directory.CreateDirectory(Application.persistentDataPath + "/Saves");

        BinaryFormatter formatter = new BinaryFormatter();
        string filename = contact.GetHashCode().ToString();
        FileStream saveFile = File.Create(Application.persistentDataPath + "/Saves/" + filename + ".cntct");
        formatter.Serialize(saveFile, contact);
        saveFile.Close();
    }
    private static void RemoveSaveFile(Contact contactToDelete)
    {
        if (!Directory.Exists(Application.persistentDataPath + "/Saves"))
        {
            Contacts = new List<Contact>();
            return;
        }

        string[] allsaves = System.IO.Directory.GetFiles(Application.persistentDataPath + "/Saves", "*.cntct"); // my own extentions
        if (allsaves.Length > 0)
        {
            FileStream saveFile;
            BinaryFormatter formatter;
            Contact contact;
            for (int i = 0; i < allsaves.Length; i++)
            {
                formatter = new BinaryFormatter();
                saveFile = File.Open(allsaves[i], FileMode.Open);
                contact = (Contact)formatter.Deserialize(saveFile);
                saveFile.Close();


                if (contact != null)
                {
                    if (contact.CompareContact(contactToDelete))
                    {
                        File.Delete(allsaves[i]);
                        Debug.Log("Found and Deleted " + contactToDelete.name);
                        return;
                    }
                }
            }
        }

        Debug.Log("Couldnt Find Contact, Nothing Deleted");
    }
    //Only needed to load at start, all contacts are stored under contacts list
    public static void LoadData()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/Saves"))
        {
            Debug.Log("Nothing to load");
            Contacts = new List<Contact>();
            return;
        }

        if (Contacts == null) Contacts = new List<Contact>();
        else Contacts.Clear();

        string[] allsaves = System.IO.Directory.GetFiles((Application.persistentDataPath + "/Saves"), "*.cntct"); // my own extentions

        if (allsaves.Length > 0)
        {
            FileStream saveFile;
            BinaryFormatter formatter;
            Contact contact;
            for (int i = 0; i < allsaves.Length; i++)
            {
                formatter = new BinaryFormatter();
                saveFile = File.Open(allsaves[i], FileMode.Open);
                contact = (Contact)formatter.Deserialize(saveFile);
                saveFile.Close();
                if (contact != null)
                {
                    Contacts.Add(contact);
                }
            }
        }


    }
    public static string AddContact(Contact contact)
    {
        if (contact == null) return "Contact is Null";
        if (FindExactContact(contact) != null)
        {
            Debug.LogError("Contact already exist");
            return "Contact already exist";
        }
        else
        {
            Contacts.Add(contact);
            SaveContact(contact);
            return "Contact added";
        }
    }
    public static void EditContact(Contact editedContact)
    {
        //editing a contact is just removing the old contact then adding it again after it was edited.
        //instead of writing a whole code of opening the file and writing it again.
        RemoveContact(editedContact); 
        AddContact(editedContact);
    }
    public static bool RemoveContact(Contact contact)
    {
        tempContact = FindExactContact(contact);
        if (tempContact != null)
        {
            if (Contacts.Remove(tempContact))// remove from list. returns true if removed successfuly 
            {
                RemoveSaveFile(contact); //Delete the save file
                return true;
            }
        }
        return false;
    }
    public static Contact FindExactContact(Contact contact)
    {
        return Contacts.Find(c => c.CompareContact(contact) == true);
    }
    public static List<Contact> GetContactsByText(string enteredText)
    {
        tempContacts.Clear();
        tempEmailsAndLinks.Clear();
        enteredText = enteredText.ToLower();
        searchedStringLength = enteredText.Length;

        for (int i = 0; i < Contacts.Count; i++)
        {
            tempContact = Contacts[i];

            if (tempContact == null) continue;

            found = false;

            //First Name
            if (tempContact.name != "")
            {
                if (tempContact.name.Length > searchedStringLength)
                {
                    index = tempContact.name.ToLower().IndexOf(enteredText);
                }
                else
                {
                    index = enteredText.IndexOf(tempContact.name.ToLower());
                }

                //If name matches
                if (index >= 0)
                {
                    tempContacts.Add(tempContact);
                    continue;
                }
            }



            //Last Name
            if (tempContact.lastname != "")
            {
                if (tempContact.lastname.Length > searchedStringLength)
                {
                    index = tempContact.lastname.ToLower().IndexOf(enteredText);
                }
                else
                {
                    index = enteredText.IndexOf(tempContact.lastname.ToLower());
                }
                //If name matches
                if (index >= 0)
                {
                    tempContacts.Add(tempContact);
                    continue;
                }
            }

            //else Check Notes (less work than next items)
            if (tempContact.description != "")
            {
                if (tempContact.description.Length > searchedStringLength)
                {
                    index = tempContact.description.ToLower().IndexOf(enteredText);
                }
                else
                {
                    index = enteredText.IndexOf(tempContact.description.ToLower());
                }
                if (index >= 0)
                {
                    tempContacts.Add(tempContact);
                    continue;
                }
            }


            //else check Number 
            for (int j = 0; j < tempContact.phoneNumbers.Count; j++)
            {
                searchedString = tempContact.phoneNumbers[j].number.ToLower();
                if (searchedString.Length > searchedStringLength)
                {
                    index = searchedString.IndexOf(enteredText);
                }
                else
                {
                    index = enteredText.IndexOf(searchedString);
                }

                if (index >= 0)
                {
                    tempContacts.Add(tempContact);
                    found = true;
                    break;
                }
            }

            //For the break from prev forloop
            if (found)
            {
                found = false;
                continue;
            }

            //Else check emails and links
            tempEmailsAndLinks.Clear();
            tempEmailsAndLinks.AddRange(tempContact.emails);
            tempEmailsAndLinks.AddRange(tempContact.links);

            for (int j = 0; j < tempEmailsAndLinks.Count; j++)
            {
                searchedString = tempEmailsAndLinks[j].ToLower();
                if (searchedString.Length > searchedStringLength)
                {
                    index = searchedString.IndexOf(enteredText);
                }
                else
                {
                    index = enteredText.IndexOf(searchedString);
                }

                if (index >= 0)
                {
                    tempContacts.Add(tempContact);
                    break;
                }
            }

        }

        return tempContacts;


    }


}
