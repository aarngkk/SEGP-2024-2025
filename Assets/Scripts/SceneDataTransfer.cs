using UnityEngine;

public class SceneDataTransfer : MonoBehaviour
{
    public static SceneDataTransfer Instance { get; private set; }

    public SceneData SceneData { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Make this object persistent
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
        }
    }

    public void SetSceneData(SceneData sceneData)
    {
        SceneData = sceneData;
    }
}