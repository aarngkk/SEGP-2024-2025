using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using TMPro;

public class CutsceneManager : MonoBehaviour
{
    private string SavePath => Application.persistentDataPath + "/SavedScenes/";
    public string logFileName; // Default filename if none is set
    private bool isPaused = false;
    public Button playButton;
    public Button pauseButton;

    private bool isCutscene1Started = false;
    public bool IsCutscene1Started() => isCutscene1Started;
    private bool isCutscene7Started = false; // New flag to track Cutscene7 state
    public bool IsCutscene7Started() => isCutscene7Started; // Public getter
    public GameObject Script1ChoicePopUp;

    [Header("Objects to Disable")]
    public GameObject bedZone;
    public GameObject dresserZone;
    public GameObject carpetObject;

    [Header("Cameras")]
    public GameObject mainCamera;
    public GameObject stageCamera;

    [Header("Script 1")]
    public PlayableDirector bedCutscene;
    public PlayableDirector dresserCutscene;

    public GameObject choicePopupPanel; // First choice (Bed/Dresser)

    public Button sadButton;
    public Button angryButton;

    [Header("Script 2")]
    public PlayableDirector bedSadCutscene;
    public PlayableDirector bedAngryCutscene;
    public PlayableDirector dresserSadCutscene;
    public PlayableDirector dresserAngryCutscene;

    public GameObject capuletChoicePopupPanel; // Second choice (Sympathetic/Annoyed)

    public Button sympatheticButton;
    public Button annoyedButton;

    [Header("Script 3")]
    public PlayableDirector capuletSympatheticCutscene;
    public PlayableDirector capuletAnnoyedCutscene;

    public GameObject capuletFinalChoicePopupPanel; // Third choice (Enraged/Composed)

    public Button enragedButton;
    public Button composedButton;

    [Header("Script 4")]
    public PlayableDirector capuletEnragedCutscene;
    public PlayableDirector capuletComposedCutscene;

    [SerializeField] private GameObject julietKneelingChoicePopupPanel; // Juliet Kneeling Choice Popup
    [SerializeField] private Button kneelButton;
    [SerializeField] private Button standButton;

    [Header("Script 5")] // NEW: Fields for 4 final cutscenes
    public PlayableDirector julietKneelsCapuletCalm;
    public PlayableDirector julietKneelsCapuletAngry;
    public PlayableDirector julietStandsCapuletCalm;
    public PlayableDirector julietStandsCapuletAngry;

    [SerializeField] private GameObject capuletThirdChoicePopupPanel;
    [SerializeField] private Button calmDownButton;
    [SerializeField] private Button remainAngryButton;

    [Header("Script 6")] // NEW: Fields for Capulet's reaction after the third choice
    public PlayableDirector capuletCalmsDownCutscene;
    public PlayableDirector capuletRemainsAngryCutscene;

    public GameObject script7PositionPanel;
    public GameObject script7ChoicePopupPanel;
    public Button desperateButton;
    public Button sorrowfulButton;

    [Header("Script 7")]
    public PlayableDirector bedDesperateCutscene;
    public PlayableDirector bedSorrowfulCutscene;
    public PlayableDirector carpetDesperateCutscene;
    public PlayableDirector carpetSorrowfulCutscene;

    public GameObject JulietSitWalkChoicePopupPanel; // Choice popup for Script 8
    public Button sitButton;
    public Button walkButton;

    [Header("Script 8")]
    public PlayableDirector julietSitCutscene;
    public PlayableDirector julietWalkCutscene;

    public PlayableDirector currentCutscene;
    private List<string> choicesMade = new List<string>();
    private Stack<string> choiceHistory = new Stack<string>(); // Stores past choices
    public bool isReplayMode = false;
    private bool isCutscene6Finished = false;
    public bool IsCutscene6Finished() => isCutscene6Finished;

    public GameObject FinishedPopUp;

    public static CutsceneManager Instance { get; private set; }
    public string currentCutsceneType { get; private set; }

