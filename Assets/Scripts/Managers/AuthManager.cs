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

    private static AuthManager _instance;

    public static AuthManager Instance
    {
        get { return _instance; }
    }

    // Delegates
    public delegate IEnumerator AuthCallback(Task<Firebase.Auth.FirebaseUser> Task, string operation);
    public event AuthCallback authCallback;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject);

        InitializeFirebase();
    }

    private void Start()
    {
        
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

        auth.SignOut();

        //Mapbox.Unity.Utilities.Console.Instance.Log("Signing out after", "lightblue");
    }

    // Handle initialization of the necessary firebase modules:
    void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;

        AuthStateChanged(this, null);
    }

    void OnDestroy()
    {
        if (auth != null) 
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
