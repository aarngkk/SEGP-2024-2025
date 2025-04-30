using UnityEngine;

public class ReplayCharacter : MonoBehaviour
{
    private Vector3 startPosition;
    private Quaternion startRotation;
    private Vector3 offset;
    private Camera cam;
    private Rigidbody rb;
    private static DragCharacter selectedCharacter = null;
    public static DragCharacter SelectedCharacter => selectedCharacter; // Allows CameraController to check selection
    private Plane groundPlane;

    private Outline outline; // Reference to Quick Outline component
    private Vector3 originalPosition;
    private Bounds stageBounds;

    private Transform parentObject; // New: Reference to parent group

    void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();
        groundPlane = new Plane(Vector3.up, Vector3.zero);
        originalPosition = transform.position;

        outline = GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = false;
        }

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

        Outline bedOutline = GameObject.FindWithTag("Bed").GetComponent<Outline>();
        Outline dresserOutline = GameObject.FindWithTag("Dresser").GetComponent<Outline>();
        bedOutline.enabled = false;
        dresserOutline.enabled = false;
    }

 


   

    public void ResetPosition()
    {
        parentObject.position = originalPosition;
    }

}
