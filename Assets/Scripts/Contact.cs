using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum PhoneNumberType
{
    Mobile,
    Work,
    Home
}

[System.Serializable]
public class Contact
{
    public string name;
    public string lastname;
    public List<PhoneNumber> phoneNumbers;
    public List<string> emails;
    public List<string> links;
    public string description;
    public DateTime dateAdded;

    public Contact(Contact c = null)
    {
        this.name = c.name;
        this.lastname = c.lastname;
        this.phoneNumbers = new List<PhoneNumber>(c.phoneNumbers);
        this.emails = new List<string>(c.emails);
        this.links = new List<string>(c.links);
        this.description = c.description;
        this.dateAdded = c.dateAdded;
    }
    public Contact()
    {
        this.name = "";
        this.lastname = "";
        this.phoneNumbers = new List<PhoneNumber>();
        this.emails = new List<string>();
        this.links = new List<string>();
        this.description = "";
        this.dateAdded = DateTime.Today;
    }

    public bool CompareContact(Contact c)
    {
        if (c == null) return false;
        if (name + lastname != c.name + c.lastname) return false;
        if (description != c.description) return false;


        if (phoneNumbers.Count == c.phoneNumbers.Count)
        {
            for (int i = 0; i < c.phoneNumbers.Count; i++)
            {
                if (c.phoneNumbers[i].type != phoneNumbers[i].type) return false;
                if (c.phoneNumbers[i].number != phoneNumbers[i].number) return false;
            }
        }
        else return false;

        if (emails.Count == c.emails.Count)
        {
            for (int i = 0; i < c.emails.Count; i++)
            {
                if (c.emails[i] != emails[i]) return false;
            }
        }
        else return false;

        if (links.Count == c.links.Count)
        {
            for (int i = 0; i < c.links.Count; i++)
            {
                if (c.links[i] != links[i]) return false;
            }
        }
        else return false;

        return true;
    }
}

[System.Serializable]
public struct PhoneNumber
{
    public string number;
    public PhoneNumberType type;
}

