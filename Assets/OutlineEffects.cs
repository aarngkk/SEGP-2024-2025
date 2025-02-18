using UnityEngine;

public class OutlineEffect : MonoBehaviour
{
    private Renderer characterRenderer;
    private Material originalMaterial;
    private Material outlineMaterial;

    public Color outlineColor = Color.yellow;
    public float outlineWidth = 0.05f;

    void Start()
    {
        characterRenderer = GetComponentInChildren<Renderer>();
        if (characterRenderer != null)
        {
            originalMaterial = characterRenderer.material;

            // Create a new material for the outline effect
            outlineMaterial = new Material(Shader.Find("Outlined/Standard"));
            outlineMaterial.CopyPropertiesFromMaterial(originalMaterial);
            outlineMaterial.SetColor("_OutlineColor", outlineColor);
            outlineMaterial.SetFloat("_OutlineWidth", outlineWidth);

            DisableOutline();
        }
    }

    public void EnableOutline()
    {
        if (characterRenderer != null)
        {
            characterRenderer.material = outlineMaterial;
        }
    }

    public void DisableOutline()
    {
        if (characterRenderer != null)
        {
            characterRenderer.material = originalMaterial;
        }
    }
}
