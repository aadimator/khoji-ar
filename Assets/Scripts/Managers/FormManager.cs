using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Text.RegularExpressions;

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
		Debug.Log ("Sign Up");
		UpdateStatus ("Sign Up");
	}

	public void OnLogin() {
		Debug.Log ("Login");
		UpdateStatus ("Login");
	}

	// Utilities
	private void ToggleButtonStates(bool toState) {
		signUpButton.interactable = toState;
		loginButton.interactable = toState;
	}

	private void UpdateStatus(string message) {
		statusText.text = message;
	}
}
