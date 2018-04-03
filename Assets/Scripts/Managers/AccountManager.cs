using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AccountManager : MonoBehaviour {

    AuthManager authManager = AuthManager.Instance;

    public void OnSignOut()
    {
        Mapbox.Unity.Utilities.Console.Instance.Log("OnSignOut()", "lightblue");

        Debug.Log("OnSignOut()");

        if (authManager == null)
        {
            Debug.Log("authmanager is null");
        }

        authManager.SignOut();

        Mapbox.Unity.Utilities.Console.Instance.Log("after auth.SignOut()", "lightblue");

        Debug.Log("Changing Scene");

        SceneManager.LoadScene("Onboarding");

        Debug.Log("Scene Changed");


        Mapbox.Unity.Utilities.Console.Instance.Log("after SceneChange", "lightblue");
    }
}
