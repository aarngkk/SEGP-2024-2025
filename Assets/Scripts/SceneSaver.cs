using System.IO;
using UnityEngine;

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

    public void SaveScene(string sceneName)
    {
        SceneData data = new SceneData { sceneName = sceneName };
        string json = JsonUtility.ToJson(data, true);

        // Create a unique file name or simply use the scene name
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

    // You could also add a method to list all saved scenes:
    public string[] GetAllSavedScenes()
    {
        if (!Directory.Exists(SavePath))
            return new string[0];

        string[] files = Directory.GetFiles(SavePath, "*.json");
        for (int i = 0; i < files.Length; i++)
        {
            // Extract the file name without the path and extension
            files[i] = Path.GetFileNameWithoutExtension(files[i]);
        }
        return files;
    }
}
