using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class DatabaseManager : MonoBehaviour {

	public static DatabaseManager sharedInstance = null;

	/// <summary>
	/// Awake this instance and initialize sharedInstance for Singelton pattern
	/// </summary>
	void Awake() {
		if (sharedInstance == null) {
			sharedInstance = this;
		} else if (sharedInstance != this) {
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);

		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl ("https://khoji-aadimator.firebaseio.com/");
	}

	public void CreateNewUser(User user, string uid) {
		string userJSON = JsonUtility.ToJson (user);
		Router.UserWithUID (uid).SetRawJsonValueAsync (userJSON);
	}

	public void GetContacts(Action<List<User>> completionBlock, string uid) {
		List<User> tmpList = new List<User> ();

		Router.ContactsOfUID (uid).GetValueAsync ().ContinueWith (task => {
			DataSnapshot users = task.Result;

			foreach(DataSnapshot user in users.Children) {
				var usertDict = (IDictionary<string, object>) user.Value;
				User newUser = new User(usertDict);
				tmpList.Add(newUser);
			}

			completionBlock(tmpList);

		});
	}

}
