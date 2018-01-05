using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;

public class AuthManager : MonoBehaviour {

	// Firebase API variables
	Firebase.Auth.FirebaseAuth auth;

	void Awake() {
		auth = FirebaseAuth.DefaultInstance;
	}

	public void SugnUpNewUser(string email, string password) {

	}

}
