using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bandit_Warrior : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isRight;
    Rigidbody2D rb;
    [SerializeField]
    private Canvas StatusSpace;
    public Transform leftRange;
    public Transform rightRange;
    Animator animator;
    void Start()
    {
        isRight = true;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        groundCheck();
        try
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        catch (Exception ex)
        {
            player = null;
        }
        if (player != null)
        {
            Physics2D.IgnoreCollision(player.GetComponent<CapsuleCollider2D>(), GetComponent<BoxCollider2D>(), true);
            Physics2D.IgnoreCollision(player.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>(), true);
        }
        if (rb.velocity.x < 0 && isRight)
        {
            animator.Play("run");
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            isRight = false;
            StatusSpace.transform.localScale = new Vector2(StatusSpace.transform.localScale.x * -1, StatusSpace.transform.localScale.y);
        }
        if (rb.velocity.x > 0 && !isRight)
        {
            animator.Play("run");
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            isRight = true;
            StatusSpace.transform.localScale = new Vector2(StatusSpace.transform.localScale.x * -1, StatusSpace.transform.localScale.y);
        }
        lookPlayer();
        follow();
        if (isFloating) animator.Play("jump");
    }
    public Transform player;
    public Transform attack_point;
    private float radius = 4;
    public LayerMask playerLayer;
    bool isFloating;
    public Transform GroundCheckPoint;
    public LayerMask grounds;
    float attackTime = 0;
    public void follow()
    {
        Debug.Log("player.position.x> transform.position.x" + player.position.x + "," + transform.position.x);
        Debug.Log("if else 2 ve left right" + (player.position.x > leftRange.TransformPoint(Vector3.zero).x) + "," + (player.position.x < rightRange.TransformPoint(Vector3.zero).x));
        if (player.position.x > leftRange.position.x && player.position.x < rightRange.position.x
            && Math.Abs(transform.position.y - player.position.y) <= 5)
        {

            Debug.Log("player.position.x> transform.position.x" + player.position.x+","+ transform.position.x);
            Vector3 p;
            if (player.position.x > transform.position.x)
            {
                p = new Vector3(player.position.x - 4, transform.position.y, transform.position.z);
            }
            else
            {
                Debug.Log("leftRange.TransformPoint(Vector3.zero).x" + leftRange.TransformPoint(Vector3.zero).x);

                p = new Vector3(player.position.x + 4, transform.position.y, transform.position.z);
            }
            transform.position = Vector3.MoveTowards(transform.position, p, 20 * Time.deltaTime);

            if (Math.Abs(transform.position.x - player.position.x) <= 5)
            {
                if (Time.time > attackTime)
                {
                    meleeAttack();
                    attackTime = Time.time + 5;
                }
            }
        }

    }

    private void groundCheck()
    {

        Collider2D[] colliders = Physics2D.OverlapCircleAll(GroundCheckPoint.position, 0.2f, grounds);
        if (colliders.Length > 0)
        {
            isFloating = false;
        }
        else
        {
            isFloating = true;
        }
    }
    public void meleeAttack()
    {

        animator.Play("normal_attack");

        Collider2D[] collisions = Physics2D.OverlapCircleAll(attack_point.position, radius, playerLayer);
        EnemyStats cs = transform.GetComponent<EnemyStats>();
        if (cs != null)
        {
            foreach (Collider2D e in collisions)
            {
                cs.GiveDamage(e);

            }
        }
        else
        {
            Debug.LogError("Missing ChracterStatsSCript");
        }

    }
    public void lookPlayer()
    {
        if (player.position.x < transform.position.x && isRight)
        {
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            isRight = false;
            StatusSpace.transform.localScale = new Vector2(StatusSpace.transform.localScale.x * -1, StatusSpace.transform.localScale.y);
        }
        if (player.position.x > transform.position.x && !isRight)
        {
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            isRight = true;
            StatusSpace.transform.localScale = new Vector2(StatusSpace.transform.localScale.x * -1, StatusSpace.transform.localScale.y);
        }
    }
}
