using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;


public class SceneSaverUI : MonoBehaviour
{
    public ScriptSelectionManager scriptSelectionManager;
    private CutsceneManager cutsceneManager;
    public TMP_InputField sceneNameInputField;
    public GameObject savePanel;
    public GameObject leavePanel;
    public GameObject overwriteConfirmPanel;
    public TextMeshProUGUI errorMessageText;
    public List<Button> mainScreenButtons;
    public Button saveButton;
    public Button undoButton;
    public bool sceneIsSaved = false;
    private bool wasUndoInteractable = false;
    private string pendingOverwriteSceneName = "";

    private void Start()
    {
        cutsceneManager = FindObjectOfType<CutsceneManager>();
    }

    public void OpenSavePanel()
    {
        if (undoButton != null)
        {
            wasUndoInteractable = undoButton.interactable;
        }
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
        if (undoButton != null) undoButton.interactable = wasUndoInteractable;
    }

    /// <summary>
    /// Called when the Save button in the pop-up panel is clicked.
    /// Checks sceneName input, retrieves the selected script, and calls SaveScene.
    /// Also, it will save the current positions of all draggable characters.
    /// </summary>
     public void OnSaveButtonClicked()
    {

        string sceneName = sceneNameInputField.text;

        if (string.IsNullOrEmpty(sceneName))
        {
            if (errorMessageText != null)
                errorMessageText.text = "Please enter a scene name before saving.";
            return;
        }

        string filePath = System.IO.Path.Combine(Application.persistentDataPath + "/SavedScenes/", sceneName + ".log");

        if (System.IO.File.Exists(filePath))
        {
            pendingOverwriteSceneName = sceneName;
            if (overwriteConfirmPanel != null)
                overwriteConfirmPanel.SetActive(true);
            return;
        }

        SaveScene(sceneName);
    }
    public void OnConfirmOverwrite()
    {
        if (!string.IsNullOrEmpty(pendingOverwriteSceneName))
        {
            SaveScene(pendingOverwriteSceneName);
            pendingOverwriteSceneName = "";
        }

        if (overwriteConfirmPanel != null)
            overwriteConfirmPanel.SetActive(false);
    }

    public void OnCancelOverwrite()
    {
        pendingOverwriteSceneName = "";
        if (overwriteConfirmPanel != null)
            overwriteConfirmPanel.SetActive(false);
    }

    private void SaveScene(string sceneName)
    {
        string directoryPath = System.IO.Path.Combine(Application.persistentDataPath, "SavedScenes");
        if (!System.IO.Directory.Exists(directoryPath))
        {
            System.IO.Directory.CreateDirectory(directoryPath);
        }

        if (cutsceneManager != null)
        {
            cutsceneManager.logFileName = sceneName;
            cutsceneManager.LogChoicesToFile();
        }
        else
        {
            Debug.LogError("CutsceneManager not found! Choices were not logged.");
        }

        sceneIsSaved = true;
        CloseSavePanel();
        SceneManager.LoadScene("Main Menu");
    }
    /*
    public void OnSaveButtonClicked()
    {
        if (TutorialManager.tutorialActive)
        {
            if (errorMessageText != null)
                errorMessageText.text = "Finish the tutorial before saving!";
            return;
        }
        string sceneName = sceneNameInputField.text;

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

        

        // Save the scene with both the scene name, the selected script, and the positions of draggable characters.
        //sceneSaver.SaveScene(sceneName, selectedScript);
        sceneIsSaved = true;  // Mark that the scene has been saved
        CloseSavePanel();
        SceneManager.LoadScene("Main Menu");
    }*/

    public void OnBackButtonPressed()
    {
        if (saveButton != null) {
            if (saveButton.interactable == true)
            {
                if (string.IsNullOrEmpty(sceneNameInputField.text) || !sceneIsSaved)
                {
                    if (savePanel != null && !savePanel.activeSelf)
                    {
                        OpenLeavePanel();
                    }
                    return; // Prevent navigating away until the scene is saved
                }
            }
        }
        
        SceneManager.LoadScene("Main Menu");
    }

    public void OnLeaveButtonClicked()
    {
        SceneManager.LoadScene("Main Menu");
    }

    
    public void OpenLeavePanel()
    {
        if (undoButton != null)
        {
            wasUndoInteractable = undoButton.interactable;
        }
        //pauses cutscene
        if (cutsceneManager != null && cutsceneManager.currentCutscene != null &&
            cutsceneManager.currentCutscene.state == PlayState.Playing)
        {
            cutsceneManager.currentCutscene.Pause();
            Debug.Log("Cutscene paused on leave panel open.");
        }
        // Disable all main screen buttons
        if (mainScreenButtons != null)
        {
            foreach (Button btn in mainScreenButtons)
            {
                btn.interactable = false;
            }
        }

        if (leavePanel != null)
        {
            leavePanel.SetActive(true);
        }
    }

    /// <summary>
    /// Closes the save pop-up panel and re-enables main screen buttons.
    /// </summary>
    public void CloseLeavePanel()
    {
        if (leavePanel != null)
        {
            leavePanel.SetActive(false);
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
        if (undoButton != null)
        {
            undoButton.interactable = wasUndoInteractable;
        }
    }

    public void OnStayButtonClicked()
    {
        CloseLeavePanel();
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
