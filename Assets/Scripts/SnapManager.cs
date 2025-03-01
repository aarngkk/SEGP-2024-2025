using UnityEngine;
using UnityEngine.UI;

public class SnapManager : MonoBehaviour
{
    public Button playSceneButton;
    public PlaySceneButton playSceneButtonScript; // Assign in Inspector
    private string currentSnapPoint = "";

    void Start()
    {
        playSceneButton.interactable = false;
    }

    public void SetCharacterSnapped(bool snapped, string snapPointName)
    {
        Debug.Log($"SetCharacterSnapped called. Snapped: {snapped}, SnapPoint: {snapPointName}");

        playSceneButton.interactable = snapped;

        if (snapped)
        {
            currentSnapPoint = snapPointName;
            playSceneButtonScript.SetCurrentSnapPoint(snapPointName); // Inform PlayButton script
        }
        else
        {
            currentSnapPoint = "";
        }
    }
}
