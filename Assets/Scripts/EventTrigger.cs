using UnityEngine;

public class EventTrigger : MonoBehaviour
{
    public Transform snapPoint; // Position where character should snap
    public bool useSnapPointRotation = true; // Toggle to use snap point rotation or not
    public Vector3 customLockedRotation = Vector3.zero; // Custom rotation

    private DragCharacter draggedCharacter; // Store reference to dragged character
    private Animator characterAnimator; // Animator reference

    private void OnTriggerEnter(Collider other)
    {
        DragCharacter dragScript = other.GetComponent<DragCharacter>();
        if (dragScript != null)
        {
            LockCharacter(other.gameObject, dragScript);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (draggedCharacter != null && other.gameObject == draggedCharacter.gameObject)
        {
            Vector3 newPosition = snapPoint.position;
            newPosition.y = draggedCharacter.transform.position.y; // Keep character's current ground level
            draggedCharacter.transform.position = newPosition;

            // Apply rotation based on user setting
            if (useSnapPointRotation)
            {
                draggedCharacter.transform.rotation = snapPoint.rotation;
            }
            else
            {
                draggedCharacter.transform.rotation = Quaternion.Euler(customLockedRotation);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (draggedCharacter != null && other.gameObject == draggedCharacter.gameObject)
        {
            draggedCharacter.enabled = true; // Re-enable dragging

            // Reset to Idle animation when leaving the snap point
            if (characterAnimator != null)
            {
                characterAnimator.SetTrigger("Idle");
            }

            draggedCharacter = null; // Reset reference
        }
    }

    private void LockCharacter(GameObject character, DragCharacter dragScript)
    {
        draggedCharacter = dragScript;
        draggedCharacter.enabled = false; // Disable dragging

        // Get Animator component from the character
        characterAnimator = character.GetComponent<Animator>();
        if (characterAnimator != null)
        {
            characterAnimator.SetTrigger("Walking"); // Play walking animation
        }

        // Snap to the right position (keeping the current Y level)
        Vector3 newPosition = snapPoint.position;
        newPosition.y = draggedCharacter.transform.position.y;
        draggedCharacter.transform.position = newPosition;

        // Apply rotation
        if (useSnapPointRotation)
        {
            draggedCharacter.transform.rotation = snapPoint.rotation;
        }
        else
        {
            draggedCharacter.transform.rotation = Quaternion.Euler(customLockedRotation);
        }

        Debug.Log(character.name + " locked in place at " + gameObject.name);
    }
}
