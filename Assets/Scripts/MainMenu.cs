using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    public void CreateNewScene()
    {
        SceneManager.LoadScene("New Scene");
    }

    public void QuitApp()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }

}
