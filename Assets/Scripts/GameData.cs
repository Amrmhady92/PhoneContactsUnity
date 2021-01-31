using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(menuName = "SOs/GameData")]
public class GameData : ScriptableObject
{

    [SerializeField] private List<Contact> contacts;

    private ContactsSaver saver;
    private FileInfo dataFile;
    private string directory;


    //Must be called
    public void Init()
    {

        directory = (Application.persistentDataPath + "\\" + "contactsData.txt");
        dataFile = new FileInfo(directory);
        if (dataFile.Exists)
        {
            Debug.Log("Exists");
            Load();
        }
        else
        {
            Debug.Log("Doesnt Exist");
            contacts = new List<Contact>();
        }

    }

    private void Save()
    {
        Debug.Log("Saving");
        StreamWriter w;
        if (saver == null)
        {
            saver = new ContactsSaver();
        }
        saver.contacts = new List<Contact>(contacts);
        string contactsToJson = JsonUtility.ToJson(saver);
        if (!dataFile.Exists)
        {
            w = dataFile.CreateText();
        }
        else
        {
            dataFile.Delete();
            w = dataFile.CreateText();
        }
        w.WriteLine(contactsToJson);
        w.Close();
    }

    private void Load()
    {
        Debug.Log("Loading");

        //Reader nad open data file to read
        StreamReader r = dataFile.OpenText();
        string info = r.ReadToEnd();
        r.Close();

        //Get the data from the read info
        saver = JsonUtility.FromJson<ContactsSaver>(info);

        if(saver == null)
        {
            //Couldnt read data
            saver = new ContactsSaver();
            saver.contacts = new List<Contact>();
        }

        contacts = new List<Contact>(saver.contacts);
    }

    public void AddContact(Contact contact)
    {
        //if (!initialized) Init();

        if (contact == null) return;

        contacts.Add(contact);
        Save();
    }

    public List<Contact> GetAllContacts()
    {
        Load();
        return contacts;
    }


    List<Contact> tempContacts = new List<Contact>();
    List<string> tempEmailsAndLinks = new List<string>();
    int searchedStringLength;
    string searchedString;
    //string splitString;
    bool found = false;
    int index;
    Contact tempContact;



    public List<Contact> GetContactsByText(string enteredText)
    {
        tempContacts.Clear();
        tempEmailsAndLinks.Clear();
        searchedStringLength = enteredText.Length;

        for (int i = 0; i < contacts.Count; i++)
        {
            if (contacts[i] == null) continue;
            found = false;
            tempContact = contacts[i];

            if (tempContact.name.Length > searchedStringLength)
            {
                //splitString = tempContact.name.Substring(0, searchedStringLength);
                index = tempContact.name.IndexOf(enteredText);
            }
            else
            {
                index = enteredText.IndexOf(tempContact.name);
                //splitString = tempContact.name;
            }
            //If name matches
            if (index > 0/*splitString == enteredText*/)
            {
                tempContacts.Add(tempContact);
                continue;
            }

            //else Check Notes (less work than next items)
            if (tempContact.note.Length > searchedStringLength)
            {
                index = tempContact.note.IndexOf(enteredText);
            }
            else
            {
                index = enteredText.IndexOf(tempContact.note);
            }
            if (index > 0)
            {
                tempContacts.Add(tempContact);
                continue;
            }

            //else check Number 
            for (int j = 0; j < tempContact.phoneNumbers.Count; j++)
            {
                searchedString = tempContact.phoneNumbers[j].number;
                if (searchedString.Length > searchedStringLength)
                {
                    //splitString = searchedString.Substring(0, searchedStringLength);
                    index = searchedString.IndexOf(enteredText);
                }
                else
                {
                    //splitString = searchedString;
                    index = enteredText.IndexOf(tempContact.name);
                }

                if (index > 0)
                {
                    tempContacts.Add(tempContact);
                    found = true;
                    break;
                }
            }

            //For the break from prev forloop
            if(found)
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
                searchedString = tempEmailsAndLinks[j];
                if (searchedString.Length > searchedStringLength)
                {
                    //splitString = searchedString.Substring(0, searchedStringLength);
                    index = searchedString.IndexOf(enteredText);
                }
                else
                {
                    //splitString = searchedString;
                    index = enteredText.IndexOf(tempContact.name);
                }

                if (index > 0)
                {
                    tempContacts.Add(tempContact);
                    break;
                }
            }

        }


        return tempContacts;


    }


    //public Contact GetContact(string name)
    //{
    //    return contacts.Find(c => c.name == name);
    //}
    //public Contact GetContact(int index)
    //{
    //    if (index > contacts.Count || index < 0) return null;
    //    return contacts[index];
    //}

    //public List<Contact> GetContacts(string name)
    //{
    //    return contacts.FindAll(c => c.name == name);
    //}

    //This is usless I guess
    //public List<Contact> GetContacts(int phoneNumber)
    //{
    //    tempContacts.Clear();
    //    string enteredText = phoneNumber.ToString();
    //    for (int j = 0; j < tempContact.phoneNumbers.Count; j++)
    //    {
    //        searchedString = tempContact.phoneNumbers[j].number.ToString();
    //        if (searchedString.Length > searchedStringLength)
    //        {
    //            //splitString = searchedString.Substring(0, searchedStringLength);
    //            index = searchedString.IndexOf(enteredText);
    //        }
    //        else
    //        {
    //            //splitString = searchedString;
    //            index = enteredText.IndexOf(tempContact.name);
    //        }

    //        if (index > 0)
    //        {
    //            tempContacts.Add(tempContact);
    //            found = true;
    //            break;
    //        }
    //    }
    //    return tempContacts;
    //    //return contacts.FindAll(c => c.phoneNumbers.Find(x => x.number == phoneNumber).number == phoneNumber);
    //}


}


public class ContactsSaver
{
    public List<Contact> contacts;
}