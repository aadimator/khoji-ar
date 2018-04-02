using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AccountManager : MonoBehaviour {

    AuthManager authManager = AuthManager.sharedInstance;

    public void OnSignOut()
    {
        Mapbox.Unity.Utilities.Console.Instance.Log("OnSignOut()", "lightblue");

        if(authManager == null)
        {
            Debug.Log("authmanager is null");
        }

        authManager.SignOut();

        Mapbox.Unity.Utilities.Console.Instance.Log("after auth.SignOut()", "lightblue");

        SceneManager.LoadScene("Onboarding");

        Mapbox.Unity.Utilities.Console.Instance.Log("after SceneChange", "lightblue");
    }
}
