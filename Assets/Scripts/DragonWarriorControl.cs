using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonWarriorControl : MonoBehaviour
{
    // Start is called before the first frame update
    public Collider2D a;
    private Animator animator;
    Rigidbody2D rb;
    private bool isRight;
    [SerializeField]
    private Canvas StatusSpace;
    private float fightLeftBoundRange;
    private float fightRightBoundRange;
    private float fightUpBoundRange;
    private float fightDownBoundRange;
    public GameObject weapon;
    void Start()
    {
        isRight = true;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        try
        {
            a = GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>();
            Physics2D.IgnoreCollision(a, GetComponent<CircleCollider2D>(), true);
        }
        catch { }
        fightLeftBoundRange = transform.position.x - 50;
        fightRightBoundRange = transform.position.x + 50;
        fightDownBoundRange = transform.position.y - 50;
        fightUpBoundRange = transform.position.y + 50;
    }
    GameObject weapon_trigger = null;
    // Update is called once per frame
    float attackTime = 0;
    void Update()
    {
        try
        {
            a = GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>();
            Physics2D.IgnoreCollision(a, GetComponent<CircleCollider2D>(), true);
        }
        catch { }
        if (rb.velocity.x < 0 && isRight)
        {
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            isRight = false;
            StatusSpace.transform.localScale = new Vector2(StatusSpace.transform.localScale.x * -1, StatusSpace.transform.localScale.y);
        }
        if (rb.velocity.x > 0 && !isRight)
        {
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            StatusSpace.transform.localScale = new Vector2(StatusSpace.transform.localScale.x * -1, StatusSpace.transform.localScale.y);
            isRight = true;
        }
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            if (player.transform.position.x >= fightLeftBoundRange && player.transform.position.x <= fightRightBoundRange
                && player.transform.position.y >= fightDownBoundRange && player.transform.position.y <= fightUpBoundRange
                && weapon_trigger == null)
            {
                if (Time.time > attackTime)
                {
                    weapon_trigger = Instantiate(weapon, transform.position, Quaternion.identity);
                    attackTime = Time.time + 5;
                    Destroy(weapon_trigger, 10);
                }
            }
        }
    }
}
