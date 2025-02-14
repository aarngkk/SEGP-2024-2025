using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHoverSmooth : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Vector3 hoverScale = new Vector3(1.2f, 1.2f, 1f);
    private Vector3 originalScale;
    private Vector3 targetScale;
    
    public float smoothSpeed = 15f; // Adjust for smoother or faster transitions

    private Button btn;

    private void Start()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;
        btn = GetComponent<Button>();
    }

    private void Update()
    {
        // Smoothly interpolate current scale towards target scale
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * smoothSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Only apply hover effect if the button is interactable.
        if (btn != null && !btn.interactable)
            return;
        
        targetScale = hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Only revert scale if the button is interactable.
        if (btn != null && !btn.interactable)
            return;
        
        targetScale = originalScale;
    }
}
