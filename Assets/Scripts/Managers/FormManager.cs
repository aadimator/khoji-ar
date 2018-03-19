using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using UnityEngine.SceneManagement;

public class FormManager : MonoBehaviour {

	// UI objects linked from the inspector
	public InputField emailInput;
	public InputField passwordInput;

	public Button signUpButton;
	public Button loginButton;

	public Text statusText;

	public AuthManager authManager;

	void Awake() {
		ToggleButtonStates (false);

		// Auth delegate subscriptions
		authManager.authCallback += HandleAuthCallback;
	}

	/// <summary>
	/// Validates the email input
	/// </summary>
	public void ValidateEmail() {
		string email = emailInput.text;
		var regexPattern = @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
                + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
                + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
                + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

		if (email != "" && Regex.IsMatch(email, regexPattern)) {
			ToggleButtonStates (true);
		} else {
			ToggleButtonStates (false);
		}
	}

	// Firebase methods
	public void OnSignUp() {

		authManager.SignUpNewUser (emailInput.text, passwordInput.text);

		Debug.Log ("Sign Up");
		UpdateStatus ("Sign Up");
	}

	public void OnLogin() {
		authManager.LoginExistingUser (emailInput.text, passwordInput.text);

		Debug.Log ("Login");
		UpdateStatus ("Login");
	}

	IEnumerator HandleAuthCallback (Task<Firebase.Auth.FirebaseUser> task, string operation) {
		if (task.IsFaulted || task.IsCanceled) {
			UpdateStatus("Sorry, there was an error creating your new account. ERROR: " + task.Exception);
		} else if (task.IsCompleted) {

			if (operation == "sign_up") {
				Firebase.Auth.FirebaseUser newUser = task.Result;
				Debug.LogFormat ("Welcome to Khoji {0}!", newUser.Email);

				User user = new User (newUser.Email, newUser.Email, "");
				DatabaseManager.sharedInstance.CreateNewUser (user, newUser.UserId);
			}
				
			UpdateStatus("Loading the game scene");

			yield return new WaitForSeconds (1.5f);
			SceneManager.LoadScene ("ManualSync");
		}
	}

	void OnDestroy() {
		authManager.authCallback -= HandleAuthCallback;
	}

	// Utilities
	private void ToggleButtonStates(bool toState) {
		signUpButton.interactable = toState;
		loginButton.interactable = toState;
	}

	private void UpdateStatus(string message) {
		statusText.text = message;
	}

	public void Quit () 
	{
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}
}
