using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class main_character__copyspell : MonoBehaviour
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
        ChangeAnimation(currentAnimation);
        rb.freezeRotation = true;

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
    float LastTimeInteract;

    void standCheck()
    {
        if (Input.anyKey)
        {
            LastTimeInteract = Time.time;
        }

        if (Time.time - LastTimeInteract >= 0.2)
        {
            ChangeAnimation("stand");
        }
    }
    void MovingProcess()
    {
        standCheck();


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
            Action("run", "left");
            directionRight = false;

        }

        if (Input.GetKey(KeyCode.A))
        {
            if (isFloating)
                ChangeAnimation("jump_attack");
            else
            {
                ChangeAnimation("normal_attack");
            }
        }
        if (Input.GetKey(KeyCode.S))
        {

            if (directionRight)
                Action("slide", "right");
            else Action("slide", "left");
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
    float jumpVelocity = 20f;
    float runVelocity = 10f;
    float slideVelocity = 20f;
    void Action(string actionName, string direction)
    {
        if (actionName == "jump")
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpVelocity, 0);
        }
        if (actionName == "run")
        {
            if (direction == "right")
                rb.velocity = new Vector3(runVelocity, rb.velocity.y, 0);
            else
                rb.velocity = new Vector3(-runVelocity, rb.velocity.y, 0);
        }
        if (actionName == "slide")
        {
            if (direction == "right")
                rb.velocity = new Vector3(slideVelocity, rb.velocity.y, 0);
            else
                rb.velocity = new Vector3(-slideVelocity, rb.velocity.y, 0);
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
    float startTimeAnimation;
    float endTimeAnimation;
    bool AnimatorIsPlaying()
    {
        return animator.GetCurrentAnimatorStateInfo(0).length >
               animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
    bool AnimatorIsPlaying(string stateName)
    {
        return AnimatorIsPlaying() && animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }
    void caculateTimeAnimation(string animationName)
    {
        startTimeAnimation = Time.time;
        endTimeAnimation = startTimeAnimation + getTimeOfAAnimation(animationName);

    }
    void ChangeAnimation(string animationName)
    {

        if (currentAnimation != "dead")
        {
            //xet theo hanh dong chinh
            if (animationName == "stand"||animationName.Contains("attack")
                ||animationName.Contains("throw"))
            {
                currentAnimation = animationName;
                animator.Play(currentAnimation);
                caculateTimeAnimation(currentAnimation);
                return;
            }
            switch (currentAnimation)
            {
                case "jump":

                    switch (animationName)
                    {
                        case "jump_throw":
                            currentAnimation = animationName;
                            animator.Play(currentAnimation);
                            caculateTimeAnimation(currentAnimation);
                            break;
                        case "jump_attack":
                            currentAnimation = animationName;
                            animator.Play(currentAnimation);
                            caculateTimeAnimation(currentAnimation);
                            break;
                    };
                    break;
                case "normal_attack":

                    if (Time.time > endTimeAnimation)
                    {
                        currentAnimation = animationName;
                        animator.Play(currentAnimation);
                        caculateTimeAnimation(currentAnimation);
                    }
                    break;
                case "jump_attack":

                    if (Time.time > endTimeAnimation)
                    {
                        currentAnimation = animationName;
                        animator.Play(currentAnimation);
                        caculateTimeAnimation(currentAnimation);
                    }
                    break;
                case "throw":

                    if (Time.time > endTimeAnimation)
                    {
                        currentAnimation = animationName;
                        animator.Play(currentAnimation);
                        caculateTimeAnimation(currentAnimation);
                    }
                    break;
                case "jump_throw":

                    if (Time.time > endTimeAnimation)
                    {
                        currentAnimation = animationName;
                        animator.Play(currentAnimation);
                        caculateTimeAnimation(currentAnimation);
                    }
                    break;

                default:
                    currentAnimation = animationName;
                    animator.Play(currentAnimation);
                    caculateTimeAnimation(currentAnimation);
                    break;

            }



        }
    }
    float getTimeOfAAnimation(string animationName)
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (animationName == clip.name) { 
                return clip.length;
            }
        }
        return 0;
        
    }
}
