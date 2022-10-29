using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Metadata;

public class Kakashi_boss : MonoBehaviour
{
    // Start is called before the first frame update
    Animator animator;
    Rigidbody2D rb;
    public Transform leftRange;
    public Transform startLocation;
    public Transform rightRange;
    public Collider2D A;
    public Transform GroundCheckPoint;
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        Physics2D.IgnoreCollision(GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>()
, GetComponent<BoxCollider2D>(), true);
        //start up
        currentAnimation = "stand";
        ChangeAnimation(currentAnimation);
        rb.freezeRotation = true;

    }

    // Update is called once per frame
    void Update()
    {
        //StartCoroutine(check());
        try
        {
            Physics2D.IgnoreCollision(GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>()
, GetComponent<BoxCollider2D>(), true);
        }
        catch { }
        Mechanics();
        //MovingProcess();
        //CopySpell();
        //upScale();

    }
    GameObject followChar;
    public float timeDelayDragonStrike = 10f; //10s dung dc 1 lan
    public float timeToNextUseTurnDS = 0;// sau thoi gian nay thi dung dc skill dragonstrike
    public float timeDelayteleportLighting = 5f;
    public float timeToNextUseTurnTL = 0;// sau thoi gian nay thi dung dc skill teleportLighting
    public float timeDelayteleport = 3f;
    public float timeToNextUseTurnTp = 0;// sau thoi gian nay thi dung dc skill teleport
    public float timeDelayKunai = 1f;
    public float timeToNextUseKunai = 0;//sau thoi gian nay thi dung dc skill kunai
    //skill ko co delay:
    //-kunai
    //-move right,left
    //-normal_atack
    public float nearbyDistance = 5f; // khaong cach duoc coi la dung canh
    public float timeToNextAction = 0;// sau khi 1 aniamtion hoan thanh
    public IEnumerator check()
    {
        yield return new WaitForSeconds(5f);
        Mechanics();
    }
    bool animationDelay = false;
    public void Mechanics()
    {
        //neu nhan vat dang cung vi tri x( tien hanh tien lai gan)
        //neu nhan vat co y qua xa y cua boss( tien hanh teleport toi vi tri nhan vat + co the
        //dung skill lighting_ball ket hop)
        // neu cung x va trong khoang cach cua skill dragon_strike thuc hien ki nang
        //co che uu tien : vi tri

        if (followChar == null)
        {
            followChar = GameObject.FindGameObjectWithTag("Player").gameObject;
            // return neu k tim thay
            if (followChar == null)
            {


                return;
            }
        }


        float currentCharLocaleScaleX = followChar.transform.localScale.x;
        Vector3 currentCharLocation = followChar.transform.position;
        Vector3 currentBossLocation = transform.position;
        if (currentBossLocation.x < leftRange.position.x || currentBossLocation.x > rightRange.position.x
            || currentCharLocation.x < leftRange.position.x || currentCharLocation.x > rightRange.position.x)
        {
            transform.position = startLocation.position;
            if (Time.time > timeToNextAction && animationDelay == false)
            {
                stand();
            }
            return;
        }
        if (currentCharLocation.x - leftRange.position.x >= -2 && currentCharLocation.x - leftRange.position.x < 0)
            followChar.transform.position = new Vector3(currentCharLocaleScaleX + 5, currentCharLocation.y, currentCharLocation.z);
        if (currentCharLocation.x - rightRange.position.x >= 2)
            followChar.transform.position = new Vector3(currentCharLocaleScaleX - 5, currentCharLocation.y, currentCharLocation.z);

        if (Vector3.Distance(currentCharLocation, currentBossLocation) <= nearbyDistance)
        {
            timeToNextAction = getTimeOfAAnimation("normal_attack");
            if (isFloating)
                ChangeAnimation("jump_attack");
            else ChangeAnimation("normal_attack");
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            Action("rotate", (currentCharLocaleScaleX < currentBossLocation.x ? "left" : "right"));
            Action("normal_attack", null);
            Debug.LogError("KAKASI FIGHT");
            return;
        }
        if (Time.time > timeToNextAction)
        {

            if (Mathf.Abs(currentCharLocation.y - currentBossLocation.y) > 3 || animationDelay == true)
            {
                if (animationDelay == false)
                    stand();
                Transform Hand = getChildByName("Hand", transform);
                Transform fightPoint = getChildByName("FirePoint", Hand);
                if (Time.time > timeToNextUseTurnTL)
                {

                    if (fightPoint != null)
                    {
                        Transform lighting_ball_point = getChildByName("lighting_ball_point", fightPoint);
                        if (lighting_ball_point != null)
                        {
                            if (animationDelay == false)
                            {
                                ChangeAnimation("teleport_light_ball");
                                animationDelay = true;
                                return;
                            }

                            //StartCoroutine(WaitAndTranform());
                            if (animationDelay && Time.time - timeToNextAction >= 1)
                            {
                                transform.position = new Vector3(currentCharLocation.x - 2, currentCharLocation.y + 0.5f, currentCharLocation.z);
                                lighting_ball_point.GetComponent<LightingBallController>().shoot();

                                isFloating = true;

                                timeToNextUseTurnTL = Time.time + timeDelayteleportLighting;
                                timeToNextAction = Time.time + getTimeOfAAnimation("teleport_light_ball") - 1;
                                animationDelay = false;
                            }

                            Debug.LogError("light ball");

                        }

                        return;
                    }
                }
                if (Time.time > timeToNextUseTurnTp)
                {
                    ChangeAnimation("teleport");
                    StartCoroutine(WaitAndTranform());
                    transform.position = new Vector3(currentCharLocation.x - 2, currentCharLocation.y + 0.5f, currentCharLocation.z);
                    timeToNextUseTurnTp = Time.time + timeDelayteleport;
                    isFloating = true;
                    timeToNextAction = Time.time + getTimeOfAAnimation("teleport");
                    return;
                }
                if (Time.time > timeToNextUseKunai)
                {
                    if (!isFloating)
                        ChangeAnimation("throw");
                    else ChangeAnimation("jump_throw");
                    Transform kunai = getChildByName("Kunai", fightPoint);
                    kunai.GetComponent<KunaiFireController>().shoot();
                    timeToNextUseKunai = Time.time + timeDelayKunai;
                    timeToNextAction = timeToNextUseKunai;
                    Action("rotate", (currentCharLocaleScaleX < currentBossLocation.x ? "left" : "right"));

                    return;
                }

            }


            else
            {
                ChangeAnimation("run");
                Action("run", (currentCharLocation.x < currentBossLocation.x ? "left" : "right"));
                timeToNextAction = Time.time + getTimeOfAAnimation("run") - 0.2f;
                return;
            }
        }
    }


    IEnumerator WaitAndTranform()
    {
        yield return new WaitForSeconds(1f);
        yield break;
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
    public LayerMask grounds;
    private void FixedUpdate()
    {
        groundCheck();
        try { 
        followChar = GameObject.FindGameObjectWithTag("Player").gameObject;
        }
        catch
        {
            followChar = null;
        }
        // return neu k tim thay
        if (followChar != null)
        {
            Vector3 currentCharLocation = followChar.transform.position;
            Vector3 currentBossLocation = transform.position;
            Physics2D.IgnoreCollision(followChar.transform.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>(), true);
            Action("rotate", (currentCharLocation.x < currentBossLocation.x ? "left" : "right"));
        }
        else
        {
            stand();
        }
    }
    private void groundCheck()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(GroundCheckPoint.position, 0.2f, grounds);
        if (colliders.Length > 0)
            isFloating = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {


    }
    bool isFloating = true;
    bool directionRight = true;
    Vector3 lastVelocity;
    float LastTimeInteract;


    void stand()
    {
        ChangeAnimation("stand");
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
    }
    void Jump()
    {
        ChangeAnimation("jump");
        Action("jump", null);
        isFloating = true;
    }
    void MoveRight()
    {
        if (!directionRight)
            transform.localScale = new Vector3(gameObject.transform.localScale.x * -1.0f,
                gameObject.transform.localScale.y,
                gameObject.transform.localScale.z);

        Action("run", "right");
        ChangeAnimation("run");
        directionRight = true;
    }
    void MoveLeft()
    {
        if (directionRight)
            transform.localScale = new Vector3(gameObject.transform.localScale.x * -1.0f,
                gameObject.transform.localScale.y,
                gameObject.transform.localScale.z);
        ChangeAnimation("run");
        Action("run", "left");
        directionRight = false;
    }


    float jumpVelocity = 20f;
    float runVelocity = 20f;
    float slideVelocity = 20f;
    void Action(string actionName, string direction)
    {
        if (actionName == "rotate" && direction == "right")
        {
            if (!directionRight)
            {
                transform.localScale = new Vector3(gameObject.transform.localScale.x * -1.0f,
                    gameObject.transform.localScale.y,
                    gameObject.transform.localScale.z);
                directionRight = true;
            }

        }
        if (actionName == "rotate" && direction == "left")
        {
            if (directionRight)
            {
                transform.localScale = new Vector3(gameObject.transform.localScale.x * -1.0f,
                    gameObject.transform.localScale.y,
                    gameObject.transform.localScale.z);
                directionRight = false;
            }
        }
        if (actionName == "jump")
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpVelocity, 0);
        }
        if (actionName == "run")
        {
            if (direction == "right")
            {
                rb.velocity = new Vector3(runVelocity, rb.velocity.y, 0);
                Action("rotate", "right");
            }
            else
            {
                rb.velocity = new Vector3(-runVelocity, rb.velocity.y, 0);
                Action("rotate", "left");
            }
        }

        if (actionName == "normal_attack")
        {
            meleeAttack();
        }

    }

    //Status: building
    string currentAnimation;
    //animation name:
    //- stand
    //- run
    //-teleport
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
            if (animationName == "stand" || animationName.Contains("attack")
                || animationName.Contains("throw"))
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
                case "slide":

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

    public Transform attack_point;
    public float radius = 1.8f;
    public LayerMask Player;
    private float delayAttackTime = 1f;
    private float attackTime = 0;
    void meleeAttack()
    {
        if (Time.time > attackTime)
        {

            Collider2D[] collisions = Physics2D.OverlapCircleAll(attack_point.position, radius, Player);
            foreach (Collider2D e in collisions)
            {
                e.GetComponent<CharacterStats>().getDamage(30);

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
    void CopySpell()
    {
        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    Instantiate(copyspell_character,
        //        new Vector3(transform.position.x - sizeXCopySpell, transform.position.y, 0), Quaternion.identity);
        //    i++;
        //}
    }
    public Camera cam;
    public float startCameraScale;
    public float speedFollowCam = 10;
    void upScale()
    {
        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    gameObject.transform.localScale = gameObject.transform.localScale * 1.5f;
        //    gameObject.transform.Translate(new Vector3(0, 5, 0));
        //    jumpVelocity *= 1.5f;
        //    speedFollowCam += 5;
        //    cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, cam.orthographicSize + 4, Time.deltaTime * 10);
        //}

        //if (Input.GetKeyDown(KeyCode.M))
        //{
        //    gameObject.transform.localScale = gameObject.transform.localScale / 1.5f;
        //    jumpVelocity /= 1.5f;
        //    speedFollowCam -= 5;
        //    cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, cam.orthographicSize - 4, Time.deltaTime * 10);
        //}
    }
}
