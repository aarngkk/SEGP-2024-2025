using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SavedScenesMenu : MonoBehaviour
{
    public SceneSaver sceneSaver; // Reference to SceneSaver
    public GameObject sceneButtonPrefab; // Assign the button prefab in Inspector
    public Transform contentParent; // Assign the Scroll View Content in Inspector

    void Start()
    {
        PopulateSavedScenes();
    }

    public void PopulateSavedScenes()
    {
        // Clear existing buttons
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        string[] savedScenes = sceneSaver.GetAllSavedScenes();

        foreach (string sceneName in savedScenes)
        {
            GameObject newButton = Instantiate(sceneButtonPrefab, contentParent);
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = sceneName;

            // Add a click event to load the scene
            newButton.GetComponent<Button>().onClick.AddListener(() => sceneSaver.LoadScene(sceneName));
        }
    }
}
