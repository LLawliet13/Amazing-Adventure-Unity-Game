using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bandit_Warrior : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public Collider2D A;
    private bool isRight;
    Rigidbody2D rb;
    [SerializeField]
    private Canvas StatusSpace;
    void Start()
    {
        isRight = true;
        rb = GetComponent<Rigidbody2D>();
        Physics2D.IgnoreCollision(A, GetComponent<BoxCollider2D>(), true);
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity.x < 0 && isRight) { 
            transform.localScale = new Vector2(transform.localScale.x*-1,transform.localScale.y);
            isRight = false;
            StatusSpace.transform.localScale = new Vector2(StatusSpace.transform.localScale.x * -1, StatusSpace.transform.localScale.y);
        }
        if (rb.velocity.x > 0 && !isRight) {
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            isRight = true;
            StatusSpace.transform.localScale = new Vector2(StatusSpace.transform.localScale.x * -1, StatusSpace.transform.localScale.y);
        }
    }
}
