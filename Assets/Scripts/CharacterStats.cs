using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    // Start is called before the first frame update
    public int HP = 10;
    public int DEF = 10;
    [SerializeField]
    private RectTransform HpBarRect;

    [SerializeField]
    private TextMeshProUGUI HpText;
    private int currentHP;
    void Start()
    {
        
        if (HpBarRect == null)
        {
            HpBarRect = GameObject.FindGameObjectWithTag("HpBar_Character").GetComponent<RectTransform>();
            Debug.Log(HpBarRect);
        }
        if (HpText == null)
        {
            HpText = GameObject.FindGameObjectWithTag("HpText_Character").GetComponent<TextMeshProUGUI>();
            Debug.Log(HpText);
        }
        currentHP = HP;
        setHp(HP, HP);

    }

    // Update is called once per frame
   
    void Update()
    {
        if (HpBarRect == null)
        {
            Debug.LogError("no barHp reference");

        }
        if (HpText == null)
        {
            Debug.LogError("no HpText reference");

        }

        if (currentHP <= 0||transform.position.y<=-100)
        {
            setHp(0, HP);
            GameMasterController.KillAndRespawnCharacter(gameObject);

        }
    }
    public IEnumerator RespawnCharacter(GameObject gameObject, Vector3 RespawnPoint)
    {
        Debug.Log("Respawnding");
        yield return new WaitForSeconds(3);
        gameObject.SetActive(true);
        gameObject.transform.position = RespawnPoint;
    }
    public void setHp(int current, int max)
    {
        float value = (float)current / max;
        HpBarRect.localScale = new Vector3(value, HpBarRect.localScale.y, HpBarRect.localScale.z);
        HpText.text = current + "/" + max + " HP";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.tag == "Kunai")
        //{
        //    this.HP -= (int)WeaponStats.kunai / DEF;
        //    Debug.Log("Damaged");
        //}
        if (collision.gameObject.tag != "Ground" && collision.gameObject.tag != "Kunai")
        {
            this.currentHP -= (int)WeaponStats.fireball / DEF;
            setHp(currentHP, HP);
            Debug.Log(this.currentHP);
        }
    }

}
