using UnityEngine;

public class DragCharacter : MonoBehaviour
{
    private Vector3 offset;
    private Camera cam;
    private Rigidbody rb;
    private bool isDragging = false;
    private static DragCharacter selectedCharacter = null; // Track the selected character
    private Plane groundPlane;

    private SkinnedMeshRenderer characterRenderer; // Now using SkinnedMeshRenderer
    private Material characterMaterial; // Store the material reference
    private Color originalColor;
    private Color originalEmission;

    public Color highlightColor = Color.yellow; // Highlight color
    public float highlightIntensity = 2.5f; // Emission intensity for selection glow
    private Vector3 originalPosition; // Store the original position

    private Bounds stageBounds; // Stores the stage area

    void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();
        groundPlane = new Plane(Vector3.up, Vector3.zero); // Define a flat plane at y=0

        // Store the original position at the start
        originalPosition = transform.position;

        // Try to find the SkinnedMeshRenderer in child objects
        characterRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        if (characterRenderer != null)
        {
            characterMaterial = characterRenderer.material; // Get the material instance
            if (characterMaterial.HasProperty("_Color"))
            {
                originalColor = characterMaterial.color; // Store original color
            }
            if (characterMaterial.HasProperty("_EmissionColor"))
            {
                originalEmission = characterMaterial.GetColor("_EmissionColor"); // Store original emission color
            }
        }

        // Find the stage and get its bounds
        GameObject stage = GameObject.FindWithTag("Stage"); // Ensure the stage has the "Stage" tag
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
            direction.y = 0; // Keep the character upright
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
        if (Input.GetMouseButton(0)) // Left click to select
        {
            SelectCharacter();
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
            targetPosition.y = transform.position.y; // Keep original Y position

            // Clamp position within stage bounds
            targetPosition.x = Mathf.Clamp(targetPosition.x, stageBounds.min.x, stageBounds.max.x);
            targetPosition.z = Mathf.Clamp(targetPosition.z, stageBounds.min.z, stageBounds.max.z);

            rb.MovePosition(targetPosition);
        }
    }

    void OnMouseUp()
    {
        isDragging = false;
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
        // Deselect the previously selected character
        if (selectedCharacter != null)
        {
            selectedCharacter.DeselectCharacter();
        }

        // Select the new character
        selectedCharacter = this;

        if (characterMaterial != null)
        {
            if (characterMaterial.HasProperty("_Color"))
            {
                characterMaterial.color = highlightColor; // Change color
            }

            if (characterMaterial.HasProperty("_EmissionColor"))
            {
                characterMaterial.SetColor("_EmissionColor", highlightColor * highlightIntensity);
                characterMaterial.EnableKeyword("_EMISSION");
            }
        }
    }

    private void DeselectCharacter()
    {
        if (selectedCharacter == this)
        {
            if (characterMaterial != null)
            {
                if (characterMaterial.HasProperty("_Color"))
                {
                    characterMaterial.color = originalColor; // Revert color
                }

                if (characterMaterial.HasProperty("_EmissionColor"))
                {
                    characterMaterial.SetColor("_EmissionColor", originalEmission);
                }
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
        // Optionally, reset rotation too:
        // transform.rotation = Quaternion.identity;
    }
}
