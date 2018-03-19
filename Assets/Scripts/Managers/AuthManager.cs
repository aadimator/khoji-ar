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

	public void SignUpNewUser(string email, string password) {
		auth.CreateUserWithEmailAndPasswordAsync (email, password).ContinueWith (task => {
			if (task.IsFaulted || task.IsCanceled) {
				Debug.LogError("Sorry, there was an error creating your new account. ERROR: " + task.Exception);
				return;
			} else if (task.IsCompleted) {
				Firebase.Auth.FirebaseUser newUser = task.Result;
				Debug.LogFormat("Welcome to Khoji {0}", newUser.Email);
			}
		});
	}

}
