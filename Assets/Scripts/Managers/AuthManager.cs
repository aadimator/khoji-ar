using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using UnityEngine.SceneManagement;

public class AuthManager : MonoBehaviour
{

    // Firebase API variables
    FirebaseAuth auth;

    public static AuthManager sharedInstance = null;

    // Delegates
    public delegate IEnumerator AuthCallback(Task<Firebase.Auth.FirebaseUser> Task, string operation);
    public event AuthCallback authCallback;

    void Awake()
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
        else if (sharedInstance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        InitializeFirebase();
    }

    public void SignUpNewUser(string email, string password)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            StartCoroutine(authCallback(task, "sign_up"));
        });
    }

    public void LoginExistingUser(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            StartCoroutine(authCallback(task, "login"));
        });
    }

    public void SignOut()
    {
        //Mapbox.Unity.Utilities.Console.Instance.Log("Signing out", "lightblue");

        if (auth == null) Debug.Log("auth is null");

        auth.SignOut();

        //Mapbox.Unity.Utilities.Console.Instance.Log("Signing out after", "lightblue");
    }

    // Handle initialization of the necessary firebase modules:
    void InitializeFirebase()
    {
        //Mapbox.Unity.Utilities.Console.Instance.Log("Setting up Firebase Auth", "lightblue");
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;

        AuthStateChanged(this, null);
    }

    void OnDestroy()
    {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }

    // Track state changes of the auth object.
    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        Firebase.Auth.FirebaseAuth senderAuth = sender as Firebase.Auth.FirebaseAuth;
        Firebase.Auth.FirebaseUser user = null;
        if (senderAuth == auth && senderAuth.CurrentUser != user)
        {
            bool signedIn = user != senderAuth.CurrentUser && senderAuth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
            }
            user = senderAuth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
            }
        }
    }
}
