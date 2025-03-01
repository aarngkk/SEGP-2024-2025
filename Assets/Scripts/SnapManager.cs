using UnityEngine;
using UnityEngine.UI;

public class SnapManager : MonoBehaviour
{
    public Button playSceneButton; // Assign in Inspector
    private bool isCharacterSnapped = false;

    public void SetCharacterSnapped(bool snapped)
    {
        isCharacterSnapped = snapped;
        playSceneButton.interactable = snapped; // Enable/Disable the button
    }
}
