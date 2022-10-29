using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.FilePathAttribute;
using UnityEngine.TextCore.Text;

public class GameMasterController : MonoBehaviour
{
    public static GameMasterController gm;
    // Start is called before the first frame update
    void Start()
    {
        if (gm == null)
        {
            gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMasterController>();
        }
    }
    
    public static void Respawn(Transform character,Vector3 location,Quaternion quaternion)
    {
        Instantiate(character, location,quaternion);
    }
    public static void Kill(GameObject gameObject)
    {
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        ObtainKunai();
        ObtainTransformSkill();
    }
    [SerializeField]
    public GameObject MainCharacter;
    [SerializeField]
    public Transform RespawnPoint;
    public IEnumerator RespawnCharacter()
    {
        yield return new WaitForSeconds(2);
        Instantiate(MainCharacter, RespawnPoint.position, Quaternion.identity);
        pendingAction();// khoi phuc cac skill cho player

    }
    public static void KillAndRespawnCharacter(GameObject player)
    {
        Destroy(player);
        gm.StartCoroutine(gm.RespawnCharacter());
    }

    public GameObject UI;
    bool isObtainTransform = false;
    bool isObtainKunai = false;
    public void ObtainKunai()
    {
        //if(GameObject.Find("SomeBoss") == null&& isObtainKunai == false)
        //{
        //    OptionsMenu om = UI.GetComponent<OptionsMenu>();
        //    om.ShowNotification("Bạn Nhận Được Kunai");
        //    om.EnableKunaiButton();
        //    isObtainKunai = true;
        //try
        //{
        //    //GameObject.FindGameObjectWithTag("Player").GetComponent<main_character_2>().canTransform = true;
        //}
        //catch
        //{
        //    Debug.LogError("No Character Found");
        //    isEnableKunaiSkill = true;
        //}
        //}
    }
    public void ObtainTransformSkill()
    {
        if (GameObject.Find("Dragon") == null && isObtainTransform == false)
        {
            OptionsMenu om = UI.GetComponent<OptionsMenu>();
            om.ShowNotification("Bạn Nhận Được Kĩ Năng Biến Hình");
            om.EnableTransformButton();
            isObtainTransform = true;
            try
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<main_character_2>().canTransform = true;
            }
            catch
            {
                Debug.LogError("No Character Found");
                isEnableTransformSkill = true;
            }
        }
    }
    bool isEnableTransformSkill = false;
    bool isEnableKunaiSkill = false;
    public void pendingAction()
    {
        //xu ly truong hop nhan vat chet sau do hoi sinh khong nhan dc skill sau khi giet quai
        if (isEnableTransformSkill)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<main_character_2>().canTransform = true;
        }
        if (isEnableKunaiSkill)
        {
            //GameObject.FindGameObjectWithTag("Player").GetComponent<main_character_2>().canTransform = true;
        }
    }
    public void ChangeSavePoint(Transform t)
    {
        RespawnPoint = t;
    }


}