    void Start()
    {
        playButton.gameObject.SetActive(false); // Hide play button initially
        pauseButton.gameObject.SetActive(true); // Show pause button

        playButton.onClick.AddListener(PlayCutscene);
        pauseButton.onClick.AddListener(PauseCutscene);
    }

    public void PlayCutscene(string cutsceneType, System.Action onCutsceneEnd = null)
    {
        Debug.Log($"Attempting to play cutscene: {cutsceneType}");

        RestoreCharacterAnimation();
        currentCutsceneType = cutsceneType; 

        // Set the flag when cutscene 1 starts
        if (cutsceneType == "Bed" || cutsceneType == "Dresser")
        {
            isCutscene1Started = true;

            // Clear snap points and reset character positions
            DragCharacter[] dragCharacters = FindObjectsOfType<DragCharacter>();
            foreach (DragCharacter character in dragCharacters)
            {
                character.ClearSnapPoints();
            }

            // Disable the DresserZone game object
            if (dresserZone != null)
            {
                dresserZone.SetActive(false);
                Debug.Log("DresserZone GameObject disabled.");
            }

            if (Script1ChoicePopUp!=null) Script1ChoicePopUp.SetActive(false);
        }

        if (cutsceneType == "BedDesperate" || cutsceneType == "BedSorrowful" ||
            cutsceneType == "CarpetDesperate" || cutsceneType == "CarpetSorrowful" )
        {

            // Clear snap points and reset character positions
            DragCharacter[] dragCharacters = FindObjectsOfType<DragCharacter>();
            foreach (DragCharacter character in dragCharacters)
            {
                character.ClearSnapPoints();
            }

            // Disable the DresserZone game object
            if (bedZone != null)
            {
                bedZone.SetActive(false);
                Debug.Log("BedZone GameObject disabled.");
            }
            if (carpetObject != null)
            {
                carpetObject.SetActive(false);
                Debug.Log("CarpetZone GameObject disabled.");
            }
        }

        switch (cutsceneType)
        {
            case "Bed": currentCutscene = bedCutscene; break;
            case "Dresser": currentCutscene = dresserCutscene; break;
            case "BedSad": currentCutscene = bedSadCutscene; break;
            case "BedAngry": currentCutscene = bedAngryCutscene; break;
            case "DresserSad": currentCutscene = dresserSadCutscene; break;
            case "DresserAngry": currentCutscene = dresserAngryCutscene; break;
            case "CapuletSympathetic":currentCutscene = capuletSympatheticCutscene; break;
            case "CapuletAnnoyed": currentCutscene = capuletAnnoyedCutscene; break;
            case "CapuletEnraged": currentCutscene = capuletEnragedCutscene; break;
            case "CapuletComposed": currentCutscene = capuletComposedCutscene; break;
            case "JulietKneelsCapuletCalm": currentCutscene = julietKneelsCapuletCalm; break;
            case "JulietKneelsCapuletAngry": currentCutscene = julietKneelsCapuletAngry; break;
            case "JulietStandsCapuletCalm": currentCutscene = julietStandsCapuletCalm; break;
            case "JulietStandsCapuletAngry":currentCutscene = julietStandsCapuletAngry;break;
            case "CapuletCalmsDown": currentCutscene = capuletCalmsDownCutscene;break;
            case "CapuletRemainsAngry":currentCutscene = capuletRemainsAngryCutscene;break;
            case "BedDesperate": currentCutscene = bedDesperateCutscene; break;
            case "BedSorrowful": currentCutscene = bedSorrowfulCutscene; break;
            case "CarpetDesperate": currentCutscene = carpetDesperateCutscene; break;
            case "CarpetSorrowful": currentCutscene = carpetSorrowfulCutscene; break;
            case "JulietSit": currentCutscene = julietSitCutscene; break;
            case "JulietWalk": currentCutscene = julietWalkCutscene; break;

            default:
                Debug.LogWarning("Invalid cutscene type!");
                onCutsceneEnd?.Invoke();
                return;
        }

        Debug.Log($"Playing cutscene: {currentCutscene.name}");
        if (!isReplayMode)
        {
            choiceHistory.Push(currentCutscene.name);
            choicesMade.Add(cutsceneType);
            currentCutscene.stopped += OnCutsceneFinished;
        }

        if (isReplayMode)
        {
            currentCutscene.stopped += (PlayableDirector director) =>
            {
                Debug.Log("Cutscene finished playing.");
                director.stopped -= OnCutsceneFinished;
                FreezeCharacterPose();
                onCutsceneEnd?.Invoke();
            };
        }

        currentCutscene.Play();
    }
    private void RestoreCharacterAnimation()
    {
        Animator julietAnimator = GameObject.Find("Juliet")?.GetComponent<Animator>();
        Animator ladyCapuletAnimator = GameObject.Find("Lady Capulet")?.GetComponent<Animator>();

        if (julietAnimator != null) julietAnimator.enabled = true;
        if (ladyCapuletAnimator != null) ladyCapuletAnimator.enabled = true;
    }

