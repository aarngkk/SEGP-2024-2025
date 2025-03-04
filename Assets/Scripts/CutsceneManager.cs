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

        Debug.Log($"Playing {cutsceneType} cutscene: {currentCutscene.name}");

        currentCutscene.Play();

        StartCoroutine(WaitForCutscene(currentCutscene, onCutsceneEnd));
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
