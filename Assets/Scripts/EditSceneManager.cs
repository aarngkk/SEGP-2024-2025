using UnityEngine;

public class EditSceneManager : MonoBehaviour
{
    private void Start()
    {
        // Get the saved scene data
        SceneData sceneData = SceneDataTransfer.Instance.SceneData;

        if (sceneData != null)
        {
            // Apply the saved data to the models in the scene
            ApplySceneData(sceneData);
        }
        else
        {
            Debug.LogError("No scene data found.");
        }
    }

    private void ApplySceneData(SceneData sceneData)
    {
        foreach (CharacterData characterData in sceneData.characters)
        {
            // Find the model by name
            GameObject model = GameObject.Find(characterData.characterName);

            if (model != null)
            {
                // Apply the saved position and rotation
                model.transform.position = characterData.position;
                model.transform.rotation = characterData.rotation;
            }
            else
            {
                Debug.LogWarning("Model not found: " + characterData.characterName);
            }
        }
    }
}