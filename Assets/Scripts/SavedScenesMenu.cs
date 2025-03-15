using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;

public class SavedScenesMenu : MonoBehaviour
{
    //public CutsceneReplayManager cutsceneReplayManager;
    //public SceneSaver sceneSaver; // Reference to SceneSaver
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

        string[] savedScenes = GetAllSavedScenes();
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
        sceneNameText.text = "Would you like to play " + sceneName + "?";
        
        Debug.Log("Opening pop-up for: " + sceneName);

        sceneOptionsPanel.SetActive(true);
        
        SetSceneButtonsInteractable(false);
        
        // Assign button functions dynamically
        playButton.onClick.RemoveAllListeners();
        backButton.onClick.RemoveAllListeners();

        playButton.onClick.AddListener(() => LoadReplayScene(sceneName));
        backButton.onClick.AddListener(() => CloseSceneOptions());

    }
    
    private void LoadReplayScene(string sceneName)
    {
        Debug.Log("Loading replay scene for: " + sceneName);
        SceneDataTransfer.Instance.SetLogFile(sceneName);
        SceneManager.LoadScene("Play All Scenes"); // Load your replay scene
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

    private string[] GetAllSavedScenes()
    {
        string savePath = Application.persistentDataPath + "/SavedScenes/";

        if (!Directory.Exists(savePath))
            return new string[0];

        string[] files = Directory.GetFiles(savePath, "*.log");
        for (int i = 0; i < files.Length; i++)
        {
            files[i] = Path.GetFileNameWithoutExtension(files[i]);
        }
        return files;
    }
}
