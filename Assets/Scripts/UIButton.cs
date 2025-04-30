using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Handles smooth scaling animations for button hover and click states
public class ButtonHoverSmooth : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public Vector3 hoverScale = new Vector3(1.2f, 1.2f, 1f); // Scale when hovered
    private Vector3 originalScale;                           // Stores the button's original scale
    private Vector3 targetScale;                             // Current target scale for smooth transition

    public float smoothSpeed = 15f; // Transition speed for scaling animation

    private Button btn;             // Reference to the Button component
    private bool isPressed = false; // Tracks if button is currently pressed

    private void Start()
    {
        // Cache initial scale and button reference
        originalScale = transform.localScale;
        targetScale = originalScale;
        btn = GetComponent<Button>();
    }

    private void Update()
    {
        // Smoothly animate scale towards target
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * smoothSpeed);
    }

    // Handle mouse enter hover state
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (btn != null && !btn.interactable)
            return;

        // Only scale up if not currently pressed
        if (!isPressed)
            targetScale = hoverScale;
    }

    // Handle mouse exit hover state
    public void OnPointerExit(PointerEventData eventData)
    {
        if (btn != null && !btn.interactable)
            return;

        // Only scale down if not currently pressed
        if (!isPressed)
            targetScale = originalScale;
    }

    // Handle mouse down click state
    public void OnPointerDown(PointerEventData eventData)
    {
        if (btn != null && !btn.interactable)
            return;
        
        isPressed = true;
        targetScale = hoverScale; // Maintain hover scale while pressed
    }

    // Handle mouse release state
    public void OnPointerUp(PointerEventData eventData)
    {
        if (btn != null && !btn.interactable)
            return;
        
        isPressed = false;
        targetScale = originalScale; // Return to normal scale
    }
}
