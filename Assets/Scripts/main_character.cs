using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main_character : MonoBehaviour
{
    // Start is called before the first frame update
    Animator animator;
    Rigidbody2D rb;
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        //start up
        currentAnimation = "stand";
        animator.Play(currentAnimation);
        rb.freezeRotation = true;
        runForce = new Vector3(5, 0, 0);
        slideForce = new Vector3(10, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {

        MovingProcess();

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        Vector3 hit = collision.contacts[0].normal;
        float angle = Vector3.Angle(hit, Vector3.up);
        //var speed = lastVelocity.magnitude;

        if (Mathf.Approximately(angle, 0))
        {
            //Down
            Debug.Log(isFloating);
            isFloating = false;
        }

        if (Mathf.Approximately(angle, 180))
        {
            //Up
            Debug.Log("Up");
            rb.velocity = new Vector2(lastVelocity.x, -lastVelocity.y) * 0.5f;


        }
        if (Mathf.Approximately(angle, 90))
        {
            // Sides
            Vector3 cross = Vector3.Cross(Vector3.forward, hit);
            if (cross.y > 0)
            { // left side of the player
                Debug.Log("Left");
            }
            else
            { // right side of the player
                Debug.Log("Right");
            }
        }
    }
    bool isFloating = true;
    bool directionRight = true;
    Vector3 lastVelocity;
    void MovingProcess()
    {
        Vector3 v = new Vector3(rb.velocity.x, 0, 0);
        if (v.x == 0)
        {
            ChangeAnimation("stand");
        }
        if (Input.GetKey(KeyCode.UpArrow) && !isFloating)
        {
            ChangeAnimation("jump");
            Action("jump", null);
            isFloating = true;

        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (!directionRight)
                transform.localScale = new Vector3(gameObject.transform.localScale.x * -1.0f,
                    gameObject.transform.localScale.y,
                    gameObject.transform.localScale.z);
            if (lastVelocity.x != 0 && currentAnimation == "run")
            {
                if (!directionRight)
                    rb.velocity = -lastVelocity;
                else
                    rb.velocity = lastVelocity;

            }
            else
                Action("run", "right");
            ChangeAnimation("run");
            directionRight = true;

        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (directionRight)
                transform.localScale = new Vector3(gameObject.transform.localScale.x * -1.0f,
                    gameObject.transform.localScale.y,
                    gameObject.transform.localScale.z);
            ChangeAnimation("run");
            if (lastVelocity.x != 0)
            {
                if (directionRight)
                    rb.velocity = -lastVelocity;
                else
                    rb.velocity = lastVelocity;

            }
            else
                Action("run", "left");
            directionRight = false;

        }

        if (Input.GetKey(KeyCode.A))
        {
            if (isFloating)
                ChangeAnimation("jump_attack");
            else ChangeAnimation("normal_attack");
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (lastVelocity.x != 0 && currentAnimation == "slide")
            {
                rb.velocity = lastVelocity;
            }
            else
            {
                rb.velocity = Vector3.zero;
                if (directionRight)
                    Action("slide", "right");
                else Action("slide", "left");
            }
            ChangeAnimation("slide");

        }
        if (Input.GetKey(KeyCode.T))
        {
            if (isFloating)
                ChangeAnimation("jump_throw");
            else ChangeAnimation("throw");
        }
        lastVelocity = rb.velocity;

    }
    Vector3 jumpForce = new Vector3(0, 30, 0);
    Vector3 runForce;
    Vector3 slideForce;
    void Action(string actionName, string direction)
    {
        if (actionName == "jump")
        {
            rb.AddForce(jumpForce, ForceMode2D.Impulse);
        }
        if (actionName == "run")
        {
            if (direction == "right")
                rb.AddForce(runForce, ForceMode2D.Impulse);
            else
                rb.AddForce(-runForce, ForceMode2D.Impulse);
        }
        if (actionName == "slide")
        {
            if (direction == "right")
                rb.AddForce(slideForce, ForceMode2D.Impulse);
            else
                rb.AddForce(-slideForce, ForceMode2D.Impulse);
        }
    }

    //Status: building
    string currentAnimation;
    //animation name:
    //- stand
    //- run
    //- slide
    //- throw
    //- jump
    //- normal_attack
    //- jump_throw
    //- jump_attack
    //- dead
    void ChangeAnimation(string animationName)
    {
        if (currentAnimation != "dead")
        {
            //xet theo hanh dong chinh
            if (currentAnimation == "jump")
            {
                switch (animationName)
                {
                    case "jump_throw":
                        currentAnimation = animationName;
                        animator.Play(currentAnimation);
                        break;
                    case "jump_attack":
                        currentAnimation = animationName;
                        animator.Play(currentAnimation);
                        break;
                    case "dead":
                        currentAnimation = animationName;
                        animator.Play(currentAnimation);
                        break;
                }
            }
            currentAnimation = animationName;
            animator.Play(currentAnimation);



        }
    }
}
