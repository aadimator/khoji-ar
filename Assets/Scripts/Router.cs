using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class Router : MonoBehaviour {

	private static DatabaseReference baseRef = FirebaseDatabase.DefaultInstance.RootReference;

	public static DatabaseReference Users() {
		return baseRef.Child ("users");
	}

	public static DatabaseReference UserWithUID(string uid) {
		return baseRef.Child ("users").Child (uid);
	}
}
