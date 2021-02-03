using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum DetailType
{
    Name,
    Phone,
    Email,
    Link,
    Description,
    DateAdded
}
public class ContactDetail : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI detailTypeText;
    [SerializeField] private TextMeshProUGUI detailValueText;
    [SerializeField] private DetailType type;
    public bool SetContactDetail(Contact contact, DetailType detailType, int index = 0)
    {
        type = detailType;
        string num = "";
        switch (detailType)
        {
            case DetailType.Name:
                detailTypeText.text = "Name:";
                detailValueText.text = contact.name + " " + contact.lastname;
                break;
            case DetailType.Phone:
                if (index > contact.phoneNumbers.Count || index < 0) return false;
                num = "";
                //If there are more mobile/home etc. put the number of it next to "Mobile : " etc.
                if (contact.phoneNumbers.Count > 1)
                {
                    int ind = 0;
                    for (int i = 0; i < contact.phoneNumbers.Count; i++)
                    {
                        if (contact.phoneNumbers[i].type == contact.phoneNumbers[index].type)
                        {
                            ind++;
                            if (contact.phoneNumbers[i].number == contact.phoneNumbers[index].number)
                            {
                                break;
                            }
                        }
                    }
                    if (ind > 0)
                    {
                        num = " " + ind.ToString();
                    }
                }
                detailTypeText.text = contact.phoneNumbers[index].type + num + ":";
                detailValueText.text = contact.phoneNumbers[index].number;
                return true;
            case DetailType.Email:
                if (index > contact.emails.Count || index < 0) return false;
                 num = "";
                //If there are more mobile/home etc. put the number of it next to "Mobile : " etc.
                if (contact.emails.Count > 1)
                {
                    int ind = 0;
                    for (int i = 0; i < contact.emails.Count; i++)
                    {
                        ind++;
                        if (contact.emails[i] == contact.emails[index])
                        {
                            break;
                        }
                    }
                    if (ind > 0)
                    {
                        num = " " + ind.ToString();
                    }
                }
                detailTypeText.text = "Email" + num + ":";
                detailValueText.text = contact.emails[index];
                return true;
            case DetailType.Link:
                if (index > contact.links.Count || index < 0) return false;
                num = "";
                //If there are more mobile/home etc. put the number of it next to "Mobile : " etc.
                if (contact.links.Count > 1)
                {
                    int ind = 0;
                    for (int i = 0; i < contact.links.Count; i++)
                    {
                        ind++;
                        if (contact.links[i] == contact.links[index])
                        {
                            break;
                        }
                    }
                    if (ind > 0)
                    {
                        num = " " + ind.ToString();
                    }
                }
                detailTypeText.text = "Link" + num + ":";
                detailValueText.text = contact.links[index];
                break;
            case DetailType.Description:
                detailTypeText.text = "Description:";
                detailValueText.text = contact.description;
                break;
            case DetailType.DateAdded:
                detailTypeText.text = "Date Added:";
                detailValueText.text = contact.dateAdded.ToShortDateString();
                break;
            default:
                return false;
        }
        return true;
    }

    public void OnContactDetailClicked()
    {
        switch (type)
        {
            case DetailType.Phone:
                //Code taken from online 
                //https://stackoverflow.com/questions/48906129/make-phone-call-in-unity?noredirect=1&lq=1


                string phoneNum =  detailValueText.text;

                ////For accessing static strings(ACTION_CALL) from android.content.Intent
                //AndroidJavaClass intentStaticClass = new AndroidJavaClass("android.content.Intent");
                //string actionCall = intentStaticClass.GetStatic<string>("ACTION_CALL");

                ////Create Uri
                //AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
                //AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", phoneNum);

                ////Pass ACTION_CALL and Uri.parse to the intent
                //AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", actionCall, uriObject);
                phoneNum.Replace("+", "");
                /// OR
                Application.OpenURL("tel://[+" + phoneNum + "]");
                ///
                break;

            case DetailType.Email:
                Application.OpenURL("mailto:" + detailValueText.text);

                break;
            case DetailType.Link:
                string link = detailValueText.text.Replace("https://", "");
                Application.OpenURL("http://" + detailValueText.text);
                break;
            default:
                //Do Nothing
                break;
        }
    }
}
