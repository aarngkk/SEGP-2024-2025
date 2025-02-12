using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;  // Add this at the top

public class ScriptSelectionManager : MonoBehaviour
{
    // UI Elements assigned via the Inspector:
    public GameObject scriptPanel;    // The pop-up panel to show/hide
    public TextMeshProUGUI scriptDetailText;     // Reference to the Text component (make sure this is UnityEngine.UI.Text)
    public Button nextButton;         // Button to go to the next script
    public Button prevButton;         // Button to go to the previous script
    public Button selectButton;       // Button to select the current script
    public Button backButton;
    public List<Button> otherButtons; // drag all the other buttons here in the Inspector
    public ScrollRect scrollRect;
    public TextMeshProUGUI selectedScriptTitleText;

    // List to store the full text details loaded from files
    private List<string> scripts = new List<string>();

    private int currentIndex = 0;

    public string SelectedScript { get; private set; }

    private void Start()
    {
        LoadScripts();                 // Load the text details from the Resources folder
        UpdateScriptDetail();          // Show the first script detail
        OpenScriptSelection();  // Immediately open the script selection
        // scriptPanel.SetActive(false);  // Hide the panel initially
        backButton.onClick.AddListener(CloseScriptSelection);
    }

    // Loads script details from text files in the Resources folder
    private void LoadScripts()
    {
        // Assume we have 6 script files. Adjust the loop count if needed.
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

        // Optional: Log how many scripts were loaded.
        Debug.Log("Loaded " + scripts.Count + " script details.");
    }

    // Opens the pop-up panel
    public void OpenScriptSelection()
    {
        scriptPanel.SetActive(true);
        // Disable all other buttons
    foreach (Button btn in otherButtons)
    {
        btn.interactable = false;
    }
    }

    // Closes the pop-up panel
    public void CloseScriptSelection()
    {
    scriptPanel.SetActive(false);
    // Re-enable the other buttons
    foreach (Button btn in otherButtons)
    {
        btn.interactable = true;
    }
    // Update the title display if a script was selected
        if (!string.IsNullOrEmpty(SelectedScript) && selectedScriptTitleText != null)
        {
            // Assume the title is the first line of the script.
            string[] lines = SelectedScript.Split('\n');
            selectedScriptTitleText.text = lines.Length > 0 ? lines[0] : "Selected Script";
        }
    }

    // Moves to the next script in the list
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
    }

    // Updates the text element with the current script detail
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
            // 2) Reset the scroll to the top
        if (scrollRect != null)
        {
            // The top is typically verticalNormalizedPosition = 1
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
}
