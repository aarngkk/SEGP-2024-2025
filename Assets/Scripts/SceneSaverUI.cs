using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;


public class SceneSaverUI : MonoBehaviour
{
    // Reference to the SceneSaver component
    //public SceneSaver sceneSaver;

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

    // Reference to the Save button (if using for enabling/disabling)
    public Button saveButton;

    // Flag to track if the scene has been saved
    public bool sceneIsSaved = false;
    public UndoRedoManager undoRedoManager;
    private CutsceneManager cutsceneManager;

    /// <summary>
    /// Called when the main Save button is pressed: opens the save pop-up panel.
    /// </summary>

    private void Start()
    {
        cutsceneManager = FindObjectOfType<CutsceneManager>();
    }

    public void OpenSavePanel()
    {
        //pauses cutscene
        if (cutsceneManager != null && cutsceneManager.currentCutscene != null &&
            cutsceneManager.currentCutscene.state == PlayState.Playing)
        {
            cutsceneManager.currentCutscene.Pause();
            Debug.Log("Cutscene paused on save panel open.");
        }
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

        if (cutsceneManager != null && cutsceneManager.currentCutscene != null &&
            cutsceneManager.currentCutscene.state == PlayState.Paused)
        {
            cutsceneManager.currentCutscene.Resume();
            Debug.Log("Cutscene resumed after closing save panel.");
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
    /// Also, it will save the current positions of all draggable characters.
    /// </summary>
    public void OnSaveButtonClicked()
    {
        // Prevent saving if the tutorial is active.
        if (TutorialManager.tutorialActive)
        {
            if (errorMessageText != null)
                errorMessageText.text = "Finish the tutorial before saving!";
            return;
        }

        // Retrieve the scene name from the input field
        string sceneName = sceneNameInputField.text;

        // Check if the input is empty
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning("Scene name is empty. Please enter a scene name.");
            if (errorMessageText != null)
            {
                errorMessageText.text = "Please enter a scene name before saving.";
            }
            return;
        }

        // Retrieve the selected script from the ScriptSelectionManager
        
        // Build the full path
        string filePath = System.IO.Path.Combine(Application.persistentDataPath + "/SavedScenes/", sceneName + ".json");

        // Check if file already exists and we haven't confirmed overwriting
        if (System.IO.File.Exists(filePath))
        {
            // Warn the user: same scene name already exists
            if (errorMessageText != null)
            {
                errorMessageText.text = "A scene with this name already exists.\n"
                + "Please enter a different name.";
            }
            Debug.Log("Save aborted: file already exists. User must rename.");

            return; // Stop here; user must press Save again to confirm
        }

        // Ensure the directory exists
        string directoryPath = System.IO.Path.Combine(Application.persistentDataPath, "SavedScenes");
        if (!System.IO.Directory.Exists(directoryPath))
        {
            System.IO.Directory.CreateDirectory(directoryPath);
        }

        CutsceneManager cutsceneManager = FindObjectOfType<CutsceneManager>();
        if (cutsceneManager != null)
        {
            cutsceneManager.logFileName = sceneName; // Set filename
            cutsceneManager.LogChoicesToFile();      // Save choices
        }
        else
        {
            Debug.LogError("CutsceneManager not found! Choices were not logged.");
        }

        // Clear Undo/Redo after saving
        if (undoRedoManager != null)
        {
            undoRedoManager.ClearHistory();
        }

        // Save the scene with both the scene name, the selected script, and the positions of draggable characters.
        //sceneSaver.SaveScene(sceneName, selectedScript);
        sceneIsSaved = true;  // Mark that the scene has been saved
        CloseSavePanel();
        SceneManager.LoadScene("Main Menu");
    }

    public void OnBackButtonPressed()
    {
        if (scriptSelectionManager != null && !string.IsNullOrEmpty(scriptSelectionManager.SelectedScript))
        {
            if (string.IsNullOrEmpty(sceneNameInputField.text) || !sceneIsSaved)
            {
                if (savePanel != null && !savePanel.activeSelf)
                {
                    OpenSavePanel();
                    if (errorMessageText != null)
                    {
                        errorMessageText.text = "Please enter a scene name before exit.";
                    }
                }
                return; // Prevent navigating away until the scene is saved
            }
        }
        
        Debug.Log("Conditions met. Loading Main Menu scene.");
        SceneManager.LoadScene("Main Menu");
    }

    public void OnCancelButtonClicked()
    {
        CloseSavePanel();
    }

    /// <summary>
    /// Called when the Discard All button is clicked.
    /// This will clear the selected script, reset the scene save state,
    /// and move all draggable characters back to their original positions.
    /// </summary>
    public void OnDiscardAllButtonClicked()
    {
       
        // Reset the scene name input and the saved flag
        sceneNameInputField.text = "";
        sceneIsSaved = false;

        // Optionally, clear any error messages
        if (errorMessageText != null)
        {
            errorMessageText.text = "All selections have been discarded.";
        }

        // Reset all draggable characters to their original positions
        DragCharacter[] draggableCharacters = FindObjectsOfType<DragCharacter>();
        foreach (DragCharacter character in draggableCharacters)
        {
            character.ResetPosition();
        }

        // Optionally, close the save panel if it's open
        if (savePanel != null && savePanel.activeSelf)
        {
            CloseSavePanel();
        }

        if (saveButton != null)
        {
            saveButton.interactable = false;
        }
        if (cutsceneManager != null && cutsceneManager.currentCutscene != null)
        {
            cutsceneManager.currentCutscene.Stop();
            Debug.Log("Cutscene stopped after discarding all.");
        }
        SceneManager.LoadScene("New Scene");
    }
}
