using UnityEngine;
using UnityEngine.UI;

public class PlaySceneButton : MonoBehaviour
{
    public Button playButton; // Assign in Inspector
    public CutsceneManager cutsceneManager; // Assign in Inspector
    private string currentSnapPoint;

    void Start()
    {
        playButton.interactable = false; // Disable button at start
    }

    public void SetCurrentSnapPoint(string snapPoint)
    {
        Debug.Log($"Received snap point: {snapPoint}");
        currentSnapPoint = snapPoint;

        // Enable play button only if a valid snap point is set
        playButton.interactable = (snapPoint == "Bed" || snapPoint == "Dresser");
    }

    public void OnPlayButtonPressed()
    {
        if (!string.IsNullOrEmpty(currentSnapPoint) && cutsceneManager != null)
        {
            // Play the corresponding Timeline cutscene before transitioning
            cutsceneManager.PlayCutscene(currentSnapPoint, LoadNextScene);
        }
        else
        {
            Debug.LogWarning("No snap point selected or CutsceneManager is missing!");
        }
    }

    private void LoadNextScene()
    {
        Debug.Log("Loading next scene...");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Play Scene");
    }
}
