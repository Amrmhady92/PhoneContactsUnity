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
}

[System.Serializable]
public struct PhoneNumber
{
    public string number;
    public PhoneNumberType type;
}

