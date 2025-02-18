using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHoverSmooth : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public Vector3 hoverScale = new Vector3(1.2f, 1.2f, 1f);
    private Vector3 originalScale;
    private Vector3 targetScale;
    
    public float smoothSpeed = 15f; // Adjust for smoother or faster transitions

    private Button btn;
    private bool isPressed = false;

    private void Start()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;
        btn = GetComponent<Button>();
    }

    private void Update()
    {
        // Smoothly interpolate current scale towards target scale.
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * smoothSpeed);
    }

    // Called when the pointer enters the button area.
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (btn != null && !btn.interactable)
            return;
        
        // Only change target scale if not pressed
        if (!isPressed)
            targetScale = hoverScale;
    }

    // Called when the pointer exits the button area.
    public void OnPointerExit(PointerEventData eventData)
    {
        if (btn != null && !btn.interactable)
            return;
        
        // Only revert scale if not pressed
        if (!isPressed)
            targetScale = originalScale;
    }

    // Called when the pointer is pressed down on the button.
    public void OnPointerDown(PointerEventData eventData)
    {
        if (btn != null && !btn.interactable)
            return;
        
        isPressed = true;
        targetScale = hoverScale; // Keep the button at the hover scale while pressed.
    }

    // Called when the pointer is released.
    public void OnPointerUp(PointerEventData eventData)
    {
        if (btn != null && !btn.interactable)
            return;
        
        isPressed = false;
        targetScale = originalScale; // Revert back to original scale.
    }
}
