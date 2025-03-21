using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class CutsceneManager : MonoBehaviour
{
    private string SavePath => Application.persistentDataPath + "/SavedScenes/";
    public string logFileName; // Default filename if none is set

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

    public PlayableDirector currentCutscene;
    private List<string> choicesMade = new List<string>();
    private Stack<string> choiceHistory = new Stack<string>(); // Stores past choices
    public bool isReplayMode = false;

    public void PlayCutscene(string cutsceneType, System.Action onCutsceneEnd = null)
    {
        Debug.Log($"Attempting to play cutscene: {cutsceneType}");

        RestoreCharacterAnimation();

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

    private void PlayNextCutscene(string nextCutsceneType)
    {
        choicePopupPanel?.SetActive(false);
        capuletChoicePopupPanel?.SetActive(false);
        capuletFinalChoicePopupPanel?.SetActive(false);
        julietKneelingChoicePopupPanel?.SetActive(false);
        capuletThirdChoicePopupPanel?.SetActive(false);

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
            currentCutscene.stopped-=OnCutsceneFinished; //removes the listener
            currentCutscene.Stop();
            Debug.Log("Current cutscene stopped.");
        }
        /*if (choiceHistory.Count == 1) // Ensure there is a previous choice to revert to
        {
            string bedDresserChoice=choiceHistory.Pop();
            DragCharacter[] draggableCharacters = FindObjectsOfType<DragCharacter>();
            foreach (DragCharacter character in draggableCharacters)
            {
                character.ResetPosition();
            }
            return;
        }*/
        choiceHistory.Pop(); // Remove the latest choice
        string previousChoice = choiceHistory.Peek(); // Get the choice before it
        choicesMade.RemoveAt(choicesMade.Count-1);
        Debug.Log($"Undoing choice, returning to: {previousChoice}");

        PlayableDirector previousDirector = GameObject.Find(previousChoice)?.GetComponent<PlayableDirector>();
        if (previousDirector != null)
        {
            currentCutscene = previousDirector;
            currentCutscene.stopped+=OnCutsceneFinished; //reattach listener
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


}
