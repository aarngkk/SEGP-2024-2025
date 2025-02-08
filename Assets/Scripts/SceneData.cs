using System;
using UnityEngine;
[System.Serializable]
public class SceneData
{
    public string sceneName;
    public string selectedScript;
    // For each placeholder, you might store its name and transform data.
    // public List<PlaceholderData> placeholders = new List<PlaceholderData>();
}

// [System.Serializable]
// public class PlaceholderData
// {
//     public string id; // e.g., "Character1"
//     public Vector3 position;
//     public Quaternion rotation;
// }
