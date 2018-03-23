namespace Mapbox.Unity.Ar {
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using Firebase;
	using Firebase.Database;
	using Firebase.Auth;


	public class ContactsManager : MonoBehaviour {

		public List<User> contactList = new List<User> ();
		public Dictionary<String, String> contactIds = new Dictionary<String, String> ();

		FirebaseAuth auth;

		void Awake() {
			contactList.Clear();
			contactIds.Clear ();


			auth = FirebaseAuth.DefaultInstance;
			DatabaseManager.sharedInstance.GetContacts (result => {
				contactList = result;

				foreach (User user in contactList) {
					Unity.Utilities.Console.Instance.Log ("Contact List: " + user.email, "lightblue");
				}
			}, auth.CurrentUser.UserId);



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
}