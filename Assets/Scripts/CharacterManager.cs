using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance;

    // Assign these in the Inspector (drag all 4 character colliders)
    public CapsuleCollider[] characterColliders;

    private CutsceneManager _cutsceneManager;

    void Awake()
    {
        Instance = this;
        _cutsceneManager = FindObjectOfType<CutsceneManager>();
    }

    // Call this whenever game state changes
    public void UpdateDraggableState()
    {
        if (_cutsceneManager == null)
            _cutsceneManager = FindObjectOfType<CutsceneManager>();

        bool allowDragging = (!_cutsceneManager.IsCutscene1Started() ||
                            (_cutsceneManager.IsCutscene6Finished() && !_cutsceneManager.IsCutscene7Started()));

        Debug.Log($"Updating draggable state. Allow dragging: {allowDragging} " +
                  $"(Cutscene1Started: {_cutsceneManager.IsCutscene1Started()}, " +
                  $"Cutscene6Finished: {_cutsceneManager.IsCutscene6Finished()}, " +
                  $"Cutscene7Started: {_cutsceneManager.IsCutscene7Started()})");

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