using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaySceneButton : MonoBehaviour
{
    public void GoToPlayScene()
    {
        SceneManager.LoadScene("Play Scene"); // Replace with your actual scene name
    }
}
