using UnityEngine;

public class DragCharacter : MonoBehaviour
{
    public UndoRedoManager undoRedoManager;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private Vector3 offset;
    private Camera cam;
    private Rigidbody rb;
    private bool isDragging = false;
    private static DragCharacter selectedCharacter = null;
    public static DragCharacter SelectedCharacter => selectedCharacter;
    private Plane groundPlane;

    private Outline outline;
    private Vector3 originalPosition;
    private Bounds stageBounds;
    private Transform parentObject;

    private Transform snapTarget = null; // New: Holds the closest snap point
    private SnapManager snapManager;

    void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();
        groundPlane = new Plane(Vector3.up, Vector3.zero);
        originalPosition = transform.position;
        snapManager = FindObjectOfType<SnapManager>();

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

        if (snapTarget != null)
        {
            parentObject.position = snapTarget.position;
            parentObject.rotation = snapTarget.rotation;

            // Find a SnapPointHighlightController in the snap target's parent
            SnapPointHighlightController snapHighlight = snapTarget.GetComponentInParent<SnapPointHighlightController>();
            if (snapHighlight != null)
            {
                snapHighlight.SetCharacterSnapped(true);
            }
        }

        if (undoRedoManager != null)
        {
            Vector3 endPosition = parentObject.position;
            Quaternion endRotation = parentObject.rotation;

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

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SnapPoint"))
        {
            snapTarget = other.transform;

            SnapPointHighlightController snapHighlight = other.GetComponentInParent<SnapPointHighlightController>();
            if (snapHighlight != null)
            {
                snapHighlight.SetCharacterSnapped(true);
            }

            // Notify SnapManager of the specific snap point
            if (snapManager != null)
            {
                snapManager.SetCharacterSnapped(true, other.gameObject.name); // Pass snap point name
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SnapPoint") && other.transform == snapTarget)
        {
            snapTarget = null;

            SnapPointHighlightController snapHighlight = other.GetComponentInParent<SnapPointHighlightController>();
            if (snapHighlight != null)
            {
                snapHighlight.SetCharacterSnapped(false);
            }

            // Notify SnapManager that no character is snapped
            if (snapManager != null)
            {
                snapManager.SetCharacterSnapped(false, "");
            }
        }
    }
}