    private void OnCutsceneFinished(PlayableDirector director)
    {
        Debug.Log("Cutscene finished playing.");
        director.stopped -= OnCutsceneFinished;

        FreezeCharacterPose();

        if (isReplayMode)
        {
            Debug.Log("Replay mode active, continuing to next cutscene.");
            return;
        }

        if (currentCutscene == bedCutscene || currentCutscene == dresserCutscene)
        {
            ShowChoicePopup();
        }
        else if (currentCutscene == bedSadCutscene || currentCutscene == bedAngryCutscene ||
                 currentCutscene == dresserSadCutscene || currentCutscene == dresserAngryCutscene)
        {
            ShowCapuletChoicePopup();
        }
        else if (currentCutscene == capuletSympatheticCutscene || currentCutscene == capuletAnnoyedCutscene)
        {
            ShowCapuletFinalChoicePopup();
        }
        else if (currentCutscene == capuletEnragedCutscene || currentCutscene == capuletComposedCutscene)
        {
            ShowJulietKneelingChoicePopup();
        }
        else if (currentCutscene == julietKneelsCapuletCalm || currentCutscene == julietKneelsCapuletAngry ||
                 currentCutscene == julietStandsCapuletCalm || currentCutscene == julietStandsCapuletAngry)
        {
            ShowCapuletThirdChoicePopup();
        }
        else if (currentCutscene == capuletCalmsDownCutscene || currentCutscene == capuletRemainsAngryCutscene)
        {
            // Cutscene 6 has finished
            isCutscene6Finished = true;
            Debug.Log("Cutscene 6 finished. Ready for script 7 choices.");

            if(script7PositionPanel!=null) script7PositionPanel.SetActive(true);

            if (carpetObject != null)
            {
                carpetObject.SetActive(true);
                Debug.Log("Carpet GameObject enabled.");
            }

            if (mainCamera != null) mainCamera.SetActive(true);
            if (stageCamera != null) stageCamera.SetActive(false);

            // Enable the bed outline immediately after Cutscene6 finishes
            Outline bedOutline = GameObject.FindWithTag("Bed")?.GetComponent<Outline>();
            if (bedOutline != null)
            {
                bedOutline.enabled = true;
                bedOutline.OutlineColor = Color.white; // Set the outline color to white
                Debug.Log("Bed outline enabled after Cutscene6.");
            }
        }
        else if (currentCutscene == bedDesperateCutscene || currentCutscene == bedSorrowfulCutscene ||
                 currentCutscene == carpetDesperateCutscene || currentCutscene == carpetSorrowfulCutscene)
        {
            ShowScript8ChoicePopup();
        }
        else if (currentCutscene == julietSitCutscene || currentCutscene == julietWalkCutscene)
        {
            ShowFinishPopUp();
        }
    }

    private void FreezeCharacterPose()
    {
        Animator julietAnimator = GameObject.Find("Juliet")?.GetComponent<Animator>();
        Animator ladyCapuletAnimator = GameObject.Find("Lady Capulet")?.GetComponent<Animator>();

        if (julietAnimator != null) julietAnimator.enabled = false;
        if (ladyCapuletAnimator != null) ladyCapuletAnimator.enabled = false;
    }

