using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SceneSaverUI : MonoBehaviour
{
    // Reference to the SceneSaver component
    public SceneSaver sceneSaver;

    // Reference to the ScriptSelectionManager component (assign via Inspector)
    public ScriptSelectionManager scriptSelectionManager;

    // Reference to the TMP_InputField where the user types the scene name
    public TMP_InputField sceneNameInputField;

    // Pop-up panel for saving (the panel that appears when you click "Save")
    public GameObject savePanel;

    // TextMeshPro element for displaying error messages
    public TextMeshProUGUI errorMessageText;
    
    // List of main screen buttons to disable when the pop-up appears
    public List<Button> mainScreenButtons;

    // Flag to track if the scene has been saved
    public bool sceneIsSaved = false;

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
    /// Closes the save pop-up panel and re-enables main screen buttons.
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
    /// Checks sceneName input, retrieves the selected script, and calls SaveScene.
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

        // Retrieve the selected script from the ScriptSelectionManager
        string selectedScript = "";
        if (scriptSelectionManager != null)
        {
            selectedScript = scriptSelectionManager.SelectedScript;
        }
        else
        {
            Debug.LogWarning("ScriptSelectionManager is not assigned.");
        }

        // Save the scene with both the scene name and the selected script
        sceneSaver.SaveScene(sceneName, selectedScript);
        // Mark that the scene has been saved
        sceneIsSaved = true;
        // Optionally close the panel after saving
        CloseSavePanel();
        SceneManager.LoadScene("Main Menu");
    }

    /// <summary>
    /// Called when the Back button is pressed.
    /// If a script is selected but the scene is not saved (or scene name is empty), show the save panel.
    /// Otherwise, navigate back to the main menu.
    /// </summary>
    public void OnBackButtonPressed()
    {
        // If a script is selected...
        if (scriptSelectionManager != null && !string.IsNullOrEmpty(scriptSelectionManager.SelectedScript))
        {
            // ...and either the scene name is empty or the scene hasn't been saved...
            if (string.IsNullOrEmpty(sceneNameInputField.text) || !sceneIsSaved)
            {
                if (savePanel != null && !savePanel.activeSelf)
                {
                    OpenSavePanel();
                }
                return; // Prevent navigating away until the scene is saved
            }
        }
        
        Debug.Log("Conditions met. Loading MainMenu scene.");
        // Otherwise, allow navigating back to the main menu
        SceneManager.LoadScene("Main Menu");
    }

    /// <summary>
    /// Called when the Cancel button in the pop-up panel is clicked.
    /// Closes the panel without saving.
    /// </summary>
    public void OnCancelButtonClicked()
    {
        CloseSavePanel();
    }

    /// <summary>
    /// Called when the Discard All button is clicked.
    /// This will clear the selected script and reset the scene save state.
    /// </summary>
    public void OnDiscardAllButtonClicked()
    {
        // Clear the script selection via the ScriptSelectionManager
        if (scriptSelectionManager != null)
        {
            scriptSelectionManager.ClearSelection();
        }

        // Reset the scene name input and the saved flag
        sceneNameInputField.text = "";
        sceneIsSaved = false;

        // Optionally, also clear any error messages
        if (errorMessageText != null)
        {
            errorMessageText.text = "All selections have been discarded.";
        }

        // Optionally, close the save panel if it's open
        if (savePanel != null && savePanel.activeSelf)
        {
            CloseSavePanel();
        }
    }
}
