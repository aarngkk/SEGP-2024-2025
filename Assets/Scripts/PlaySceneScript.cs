using UnityEngine;
using UnityEngine.UI;

public class PlaySceneButton : MonoBehaviour
{
    public Button playButton; // Assign in Inspector
    public CutsceneManager cutsceneManager; // Assign in Inspector
    private string currentSnapPoint;

    public Outline bedOutline;      // Assign in Inspector (Bed 3D model)
    public Outline dresserOutline;  // Assign in Inspector (Dresser 3D model)

    void Start()
    {
        playButton.interactable = false; // Disable button at start

        // Ensure outlines are enabled at start
        SetOutlineState(true);
    }

    public void SetCurrentSnapPoint(string snapPoint)
    {
        Debug.Log($"Received snap point: {snapPoint}");
        currentSnapPoint = snapPoint;

        // Enable play button only if a valid snap point is set
        playButton.interactable = (snapPoint == "Bed" || snapPoint == "Dresser");

        // Enable/disable outline based on whether a snap point is selected
        SetOutlineState(string.IsNullOrEmpty(snapPoint));
    }

    private void SetOutlineState(bool enable)
    {
        if (bedOutline != null)
            bedOutline.enabled = enable;

        if (dresserOutline != null)
            dresserOutline.enabled = enable;
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
