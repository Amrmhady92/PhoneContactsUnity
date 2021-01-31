using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public List<PhoneNumber> phoneNumbers;
    public List<string> emails;
    public List<string> links;
    public string note;
}

[System.Serializable]
public struct PhoneNumber
{
    public string number;
    public PhoneNumberType type;
}