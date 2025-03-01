using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaySceneButton : MonoBehaviour
{
    public string nextSceneName = "Play Scene"; // Change this to your target scene
    private string currentSnapPoint;

    public CutsceneManager cutsceneManager; // Assign in Inspector

    public void SetCurrentSnapPoint(string snapPoint)
    {
        Debug.Log($"Received snap point: {snapPoint}");
        currentSnapPoint = snapPoint;
    }

    public void OnPlayButtonPressed()
    {
        if (!string.IsNullOrEmpty(currentSnapPoint) && cutsceneManager != null)
        {
            // Play the corresponding cutscene, then load scene after it ends
            cutsceneManager.PlayCutscene(currentSnapPoint, LoadNextScene);
        }
        else
        {
            Debug.LogWarning("No snap point selected or CutsceneManager is missing!");
        }
    }

    private void LoadNextScene()
    {
        Debug.Log("Loading next scene: " + nextSceneName);
        SceneManager.LoadScene(nextSceneName);
    }
}
