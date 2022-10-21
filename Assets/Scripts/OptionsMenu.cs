using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject optionsMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (isPaused)
            {
                Resume();
                isPaused = false;
            }
            else
            {
                Pause();
                isPaused = true;
            }
        }
    }
   
    public void Pause()
    {
        optionsMenu.SetActive(true);

        isPaused = true;
        Time.timeScale = 0f;
    }
    public void Resume()
    {
        optionsMenu.SetActive(false);

        isPaused = false;
        Time.timeScale = 1f;
    }
    public void Quit()
    {
        Application.Quit();
    }
}
