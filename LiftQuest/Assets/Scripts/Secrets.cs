using System.IO;
using UnityEngine;

[System.Serializable]
public class Secrets
{
    public string serverIP;
    public int serverPort;

    public static Secrets Load()
    {
        string path = Path.Combine(Application.dataPath, "Scripts/secrets.json");
        if (!File.Exists(path))
        {
            Debug.LogError("Secrets file not found: " + path);
            return null;
        }

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<Secrets>(json);
    }
}
