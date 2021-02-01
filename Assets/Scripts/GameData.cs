using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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
        saver = new ContactsSaver();
        directory = (Application.persistentDataPath + "\\" + "contactsData.txt");
        Debug.Log(directory);
        //dataFile = new FileInfo(directory);
        LoadData();
        //if (dataFile.Exists)
        //{
        //    Debug.Log("Exists");
        //    Load();
        //}
        //else
        //{
        //    Debug.Log("Doesnt Exist");
        //    contacts = new List<Contact>();
        //}

    }

    //My initial Save Load
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

    //SaveData() & LoadData() Code used from 
    //https://www.sitepoint.com/saving-and-loading-player-game-data-in-unity/

    //To do -- save each contact in a file
    public void SaveData()
    {
        if (!Directory.Exists("Saves"))
            Directory.CreateDirectory("Saves");

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile = File.Create("Saves/save.binary");

        if (saver == null)
        {
            saver = new ContactsSaver();
        }
        saver.contacts = contacts;

        formatter.Serialize(saveFile, saver);

        saveFile.Close();
    }



    public void LoadData()
    {
        if (!Directory.Exists("Saves"))
        {
            Debug.Log("Nothing to load");
            contacts = new List<Contact>();
            return;
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile = File.Open("Saves/save.binary", FileMode.Open);
        saver = (ContactsSaver)formatter.Deserialize(saveFile);
        saveFile.Close();
        Debug.Log("Saver:"+saver.contacts.Count);
        if (saver != null) contacts = saver.contacts;
        else contacts = new List<Contact>();
    }

    public void AddContact(Contact contact)
    {
        //if (!initialized) Init();

        if (contact == null) return;

        contacts.Add(contact);
        SaveData();
    }

    public List<Contact> GetAllContacts()
    {
        LoadData();
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

[System.Serializable]
public class ContactsSaver
{
    public List<Contact> contacts;
}