    private void ShowChoicePopup()
    {
        if (choicePopupPanel != null)
        {
            choicePopupPanel.SetActive(true);

            sadButton.onClick.RemoveAllListeners();
            angryButton.onClick.RemoveAllListeners();

            if (currentCutscene == dresserCutscene)
            {
                // If previous cutscene was dresser, play dresser variations
                sadButton.onClick.AddListener(() => PlayNextCutscene("DresserSad"));
                angryButton.onClick.AddListener(() => PlayNextCutscene("DresserAngry"));
            }
            else
            {
                // Otherwise, assume it was the bed cutscene
                sadButton.onClick.AddListener(() => PlayNextCutscene("BedSad"));
                angryButton.onClick.AddListener(() => PlayNextCutscene("BedAngry"));
            }
        }
        else
        {
            Debug.LogError("Choice popup panel is not assigned!");
        }
    }

    private void ShowCapuletChoicePopup()
    {
        if (capuletChoicePopupPanel != null)
        {
            capuletChoicePopupPanel.SetActive(true);

            sympatheticButton.onClick.RemoveAllListeners();
            annoyedButton.onClick.RemoveAllListeners();

            sympatheticButton.onClick.AddListener(() => PlayNextCutscene("CapuletSympathetic"));
            annoyedButton.onClick.AddListener(() => PlayNextCutscene("CapuletAnnoyed"));
        }
        else
        {
            Debug.LogError("Capulet choice popup panel is not assigned!");
        }
    }

    private void ShowCapuletFinalChoicePopup()
    {
        if (capuletFinalChoicePopupPanel != null)
        {
            capuletFinalChoicePopupPanel.SetActive(true);

            enragedButton.onClick.RemoveAllListeners();
            composedButton.onClick.RemoveAllListeners();

            enragedButton.onClick.AddListener(() => PlayNextCutscene("CapuletEnraged"));
            composedButton.onClick.AddListener(() => PlayNextCutscene("CapuletComposed"));
        }
        else
        {
            Debug.LogError("Capulet final choice popup panel is not assigned!");
        }
    }

    private void ShowJulietKneelingChoicePopup()
    {
        if (julietKneelingChoicePopupPanel != null)
        {
            julietKneelingChoicePopupPanel.SetActive(true);

            kneelButton.onClick.RemoveAllListeners();
            standButton.onClick.RemoveAllListeners();

            // Determine the previous Capulet choice
            bool capuletWasComposed = choicesMade.Contains("CapuletComposed");

            kneelButton.onClick.AddListener(() =>
            {
                string nextCutscene = capuletWasComposed ? "JulietKneelsCapuletCalm" : "JulietKneelsCapuletAngry";
                PlayNextCutscene(nextCutscene);
            });

            standButton.onClick.AddListener(() =>
            {
                string nextCutscene = capuletWasComposed ? "JulietStandsCapuletCalm" : "JulietStandsCapuletAngry";
                PlayNextCutscene(nextCutscene);
            });
        }
        else
        {
            Debug.LogError("Juliet kneeling choice popup panel is not assigned!");
        }
    }

    private void ShowCapuletThirdChoicePopup()
    {
        if (capuletThirdChoicePopupPanel != null)
        {
            capuletThirdChoicePopupPanel.SetActive(true);

            calmDownButton.onClick.RemoveAllListeners();
            remainAngryButton.onClick.RemoveAllListeners();

            calmDownButton.onClick.AddListener(() => PlayNextCutscene("CapuletCalmsDown"));
            remainAngryButton.onClick.AddListener(() => PlayNextCutscene("CapuletRemainsAngry"));
        }
        else
        {
            Debug.LogError("Capulet third choice popup panel is not assigned!");
        }
    }

