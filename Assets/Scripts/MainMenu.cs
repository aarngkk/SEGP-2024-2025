using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Loads the "New Scene" menu
    public void CreateNewScene()
    {
        SceneManager.LoadScene("New Scene");
    }

    // Handles quitting the application
    public void QuitApp()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }

}
