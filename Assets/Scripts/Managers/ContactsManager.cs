using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Auth;



public class ContactsManager : MonoBehaviour {

	public List<User> contactList = new List<User> ();
    public List<Location> contactLocations = new List<Location>();
	public Dictionary<String, String> contactIds = new Dictionary<String, String> ();

	FirebaseAuth auth;

	void Awake() {
		contactList.Clear();
		contactIds.Clear ();
        contactLocations.Clear();

        auth = FirebaseAuth.DefaultInstance;
		DatabaseManager.sharedInstance.GetContacts (result => {
			contactList = result;

			PrintUsers(contactList);

		}, auth.CurrentUser.UserId);

        DatabaseManager.sharedInstance.GetContactLocations(result => {
            contactLocations = result;

            PrintLocations(contactLocations);
        }, auth.CurrentUser.UserId);



//		Router.Users ().ChildAdded += NewUserAdded;

//		Router.Users ().OrderByChild ("name");
//		Router.Users ().LimitToFirst (10);

	}

    private void PrintLocations(List<Location> contactLocations)
    {
        Mapbox.Unity.Utilities.Console.Instance.Log("Contact Location Size: " + contactLocations.Count, "lightblue");
        foreach (Location location in contactLocations)
        {
            Mapbox.Unity.Utilities.Console.Instance.Log("Contact Location: " + 
                location.latitude.ToString() + " : " + location.longitude.ToString(), "lightblue");
        }
    }

    void PrintUsers(List<User> contacts) {
		Mapbox.Unity.Utilities.Console.Instance.Log ("Contact List Size: " + contacts.Count, "lightblue");
		foreach (User user in contacts) {
			Mapbox.Unity.Utilities.Console.Instance.Log ("Contact List: " + user.email, "lightblue");
		}
	}


	void NewUserAdded(object sender, ChildChangedEventArgs args) {
		if (args.Snapshot.Value == null) {
			Debug.Log ("Sorry, there was no data at that node.");
		} else {
			Debug.Log ("New user has been added!");
		}
	}
}