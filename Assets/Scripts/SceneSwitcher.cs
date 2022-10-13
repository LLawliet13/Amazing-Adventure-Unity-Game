using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    // Start is called before the first frame update
    public static void LoadSceneMode(int index)
    {

        SceneManager.LoadScene(index);
    }
    private static int currentStage = 0;
    public int CurrentStage { get; set; }
    public static void Restart()
    {
        SceneManager.LoadScene(currentStage);
    }
    
}
