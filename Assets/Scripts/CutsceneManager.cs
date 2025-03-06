using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI; // Import UI namespace
using System.Collections;

public class CutsceneManager : MonoBehaviour
{
    public PlayableDirector bedCutscene;
    public PlayableDirector dresserCutscene;
    public PlayableDirector bedSadCutscene;
    public PlayableDirector bedAngryCutscene;
    public PlayableDirector dresserSadCutscene;
    public PlayableDirector dresserAngryCutscene;
    private PlayableDirector currentCutscene;

    public GameObject choicePopupPanel; // Assign in Inspector
    public Button sadButton; // Assign in Inspector
    public Button angryButton; // Assign in Inspector


    public void PlayCutscene(string cutsceneType, System.Action onCutsceneEnd = null)
    {
        Debug.Log($"Attempting to play cutscene: {cutsceneType}");

        // Ensure the Animators are re-enabled before playing a new cutscene
        RestoreCharacterAnimation();

        switch (cutsceneType)
        {
            case "Bed":
                currentCutscene = bedCutscene;
                break;
            case "Dresser":
                currentCutscene = dresserCutscene;
                break;
            case "BedSad":
                currentCutscene = bedSadCutscene;
                break;
            case "BedAngry":
                currentCutscene = bedAngryCutscene;
                break;
            case "DresserSad":
                currentCutscene = dresserSadCutscene;
                break;
            case "DresserAngry":
                currentCutscene = dresserAngryCutscene;
                break;
            default:
                Debug.LogWarning("Invalid cutscene type!");
                onCutsceneEnd?.Invoke();
                return;
        }

        Debug.Log($"Playing cutscene: {currentCutscene.name}");

        currentCutscene.stopped += OnCutsceneFinished;
        currentCutscene.Play();
    }
    private void RestoreCharacterAnimation()
    {
        Animator julietAnimator = GameObject.Find("Juliet")?.GetComponent<Animator>();
        Animator ladyCapuletAnimator = GameObject.Find("Lady Capulet")?.GetComponent<Animator>();

        if (julietAnimator != null)
        {
            julietAnimator.enabled = true;  // Re-enable the Animator so animations work again
        }
        if (ladyCapuletAnimator != null)
        {
            ladyCapuletAnimator.enabled = true;
        }
    }

    private void OnCutsceneFinished(PlayableDirector director)
    {
        Debug.Log("Cutscene finished playing.");
        director.stopped -= OnCutsceneFinished;

        // Disable Animator to freeze characters in final pose
        FreezeCharacterPose();

        // Show the choice popup
        ShowChoicePopup();
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

            // Assign button actions based on previous choice
            sadButton.onClick.RemoveAllListeners();
            angryButton.onClick.RemoveAllListeners();

            if (currentCutscene == bedCutscene)
            {
                sadButton.onClick.AddListener(() => PlayNextCutscene("BedSad"));
                angryButton.onClick.AddListener(() => PlayNextCutscene("BedAngry"));
            }
            else if (currentCutscene == dresserCutscene)
            {
                sadButton.onClick.AddListener(() => PlayNextCutscene("DresserSad"));
                angryButton.onClick.AddListener(() => PlayNextCutscene("DresserAngry"));
            }
        }
        else
        {
            Debug.LogError("Choice popup panel is not assigned!");
        }
    }

    private void PlayNextCutscene(string nextCutsceneType)
    {
        choicePopupPanel.SetActive(false);
        PlayCutscene(nextCutsceneType);
    }
}
