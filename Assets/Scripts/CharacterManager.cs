using UnityEngine;

public class CharacterManager : MonoBehaviour
{   // Singleton instance for global access
    public static CharacterManager Instance;

    // Assign these in the Inspector (drag all 4 character colliders)
    public CapsuleCollider[] characterColliders;
    // Reference to the CutsceneManager
    private CutsceneManager _cutsceneManager;

    void Awake()
    {
        Instance = this;        // Set the static instance
        _cutsceneManager = FindObjectOfType<CutsceneManager>(); // Find CutsceneManager in the scene
    }

    // Call this method whenever the game state changes (e.g., when cutscenes start or end)
    public void UpdateDraggableState()
    {   // If CutsceneManager reference is lost, re-find it
        if (_cutsceneManager == null)
            _cutsceneManager = FindObjectOfType<CutsceneManager>();
        
        // Determine if characters should be draggable:
        // Allow dragging if cutscene 1 has NOT started OR cutscene 6 is finished and cutscene 7 has NOT started yet
        bool allowDragging = (!_cutsceneManager.IsCutscene1Started() ||
                            (_cutsceneManager.IsCutscene6Finished() && !_cutsceneManager.IsCutscene7Started()));
        
        // Debugging output to understand the current state
        Debug.Log($"Updating draggable state. Allow dragging: {allowDragging} " +
                  $"(Cutscene1Started: {_cutsceneManager.IsCutscene1Started()}, " +
                  $"Cutscene6Finished: {_cutsceneManager.IsCutscene6Finished()}, " +
                  $"Cutscene7Started: {_cutsceneManager.IsCutscene7Started()})");

         // Enable or disable colliders based on allowDragging
        foreach (var collider in characterColliders)
        {
            if (collider != null)
            {
                collider.enabled = allowDragging;
                Debug.Log($"Collider {collider.name} enabled: {collider.enabled}");
            }
        }
    }
}