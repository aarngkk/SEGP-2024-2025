using UnityEngine;
using UnityEngine.UI;

public class SceneSaverUI : MonoBehaviour
{
    // Reference to the SceneSaver component
    public SceneSaver sceneSaver;

    // Reference to the InputField where the user types the scene name
    public InputField sceneNameInputField;

    /// <summary>
    /// This method will be called by the Save button.
    /// It reads the scene name from the input field and calls SaveScene.
    /// </summary>
    public void OnSaveButtonClicked()
    {
        // Retrieve the scene name from the input field
        string sceneName = sceneNameInputField.text;

        // Optional: check if the input is not empty
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning("Scene name is empty. Please enter a scene name.");
            return;
        }

        // Call the SaveScene method from your SceneSaver script
        sceneSaver.SaveScene(sceneName);
    }
}
