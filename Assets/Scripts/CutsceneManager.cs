using UnityEngine;
using UnityEngine.Playables;
using System.Collections;

public class CutsceneManager : MonoBehaviour
{
    public PlayableDirector bedCutscene;     // Assign in Inspector
    public PlayableDirector dresserCutscene; // Assign in Inspector

    private PlayableDirector currentCutscene;

    private Vector3 julietOriginalPos, ladyCapuletOriginalPos;
    private Quaternion julietOriginalRot, ladyCapuletOriginalRot;

    public void PlayCutscene(string cutsceneType, System.Action onCutsceneEnd)
    {
        Debug.Log($"Attempting to play cutscene: {cutsceneType}");

        if (cutsceneType == "Bed" && bedCutscene != null)
        {
            currentCutscene = bedCutscene;
        }
        else if (cutsceneType == "Dresser" && dresserCutscene != null)
        {
            currentCutscene = dresserCutscene;
        }
        else
        {
            Debug.LogWarning("Invalid cutscene type or cutscene not assigned!");
            onCutsceneEnd?.Invoke();
            return;
        }

        // Find characters in the scene
        GameObject juliet = GameObject.FindWithTag("Juliet");
        GameObject ladyCapulet = GameObject.FindWithTag("LadyCapulet");

        if (juliet != null && ladyCapulet != null)
        {
            // Store original positions
            julietOriginalPos = juliet.transform.position;
            ladyCapuletOriginalPos = ladyCapulet.transform.position;
            julietOriginalRot = juliet.transform.rotation;
            ladyCapuletOriginalRot = ladyCapulet.transform.rotation;

            // Move characters to preset positions
            MoveToCutscenePosition(juliet, "JulietCutscene");
            MoveToCutscenePosition(ladyCapulet, "LadyCapuletCutscene");
        }
        else
        {
            Debug.LogWarning("Juliet or Lady Capulet not found in scene!");
        }

        // Play cutscene
        currentCutscene.Play();

        // Wait for cutscene to end, then reset positions
        StartCoroutine(WaitForCutscene(currentCutscene, onCutsceneEnd));
    }

    private void MoveToCutscenePosition(GameObject character, string cutscenePositionTag)
    {
        GameObject cutscenePosition = GameObject.FindWithTag(cutscenePositionTag);
        if (cutscenePosition != null)
        {
            character.transform.position = cutscenePosition.transform.position;
            character.transform.rotation = cutscenePosition.transform.rotation;
        }
        else
        {
            Debug.LogWarning($"Cutscene position {cutscenePositionTag} not found!");
        }
    }

    private IEnumerator WaitForCutscene(PlayableDirector director, System.Action onCutsceneEnd)
    {
        while (director.state == PlayState.Playing)
        {
            yield return null;
        }

        // Restore original positions
        GameObject juliet = GameObject.FindWithTag("Juliet");
        GameObject ladyCapulet = GameObject.FindWithTag("LadyCapulet");

        if (juliet != null && ladyCapulet != null)
        {
            juliet.transform.position = julietOriginalPos;
            ladyCapulet.transform.position = ladyCapuletOriginalPos;
            juliet.transform.rotation = julietOriginalRot;
            ladyCapulet.transform.rotation = ladyCapuletOriginalRot;
        }

        // Call next action after cutscene
        onCutsceneEnd?.Invoke();
    }
}
