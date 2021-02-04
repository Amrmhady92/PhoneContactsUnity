# PhoneContactsUnity

Welcome to PhoneBook 

How to Install:
Manually, you can build the project for Android and install the APK on your android device.
A version of the APK is also available in the repo to install directly without the need for Building.

Instructions for use:
The App should be clear on how to use.
However, the following should explain:

#Adding a contact:
	To Add a contact, press on the [+] button on the top right of the screen, then you will be presented with the create Contact screen. pressing on the [+] button on the top right of the screen will give you the option to Add a detail to the contact. Details include Name (First and Last), Phone, Email, Links and Description.
Each detail can be added by pressing the [+] button then selecting the type of detail from the drop down list.
A new window will appear according to the selected detail type. Enter the desired values in the corresponding parameters, ie. input fields.

Make sure to set a name detail, otherwise you cant finish making the contact.
The [Cancel] button returns to the Main screen without saving the contact.
The [Finish] button saves the contact, and returns to the Main screen. (Note: if a contact already exists, a prompt will show and notify you).

In the Contact Creation screen, you can delete entered details by pressing on the details, a prompt to delete the detail will show.

The contacts are saved under "\Android\data\com.JuxtapoZition.PhoneBookUnity\files\Saves", save files are not named after the contacts, save files have the extention ".cntct"
	
#Viewing Contacts:
	After creating contacts, you can preview available contacts by pressing on their names in the Main screen, you will then be moved to the View Contact screen, pressing on the Phone numbers, Emails or links should start a call or go to the link provided.

pressing the [Edit] button will send you to the Contact Creation screen with the filled contact details, so you can add or remove details from it.

pressing the [Remove] button will delete the contact from the list, a prompt will show to confirm deletion.

#Searching Contacts:
	On the top right of the screen a [Search] button can pressed to show the search bar.
Input given to search bar will search for everything a contact may have, Name, Phone, Email, Link, or Description. any match should appear in the list in the Main screen.

#Sorting Contacts:
	You can sort contacts by Name (a: ascending, D: descending orders) or by Date Added (ascending or descending), also works on searched contacts.

#Navigation:
	Using the Back button on your android device, should return to Main screen if you are in the View Contact screen.
Leaving the Create Contact Screen is only doable throught the [Finish] or [Cancel] buttons.


============================================================================

Implementiation and Shortcomings:
Contacts are saved using the BinaryFormatter by serialization of the Class (Contact), and saved in the .cntct format, the naming is the HashCode of the class.

Pooling is used for repeated objects, such as ContactHolders for the Main Screen, Holders only contain First name and the first available phone number, Email, or Link (in that order). ContactDetail objects are used in the View Contact screen and the Contact Creation screen, with slight difference of the OnContactDetailClicked. (ie. Use detail, and Remove detail).

LeanTween asset is used for animating panels.

Problems with UI when switching to Landscape mode, buttons change in sizes, however doesnt go outside screen.

Class UIMover is used to play animation for Panels.

Handler Class is used mainly for the Main screen, with some association with the other screens. (not optimal but due to time and deadline).

UI Art is not implemented, also due to limited time.

============================================================================

Feel Free to contact me about any detail in the project.
Thanks for Reading.