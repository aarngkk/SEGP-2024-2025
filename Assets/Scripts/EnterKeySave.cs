using UnityEngine;

public class EnterKeySave : MonoBehaviour
{
    // Reference to SceneSaverUI so we can call OnSaveButtonClicked()
    public SceneSaverUI sceneSaverUI;

    void Update()
    {
        // Check if the user pressed Enter/Return OR the keypad Enter
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            // Make sure we have a reference to sceneSaverUI
            if (sceneSaverUI != null)
            {
                Debug.Log("Enter key pressed. Invoking OnSaveButtonClicked().");
                sceneSaverUI.OnSaveButtonClicked();
            }
            else
            {
                Debug.LogWarning("sceneSaverUI reference is not assigned on EnterKeySave!");
            }
        }
    }
}
