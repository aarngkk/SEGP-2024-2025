using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;  // Add this at the top

public class ScriptSelectionManager : MonoBehaviour
{
    // UI Elements assigned via the Inspector:
    public GameObject scriptPanel;                  // The pop-up panel to show/hide
    public TextMeshProUGUI scriptDetailText;          // Displays the current script detail
    public Button nextButton;                         // Button to go to the next script
    public Button prevButton;                         // Button to go to the previous script
    public Button selectButton;                       // Button to select the current script
    public Button backButton;                         // Button to close the panel
    public List<Button> otherButtons;                 // Other buttons to disable/enable
    public ScrollRect scrollRect;                     // Scroll area for the script text
    // public TextMeshProUGUI selectedScriptTitleText;   // Displays the selected script's title

    // Reference to the Save button (to enable/disable based on selection)
    public Button saveButton;
    // This button's label will update to "Script" if nothing is selected,
    // or to the selected script's title when a script is selected.
    public Button scriptButton;
    // List to store the full text details loaded from files
    private List<string> scripts = new List<string>();

    private int currentIndex = 0;

    // The currently selected script (read-only property)
    public string SelectedScript { get; private set; }

    private void Start()
    {
        LoadScripts();                // Load the script texts from Resources
        UpdateScriptDetail();         // Display the first script detail
        OpenScriptSelection();        // Open the script selection panel immediately
        backButton.onClick.AddListener(CloseScriptSelection);

        // Initially, if no script is selected, disable the save button.
        if (saveButton != null)
        {
            saveButton.interactable = false;
        }
        UpdateScriptButtonLabel();
    }

    // Loads script details from text files in the Resources folder
    private void LoadScripts()
    {
        // Assume we have 8 script files. Adjust the count if needed.
        for (int i = 1; i <= 8; i++)
        {
            // Build the file name (without extension)
            string fileName = "ScriptDetail" + i;

            // Load the text asset from Resources
            TextAsset textAsset = Resources.Load<TextAsset>(fileName);
            if (textAsset != null)
            {
                scripts.Add(textAsset.text);
            }
            else
            {
                Debug.LogWarning("Could not load " + fileName + ".txt from Resources!");
            }
        }

        Debug.Log("Loaded " + scripts.Count + " script details.");
    }

    // Opens the pop-up panel and disables all other buttons
    public void OpenScriptSelection()
    {
        scriptPanel.SetActive(true);
        foreach (Button btn in otherButtons)
        {
            btn.interactable = false;
        }
    }

    // Closes the pop-up panel, re-enables other buttons, and updates the title display
    public void CloseScriptSelection()
    {
        scriptPanel.SetActive(false);
        foreach (Button btn in otherButtons)
        {
            btn.interactable = true;
        }
        // Update the title display if a script was selected
        // if (!string.IsNullOrEmpty(SelectedScript) && selectedScriptTitleText != null)
        // {
        //     string[] lines = SelectedScript.Split('\n');
        //     selectedScriptTitleText.text = lines.Length > 0 ? lines[0] : "Selected Script";
        // }
        if (saveButton != null)
        {
            saveButton.interactable = false;
        }

    }

    // Advances to the next script in the list
    public void ShowNextScript()
    {
        if (scripts.Count == 0) return;

        currentIndex = (currentIndex + 1) % scripts.Count;
        UpdateScriptDetail();
    }

    // Moves to the previous script in the list
    public void ShowPreviousScript()
    {
        if (scripts.Count == 0) return;

        currentIndex = (currentIndex - 1 + scripts.Count) % scripts.Count;
        UpdateScriptDetail();
    }

    // Selects the current script and logs it
    public void SelectCurrentScript()
    {
        if (scripts.Count == 0) return;

        SelectedScript = scripts[currentIndex];
        Debug.Log("Selected Script: " + SelectedScript);
        CloseScriptSelection();

        // If a script is now selected, enable the save button.
        if (saveButton != null)
        {
            saveButton.interactable = true;
        }
        
        UpdateScriptButtonLabel();
    }

    // Updates the text element with the current script detail and resets the scroll position
    private void UpdateScriptDetail()
    {
        if (scriptDetailText == null)
        {
            Debug.LogError("scriptDetailText is not assigned in the Inspector!");
            return;
        }

        if (scripts.Count > 0)
        {
            scriptDetailText.text = scripts[currentIndex];
            if (scrollRect != null)
            {
                scrollRect.verticalNormalizedPosition = 1f;
            }
        }
        else
        {
            scriptDetailText.text = "No script details loaded.";
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && scriptPanel.activeSelf)
        {
            CloseScriptSelection();
        }
    }

    /// <summary>
    /// Clears the current script selection.
    /// Call this method when the user clicks the "Discard All" button.
    /// </summary>
    public void ClearSelection()
    {
        SelectedScript = "";
        // if (selectedScriptTitleText != null)
        // {
        //     selectedScriptTitleText.text = "";
        // }
        // Debug.Log("Script selection cleared.");

        // Disable the save button since no script is selected now.
        if (saveButton != null)
        {
            saveButton.interactable = false;
        }
        UpdateScriptButtonLabel();
        Debug.Log("Script selection cleared.");
    }
    /// <summary>
    /// Updates the text on the script selection button.
    /// If no script is selected, it shows the default text ("Script").
    /// Otherwise, it displays the first line of the selected script.
    /// </summary>
    private void UpdateScriptButtonLabel()
{
    if (scriptButton != null)
    {
        TextMeshProUGUI buttonText = scriptButton.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null)
        {
            if (string.IsNullOrEmpty(SelectedScript))
            {
                buttonText.text = "SCRIPT";
            }
            else
            {
                // Split the selected script into lines
                string[] lines = SelectedScript.Split('\n');

                // Find the first non-empty, trimmed line
                string firstLine = "";
                foreach (string line in lines)
                {
                    if (!string.IsNullOrEmpty(line.Trim()))
                    {
                        firstLine = line.Trim();
                        break;
                    }
                }

                // Update button text with the first non-empty line or fallback text
                buttonText.text = !string.IsNullOrEmpty(firstLine) ? firstLine : "Script";
            }
        }
    }
}

}
