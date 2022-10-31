using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;
    Transform batCave;
    // Start is called before the first frame update
    void Start()
    {
        batCave = transform.parent;
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    Vector3 beforeAttack;
    Vector3 target = Vector3.zero;
    public float attackRange = 2;
    public float attackSpeed = 30;
    public bool isRight = false;
    void Update()
    {
        if ((transform.position.x < batCave.position.x - 100) ||
            (transform.position.x > batCave.position.x + 100))
        {
            target = beforeAttack;
        }
        //di chuyen bat lien tuc+
        if (target != Vector3.zero)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, attackSpeed * Time.deltaTime);
            if (beforeAttack != target)
            {


                if ((target.x > transform.position.x && !isRight) ||
                (target.x < transform.position.x && isRight))
                {
                    transform.localScale = new Vector3(transform.localScale.x * -1,
                        transform.localScale.y, transform.localScale.z);
                    isRight = !isRight;
                }
                if (target == transform.position)
                {
                    target = beforeAttack;
                }
            }
            if (beforeAttack == transform.position)
            {
                target = Vector3.zero;
                isBack = true;
            }
        }
        if (target == Vector3.zero)
        {
            animator.Play("idle");
        }
        else
        {
            animator.Play("attack");
        }
        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        if (Player == null)
            return;
        if (Time.time > timeToAttack && isBack)
        {
            if (Vector2.Distance(Player.transform.position, transform.position) <= attackRange)
            {
                isBack = false;
                timeToAttack = Time.time + 3;
                beforeAttack = transform.position;
                target = Player.transform.position;
            }
        }


    }
    float timeToAttack = 0;
    bool isBack = true;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Player")
        {
            target = beforeAttack;
            EnemyStats es = transform.GetComponent<EnemyStats>();
            if (es == null)
            {
                Debug.LogError("Missing EnemyStats");
                return;
            }
            if (target != Vector3.zero)
                es.GiveDamage(collision);
        }
    }
}
