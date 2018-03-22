using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Auth;

public class ContactsManager : MonoBehaviour {

	public List<User> contactList = new List<User> ();

	FirebaseAuth auth;

	void Awake() {
		contactList.Clear();

		auth = FirebaseAuth.DefaultInstance;

		DatabaseManager.sharedInstance.GetContacts (result => {
			contactList = result;
			Debug.Log(contactList[0].email);
		}, auth.CurrentUser.UserId);

		Debug.Log (contactList[0].email);

//		Router.Users ().ChildAdded += NewUserAdded;

//		Router.Users ().OrderByChild ("name");
//		Router.Users ().LimitToFirst (10);

	}

	void NewUserAdded(object sender, ChildChangedEventArgs args) {
		if (args.Snapshot.Value == null) {
			Debug.Log ("Sorry, there was no data at that node.");
		} else {
			Debug.Log ("New user has been added!");
		}
	}
}
