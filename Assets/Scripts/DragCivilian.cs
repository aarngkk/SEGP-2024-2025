using UnityEngine;

public class DragCharacter : MonoBehaviour
{
    public UndoRedoManager undoRedoManager; // Assign via Inspector or use a singleton pattern
    private Vector3 startPosition;
    private Quaternion startRotation;
    private Vector3 offset;
    private Camera cam;
    private Rigidbody rb;
    private bool isDragging = false;
    private static DragCharacter selectedCharacter = null;
    private Plane groundPlane;

    private Outline outline; // Reference to Quick Outline component
    private Vector3 originalPosition;
    private Bounds stageBounds;

    void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();
        groundPlane = new Plane(Vector3.up, Vector3.zero);
        originalPosition = transform.position;

        // Get the Outline component and disable it by default
        outline = GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = false;
        }

        // Get stage bounds
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
    }

    void Update()
    {
        // Rotate character to face the mouse when right-click is held
        if (Input.GetMouseButton(1) && selectedCharacter == this && GetMouseWorldPosition(out Vector3 mouseWorldPos))
        {
            Vector3 direction = mouseWorldPos - transform.position;
            direction.y = 0;
            if (direction.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }

        // Deselect if the player clicks elsewhere
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit hit) || hit.transform != transform)
            {
                DeselectCharacter();
            }
        }
    }

    void OnMouseDown()
    {
        if (Input.GetMouseButton(0))
        {
            SelectCharacter();
            // Store the transform data when dragging starts
            startPosition = transform.position;
            startRotation = transform.rotation;

            if (GetMouseWorldPosition(out Vector3 worldPosition))
            {
                offset = transform.position - worldPosition;
                isDragging = true;
            }
        }
    }

    void OnMouseDrag()
    {
        if (isDragging && selectedCharacter == this && GetMouseWorldPosition(out Vector3 worldPosition))
        {
            Vector3 targetPosition = worldPosition + offset;
            targetPosition.y = transform.position.y;

            // Clamp position within stage bounds
            targetPosition.x = Mathf.Clamp(targetPosition.x, stageBounds.min.x, stageBounds.max.x);
            targetPosition.z = Mathf.Clamp(targetPosition.z, stageBounds.min.z, stageBounds.max.z);

            rb.MovePosition(targetPosition);
        }
    }

    void OnMouseUp()
    {
        isDragging = false;
         // If we have an UndoRedoManager assigned, record the move
        if (undoRedoManager != null)
        {
            Vector3 endPosition = transform.position;
            Quaternion endRotation = transform.rotation;

            // Pass the old/new data to the manager
            undoRedoManager.RecordMove(this, startPosition, startRotation, endPosition, endRotation);
        }
    }

    private bool GetMouseWorldPosition(out Vector3 worldPosition)
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (groundPlane.Raycast(ray, out float distance))
        {
            worldPosition = ray.GetPoint(distance);
            return true;
        }
        worldPosition = Vector3.zero;
        return false;
    }

    private void SelectCharacter()
    {
        // Deselect previous character
        if (selectedCharacter != null)
        {
            selectedCharacter.DeselectCharacter();
        }

        selectedCharacter = this;

        // Enable the outline effect
        if (outline != null)
        {
            outline.enabled = true;
        }
    }

    private void DeselectCharacter()
    {
        if (selectedCharacter == this)
        {
            // Disable the outline effect
            if (outline != null)
            {
                outline.enabled = false;
            }

            selectedCharacter = null;
        }
    }

    /// <summary>
    /// Resets the character's position to its original position.
    /// </summary>
    public void ResetPosition()
    {
        transform.position = originalPosition;
    }
}
