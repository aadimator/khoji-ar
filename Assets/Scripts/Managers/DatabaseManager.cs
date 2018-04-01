using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Threading.Tasks;

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
		Dictionary<String, String> contactIds = new Dictionary<String, String> ();

		GetContactIds (result => {
			contactIds = result;
			tmpList.Clear();

			Mapbox.Unity.Utilities.Console.Instance.Log("Number of contact ids: " + contactIds.Count, "lightblue");

			foreach (String id in contactIds.Keys) {
				Router.UserWithUID (id).GetValueAsync ().ContinueWith (task => {
					DataSnapshot user = task.Result;

					var usertDict = (IDictionary<string, object>) user.Value;
					User newUser = new User(usertDict);
					tmpList.Add(newUser);

					if (tmpList.Count == contactIds.Count)
						completionBlock(tmpList);
				});
			}	
		}, uid);
	}

	public void GetContactLocations(Action<List<UserLocation>> completionBlock, string uid) {
		List<UserLocation> tmpList = new List<UserLocation> ();
		Dictionary<String, String> contactIds = new Dictionary<String, String> ();

		GetContactIds (result => {
			contactIds = result;
			tmpList.Clear();

			Mapbox.Unity.Utilities.Console.Instance.Log("Number of contact ids: " + contactIds.Count, "lightblue");

			foreach (String id in contactIds.Keys) {
				Router.LocationOfUID (id).GetValueAsync ().ContinueWith (task => {
					DataSnapshot location = task.Result;

					var locationDict = (IDictionary<string, object>) location.Value;
					UserLocation userLoc = new UserLocation(locationDict);
					tmpList.Add(userLoc);

					if (tmpList.Count == contactIds.Count)
						completionBlock(tmpList);
				});
			}	
		}, uid);
	}

	public void GetContactIds(Action<Dictionary<String, String>> completionBlock, string uid) {
		Dictionary<String, String> tmpList = new Dictionary<String, String> ();

		Router.ContactsOfUID (uid).GetValueAsync ().ContinueWith (task => {
			DataSnapshot users = task.Result;

			foreach(DataSnapshot user in users.Children) {
				tmpList.Add(user.Key, user.Value.ToString());
			}

			completionBlock(tmpList);

		});
	}



}
