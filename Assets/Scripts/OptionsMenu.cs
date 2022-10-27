using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject optionsMenu;
    main_character_2 char_script;
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
        if (char_script == null)
            try
            {
                char_script = GameObject.FindGameObjectWithTag("Player").GetComponent<main_character_2>();
            }
            catch
            {

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

    public void MoveLeft()
    {
        try
        {
            char_script.MoveLeft();
        }
        catch { }
    }
    public void MoveRight()
    {
        try
        {
            char_script.MoveRight();
        }
        catch { }
    }
    public void Jump()
    {
        try
        {
            char_script.Jump();
        }
        catch { }
    }
    public void Slide()
    {
        try
        {
            char_script.Slide();
        }
        catch { }
    }
    public void Attack()
    {
        try
        {
            char_script.meleeAttack();
        }
        catch { }
    }
    public void ThrowKunai()
    {
        try
        {
            char_script.ThrowKunaiAuto();
        }
        catch { }
    }
    public void CopySpell()
    {
        try
        {
            char_script.CopySpell();
        }
        catch { }
    }

    public void UpScale()
    {
        try
        {
            char_script.upScale();
        }
        catch { }
    }
    [SerializeField]
    private GameObject NotifyTable;
    [SerializeField]
    private TextMeshProUGUI NotificationText;
    public void ShowNotification(string content)
    {
        Time.timeScale = 0f;
        NotifyTable.SetActive(true);
        NotificationText.text = content;
    }
    public void CloseNotification()
    {
        Debug.Log("CloseNotification");
        NotifyTable.SetActive(false);
        Time.timeScale = 1f;

    }
    [SerializeField]
    private GameObject kunaiButton;
    public void EnableKunaiButton()
    {
        kunaiButton.SetActive(true);
    }
}
