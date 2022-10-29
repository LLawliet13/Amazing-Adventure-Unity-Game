using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject optionsMenu;
    public GameObject NPCDialogPanel;
    public GameObject controlPanel;
    main_character_2 char_script;
    // Start is called before the first frame update
    void Start()
    {
        loadDialog();
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
    public bool moveLeft = false;
    public bool moveRight = false;
    public bool jump = false;
    public bool meleeAttack = false;
    public bool throwKunai = false;
    public bool dash = false;
    public void CancelMoveLeft()
    {
        moveLeft = false;
    }
    public void MoveLeft()
    {
        moveLeft = true;
    }
    public void CancelMoveRight()
    {
        moveRight = false;
    }

    public void MoveRight()
    {
        moveRight = true;
    }
    public void CancelJump()
    {
        jump = false;
    }
    public void Jump()
    {
        jump = true;
    }
    public void CancelSlide()
    {
        dash = false;
    }
    public void Slide()
    {
        dash = true;
    }
    public void Attack()
    {
        meleeAttack = true;
    }
    public void CancelAttack()
    {
        meleeAttack = false;
    }
    public void ThrowKunai()
    {
        throwKunai = true;
    }
    public void CancelThrowKunai()
    {
        throwKunai = false;
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
    Dictionary<string, NPCDialog> NPCDialogList = new Dictionary<string, NPCDialog>();


    private void loadDialog()
    {
        List<String> list = new List<String>();
        list.Add("Hãy đi theo con đường phía trước và hoàn thành các nhiệm vụ sau để hoàn thành khóa tốt nghiệp của con\n" +
            "-Tìm lại cuốn bí kíp abc\n" +
            "-Học được kĩ năng dùng kunai");
        NPCDialogList.Add("kakasi_say_start_game", new NPCDialog(list, "kakashi teacher"));
    }
    public void DialogTrigger(string name_of_dialog, int dialogIndex)
    {
        moveLeft = false;
        moveRight = false;
        jump = false;
        meleeAttack = false;
        throwKunai = false;
        dash = false;
        runDialog(NPCDialogList[name_of_dialog].Dialogs.ElementAt(dialogIndex)
                , NPCDialogList[name_of_dialog].NPCName);
    }
    public void EndDialog()
    {
        controlPanel.SetActive(true);
        NPCDialogPanel.SetActive(false);

    }
    [SerializeField]
    private TextMeshProUGUI CharNameText;
    [SerializeField]
    private TextMeshProUGUI DialogText;
    void DialogType(string dialog)
    {
        DialogText.text = dialog;
        //DialogText.text = "";
        //float nextTime = 0;
        //float delay = 0.2f;
        //char[] chars = dialog.ToCharArray();
        //for (int i = 0; i< chars.Length;i++)
        //{
        //    if (Time.time > nextTime)
        //    {
        //        DialogText.text += chars.ElementAt(i);
        //        nextTime = Time.time + delay;
        //    }
        //    else
        //    {
        //        i--;
        //    }
        //}
    }
    private void runDialog(string dialog, string characterName)
    {
        controlPanel.SetActive(false);
        NPCDialogPanel.SetActive(true);
        DialogText.text = "";
        CharNameText.SetText(characterName);
        DialogType(dialog);

    }
    private class NPCDialog
    {

        public List<String> Dialogs;
        public String NPCName;
        public NPCDialog(List<String> dialogs, string NPCName)
        {
            this.Dialogs = dialogs;
            this.NPCName = NPCName;

        }


    }
}