    private void ShowScript8ChoicePopup()
    {
        if (JulietSitWalkChoicePopupPanel != null)
        {
            JulietSitWalkChoicePopupPanel.SetActive(true);

            sitButton.onClick.RemoveAllListeners();
            walkButton.onClick.RemoveAllListeners();

            sitButton.onClick.AddListener(() => PlayNextCutscene("JulietSit"));
            walkButton.onClick.AddListener(() => PlayNextCutscene("JulietWalk"));
        }
        else
        {
            Debug.LogError("Script 8 choice popup panel is not assigned!");
        }
    }

    private void ShowFinishPopUp()
    {
        if (FinishedPopUp != null)
        {
            FinishedPopUp.SetActive(true);
        }
        else
        {
            Debug.LogError("Finished Pop Up Not Assigned!!");
        }
    }

    private void PlayNextCutscene(string nextCutsceneType)
    {
        choicePopupPanel?.SetActive(false);
        capuletChoicePopupPanel?.SetActive(false);
        capuletFinalChoicePopupPanel?.SetActive(false);
        julietKneelingChoicePopupPanel?.SetActive(false);
        capuletThirdChoicePopupPanel?.SetActive(false);
        JulietSitWalkChoicePopupPanel?.SetActive(false);

        PlayCutscene(nextCutsceneType);
    }

    public void LogChoicesToFile()
    {
        if (choicesMade.Count == 0)
        {
            Debug.LogWarning("No choices have been made yet. Nothing to save.");
            return;
        }
       if (string.IsNullOrWhiteSpace(logFileName))
        {
        logFileName = "default_log"; // Fallback filename
        }
        try
        {
            string filePath = SavePath + logFileName + ".log";
            File.WriteAllLines(filePath, choicesMade); // Save choices as lines in a file
            Debug.Log($"Choices logged to: {filePath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to write log file: {e.Message}");
        }
    }

    public void UndoChoice()
    {
        if (choiceHistory.Count < 2) // Ensure there is a previous choice to revert to
        {
            Debug.LogWarning("No previous choice to undo!");
            return;
        }

        if (currentCutscene != null && currentCutscene.state == PlayState.Playing)
        {
            currentCutscene.stopped -= OnCutsceneFinished; // Removes the listener
            currentCutscene.Stop();
            Debug.Log("Current cutscene stopped.");
        }

        choiceHistory.Pop(); // Remove the latest choice
        string previousChoice = choiceHistory.Peek(); // Get the choice before it
        choicesMade.RemoveAt(choicesMade.Count - 1);
        Debug.Log($"Undoing choice, returning to: {previousChoice}");

        // Reset the state of isCutscene6Finished and isCutscene7Started
        isCutscene6Finished = false;
        isCutscene7Started = false; // Reset the Cutscene7 flag
        Debug.Log("Cutscene7 state reset. Bed outline and snap points re-enabled.");

        // Re-enable the Carpet GameObject
        if (carpetObject != null)
        {
            carpetObject.SetActive(true);
            Debug.Log("Carpet GameObject re-enabled.");
        }

        // Re-enable the BedZone GameObject
        if (bedZone != null)
        {
            bedZone.SetActive(true);
            Debug.Log("BedZone GameObject re-enabled.");
        }

        // Reset the Bed and Carpet outlines to white
        Outline bedOutline = GameObject.FindWithTag("Bed")?.GetComponent<Outline>();
        Outline carpetOutline = GameObject.FindWithTag("CarpetZone")?.GetComponent<Outline>();

        if (bedOutline != null)
        {
            bedOutline.enabled = true;
            bedOutline.OutlineColor = Color.white; // Reset the outline color to white
            Debug.Log("Bed outline re-enabled and reset to white.");
        }

        if (carpetOutline != null)
        {
            carpetOutline.enabled = true;
            carpetOutline.OutlineColor = Color.white; // Reset the outline color to white
            Debug.Log("Carpet outline re-enabled and reset to white.");
        }

        // Reset the characters' positions to their original positions
        DragCharacter[] dragCharacters = FindObjectsOfType<DragCharacter>();
        foreach (DragCharacter character in dragCharacters)
        {
            character.ResetPosition();
        }

        // Play the previous cutscene
        PlayableDirector previousDirector = GameObject.Find(previousChoice)?.GetComponent<PlayableDirector>();
        if (previousDirector != null)
        {
            currentCutscene = previousDirector;
            currentCutscene.stopped += OnCutsceneFinished; // Reattach listener
            OnCutsceneFinished(currentCutscene); // Call the existing logic to show the choice panel
        }
        else
        {
            Debug.LogError("Could not find PlayableDirector for previous choice: " + previousChoice);
        }
    }

