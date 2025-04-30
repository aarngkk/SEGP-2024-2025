using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [Header("Tutorial Steps (in order)")]
    public GameObject[] tutorialSteps;    // Array of UI panels for each tutorial step
    private int currentStep = 0;          // Index tracking current tutorial step
    public GameObject Script1ChoicePopUp; // Popup to show after tutorial completes

    public Button nextButton;  // Button to progress through tutorial steps
    public Button skipButton;  // Button to skip entire tutorial

    // Reference to script selection UI manager
    public ScriptSelectionManager scriptSelectionManager;

    // Static flag to track tutorial state globally
    public static bool tutorialActive = true;

    // Parent container for all tutorial UI elements
    public GameObject instructionsOverlay;

    void Start()
    {
        // Hide all tutorial steps initially
        foreach (GameObject step in tutorialSteps)
            step.SetActive(false);

        // Activate first step if available
        if (tutorialSteps.Length > 0)
        {
            tutorialSteps[0].SetActive(true);
        }

        // Setup button click listeners
        nextButton.onClick.AddListener(OnNextClicked);
        skipButton.onClick.AddListener(OnSkipClicked);
    }

    // Handles progression to next tutorial step
    void OnNextClicked()
    {
        // Deactivate current step
        tutorialSteps[currentStep].SetActive(false);
        currentStep++;

        // Show next step if available
        if (currentStep < tutorialSteps.Length)
        {
            tutorialSteps[currentStep].SetActive(true);

            // Special handling for step 6 (script selection)
            if (currentStep == 6 && scriptSelectionManager != null)
            {
                scriptSelectionManager.OpenScriptSelection();
            }
            // Special handling for step 7 (script selection close)
            else if (currentStep == 7 && scriptSelectionManager != null)
            {
                scriptSelectionManager.CloseScriptSelection();
            }
        }
        else
        {
            // Complete tutorial if no steps remain
            EndTutorial();
        }
    }

    // Handles skip button click
    void OnSkipClicked()
    {
        EndTutorial();
    }

    // Cleans up tutorial UI and marks completion
    void EndTutorial()
    {
        // Hide all tutorial steps
        foreach (GameObject step in tutorialSteps)
            step.SetActive(false);

        // Hide main tutorial overlay
        if (instructionsOverlay != null)
        {
            instructionsOverlay.SetActive(false);
        }
        else
        {
            Debug.LogWarning("InstructionsOverlay is not assigned!");
        }

        // Update global tutorial state
        tutorialActive = false;

        // Show first choice popup
        Script1ChoicePopUp.SetActive(true);
    }
}
