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
    public Button backButton;                         // Button to close the panel
    public List<Button> otherButtons;                 // Other buttons to disable/enable
    public ScrollRect scrollRect;                     // Scroll area for the script text
    public Button saveButton;
    public Button undoButton;
    public Button redoButton;
    public Button scriptButton;

    public TMP_Text buttonText;
    public TMP_Text titleText;


    private List<string> scripts = new List<string>();
    private int currentIndex = 0;
    public string SelectedScript { get; private set; }
    public static bool IsPanelOpen { get; private set; }

    private void Start()
    {
        LoadScripts();
        UpdateScriptDetail();
        backButton.onClick.AddListener(CloseScriptSelection);

        if (saveButton != null) saveButton.interactable = false;
        if (undoButton != null) undoButton.interactable = false;
        if (redoButton != null) redoButton.interactable = false;
    }

    // Loads script details from text files in the Resources folder
    private void LoadScripts()
    {
        for (int i = 1; i <= 8; i++)
        {
            string fileName = "ScriptDetail" + i;
            TextAsset textAsset = Resources.Load<TextAsset>(fileName);
            if (textAsset != null) scripts.Add(textAsset.text);
            else Debug.LogWarning("Could not load " + fileName + ".txt from Resources!");
        }
        Debug.Log("Loaded " + scripts.Count + " script details.");
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
        UpdateTitle(currentIndex);
        UpdateScriptButtonLabel(currentIndex);
        if (currentIndex >= 0) {
            if (saveButton != null)
            {
                saveButton.interactable = true;
            }
            if (redoButton != null) 
            {
                redoButton.interactable = true;
            }
        }
        if (currentIndex > 0) {
            if (undoButton != null) 
            {
                undoButton.interactable = true;
            }
        }
    }

    private void UpdateScriptButtonLabel(int currentIndex)
    {
        int scriptNumber = currentIndex + 1;
        buttonText.text = "Script " + scriptNumber;
    }

    private void UpdateTitle(int currentIndex)
    {
        switch(currentIndex)
        {
            case 0: titleText.text = "Lines 103-115";break;
            case 1: titleText.text = "Lines 116-125";break;
            case 2: titleText.text = "Lines 126-145";break;
            case 3: titleText.text = "Lines 146-157b";break;
            case 4: titleText.text = "Lines 158-175b";break;
            case 5: titleText.text = "Lines 176-196";break;
            case 6: titleText.text = "Lines 197-226";break;
            case 7: titleText.text = "Lines 227-243";break;
            default: return;            
        }
    }

    // Opens the pop-up panel and disables all other buttons
    public void OpenScriptSelection()
    {
        UpdateScriptDetail();
        scriptPanel.SetActive(true);
        IsPanelOpen = true;
        foreach (Button btn in otherButtons) btn.interactable = false;
    }

    // Closes the pop-up panel, re-enables other buttons, and updates the title display
    public void CloseScriptSelection()
    {
        scriptPanel.SetActive(false);
        IsPanelOpen = false; // Mark the panel as closed

        foreach (Button btn in otherButtons) btn.interactable = true;
        if (saveButton != null) saveButton.interactable = false;
        if (undoButton != null) undoButton.interactable = false;
        if (redoButton != null) redoButton.interactable = false;
    }

}
