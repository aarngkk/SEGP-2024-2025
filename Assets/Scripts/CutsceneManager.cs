using UnityEngine;
using UnityEngine.Playables;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager Instance { get; private set; } // Singleton reference

    public PlayableDirector bedCutscene;
    public PlayableDirector dresserCutscene;

    private System.Action onCutsceneEnd; // Callback for when cutscene ends

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayCutscene(string snapPoint, System.Action onComplete = null)
    {
        Debug.Log($"Playing cutscene for: {snapPoint}");

        PlayableDirector director = null;

        if (snapPoint == "BedSnapPoint")
            director = bedCutscene;
        else if (snapPoint == "DresserSnapPoint")
            director = dresserCutscene;

        if (director != null)
        {
            onCutsceneEnd = onComplete; // Store callback for when cutscene ends
            director.stopped -= CutsceneFinished; // Ensure event isn't added multiple times
            director.stopped += CutsceneFinished; // Subscribe to event
            director.Play(); // Play the cutscene
            Debug.Log($"Cutscene {snapPoint} started.");
        }
        else
        {
            Debug.LogError("Cutscene not found!");
        }
    }

    private void CutsceneFinished(PlayableDirector director)
    {
        Debug.Log("Cutscene finished.");
        director.stopped -= CutsceneFinished; // Unsubscribe to prevent memory leaks

        onCutsceneEnd?.Invoke(); // Call scene transition
    }
}
