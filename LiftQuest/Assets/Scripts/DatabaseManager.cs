using UnityEngine;
using Firebase.Database;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Collections;
using System.Xml.Serialization;
using Firebase;
using Firebase.Extensions;
using System.Collections.Generic;
using UnityEngine.SceneManagement;



public class DatabaseManager: MonoBehaviour
{
    public InputField username;
    public InputField password;
    public InputField password2;
    public InputField resting_measurement;
    public InputField above_head_measurement;
    public InputField floor_measurement;
    public InputField stage_completed;
    public InputField login_username;
    public InputField login_password;

    public Text username_text;
    public Text password_text;
    public Text resting_text;
    public Text above_head_text;
    public Text floor_text;
    public Text stage_text;

    private string profileID;
    private DatabaseReference dbReference;
    private string databaseURL = "https://liftquestprofiles-default-rtdb.firebaseio.com/";
    public GameObject UsernameError;
    public GameObject UsernameSuccess;
    
    // START, REGISTER, LOGIN / LOGOUT
    
    void Start()
    {
        
        //profileID = SystemInfo.deviceUniqueIdentifier;
        profileID = PlayerPrefs.GetString("ProfileID", "");

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;
                FirebaseDatabase.GetInstance(app, databaseURL);
                dbReference = FirebaseDatabase.DefaultInstance.RootReference;
                Debug.Log("Firebase initialized successfully!");
            }
            else
            {
                Debug.LogError("Failed to initialize Firebase: " + task.Result);
            }
        });
    }

    public void CreateProfile()
    {

        
        if (dbReference == null)
        {
            Debug.LogError("Database reference is null. Ensure Firebase is initialized.");
            return;
        }

        if (string.IsNullOrEmpty(username.text))
        {
            Debug.LogError("Username cannot be empty.");
            return;
        }

        if(password.text != password2.text)
        {
            Debug.LogError("Passwords must match.");
            return;
        }

        /*string profileKey = dbReference.Child("profiles").Push().Key;
        profileID = profileKey;
        PlayerPrefs.SetString("ProfileID", profileID);
        PlayerPrefs.Save();

        Profile newProfile = new Profile(username.text, password.text, 0, 0, 0, 0);
        string json = JsonUtility.ToJson(newProfile);

        dbReference.Child("profiles").Child(profileKey).SetRawJsonValueAsync(json)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    Debug.LogError("Failed to create profile: " + task.Exception);
                }
                else
                {
                    Debug.Log("Profile created successfully!");
                }
            });*/
        // Check if the username already exists
        dbReference.Child("profiles")
            .OrderByChild("username")
            .EqualTo(username.text)
            .GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    Debug.LogError("Error checking for existing username: " + task.Exception);
                    return;
                }

                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    Debug.LogError("Username already exists. Choose a different username.");
                    UsernameError.SetActive(true);
                    return;
                }

                // Username does not exist — proceed with profile creation
                string profileKey = dbReference.Child("profiles").Push().Key;
                profileID = profileKey;
                PlayerPrefs.SetString("ProfileID", profileID);
                PlayerPrefs.Save();

                Profile newProfile = new Profile(username.text, password.text, 0, 0, 0, 0);
                string json = JsonUtility.ToJson(newProfile);

                dbReference.Child("profiles").Child(profileKey).SetRawJsonValueAsync(json)
                    .ContinueWithOnMainThread(writeTask =>
                    {
                        if (writeTask.IsFaulted || writeTask.IsCanceled)
                        {
                            Debug.LogError("Failed to create profile: " + writeTask.Exception);
                        }
                        else
                        {
                            Debug.Log("Profile created successfully!");
                            UsernameSuccess.SetActive(true);
                        }
                    });
            });
    
    }
    
    public void LoginUser()
    {
        StartCoroutine(LoginRoutine());
    }

    private IEnumerator LoginRoutine()
    {
        if (dbReference == null)
        {
            Debug.LogError("Database reference is null. Ensure Firebase is initialized.");
            yield break;
        }

        if (string.IsNullOrEmpty(login_username.text) || string.IsNullOrEmpty(login_password.text))
        {
            Debug.LogWarning("Username or password field is empty.");
            yield break;
        }

        var usersTask = dbReference.Child("profiles").GetValueAsync();
        yield return new WaitUntil(() => usersTask.IsCompleted);

        if (usersTask.Exception != null)
        {
            Debug.LogError("Database error: " + usersTask.Exception);
            yield break;
        }

        DataSnapshot snapshot = usersTask.Result;
        bool loginSuccess = false;
        string matchedProfileID = null;

        foreach (var user in snapshot.Children)
        {
            string dbUsername = user.Child("username").Value?.ToString();
            //Debug.Log("Username: " + dbUsername);
            string dbPassword = user.Child("password").Value?.ToString();
            //Debug.Log("Password: " + dbPassword);

            if (dbUsername == login_username.text && dbPassword == login_password.text)
            {
                loginSuccess = true;
                matchedProfileID = user.Key;
                break;
            }
        }

        if (loginSuccess)
        {
            profileID = matchedProfileID;
            PlayerPrefs.SetString("ProfileID", profileID);
            PlayerPrefs.Save();

            Debug.Log("Login successful! Profile ID: " + profileID);
        }
        else
        {
            Debug.LogWarning("Login failed: Invalid credentials.");
        }

        SceneManager.LoadScene("LevelSelection");

    }

    public void Logout()
    {
        PlayerPrefs.DeleteKey("ProfileID");
        PlayerPrefs.Save();
        profileID = "";
        Debug.Log("Logged out. Cleared stored ProfileID.");
    }

    // GETTERS
    
    public void DisplayUsername()
    {
        StartCoroutine(GetUsername((string username) =>
        {
            username_text.text = username;
        }));
    }
    
    public IEnumerator GetUsername(System.Action<string>onCallback)
    {
        var username_data = dbReference.Child("profiles").Child(profileID).Child("username").GetValueAsync();

        yield return new WaitUntil(predicate: () => username_data.IsCompleted);

        if(username_data != null)
        {
            DataSnapshot snapshot = username_data.Result;

            onCallback.Invoke(snapshot.Value.ToString());
        }
        Debug.Log("GetUsername called.");
    }

    public void DisplayPassword()
    {
        StartCoroutine(GetPassword((string password) =>
        {
            password_text.text = password;
        }));
    }
    
    public IEnumerator GetPassword(System.Action<string>onCallback)
    {
        var password_data = dbReference.Child("profiles").Child(profileID).Child("password").GetValueAsync();

        yield return new WaitUntil(predicate: () => password_data.IsCompleted);

        if(password_data != null)
        {
            DataSnapshot snapshot = password_data.Result;

            onCallback.Invoke(snapshot.Value.ToString());
        }
        Debug.Log("GetPassword called.");
    }

    public void DisplayRestingMeasurement()
    {
        StartCoroutine(GetResting((double resting_measurement) =>
        {
            resting_text.text = resting_measurement.ToString();
        }));
    }

    public IEnumerator GetResting(System.Action<double>onCallback)
    {
        var resting_data = dbReference.Child("profiles").Child(profileID).Child("resting_measurement").GetValueAsync();

        yield return new WaitUntil(predicate: () => resting_data.IsCompleted);

        if(resting_data != null)
        {
            DataSnapshot snapshot = resting_data.Result;

            onCallback.Invoke(double.Parse(snapshot.Value.ToString()));
        }
        Debug.Log("GetResting called.");
    }

    public void DisplayAboveHeadMeasurement()
    {
        StartCoroutine(GetResting((double above_head_measurement) =>
        {
            above_head_text.text = above_head_measurement.ToString();
        }));
    }
    
    public IEnumerator GetAboveHead(System.Action<double>onCallback)
    {
        var above_head_data = dbReference.Child("profiles").Child(profileID).Child("above_head_measurement").GetValueAsync();

        yield return new WaitUntil(predicate: () => above_head_data.IsCompleted);

        if(above_head_data != null)
        {
            DataSnapshot snapshot = above_head_data.Result;

            onCallback.Invoke(double.Parse(snapshot.Value.ToString()));
        }
        Debug.Log("GetAboveHead called.");
    }

    public void DisplayFloorMeasurement()
    {
        StartCoroutine(GetFloor((double floor_measurement) =>
        {
            floor_text.text = floor_measurement.ToString();
        }));
    }
    
    public IEnumerator GetFloor(System.Action<double>onCallback)
    {
        var floor_data = dbReference.Child("profiles").Child(profileID).Child("floor_measurement").GetValueAsync();

        yield return new WaitUntil(predicate: () => floor_data.IsCompleted);

        if(floor_data != null)
        {
            DataSnapshot snapshot = floor_data.Result;

            onCallback.Invoke(double.Parse(snapshot.Value.ToString()));
        }
        Debug.Log("GetFloor called.");
    }

    public void DisplayStageCompleted()
    {
        StartCoroutine(GetStage((int stage_completed) =>
        {
            stage_text.text = stage_completed.ToString();
        }));
    }
    
    public IEnumerator GetStage(System.Action<int>onCallback)
    {
        var stage_data = dbReference.Child("profiles").Child(profileID).Child("stage_completed").GetValueAsync();

        yield return new WaitUntil(predicate: () => stage_data.IsCompleted);

        if(stage_data != null)
        {
            DataSnapshot snapshot = stage_data.Result;

            onCallback.Invoke(int.Parse(snapshot.Value.ToString()));
        }
        Debug.Log("GetStage called.");
    }

    public void GetProfileButton()
    {
        GetProfileInfo();
    }

    public void GetProfileInfo()
    {
        StartCoroutine(GetUsername((string username) => {
            username_text.text = "Username: " +username;
        }));

        StartCoroutine(GetPassword((string password) => {
            password_text.text = "Password: " + password;
        }));

        StartCoroutine(GetResting((double resting_measurement) => {
            resting_text.text = "Resting measurement: " + resting_measurement.ToString();
        }));

        StartCoroutine(GetAboveHead((double above_head_measurement) => {
            above_head_text.text = "Above head measurement: " + above_head_measurement.ToString();
        }));

        StartCoroutine(GetFloor((double floor_measurement) => {
            floor_text.text = "Floor measurement: " + floor_measurement.ToString();
        }));

        StartCoroutine(GetStage((int stage_completed) => {
            stage_text.text = "Stage completed: " + stage_completed.ToString();
        }));
        Debug.Log("GetProfileInfo called.");
    }

    // SETTERS
    
    public void UpdateUsername()
    {
        dbReference.Child("profiles").Child("username").SetValueAsync(username.text);
        Debug.Log("Username updated.");
    }

    public void UpdatePassword()
    {
        if (string.IsNullOrEmpty(profileID))
        {
            Debug.LogError("Profile ID is not set. Cannot update.");
            return;
        }

        var passwordUpdate = new Dictionary<string, object>
        {
            { "password", password.text }
        };

        dbReference
            .Child("profiles")
            .Child(profileID) // ✅ Make sure this is the actual profile ID from login
            .UpdateChildrenAsync(passwordUpdate)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    Debug.LogError("Failed to update password: " + task.Exception);
                }
                else
                {
                    Debug.Log("Password updated successfully!");
                }
            });
    }

    public void UpdateResting()
    {
        if (string.IsNullOrEmpty(profileID))
        {
            Debug.LogError("Profile ID is not set. Cannot update.");
            return;
        }

        var restingUpdate = new Dictionary<string, object>
        {
            { "resting_measurement", resting_measurement.text }
        };

        dbReference
            .Child("profiles")
            .Child(profileID) // ✅ Make sure this is the actual profile ID from login
            .UpdateChildrenAsync(restingUpdate)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    Debug.LogError("Failed to update resting measurement: " + task.Exception);
                }
                else
                {
                    Debug.Log("Resting measurement updated successfully!");
                }
            });
    }

    public void UpdateAboveHead()
    {
        if (string.IsNullOrEmpty(profileID))
        {
            Debug.LogError("Profile ID is not set. Cannot update.");
            return;
        }

        var aboveHeadUpdate = new Dictionary<string, object>
        {
            { "above_head_measurement", above_head_measurement.text }
        };

        dbReference
            .Child("profiles")
            .Child(profileID) // ✅ Make sure this is the actual profile ID from login
            .UpdateChildrenAsync(aboveHeadUpdate)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    Debug.LogError("Failed to update above head measurement: " + task.Exception);
                }
                else
                {
                    Debug.Log("Above head measurement updated successfully!");
                }
            });
    }

     public void UpdateFloor()
    {
        if (string.IsNullOrEmpty(profileID))
        {
            Debug.LogError("Profile ID is not set. Cannot update.");
            return;
        }

        var floorUpdate = new Dictionary<string, object>
        {
            { "floor_measurement", floor_measurement.text }
        };

        dbReference
            .Child("profiles")
            .Child(profileID) // ✅ Make sure this is the actual profile ID from login
            .UpdateChildrenAsync(floorUpdate)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    Debug.LogError("Failed to update floor measurement: " + task.Exception);
                }
                else
                {
                    Debug.Log("Floor measurement updated successfully!");
                }
            });
    }

     public void UpdateStage()
    {
        if (string.IsNullOrEmpty(profileID))
        {
            Debug.LogError("Profile ID is not set. Cannot update.");
            return;
        }

        var stageUpdate = new Dictionary<string, object>
        {
            { "stage_completed", stage_completed.text }
        };

        dbReference
            .Child("profiles")
            .Child(profileID) // ✅ Make sure this is the actual profile ID from login
            .UpdateChildrenAsync(stageUpdate)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    Debug.LogError("Failed to update stage completed: " + task.Exception);
                }
                else
                {
                    Debug.Log("Stage completed updated successfully!");
                }
            });
    }

}
