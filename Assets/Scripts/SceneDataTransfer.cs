using UnityEngine;

public class SceneDataTransfer : MonoBehaviour
{
    //singleton instance accessible from other scripts
    public static SceneDataTransfer Instance { get; private set; }

    // Stores the selected log file name
    public string LogFileName { get; private set; }

    // Called when the object is initialized
    private void Awake()
    {
        // Ensure only one instance exists (singleton pattern)
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep data when changing scenes
        }
        else
        {
            // Destroy duplicate instances
            Destroy(gameObject);
        }
    }

    // Sets the name of the log file to be used later
    public void SetLogFile(string fileName)
    {
        LogFileName = fileName;
    }
}
