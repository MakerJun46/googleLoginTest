using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Firebase.Auth;
using UnityEngine.UI;
using Firebase.Database;
using System;

public class GoogleFirebase : MonoBehaviour
{
    private FirebaseAuth auth;
    private DatabaseReference reference;
    public string FireBaseId = string.Empty;
    public static GoogleFirebase Instance = null;

    public Text FirebaseLogin;
    public Text GoogleLoginSatus;

    public InputField emailInput;
    public InputField passwordInput;

    public string databasename;

    [Serializable]
    public class User
    {
        public string username;
        public string email;

        public User()
        {
        }

        public User(string username, string email)
        {
            this.username = username;
            this.email = email;
        }
    }

    public class Info
    {
        bool hasThisHand;
        int Level;

        public Info(bool hasThis, int level)
        {
            hasThisHand = hasThis;
            Level = level;
        }
    }

    void Start()
    {
        //m_saved_state = GoogleSavedFileState.GOOGLE_SAVED_STATE_NONE; 


        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            // enables saving game progress. 
            .EnableSavedGames().RequestIdToken().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        auth = FirebaseAuth.DefaultInstance;
        Instance = this;


        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }
    public void Login_Google()
    {
        Debug.Log("GameCenter Login");
        if (!Social.localUser.authenticated) // 로그인 되어 있지 않다면 
        {
            Social.localUser.Authenticate(success => // 로그인 시도 
            {
                if (success) // 성공하면 
                {
                    Debug.Log("google game service Success");
                    GoogleLoginSatus.text = "Login Success";
                    //SystemMessageManager.Instance.AddMessage("google game service Success"); 
                    StartCoroutine(TryFirebaseLogin()); // Firebase Login 시도 
                }
                else // 실패하면 
                {
                    GoogleLoginSatus.text = "Login Failed";
                    Debug.Log("google game service Fail");
                }
            });

            databasename = Social.localUser.userName;
        }
    }

    public void Login_Email()
    {
        string email = emailInput.text.Trim();
        string password = passwordInput.text.Trim();


        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            // Firebase user has been created.
            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);

            databasename = emailInput.text;
        });
    }

    public void TryGoogleLogout()
    {
        if (Social.localUser.authenticated) // 로그인 되어 있다면 
        {
            PlayGamesPlatform.Instance.SignOut(); // Google 로그아웃 
            auth.SignOut(); // Firebase 로그아웃 
            GoogleLoginSatus.text = "LogOut";
        }
    }

    IEnumerator TryFirebaseLogin()
    {
        while (string.IsNullOrEmpty(((PlayGamesLocalUser)Social.localUser).GetIdToken()))
            yield return null;

        string idToken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();

        Firebase.Auth.Credential credential =
            Firebase.Auth.GoogleAuthProvider.GetCredential(idToken, null);
        auth.SignInWithCredentialAsync(credential).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithCredentialAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result;
            FirebaseLogin.text = "Success!";
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });

        FirebaseLogin.text = "firebaseLoginSuccess";
        //ReadUserInfos(databasename);
    }


    public void FirebaseDataSave()
    {
        GameManager.instance.User.setData(GameManager.instance.hasHand);

        writeNewUser();
        //writeNewUser1("guswns67711", "guswns6711@naver.com");
    }

    private void writeNewUser()
    {
        string json = JsonUtility.ToJson(GameManager.instance.User);

        reference.Child("HandData").SetRawJsonValueAsync(json);
        FirebaseLogin.text = "Data1 Saved";
    }
    private void writeNewUser1(string userid, string email)
    {
        User user = new User(userid, email);

        string json = JsonUtility.ToJson(user);

        reference.Child("HandData").SetRawJsonValueAsync(json);
        FirebaseLogin.text = "Data2 Saved";
    }

    public void ReadDataButton()
    {
        ReadUserInfos();
    }

    public void ReadUserInfos()
    {
        // 특정 데이터셋의 DB 참조 얻기
        reference = FirebaseDatabase.DefaultInstance.GetReference("HandData");
        FirebaseLogin.text = "Ready" + reference;
        ArrayList childs = new ArrayList();

        List<bool> test = new List<bool>();

        List<string> tmp1 = new List<string>();

        string tmp = null;

        string tmp2 = null;

        string test_s;

        StartCoroutine(LoadUserData());

        //reference.GetValueAsync().ContinueWith( task =>
        //    {
        //        Debug.Log("inFunc");

        //        if (task.IsFaulted)
        //        {
        //            // Handle the error...
        //        }
        //        else if (task.IsCompleted)
        //        {
        //            DataSnapshot snapshot = task.Result;

        //            tmp = snapshot.Child("DR").Value.ToString();

        //            tmp2 = tmp == null ? "yes" : "no";

        //            Debug.Log("tmp inFunc : " + tmp);
        //            Debug.Log("tmp2 inFunc : " + tmp2);
        //        }
        //    });


        //Debug.Log("tmp outFunc : " + tmp);
        //Debug.Log("tmp2 outFunc : " + tmp2);

        //FirebaseLogin.text = isin ? "true" : "false";




        //if (GameManager.instance.hasHand.Count > 0)
        //{
        //    //DistributePrefab.instance.SetData();
        //    GoogleLoginSatus.text = "handdataLoaded";
        //}
        //else
        //{
        //    for (int i = 0; i < 57; i++)
        //    {
        //        GameManager.instance.hasHand.Add(false);
        //    }
        //    GoogleLoginSatus.text = "nodata_thisID";
        //}
    }

    IEnumerator LoadUserData()
    {
        FirebaseLogin.text = "LoadUserData() begin";
        var DBTask = FirebaseDatabase.DefaultInstance.GetReference("HandData").GetValueAsync();
        string DR = null;
        string DS = null;

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if(DBTask.Exception != null)
        {
            Debug.Log($"Faild to load task with {DBTask.Exception}");
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;

            int count = 0;

            foreach(var item in snapshot.Children)
            {
                string tmp = item.Value.ToString();

                GameManager.instance.hasHand.Add(tmp);
                count++;
            }
        }

        GameManager.instance.print();

        FirebaseLogin.text = "LoadUserData() End";
    }

}
