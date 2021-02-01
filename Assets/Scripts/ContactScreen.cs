using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ContactScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Pooler contactDetailsPool;

    public bool DisplayContact(Contact contact)
    {
        if (contact == null) return false;



        return true;

    }
}
