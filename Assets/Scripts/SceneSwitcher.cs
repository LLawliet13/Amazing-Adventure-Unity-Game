using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public int nextScene;
    public int prevScene;
    public int currentScene;
    // Start is called before the first frame update
    public void LoadNextScene()
    {

        SceneManager.LoadScene(nextScene);
    }
    public void LoadPrevScene()
    {
        if (prevScene < 0) Application.Quit();  
        else
        SceneManager.LoadScene(prevScene);
    }
    public void Restart()
    {
        SceneManager.LoadScene(currentScene);
    }
    public void Quit()
    {
        Application.Quit();
    }
    
}
