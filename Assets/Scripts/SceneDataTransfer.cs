using UnityEngine;

public class SceneDataTransfer : MonoBehaviour
{
    public static SceneDataTransfer Instance { get; private set; }

    public string LogFileName { get; private set; } // Stores the selected log file name

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep data when changing scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetLogFile(string fileName)
    {
        LogFileName = fileName;
    }
}
