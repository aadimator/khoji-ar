using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Firebase;
using Firebase.Auth;

public class AuthManager : MonoBehaviour {

	// Firebase API variables
	FirebaseAuth auth;

	public static AuthManager sharedInstance = null;

	// Delegates
	public delegate IEnumerator AuthCallback ( Task<Firebase.Auth.FirebaseUser> Task, string operation);
	public event AuthCallback authCallback;

	void Awake() {
		if (sharedInstance == null) {
			sharedInstance = this;
		} else if (sharedInstance != this) {
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);

		auth = FirebaseAuth.DefaultInstance;
	}

	public void SignUpNewUser(string email, string password) {
		auth.CreateUserWithEmailAndPasswordAsync (email, password).ContinueWith (task => {
			StartCoroutine(authCallback(task, "sign_up"));
		});
	}

	public void LoginExistingUser(string email, string password) {
		auth.SignInWithEmailAndPasswordAsync (email, password).ContinueWith (task => {
			StartCoroutine(authCallback(task, "login"));
		});
	}
}
