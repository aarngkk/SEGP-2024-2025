using System.IO;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterData
{
    public string characterName;  // Use the GameObject's name or a unique ID
    public Vector3 position;
    public Quaternion rotation;
}

[System.Serializable]
public class SceneData
{
    public string sceneName;
    public string selectedScript;
    public List<CharacterData> characters;  // List of character positions/rotations
}

public class SceneSaver : MonoBehaviour
{
    // Path to save files (adjust as necessary)
    private string SavePath => Application.persistentDataPath + "/SavedScenes/";

    public GameObject[] characterPrefabs;
    private void Start()
    {
        // Ensure the directory exists
        if (!Directory.Exists(SavePath))
            Directory.CreateDirectory(SavePath);
        if (PlayerPrefs.HasKey("LastLoadedScene"))
        {
            string sceneName = PlayerPrefs.GetString("LastLoadedScene");
            PlayerPrefs.DeleteKey("LastLoadedScene"); // Remove after loading
            RestoreSavedScene(sceneName);
        }
    }
    public void RestoreSavedScene(string sceneName)
    {
        SceneData data = LoadScene(sceneName);
        if (data == null) return;

        foreach (CharacterData charData in data.characters)
        {
            // Find the corresponding prefab
            GameObject prefab = FindPrefabByName(charData.characterName);
            if (prefab != null)
            {
                // Spawn character at saved position and rotation
                GameObject newCharacter = Instantiate(prefab, charData.position, charData.rotation);
                newCharacter.name = charData.characterName; // Keep the original name
            }
            else
            {
                Debug.LogError("Prefab not found for: " + charData.characterName);
            }
        }
    }
     public GameObject FindPrefabByName(string characterName)
    {
        foreach (GameObject prefab in characterPrefabs)
        {
            if (prefab.name == characterName)
            {
                return prefab;
            }
        }
        return null;
    }

    public void SaveScene(string sceneName, string selectedScript)
    {
        // Create a SceneData object and fill it with basic data
        SceneData data = new SceneData
        {
            sceneName = sceneName,
            selectedScript = selectedScript,
            characters = new List<CharacterData>()
        };

        // Find all draggable characters in the scene
        DragCharacter[] draggableCharacters = FindObjectsOfType<DragCharacter>();
        foreach (DragCharacter character in draggableCharacters)
        {
            CharacterData charData = new CharacterData
            {
                // Using the GameObject's name as an identifier
                characterName = character.gameObject.name,
                position = character.transform.position,
                rotation = character.transform.rotation
            };

            data.characters.Add(charData);
        }

        // Convert the scene data to JSON
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