    public void SkipCutscene()
    {
        if (currentCutscene == null || currentCutscene.state != PlayState.Playing)
        {
            Debug.LogWarning("No cutscene is currently playing to skip!");
            return;
        }

        Debug.Log($"Skipping cutscene: {currentCutscene.name}");
        currentCutscene.stopped -= OnCutsceneFinished;
        currentCutscene.Stop();
        OnCutsceneFinished(currentCutscene);
    }

    public void ShowScript7ChoicePopup(string zone)

    {
        if (script7PositionPanel!=null) script7PositionPanel.SetActive(false);
        if (script7ChoicePopupPanel != null)
        {
            script7ChoicePopupPanel.SetActive(true);

            desperateButton.onClick.RemoveAllListeners();
            sorrowfulButton.onClick.RemoveAllListeners();

            desperateButton.onClick.AddListener(() => PlayScript7Cutscene(zone, "Desperate"));
            sorrowfulButton.onClick.AddListener(() => PlayScript7Cutscene(zone, "Sorrowful"));

            if (stageCamera != null)
            {
                stageCamera.SetActive(true);
                if (mainCamera != null) mainCamera.SetActive(false);
            }
        }
        else
        {
            Debug.LogError("Script 7 choice popup panel is not assigned!");
        }
    }
    private void PlayScript7Cutscene(string zone, string emotion)
    {
        // Hide the Script7ChoicePopup panel
        script7ChoicePopupPanel.SetActive(false);

        // Disable the carpet GameObject
        if (carpetObject != null)
        {
            carpetObject.SetActive(false);
            Debug.Log("Carpet GameObject disabled.");
        }

        // Disable the BedZone GameObject
        if (bedZone != null)
        {
            bedZone.SetActive(false);
            Debug.Log("BedZone GameObject disabled.");
        }

        // Disable the Bed outline component
        Outline bedOutline = GameObject.FindWithTag("Bed")?.GetComponent<Outline>();
        if (bedOutline != null)
        {
            bedOutline.enabled = false;
            Debug.Log("Bed outline disabled.");
        }

        // Set the flag to indicate that Cutscene7 has started
        isCutscene7Started = true;
        Debug.Log("Cutscene7 started. Bed outline and snap points disabled.");

        // Play the selected cutscene
        string cutsceneType = $"{zone}{emotion}"; // e.g., "BedDesperate" or "CarpetSorrowful"
        PlayCutscene(cutsceneType);
    }


    public void HideScript7ChoicePopup()
    {
        if (script7ChoicePopupPanel != null)
        {
            script7ChoicePopupPanel.SetActive(false);
        }
    }
   public void HideFinishPopup()
    {
        if (FinishedPopUp != null)
        {
            FinishedPopUp.SetActive(false);
            Debug.Log("Finished popup hidden from SceneSaverUI.");
        }
    }

    
    

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void PlayCutscene()
    {
        if (currentCutscene != null && isPaused)
        {
            currentCutscene.Play();
            isPaused = false;
            UpdateButtonVisibility();
        }
    }

    public void PauseCutscene()
    {
        if (currentCutscene != null && !isPaused)
        {
            currentCutscene.Pause();
            isPaused = true;
            UpdateButtonVisibility();
        }
    }

    private void UpdateButtonVisibility()
    {
        playButton.gameObject.SetActive(isPaused);
        pauseButton.gameObject.SetActive(!isPaused);
    }

}
