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

>
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

 
    public void OnDiscardAllButtonClicked()
    {
        SceneManager.LoadScene("New Scene");
    }
}
