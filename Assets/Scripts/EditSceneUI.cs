using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EditSceneUI : MonoBehaviour
{
    public SceneSaver sceneSaver;
    public Button saveButton;
    public TMP_InputField scriptInput;

    void Start()
    {
        saveButton.onClick.AddListener(SaveCurrentScene);
    }

    private void SaveCurrentScene()
    {
        string selectedScript = scriptInput.text; // Get script input text
        
        // Check if the scene was loaded
        if (!string.IsNullOrEmpty(SceneDataTransfer.Instance.SceneName))
        {
            // Save to the same file name
            sceneSaver.SaveScene(SceneDataTransfer.Instance.SceneName, selectedScript);
            Debug.Log("Edited scene saved as: " + SceneDataTransfer.Instance.SceneName);
        }
        else
        {
            Debug.LogError("No scene is currently loaded. Please provide a scene name.");
        }
    }
}
