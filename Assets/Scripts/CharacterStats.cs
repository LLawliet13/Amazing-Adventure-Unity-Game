using System;
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
        if (transform.tag == "Player")
        {

            if (HpBarRect == null)
            {
                HpBarRect = GameObject.FindGameObjectWithTag("HpBar_Character").GetComponent<RectTransform>();
            }
            if (HpText == null)
            {
                HpText = GameObject.FindGameObjectWithTag("HpText_Character").GetComponent<TextMeshProUGUI>();
            }
        }
        currentHP = HP;
        setHp(HP, HP);

    }

    // Update is called once per frame

    void Update()
    {
        //if (HpBarRect == null)
        //{
        //    Debug.LogError("no barHp reference");

        //}
        //if (HpText == null)
        //{
        //    Debug.LogError("no HpText reference");

        //}
        isCuuViTime();
        if (currentHP <= 0 || transform.position.y <= -100)
        {
            setHp(0, HP);
            GameMasterController.KillAndRespawnCharacter(gameObject);

        }
    }

    public void setHp(int current, int max)
    {
        float value = (float)current / max;
        if (transform.tag == "Player")
        {
            HpBarRect.localScale = new Vector3(value, HpBarRect.localScale.y, HpBarRect.localScale.z);
            HpText.text = current + "/" + max + " HP";
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.tag == "Kunai")
        //{
        //    this.HP -= (int)WeaponStats.kunai / DEF;
        //    Debug.Log("Damaged");
        //}
        //if (collision.gameObject.tag != "Ground" && collision.gameObject.tag != "Kunai"
        //    && collision.gameObject.tag != "NPC")
        //{
        //    this.currentHP -= (int)WeaponStats.fireball / DEF;
        //    setHp(currentHP, HP);
        //    isHpDecrease();
        //    //Debug.Log(this.currentHP);
        //}
    }
    public int ATK = 10;
    private float meleeATK { get => ATK * 0.8f; }

    public void GiveDamage(Collider2D collision)
    {
        collision.GetComponent<EnemyStats>().getDamage(meleeATK);

    }
    float timeTobeDecreaseHP = 0;


    public void isCuuViTime()
    {
        // tru hp khi trong trang thai cuu vi
        if (!GameObject.FindGameObjectWithTag("Player").GetComponent<main_character_2>().IsNaruto()
            &&Time.time>=timeTobeDecreaseHP)
        {
            currentHP = currentHP - 10;
            setHp(currentHP, HP);
            timeTobeDecreaseHP = Time.time + 1;
        }
    }

    internal void getDamage(int v)
    {
        this.currentHP -= (int)(v / DEF);
        setHp(currentHP, HP);
        isHpDecrease();
    }
    public void isHpDecrease()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<main_character_2>().ChangeAnimation("being_attacked");

    }
    public void heal(bool healFull, int x)
    {
        if (healFull == false)
        {

            this.currentHP += x;
            if (currentHP > HP) currentHP = HP;
        }
        else
        {
            this.currentHP = HP;
        }
        setHp(currentHP, HP);

    }
}
