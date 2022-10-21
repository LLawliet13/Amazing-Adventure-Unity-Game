using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : MonoBehaviour
{
    //
    List<string> unbreakable = new List<string>
    {
        "Boss_kakashi"
    };
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
        currentHP = HP;
        setHp(HP, HP);
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(HpBarRect == null)
        {
            Debug.LogError("no barHp reference");
            Debug.LogError(gameObject.name);

        }
        if (HpText == null)
        {
            Debug.LogError("no HpText reference");

        }

        if (currentHP <= 0) GameMasterController.Kill(gameObject);
    }
    public void setHp(int current, int max)
    {
        float value = (float)current/max;
        HpBarRect.localScale = new Vector3(value, HpBarRect.localScale.y, HpBarRect.localScale.z);
        HpText.text = current +"/"+max+" HP";
    }
    public void getDamage(int damage)
    {
        this.currentHP -= (int)(damage/DEF);
        setHp(currentHP, HP);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.tag == "Kunai")
        //{
        //    this.HP -= (int)WeaponStats.kunai / DEF;
        //    Debug.Log("Damaged");
        //}
        if(collision.gameObject.tag == "Kunai") {
            if (transform.name == "Boss_kakashi") return;// kakashi mien nhiem sat thuong cua kunai
        this.currentHP -= (int)WeaponStats.kunai / DEF;
        setHp(currentHP, HP);
        Debug.Log(this.currentHP);
        }
    }
}
