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
    public Button saveButton;
    public Button scriptButton;


    private List<string> scripts = new List<string>();
    private int currentIndex = 0;
    public string SelectedScript { get; private set; }
    public static bool IsPanelOpen { get; private set; }

    private void Start()
    {
        LoadScripts();                // Load the script texts from Resources
        UpdateScriptDetail();         // Display the first script detail
        //OpenScriptSelection();        // Open the script selection panel immediately
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
        IsPanelOpen = true; // Mark the panel as open

        foreach (Button btn in otherButtons)
        {
            btn.interactable = false;
        }
    }

    // Closes the pop-up panel, re-enables other buttons, and updates the title display
    public void CloseScriptSelection()
    {
        scriptPanel.SetActive(false);
        IsPanelOpen = false; // Mark the panel as closed

        foreach (Button btn in otherButtons)
        {
            btn.interactable = true;
        }
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

/*
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && scriptPanel.activeSelf)
        {
            CloseScriptSelection();
        }
        if (CutsceneManager.Instance != null && CutsceneManager.Instance.currentCutscene != null)
        {
            string name = CutsceneManager.Instance.currentCutscene.name;

            if (name.Equals("bedCutscene") || name.Equals("dresserCutscene"))
                currentIndex = 0; // Script 1
            else if (name.Equals("bedSadCutscene") || name.Contains("dresserSad") || name.Contains("bedAngryCutscene") || name.Contains("dresserAngry") )
                currentIndex = 1; // Script 2
            else if (name.Contains("Sympathetic") || name.Contains("Annoyed"))
                currentIndex = 2; // Script 3
            else if (name.Contains("Enraged") || name.Contains("Composed"))
                currentIndex = 3; // Script 4
            else if (name.Contains("Juliet"))
                currentIndex = 4; // Script 5
            else if (name.Contains("CalmsDown") || name.Contains("RemainsAngry"))
                currentIndex = 5; // Script 6
            else if (name.Contains("BedDesperate") || name.Contains("BedSorrowful"))
                currentIndex = 6; // Script 7

            UpdateScriptDetail();
        }
    }*/
    private void Update()
    {
        if (CutsceneManager.Instance == null || CutsceneManager.Instance.currentCutsceneType == null)
            return;

        string type = CutsceneManager.Instance.currentCutsceneType;

        switch (type)
        {
            case "Bed":
            case "Dresser":
                currentIndex = 0; // Script 1 (index 0)
                break;
            case "BedSad":
            case "DresserSad":
            case "BedAngry":
            case "DresserAngry":
                currentIndex = 1; // Script 2
                break;
            case "CapuletSympathetic":
            case "CapuletAnnoyed":
                currentIndex = 2; // Script 3
                break;
            case "CapuletEnraged":
            case "CapuletComposed":
                currentIndex = 3; // Script 1 (index 0)
                break;
            case "JulietKneelsCapuletCalm":
            case "JulietKneelsCapuletAngry":
            case "JulietStandsCapuletCalm":
            case "JulietStandsCapuletAngry":
                currentIndex = 4; // Script 2
                break;
            case "CapuletCalmsDown":
            case "CapuletRemainsAngry":
                currentIndex = 5; // Script 3
                break;
            case "BedDesperate":
            case "BedSorrowful":
                currentIndex = 6; // Script 3
                break;
            case "CarpetDesperate":
            case "CarpetSorrowful":
                currentIndex = 7; // Script 3
                break;            
            default:
                return;
        }

        UpdateScriptDetail();
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
