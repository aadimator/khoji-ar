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
		} else if (sharedInstance != null) {
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);

		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl ("https://khoji-aadimator.firebaseio.com/");
	}

}
