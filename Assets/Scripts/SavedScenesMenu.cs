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

    public GameObject sceneOptionsPanel; // The pop-up panel (assign in Inspector)
    public TMP_Text sceneNameText; // Displays selected scene name (assign in Inspector)
    public Button playButton, editButton, backButton; // Assign in Inspector
    private string selectedSceneName; 

    private List<Button> allSceneButtons = new List<Button>(); 

    void Start()
    {
        if (sceneSaver == null)
        {
            sceneSaver = FindObjectOfType<SceneSaver>(); // Auto-assign if not set
        }
        layoutGroup = contentParent.GetComponent<VerticalLayoutGroup>(); // Get layout group
        PopulateSavedScenes();

        sceneOptionsPanel.SetActive(false);
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
        allSceneButtons.Clear();

        foreach (string sceneName in savedScenes)
        {
            GameObject newButton = Instantiate(sceneButtonPrefab, contentParent);
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = sceneName;
           
            Button buttonComponent = newButton.GetComponent<Button>();
            allSceneButtons.Add(buttonComponent);
            // Add a click event to load the scene
            newButton.GetComponent<Button>().onClick.AddListener(() => ShowSceneOptions(sceneName));
        }
         LayoutRebuilder.ForceRebuildLayoutImmediate(contentParent.GetComponent<RectTransform>());
   
    }

    public void ShowSceneOptions(string sceneName) {
        selectedSceneName = sceneName;
        sceneNameText.text = "Would you like to edit or play " + sceneName;
        
        Debug.Log("Opening pop-up for: " + sceneName);

        sceneOptionsPanel.SetActive(true);
        
        SetSceneButtonsInteractable(false);
        
        // Assign button functions dynamically
        playButton.onClick.RemoveAllListeners();
        editButton.onClick.RemoveAllListeners();
        backButton.onClick.RemoveAllListeners();

        playButton.onClick.AddListener(() => PlayScene(sceneName));
        editButton.onClick.AddListener(() => EditScene(sceneName));
        backButton.onClick.AddListener(() => CloseSceneOptions());

    }
    /*
    public void LoadSceneAndRestore(string sceneName)
    {
        // Store the selected scene name before switching scenes
        PlayerPrefs.SetString("LastLoadedScene", sceneName);
        PlayerPrefs.Save();

        // Load the scene where the model will be displayed
        SceneManager.LoadScene("Load Scene"); // Change to your actual scene name
        Debug.Log("Loading load scene 1");
    }*/
    private void EditScene(string sceneName)
    {
        Debug.Log("button clicked for scene: " + sceneName);
        // Load the saved scene data
        SceneData sceneData = sceneSaver.LoadScene(sceneName);

        if (sceneData != null)
        {
            Debug.Log("Scene data loaded successfully: " + sceneData.sceneName);
            // Pass the scene data to the next scene
            SceneDataTransfer.Instance.SetSceneData(sceneData, sceneName);

            // Load the new scene
            SceneManager.LoadScene("Edit Scene"); // Replace "EditScene" with your target scene name
            Debug.Log("Editing scene: "+ sceneName);
        }
        else
        {
            Debug.LogError("Failed to load scene data for: " + sceneName);
        }
    }
    private void PlayScene(string sceneName)
    {
        Debug.Log("button clicked for scene: " + sceneName);
        // Load the saved scene data
        SceneData sceneData = sceneSaver.LoadScene(sceneName);

        if (sceneData != null)
        {
            Debug.Log("Scene data loaded successfully: " + sceneData.sceneName);
            // Pass the scene data to the next scene
            SceneDataTransfer.Instance.SetSceneData(sceneData, sceneName);

            // Load the new scene
            SceneManager.LoadScene("Play Scene"); // Replace "EditScene" with your target scene name
            Debug.Log("Playing scene: " + sceneName);
        }
        else
        {
            Debug.LogError("Failed to load scene data for: " + sceneName);
        }
    }
    public void CloseSceneOptions()
    {
        sceneOptionsPanel.SetActive(false);
        SetSceneButtonsInteractable(true);
    }
    private void SetSceneButtonsInteractable(bool interactable)
    {
        foreach (Button btn in allSceneButtons)
        {
            btn.interactable = interactable;
        }
    }
}
