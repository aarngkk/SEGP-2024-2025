using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Collections.Generic;

public class SaveFileList : MonoBehaviour
{
    public Transform fileListContent; // Parent object for the list of saved files
    public GameObject fileListItemPrefab; // Prefab for each file list item
    //public TextMeshProUGUI errorMessageText; // Text to display errors (e.g., "No saved files found")

    private string savePath => Application.persistentDataPath + "/SavedScenes/"; // Path to saved scenes

    private void Start()
    {
        // Ensure the save directory exists
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        // Populate the list of saved files when the script starts
        PopulateFileList();
    }

    /// <summary>
    /// Populates the UI with a list of saved files.
    /// </summary>
    private void PopulateFileList()
    {
        // Clear the existing list
        foreach (Transform child in fileListContent)
        {
            Destroy(child.gameObject);
        }

        // Get all saved scene files
        string[] savedFiles = Directory.GetFiles(savePath, "*.json");

        /*if (savedFiles.Length == 0)
        {
            errorMessageText.text = "No saved files found.";
            return;
        }*/

        // Populate the list with saved files
        foreach (string filePath in savedFiles)
        {
            // Extract the file name (without extension)
            string fileName = Path.GetFileNameWithoutExtension(filePath);

            // Instantiate a new list item
            GameObject listItem = Instantiate(fileListItemPrefab, fileListContent);
            listItem.GetComponentInChildren<TextMeshProUGUI>().text = fileName;
        }
    }
}