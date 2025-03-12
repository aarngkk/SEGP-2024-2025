using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
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

    private PlayableDirector currentCutscene;

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
            case "CapuletEnraged":
                currentCutscene = capuletEnragedCutscene;
                break;
            case "CapuletComposed":
                currentCutscene = capuletComposedCutscene;
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

            sadButton.onClick.AddListener(() => PlayNextCutscene("BedSad"));
            angryButton.onClick.AddListener(() => PlayNextCutscene("BedAngry"));
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

            kneelButton.onClick.AddListener(() => Debug.Log("Juliet chooses to kneel."));
            standButton.onClick.AddListener(() => Debug.Log("Juliet refuses to kneel."));
        }
        else
        {
            Debug.LogError("Juliet kneeling choice popup panel is not assigned!");
        }
    }

    private void PlayNextCutscene(string nextCutsceneType)
    {
        choicePopupPanel?.SetActive(false);
        capuletChoicePopupPanel?.SetActive(false);
        capuletFinalChoicePopupPanel?.SetActive(false);
        julietKneelingChoicePopupPanel?.SetActive(false);

        PlayCutscene(nextCutsceneType);
    }
}
