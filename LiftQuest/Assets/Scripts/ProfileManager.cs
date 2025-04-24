using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class CalibrationData
{
    public string profileName;
    public Vector3 restPosition;
    public Vector3 curlPosition;

    public CalibrationData(string profileName, Vector3 restPosition, Vector3 curlPosition)
    {
        this.profileName = profileName;
        this.restPosition = restPosition;
        this.curlPosition = curlPosition;
    }
}

public class ProfileManager : MonoBehaviour
{
    private const string ProfilesFolder = "Profiles";
    private string profilesPath;

    public List<string> availableProfiles = new List<string>();
    public CalibrationData currentProfile;

    void Awake()
    {
        profilesPath = Path.Combine(Application.persistentDataPath, ProfilesFolder);
        if (!Directory.Exists(profilesPath))
        {
            Directory.CreateDirectory(profilesPath);
        }

        LoadAvailableProfiles();
        
        Debug.Log("Profiles directory: " + profilesPath);
    }

    public void LoadAvailableProfiles()
    {
        availableProfiles.Clear();
        string[] files = Directory.GetFiles(profilesPath, "*.json");

        foreach (string file in files)
        {
            string profileName = Path.GetFileNameWithoutExtension(file);
            availableProfiles.Add(profileName);
        }
        
        Debug.Log($"Found {availableProfiles.Count} profiles");
    }

    public void SaveProfile(CalibrationData profile)
    {
        string filePath = Path.Combine(profilesPath, profile.profileName + ".json");
        string json = JsonUtility.ToJson(profile);
        File.WriteAllText(filePath, json);
        Debug.Log($"Profile saved: {profile.profileName} at {filePath}");
        
        LoadAvailableProfiles();
    }

    public bool LoadProfile(string profileName)
    {
        string filePath = Path.Combine(profilesPath, profileName + ".json");

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            currentProfile = JsonUtility.FromJson<CalibrationData>(json);
            Debug.Log($"Profile loaded: {profileName}");
            return true;
        }

        Debug.LogError($"Profile not found: {profileName}");
        return false;
    }

    public void DeleteProfile(string profileName)
    {
        string filePath = Path.Combine(profilesPath, profileName + ".json");

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log($"Profile deleted: {profileName}");
            LoadAvailableProfiles();
        }
        else
        {
            Debug.LogError($"Profile not found: {profileName}");
        }
    }

    public bool ProfileExists(string profileName)
    {
        string filePath = Path.Combine(profilesPath, profileName + ".json");
        return File.Exists(filePath);
    }
}

