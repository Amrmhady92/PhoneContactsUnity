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
}

[System.Serializable]
public struct PhoneNumber
{
    public string number;
    public PhoneNumberType type;
}

