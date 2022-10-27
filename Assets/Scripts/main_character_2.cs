using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class main_character_2 : MonoBehaviour
{
    // Start is called before the first frame update
    Animator animator;
    Rigidbody2D rb;
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        //start up
        setUpPriorityAnimation();
        currentAnimation = "stand";
        ChangeAnimation(currentAnimation);
        rb.freezeRotation = true;
        cam = Camera.main;
        startCameraScale = cam.orthographicSize;

    }
    Dictionary<String, AnimationCustom> animationPriority = new Dictionary<string, AnimationCustom>();
    // animation co priority cao hon khi chay se khong bi animation co priority thap hon ngat
    // cac animation co priority thap hon se dc chay khi AnimationTime cua animation co priority cao hon ket thuc
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
    void setUpPriorityAnimation()
    {
        animationPriority.Add("dead", new AnimationCustom("dead", true, 5, 999999));
        animationPriority.Add("stand", new AnimationCustom("stand", false, 5, getTimeOfAAnimation("stand")));
        animationPriority.Add("throw", new AnimationCustom("throw", true, 5, getTimeOfAAnimation("throw")));
        animationPriority.Add("jump_throw", new AnimationCustom("jump_throw", true, 5, getTimeOfAAnimation("jump_throw")));
        animationPriority.Add("jump_attack", new AnimationCustom("jump_attack", true, 4, getTimeOfAAnimation("jump_attack")));
        animationPriority.Add("normal_attack", new AnimationCustom("normal_attack", true, 4, getTimeOfAAnimation("normal_attack")));
        animationPriority.Add("jump", new AnimationCustom("jump", true, 3, getTimeOfAAnimation("jump")));
        animationPriority.Add("slide", new AnimationCustom("slide", true, 2, getTimeOfAAnimation("slide")));
        animationPriority.Add("run", new AnimationCustom("run", true, 1, getTimeOfAAnimation("run")));
    }
    // Update is called once per frame
    void Update()
    {

        MovingProcess();
    



    }
    private void FixedUpdate()
    {
        groundCheck();
        wallCheck();
    }
    public LayerMask grounds;
    public Transform GroundCheckPoint;
    public Transform WallCheckPoint1;
    public Transform WallCheckPoint2;
    public Transform WallCheckPoint3;
    public Transform WallCheckPoint4;

    private void setFriction(float f)
    {
        //rb.sharedMaterial.friction = f;

    }
    private void wallCheck()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(WallCheckPoint1.position, 0.2f, grounds);
        Collider2D[] colliders2 = Physics2D.OverlapCircleAll(WallCheckPoint2.position, 0.2f, grounds);
        Collider2D[] colliders3 = Physics2D.OverlapCircleAll(WallCheckPoint3.position, 0.2f, grounds);
        Collider2D[] colliders4 = Physics2D.OverlapCircleAll(WallCheckPoint4.position, 0.2f, grounds);
        if (colliders.Length > 0 || colliders2.Length > 0 || colliders3.Length > 0 || colliders4.Length > 0)
        {

            //setFriction(0f);
            isFloating = false;
        }

    }
    private void groundCheck()
    {

        Collider2D[] colliders = Physics2D.OverlapCircleAll(GroundCheckPoint.position, 0.2f, grounds);
        if (colliders.Length > 0)
        {
            isFloating = false;
            //setFriction(1f);
        }
        else
        {
            isFloating = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
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

        if (Time.time - LastTimeInteract >= 0.5)
        {
            ChangeAnimation("stand");
        }
    }
    public void Jump()
    {
        if (!isFloating)
        {
            ChangeAnimation("jump");
            Action("jump", null);
            isFloating = true;
        }
    }
    public void MoveRight()
    {
        if (!directionRight)
            transform.localScale = new Vector3(gameObject.transform.localScale.x * -1.0f,
                gameObject.transform.localScale.y,
                gameObject.transform.localScale.z);

        Action("run", "right");
        ChangeAnimation("run");
        directionRight = true;
    }
    public void MoveLeft()
    {
        if (directionRight)
            transform.localScale = new Vector3(gameObject.transform.localScale.x * -1.0f,
                gameObject.transform.localScale.y,
                gameObject.transform.localScale.z);
        ChangeAnimation("run");
        Action("run", "left");
        directionRight = false;
    }
    public void Slide()
    {
        if (directionRight)
            Action("slide", "right");
        else Action("slide", "left");
        ChangeAnimation("slide");
    }
    private float kunaiUseTime = 0;
    public void ThrowKunai()
    {
        if (kunaiUseTime <= Time.time)
        {
           
            if (isFloating)
                ChangeAnimation("jump_throw");
            else ChangeAnimation("throw");
            getChildByName("Kunai", getChildByName("FirePoint",
                getChildByName("Hand", transform))).GetComponent<KunaiFireController>().shoot();
            kunaiUseTime += 1f;
        }
    }
    public void ThrowKunaiAuto()
    {
        if (kunaiUseTime <= Time.time)
        {
            if (isFloating)
                ChangeAnimation("jump_throw");
            else ChangeAnimation("throw");
            getChildByName("Kunai", getChildByName("FirePoint",
                getChildByName("Hand", transform))).GetComponent<KunaiFireController>().shootAuto();
            kunaiUseTime += 1f;
        }
    }
    Transform getChildByName(string name, Transform transform)
    {
        foreach (Transform child in transform)
        {
            if (child.name == name)
            {
                return child;
            }
        }
        return null;
    }
    public void MovingProcess()
    {
        standCheck();


        if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)))
        {
            Jump();
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            MoveRight();

        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            MoveLeft();

        }

        if (Input.GetMouseButton(1))
        {
            meleeAttack();

        }
        if (Input.GetKey(KeyCode.Space))
        {

            Slide();

        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            ThrowKunai();
        }
        if (Input.GetKeyDown(KeyCode.B))
            upScale();
        if (Input.GetKeyDown(KeyCode.C))
        {
            CopySpell();
        }
        lastVelocity = rb.velocity;

    }
    float jumpVelocity = 20f;
    float runVelocity = 8f;
    float slideVelocity = 18f;
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
            {
                rb.velocity = new Vector3(slideVelocity, rb.velocity.y, 0);
                //Debug.Log(rb.velocity.y);
            }
            else
                rb.velocity = new Vector3(-slideVelocity, rb.velocity.y, 0);
        }
    }

    //Status: building
    string currentAnimation;

    float endTimeAnimation = 0;

    void caculateTimeAnimation(string animationName)
    {
        endTimeAnimation = Time.time + animationPriority[animationName].animationTime;
    }

    void ChangeAnimation(string animationName)
    {
        if (animationPriority[currentAnimation].priority < animationPriority[animationName].priority)
        {
            currentAnimation = animationName;
            animator.Play(currentAnimation);
            if (animationPriority[currentAnimation].isNeedCheckTimeAnimation)
            {
                caculateTimeAnimation(currentAnimation);
            }
        }
        else
        {
            if (Time.time >= endTimeAnimation)
            {
                currentAnimation = animationName;
                animator.Play(currentAnimation);
                if (animationPriority[currentAnimation].isNeedCheckTimeAnimation)
                {
                    caculateTimeAnimation(currentAnimation);
                }
            }
        }


    }

    public Transform attack_point;
    public float radius = 1.8f;
    public LayerMask enemies;
    private float delayAttackTime = 1f;
    private float attackTime = 0;
    public void meleeAttack()
    {
        if (Time.time > attackTime)
        {
            if (isFloating)
                ChangeAnimation("jump_attack");
            else
            {
                ChangeAnimation("normal_attack");
            }

            Collider2D[] collisions = Physics2D.OverlapCircleAll(attack_point.position, radius, enemies);
            foreach (Collider2D e in collisions)
            {
                e.GetComponent<EnemyStats>().getDamage(10);
                Debug.LogError("attacked");

            }
            attackTime = Time.time + delayAttackTime;
        }
    }
    float getTimeOfAAnimation(string animationName)
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (animationName == clip.name)
            {
                return clip.length;
            }
        }
        return 0;

    }
    ////////////////

    //Skills
    public GameObject copyspell_character;
    int i = 1;
    public int sizeXCopySpell = 5;
    List<GameObject> copy_char = new List<GameObject>();
    public void CopySpell()
    {
        int count = 0;
        foreach (var c in copy_char)
        {
            if (c != null)
                count++;
        }
        if (count <= sizeXCopySpell)
        {
            copy_char.Add(Instantiate(copyspell_character,
                new Vector3(transform.position.x - sizeXCopySpell, transform.position.y, 0), Quaternion.identity))
            ;
        }
    }
    public float camEdgePosition(String edge)
    {
        if (cam)
        {
            if (edge == "bottom")
                return cam.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).y;
            if (edge == "right")
                return cam.ViewportToWorldPoint(new Vector3(1.0f, 0f, 0f)).x;
            if (edge == "top")
                return cam.ViewportToWorldPoint(new Vector3(0f, 1.0f, 0f)).y;
            if (edge == "left")
                return cam.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).x;
        }
        return 0.0f;

    }


    public Camera cam;
    public float startCameraScale;
    public float speedFollowCam = 10;
    bool scaleUp = false;
    public void upScale()
    {
        if (scaleUp == false)
        {
            gameObject.transform.localScale = gameObject.transform.localScale * 1.5f;
            gameObject.transform.Translate(new Vector3(0, 5, 0));
            jumpVelocity *= 1.5f;
            speedFollowCam += 5;
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, cam.orthographicSize + 4, Time.deltaTime * 10);
            scaleUp = true;
        }

        if (scaleUp == true)
        {
            gameObject.transform.localScale = gameObject.transform.localScale / 1.5f;
            jumpVelocity /= 1.5f;
            speedFollowCam -= 5;
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, cam.orthographicSize - 4, Time.deltaTime * 10);
            scaleUp = false;
        }
    }
    public GameObject AutoDetect()
    {
        float left = camEdgePosition("left");
        float right = camEdgePosition("right");
        float bottom = camEdgePosition("bottom");
        float top = camEdgePosition("top");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            float x = enemy.transform.position.x;
            float y = enemy.transform.position.y;
            if (x >= left && x<=right&&
                y>=bottom && y<= top)
            {
                return enemy;
            }
        }
        return null;
    }
}
public class AnimationCustom
{
    public String animationName;
    public bool isNeedCheckTimeAnimation;
    public int priority;
    public float animationTime;

    public AnimationCustom(string animationName, bool isNeedCheckTimeAnimation, int priority, float animationTime)
    {
        this.animationName = animationName;
        this.isNeedCheckTimeAnimation = isNeedCheckTimeAnimation;
        this.priority = priority;
        this.animationTime = animationTime;
    }
}
