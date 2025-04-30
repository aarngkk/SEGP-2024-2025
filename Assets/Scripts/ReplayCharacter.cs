using UnityEngine;

public class ReplayCharacter : MonoBehaviour
{
    private Vector3 startPosition;
    private Quaternion startRotation;
    private Vector3 offset;
    private Camera cam;
    private Rigidbody rb;
    private static DragCharacter selectedCharacter = null;
    public static DragCharacter SelectedCharacter => selectedCharacter; 
    private Plane groundPlane;

    private Outline outline; 
    private Vector3 originalPosition;
    private Bounds stageBounds;

    private Transform parentObject; 

    void Start()
    {
        // Initialize component references
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();
        groundPlane = new Plane(Vector3.up, Vector3.zero);
        originalPosition = transform.position;

        // Setup outline effect if available
        outline = GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = false;
        }

        // Find and validate stage boundaries
        GameObject stage = GameObject.FindWithTag("Stage");
        if (stage != null)
        {
            BoxCollider stageCollider = stage.GetComponent<BoxCollider>();
            if (stageCollider != null)
            {
                stageBounds = stageCollider.bounds;
            }
            else
            {
                Debug.LogError("Stage does not have a BoxCollider!");
            }
        }
        else
        {
            Debug.LogError("No GameObject with the 'Stage' tag found!");
        }

        // Find the parent GameObject (if this character has one)
        parentObject = transform.parent != null ? transform.parent : transform;

        // Disable furniture outlines at start
        Outline bedOutline = GameObject.FindWithTag("Bed").GetComponent<Outline>();
        Outline dresserOutline = GameObject.FindWithTag("Dresser").GetComponent<Outline>();
        bedOutline.enabled = false;
        dresserOutline.enabled = false;
    }

    // Resets character to original position
    public void ResetPosition()
    {
        parentObject.position = originalPosition;
    }

}
