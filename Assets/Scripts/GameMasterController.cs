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
    }
    [SerializeField]
    public GameObject MainCharacter;
    [SerializeField]
    public Transform RespawnPoint;
    public IEnumerator RespawnCharacter()
    {
        yield return new WaitForSeconds(2);
        Instantiate(MainCharacter, RespawnPoint.position, Quaternion.identity);
    }
    public static void KillAndRespawnCharacter(GameObject player)
    {
        Destroy(player);
        gm.StartCoroutine(gm.RespawnCharacter());
    }

    public GameObject UI;
    bool isObtainKunai = false;
    public void ObtainKunai()
    {
        if(GameObject.Find("Dragon") == null&&isObtainKunai == false)
        {
            OptionsMenu om = UI.GetComponent<OptionsMenu>();
            om.ShowNotification("Bạn Nhận Được Kunai");
            om.EnableKunaiButton();
            isObtainKunai=true;
        }
    }
    public void ChangeSavePoint(Transform t)
    {
        RespawnPoint = t;
    }


}
