using UnityEngine;
using UnityEngine.Playables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;

public class CutsceneReplayManager : MonoBehaviour
{
    public CutsceneManager cutsceneManager;
    public UnityEngine.UI.Button skipButton;

    private Queue<string> cutsceneQueue = new Queue<string>();
    private string currentCutsceneType = "";
    private bool cutsceneFinished = false;

    private void Start()
    {
        string logFileName = SceneDataTransfer.Instance.LogFileName;
        if (!string.IsNullOrEmpty(logFileName))
        {
            cutsceneManager.isReplayMode=true;
            LoadChoicesAndReplay(logFileName);

            if (skipButton != null)
                skipButton.onClick.AddListener(SkipToNextCutscene);
        }
        else
        {
            Debug.LogError("No log file selected!");
        }
    }
    

    private void LoadChoicesAndReplay(string logFileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, "SavedScenes", logFileName + ".log");

        //string filePath = SavePath + logFileName + ".log";
        if (!File.Exists(filePath))
        {
            Debug.LogError("Log file not found: " + filePath);
            return;
        }

        // Read choices from file and enqueue them
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

    private IEnumerator ReplayCutscenes()
    {

        while (cutsceneQueue.Count > 0)
        {
            currentCutsceneType = cutsceneQueue.Dequeue();
            Debug.Log("Replaying cutscene: " + currentCutsceneType);

            cutsceneFinished = false;
            cutsceneManager.PlayCutscene(currentCutsceneType, () => cutsceneFinished = true);

            yield return new WaitUntil(() => cutsceneFinished); // Wait for the cutscene to finish
        }

        Debug.Log("Cutscene replay finished.");
        SceneManager.LoadScene("Main Menu");
    }

    public void SkipToNextCutscene()
    {
        if (!cutsceneFinished && cutsceneManager.currentCutscene != null)
        {
            Debug.Log("Skipping current cutscene.");

            cutsceneManager.ResumeAndUpdateButtons();
            cutsceneManager.currentCutscene.Stop(); // This will trigger cutsceneFinished = true

        }
    }

}
