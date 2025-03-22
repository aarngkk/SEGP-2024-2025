using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [Header("Tutorial Steps (in order)")]
    public GameObject[] tutorialSteps;  // Each step is a UI element in the overlay.
    private int currentStep = 0;

    public Button nextButton;  // Button to move to the next step.
    public Button skipButton;  // Button to skip the tutorial.

    // Reference to the ScriptSelectionManager that will run after the tutorial.
    public ScriptSelectionManager scriptSelectionManager;

    // This flag indicates if the tutorial is active.
    public static bool tutorialActive = true;

    // The parent panel or object for the entire instructions UI.
    public GameObject instructionsOverlay;

    void Start()
    {
        // Initially hide all steps.
        foreach (GameObject step in tutorialSteps)
            step.SetActive(false);

        // Show the first step, if available.
        if (tutorialSteps.Length > 0)
        {
            tutorialSteps[0].SetActive(true);
        }

        nextButton.onClick.AddListener(OnNextClicked);
        skipButton.onClick.AddListener(OnSkipClicked);
    }

    void OnNextClicked()
    {
        // Hide the current step
        tutorialSteps[currentStep].SetActive(false);
        currentStep++;

        // If we still have more steps, show the new step
        if (currentStep < tutorialSteps.Length)
        {
            tutorialSteps[currentStep].SetActive(true);

            // Check if we've just moved onto step #3 (index 2)
            if (currentStep == 6 && scriptSelectionManager != null)
            {
                scriptSelectionManager.OpenScriptSelection();
            }
            else if (currentStep == 7 && scriptSelectionManager != null)
            {
                scriptSelectionManager.CloseScriptSelection();
            }
        }
        else
        {
            // If no more steps, end the tutorial
            EndTutorial();
        }
    }

    void OnSkipClicked()
    {
        EndTutorial();
    }

    void EndTutorial()
    {
        // Hide all tutorial steps
        foreach (GameObject step in tutorialSteps)
            step.SetActive(false);

        // Hide the entire instructions overlay
        if (instructionsOverlay != null)
        {
            instructionsOverlay.SetActive(false);
        }
        else
        {
            Debug.LogWarning("InstructionsOverlay is not assigned!");
        }

        // Mark the tutorial as inactive
        tutorialActive = false;
    }
}
