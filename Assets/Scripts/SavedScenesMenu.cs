using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SavedScenesMenu : MonoBehaviour
{
    public SceneSaver sceneSaver; // Reference to SceneSaver
    public GameObject sceneButtonPrefab; // Assign the button prefab in Inspector
    public Transform contentParent; // Assign the Scroll View Content in Inspector
    private VerticalLayoutGroup layoutGroup;

    void Start()
    {
        if (sceneSaver == null)
        {
            sceneSaver = FindObjectOfType<SceneSaver>(); // Auto-assign if not set
        }
        layoutGroup = contentParent.GetComponent<VerticalLayoutGroup>(); // Get layout group
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
        if (layoutGroup != null)
        {
            layoutGroup.spacing = 80f; // Change this value to increase spacing
        }

        foreach (string sceneName in savedScenes)
        {
            GameObject newButton = Instantiate(sceneButtonPrefab, contentParent);
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = sceneName;
           //RectTransform buttonRect = newButton.GetComponent<RectTransform>();
            //buttonRect.sizeDelta = new Vector2(100, 30); // Set button size

        // Set spacing between buttons
            /*VerticalLayoutGroup layoutGroup = contentParent.GetComponent<VerticalLayoutGroup>();
            if (layoutGroup != null)
            {
                layoutGroup.spacing = 15; // Adjust spacing dynamically
            }*/
            // Add a click event to load the scene
            newButton.GetComponent<Button>().onClick.AddListener(() => OnSceneButtonClicked(sceneName));
        }
         LayoutRebuilder.ForceRebuildLayoutImmediate(contentParent.GetComponent<RectTransform>());
   
    }
    
    public void LoadSceneAndRestore(string sceneName)
    {
        // Store the selected scene name before switching scenes
        PlayerPrefs.SetString("LastLoadedScene", sceneName);
        PlayerPrefs.Save();

        // Load the scene where the model will be displayed
        SceneManager.LoadScene("Load Scene"); // Change to your actual scene name
        Debug.Log("Loading load scene 1");
    }
    private void OnSceneButtonClicked(string sceneName)
    {
        Debug.Log("button clicked for scene: " + sceneName);
        // Load the saved scene data
        SceneData sceneData = sceneSaver.LoadScene(sceneName);

        if (sceneData != null)
        {
            Debug.Log("Scene data loaded successfully: " + sceneData.sceneName);
            // Pass the scene data to the next scene
            SceneDataTransfer.Instance.SetSceneData(sceneData);

            // Load the new scene
            SceneManager.LoadScene("Load Scene"); // Replace "EditScene" with your target scene name
            Debug.Log("Loading load scene 1");
        }
        else
        {
            Debug.LogError("Failed to load scene data for: " + sceneName);
        }
    }
}
