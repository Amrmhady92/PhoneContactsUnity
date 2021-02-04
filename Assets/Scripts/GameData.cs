using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[CreateAssetMenu(menuName = "SOs/GameData")]
public class GameData : ScriptableObject
{

    [SerializeField] private List<Contact> contacts;

    //private ContactsSaver saver;
    private FileInfo dataFile;
    //private string directory;




    //My initial Save Load using JSON, can test later to see which is faster
    private void Save()
    {
        //Debug.Log("Saving");
        //StreamWriter w;
        //if (saver == null)
        //{
        //    saver = new ContactsSaver();
        //}
        //saver.contacts = new List<Contact>(contacts);
        //string contactsToJson = JsonUtility.ToJson(saver);
        //if (!dataFile.Exists)
        //{
        //    w = dataFile.CreateText();
        //}
        //else
        //{
        //    dataFile.Delete();
        //    w = dataFile.CreateText();
        //}
        //w.WriteLine(contactsToJson);
        //w.Close();
    }

    private void Load()
    {
        //Debug.Log("Loading");

        ////Reader nad open data file to read
        //StreamReader r = dataFile.OpenText();
        //string info = r.ReadToEnd();
        //r.Close();

        ////Get the data from the read info
        //saver = JsonUtility.FromJson<ContactsSaver>(info);

        //if(saver == null)
        //{
        //    //Couldnt read data
        //    saver = new ContactsSaver();
        //    saver.contacts = new List<Contact>();
        //}

        //contacts = new List<Contact>(saver.contacts);
    }

    //SaveData() & LoadData() Code used from 
    //https://www.sitepoint.com/saving-and-loading-player-game-data-in-unity/

    //To do -- save each contact in a file
    //public void SaveData()
    //{

    //    //if (saver == null)
    //    //{
    //    //    saver = new ContactsSaver();
    //    //}
    //    //saver.contacts = contacts;
    //    if (contacts == null || contacts.Count == 0) return;



    //    if (!Directory.Exists("Saves"))
    //        Directory.CreateDirectory("Saves");

    //    BinaryFormatter formatter = new BinaryFormatter();
    //    string filename = "";
    //    //FileStream saveFile = File.Create("Saves/save.binary");
    //    FileStream saveFile = File.Create("Saves/"+ filename + ".cntct");
    //    Debug.Log("saving");


    //    formatter.Serialize(saveFile, saver);

    //    saveFile.Close();
    //}

    //Must be called
    public void Init()
    {
        //saver = new ContactsSaver();
        LoadData();
    }
    private void SaveContact(Contact contact)
    {
        if (contacts == null || contacts.Count == 0) return;

        int index = contacts.Count;


        if (!Directory.Exists(Application.persistentDataPath + "/Saves")) 
            Directory.CreateDirectory(Application.persistentDataPath + "/Saves");

        BinaryFormatter formatter = new BinaryFormatter();
        string filename = contact.GetHashCode().ToString();//contact.name;//contact.GetHashCode().ToString();
        FileStream saveFile = File.Create(Application.persistentDataPath + "/Saves/" + filename + ".cntct");
        formatter.Serialize(saveFile, contact);
        saveFile.Close();
    }

    private void RemoveSaveFile(Contact contactToDelete)
    {
        if (!Directory.Exists(Application.persistentDataPath + "/Saves"))
        {
            contacts = new List<Contact>();
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
                    if(contact.CompareContact(contactToDelete))
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
    public void LoadData()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/Saves"))
        {
            Debug.Log("Nothing to load");
            contacts = new List<Contact>();
            return;
        }

        if (contacts == null) contacts = new List<Contact>();
        else contacts.Clear();
        
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


                if(contact != null)
                {
                    //Debug.Log("Loaded Contact: " + contact.name);
                    contacts.Add(contact);
                }
            }
        }
        //if (saver != null) contacts = saver.contacts;
        //else contacts = new List<Contact>();

        //FileStream saveFile = File.Open("Saves/save.binary", FileMode.Open);

    }

    public string AddContact(Contact contact)
    {
        //if (!initialized) Init();

        if (contact == null) return "Contact is Null";

        //Contact foundContact = contacts.Find(c => c.name.ToLower() + c.lastname.ToLower() == contact.name.ToLower() + contact.lastname.ToLower());
        //if (foundContact != null)
        if (FindExactContact(contact) != null)
        {
            Debug.LogError("Contact already exist");
            return "Contact already exist";
        }
        else
        {
            contacts.Add(contact);
            SaveContact(contact);
            return "Contact added";
        }
    }

    public void EditContact(Contact editedContact)
    {
        RemoveContact(editedContact);
        AddContact(editedContact);
    }
    public bool RemoveContact(Contact contact)
    {
        //Contact foundContact = contacts.Find(c => c.name + c.lastname == contact.name + contact.lastname);
        //Testing, might be slower
        Contact found = FindExactContact(contact);
        if (found != null)
        {
            //if (foundContact.CompareContact(contact))
            //{
            Debug.Log("Found");
            if (contacts.Remove(found))// remove from list
            {
                Debug.Log("Removed from List");
            }
            RemoveSaveFile(contact); //Delete the save file
            return true;
            //}
            //else
            //{
            //    return false;
            //}

        }
        else
        {
            Debug.LogError("Couldnt find contact to remove");
            return false;
        }
    }

    public Contact FindExactContact(Contact contact)
    {
        return contacts.Find(c => c.CompareContact(contact) == true);
    }

    public List<Contact> GetAllContacts()
    {
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
        enteredText = enteredText.ToLower();
        searchedStringLength = enteredText.Length;

        for (int i = 0; i < contacts.Count; i++)
        {
            tempContact = contacts[i];

            if (tempContact == null) continue;

            found = false;

            //First Name
            if (tempContact.name.Length > searchedStringLength)
            {
                //splitString = tempContact.name.Substring(0, searchedStringLength);
                index = tempContact.name.ToLower().IndexOf(enteredText);
            }
            else
            {
                index = enteredText.IndexOf(tempContact.name.ToLower());
                //splitString = tempContact.name;
            }
            //Debug.Log("index for contact name " + tempContact.name + " : " + index);

            //If name matches
            if (index >= 0/*splitString == enteredText*/)
            {
                tempContacts.Add(tempContact);
                continue;
            }


            //Last Name
            if (tempContact.lastname.Length > searchedStringLength)
            {
                //splitString = tempContact.name.Substring(0, searchedStringLength);
                index = tempContact.lastname.ToLower().IndexOf(enteredText);
            }
            else
            {
                index = enteredText.IndexOf(tempContact.lastname.ToLower());
                //splitString = tempContact.name;
            }
            //Debug.Log("index for contact name " + tempContact.name + " : " + index);

            //If name matches
            if (index >= 0/*splitString == enteredText*/)
            {
                tempContacts.Add(tempContact);
                continue;
            }


            //else Check Notes (less work than next items)
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

            //else check Number 
            for (int j = 0; j < tempContact.phoneNumbers.Count; j++)
            {
                searchedString = tempContact.phoneNumbers[j].number.ToLower();
                if (searchedString.Length > searchedStringLength)
                {
                    //splitString = searchedString.Substring(0, searchedStringLength);
                    index = searchedString.IndexOf(enteredText);
                }
                else
                {
                    //splitString = searchedString;
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
                    //splitString = searchedString.Substring(0, searchedStringLength);
                    index = searchedString.IndexOf(enteredText);
                }
                else
                {
                    //splitString = searchedString;
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

//[System.Serializable]
//public class ContactsSaver
//{
//    public List<Contact> contacts;
//}