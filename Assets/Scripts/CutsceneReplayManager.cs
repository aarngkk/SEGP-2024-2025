using UnityEngine;
using UnityEngine.Playables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;

// Manages the replay of recorded cutscene sequences from log files
public class CutsceneReplayManager : MonoBehaviour
{
    public CutsceneManager cutsceneManager;
    public UnityEngine.UI.Button skipButton;

    private Queue<string> cutsceneQueue = new Queue<string>();
    private string currentCutsceneType = "";
    private bool cutsceneFinished = false;

    private void Start()
    {
        // Check for transferred log file name from previous scene
        string logFileName = SceneDataTransfer.Instance.LogFileName;
        if (!string.IsNullOrEmpty(logFileName))
        {
            cutsceneManager.isReplayMode=true; // Enable replay mode in cutscene manager
            LoadChoicesAndReplay(logFileName); // Load and start replay sequence

            // Setup skip button functionality
            if (skipButton != null)
                skipButton.onClick.AddListener(SkipToNextCutscene);
        }
        else
        {
            Debug.LogError("No log file selected!");
        }
    }

    // Loads cutscene choices from log file and prepares them for replay
    private void LoadChoicesAndReplay(string logFileName)
    {
        // Construct full path to log file in persistent data directory
        string filePath = Path.Combine(Application.persistentDataPath, "SavedScenes", logFileName + ".log");

        if (!File.Exists(filePath))
        {
            Debug.LogError("Log file not found: " + filePath);
            return;
        }

        // Read all choices from log file and enqueue them
        string[] choices = File.ReadAllLines(filePath);
        foreach (string choice in choices)
        {

            cutsceneQueue.Enqueue(choice);
        }

        if (cutsceneQueue.Count > 0)
        {
            StartCoroutine(ReplayCutscenes());
        }
        else
        {
            Debug.LogWarning("Log file is empty. No cutscenes to replay.");
        }
    }

    // Coroutine that sequentially replays all cutscenes in the queue
    private IEnumerator ReplayCutscenes()
    {
        // Process all cutscenes in queue
        while (cutsceneQueue.Count > 0)
        {
            currentCutsceneType = cutsceneQueue.Dequeue();
            Debug.Log("Replaying cutscene: " + currentCutsceneType);

            cutsceneFinished = false;
            cutsceneManager.PlayCutscene(currentCutsceneType, () => cutsceneFinished = true);

            yield return new WaitUntil(() => cutsceneFinished); // Wait for the cutscene to finish
        }

        // Return to main menu when replay completes
        Debug.Log("Cutscene replay finished.");
        SceneManager.LoadScene("Main Menu");
    }

    // Skips the currently playing cutscene and moves to next in queue
    public void SkipToNextCutscene()
    {
        if (!cutsceneFinished && cutsceneManager.currentCutscene != null)
        {
            Debug.Log("Skipping current cutscene.");

            // Clean up current cutscene state
            cutsceneManager.ResumeAndUpdateButtons();
            cutsceneManager.currentCutscene.Stop(); // Triggers completion callback
        }
    }
}
