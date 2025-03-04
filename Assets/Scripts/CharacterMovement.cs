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
    }

    void Update()
    {
        if (Input.GetMouseButton(1) && selectedCharacter == this && GetMouseWorldPosition(out Vector3 mouseWorldPos))
        {
            Vector3 direction = mouseWorldPos - parentObject.position;
            direction.y = 0;
            if (direction.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                parentObject.rotation = Quaternion.Slerp(parentObject.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }

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
            startPosition = parentObject.position;
            startRotation = parentObject.rotation;

            if (GetMouseWorldPosition(out Vector3 worldPosition))
            {
                offset = parentObject.position - worldPosition;
                isDragging = true;
            }
        }
    }

    void OnMouseDrag()
    {
        if (isDragging && selectedCharacter == this && GetMouseWorldPosition(out Vector3 worldPosition))
        {
            Vector3 targetPosition = worldPosition + offset;
            targetPosition.y = parentObject.position.y;

            targetPosition.x = Mathf.Clamp(targetPosition.x, stageBounds.min.x, stageBounds.max.x);
            targetPosition.z = Mathf.Clamp(targetPosition.z, stageBounds.min.z, stageBounds.max.z);

            rb.MovePosition(targetPosition);
        }
    }

    void OnMouseUp()
    {
        isDragging = false;

        if (undoRedoManager != null)
        {
            Vector3 endPosition = parentObject.position;
            Quaternion endRotation = parentObject.rotation;
            undoRedoManager.RecordMove(this, startPosition, startRotation, endPosition, endRotation);
        }

        CheckSnapPoint();
    }

    void CheckSnapPoint()
    {
        Collider[] colliders = Physics.OverlapSphere(parentObject.position, 0.5f);
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("BedZone"))
            {
                Debug.Log("Snapped to Bed");
                FindObjectOfType<PlaySceneButton>().SetCurrentSnapPoint("Bed");
                return;
            }
            else if (col.CompareTag("DresserZone"))
            {
                Debug.Log("Snapped to Dresser");
                FindObjectOfType<PlaySceneButton>().SetCurrentSnapPoint("Dresser");
                return;
            }
        }

        Debug.Log("Not snapped to a valid point.");
        FindObjectOfType<PlaySceneButton>().SetCurrentSnapPoint(""); // Disable play button
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
        if (selectedCharacter != null)
        {
            selectedCharacter.DeselectCharacter();
        }

        selectedCharacter = this;

        if (outline != null)
        {
            outline.enabled = true;
        }
    }

    private void DeselectCharacter()
    {
        if (selectedCharacter == this)
        {
            if (outline != null)
            {
                outline.enabled = false;
            }

            selectedCharacter = null;
        }
    }

    public void ResetPosition()
    {
        parentObject.position = originalPosition;
    }
}
