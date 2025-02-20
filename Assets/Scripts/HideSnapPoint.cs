using UnityEngine;

public class SnapPointIndicator : MonoBehaviour
{
    public GameObject indicator; // Assign the SnapIndicator plane here

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<DragCharacter>())
        {
            indicator.SetActive(false); // Hide indicator when character enters
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<DragCharacter>())
        {
            indicator.SetActive(true); // Show indicator when character leaves
        }
    }
}
