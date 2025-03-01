using UnityEngine;

public class SnapPointHighlightController : MonoBehaviour
{
    private Outline outline;
    private bool isCharacterSnapped = false;

    void Start()
    {
        outline = GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = false; // Default: Not highlighted
        }
    }

    public void SetHighlight(bool highlight)
    {
        if (outline != null)
        {
            outline.enabled = highlight;
        }
    }

    public void SetCharacterSnapped(bool snapped)
    {
        isCharacterSnapped = snapped;
        SetHighlight(snapped); // Highlight when a character is snapped
    }
}
