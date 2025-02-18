using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class SceneSaverUI : MonoBehaviour
{
    // Reference to the SceneSaver component
    public SceneSaver sceneSaver;

    // Reference to the TMP_InputField where the user types the scene name
    public TMP_InputField sceneNameInputField;

    // Pop-up panel for saving (the panel that appears when you click "Save")
    public GameObject savePanel;

    // TextMeshPro element for displaying error messages
    public TextMeshProUGUI errorMessageText;
    public List<Button> mainScreenButtons;

    /// <summary>
    /// Called when the main Save button is pressed: opens the save pop-up panel.
    /// </summary>
    public void OpenSavePanel()
    {
        // Disable all main screen buttons
        if (mainScreenButtons != null)
        {
            foreach (Button btn in mainScreenButtons)
            {
                btn.interactable = false;
            }
        }

        if (savePanel != null)
        {
            // Clear any previous error message and input text
            if (errorMessageText != null)
                errorMessageText.text = "";
            sceneNameInputField.text = "";

            savePanel.SetActive(true);
        }
    }
    /// <summary>
    /// Closes the save pop-up panel. Called by a Cancel button or after saving.
    /// </summary>
    public void CloseSavePanel()
    {
        if (savePanel != null)
        {
            savePanel.SetActive(false);
        }
        
        // Re-enable all main screen buttons
        if (mainScreenButtons != null)
        {
            foreach (Button btn in mainScreenButtons)
            {
                btn.interactable = true;
            }
        }
    }

    /// <summary>
    /// Called when the Save button in the pop-up panel is clicked.
    /// Checks sceneName input and calls SaveScene.
    /// </summary>
    public void OnSaveButtonClicked()
    {
        // Retrieve the scene name from the input field
        string sceneName = sceneNameInputField.text;

        // Check if the input is empty
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning("Scene name is empty. Please enter a scene name.");
            // Display warning in the errorMessageText
            if (errorMessageText != null)
            {
                errorMessageText.text = "Please enter a scene name before saving.";
            }
            return;
        }
        FindObjectOfType<DragCharacter>().SaveScene(sceneName);

        // If you have a ScriptSelectionManager, you could get the selected script:
        // string selectedScript = yourScriptSelectionManager.SelectedScript;

        // Save the scene
        //sceneSaver.SaveScene(sceneName);

        // Optionally close the panel
        CloseSavePanel();
    }

    /// <summary>
    /// Called when the Cancel button in the pop-up panel is clicked.
    /// Closes the panel without saving.
    /// </summary>
    public void OnCancelButtonClicked()
    {
        CloseSavePanel();
    }
}
