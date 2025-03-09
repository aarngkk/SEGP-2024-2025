using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    public PlayableDirector bedCutscene;
    public PlayableDirector dresserCutscene;
    public PlayableDirector bedSadCutscene;
    public PlayableDirector bedAngryCutscene;
    public PlayableDirector dresserSadCutscene;
    public PlayableDirector dresserAngryCutscene;
    public PlayableDirector capuletSympatheticCutscene; // New
    public PlayableDirector capuletAnnoyedCutscene; // New

    private PlayableDirector currentCutscene;

    public GameObject choicePopupPanel; // Assign in Inspector (First choice: Bed/Dresser)
    public GameObject capuletChoicePopupPanel; // Assign in Inspector (Second choice: Sympathetic/Annoyed)

    public Button sadButton; // Assign in Inspector
    public Button angryButton; // Assign in Inspector
    public Button sympatheticButton; // New
    public Button annoyedButton; // New

    public void PlayCutscene(string cutsceneType, System.Action onCutsceneEnd = null)
    {
        Debug.Log($"Attempting to play cutscene: {cutsceneType}");

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
            case "CapuletSympathetic":
                currentCutscene = capuletSympatheticCutscene;
                break;
            case "CapuletAnnoyed":
                currentCutscene = capuletAnnoyedCutscene;
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

        if (julietAnimator != null) julietAnimator.enabled = true;
        if (ladyCapuletAnimator != null) ladyCapuletAnimator.enabled = true;
    }

    private void OnCutsceneFinished(PlayableDirector director)
    {
        Debug.Log("Cutscene finished playing.");
        director.stopped -= OnCutsceneFinished;

        FreezeCharacterPose();

        // Show the FIRST choice popup after the initial bed or dresser cutscene
        if (currentCutscene == bedCutscene || currentCutscene == dresserCutscene)
        {
            ShowChoicePopup();
        }
        // Show the SECOND choice popup after the sad/angry Juliet scene
        else if (currentCutscene == bedSadCutscene || currentCutscene == bedAngryCutscene ||
                 currentCutscene == dresserSadCutscene || currentCutscene == dresserAngryCutscene)
        {
            ShowCapuletChoicePopup();
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

    private void PlayNextCutscene(string nextCutsceneType)
    {
        choicePopupPanel.SetActive(false);
        capuletChoicePopupPanel.SetActive(false);
        PlayCutscene(nextCutsceneType);
    }
}
