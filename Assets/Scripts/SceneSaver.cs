using System.IO;
using UnityEngine;

[System.Serializable]
public class SceneData
{
    public string sceneName;
    public string selectedScript;
    // Optionally, add additional data such as placeholder positions, etc.
}

public class SceneSaver : MonoBehaviour
{
    // Path to save files (adjust as necessary)
    private string SavePath => Application.persistentDataPath + "/SavedScenes/";

    private void Start()
    {
        // Ensure the directory exists
        if (!Directory.Exists(SavePath))
            Directory.CreateDirectory(SavePath);
    }

public void SaveScene(string sceneName, string selectedScript)
    {
        // Create a SceneData object and fill it with data
        SceneData data = new SceneData 
        {
            sceneName = sceneName,
            selectedScript = selectedScript
        };

        string json = JsonUtility.ToJson(data, true);

        // Use the scene name to create a unique file name
        string filePath = SavePath + sceneName + ".json";
        File.WriteAllText(filePath, json);

        Debug.Log("Scene saved to " + filePath);
    }

    public SceneData LoadScene(string sceneName)
    {
        string filePath = SavePath + sceneName + ".json";
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            SceneData data = JsonUtility.FromJson<SceneData>(json);
            return data;
        }
        else
        {
            Debug.LogError("Scene not found: " + filePath);
            return null;
        }
    }

    // Optional: List all saved scenes
    public string[] GetAllSavedScenes()
    {
        if (!Directory.Exists(SavePath))
            return new string[0];

        string[] files = Directory.GetFiles(SavePath, "*.json");
        for (int i = 0; i < files.Length; i++)
        {
            // Extract file name without path or extension
            files[i] = Path.GetFileNameWithoutExtension(files[i]);
        }
        return files;
    }
}
