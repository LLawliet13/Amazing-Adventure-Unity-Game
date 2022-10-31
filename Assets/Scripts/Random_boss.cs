using Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using Random = UnityEngine.Random;

public class Random_boss : MonoBehaviour
{


    // Start is called before the first frame update
    Animator animator;
    Rigidbody2D rb;
    public Transform leftRange;
    public Transform startLocation;
    public Transform rightRange;
    public Collider2D A;
    public Transform GroundCheckPoint;
    public void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        try
        {
            Physics2D.IgnoreCollision(GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>()
    , GetComponent<BoxCollider2D>(), true);
        }
        catch
        {
            Debug.Log("No Character Found");
        }
        //start up
        animator.Play("stand");
        rb.freezeRotation = true;

    }


    // Update is called once per frame
    public void Update()
    {

        try
        {
            Physics2D.IgnoreCollision(GameObject.FindGameObjectWithTag("Player")
                .GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>(), true);
        }
        catch
        {
            return;
        }
        Mechanics();



    }


    bool animationDelay = false;
    /// <summary>
    /// ////////////////////////////////////////////////////////
    /// </summary>
    //Các thứ cần 
    // class định nghĩa kĩ năng
    public delegate void Action();

    public class Skill
    {
        public string name;
        public float cooldown;
        //1 ham danh gia vi tri hien tai co the dung skill duoc hay khong
        // 1 ham sort va dung theo skill cooldown dai nhat
        // dua tren so con duong phai di => cho ra 1 con so the hien do phuc tap
        public float xRange;//vung skill cos the thi trien
        public float yRange;
        public bool canFloating;//ki nang co the thi trien ngay tren khong
        public bool isCooldown;
        public float SkillDuration;
        private event Action action;
        public float timeToNextUse;
        public void SkillAction()
        {
            action.Invoke();
        }
        public Skill(string name, float cooldown, float xRange, float yRange, bool canFloating, bool isCooldown, float skillDuration, Action actionDetail)
        {
            this.name = name;
            this.cooldown = cooldown;
            this.xRange = xRange;
            this.yRange = yRange;
            this.canFloating = canFloating;
            this.isCooldown = isCooldown;
            SkillDuration = skillDuration;
            this.action += actionDetail;
        }
    }

    // 1 hàm đánh giá vị trí 

    //ân định việc boss sẽ không làm gì 
    //khi đang thực hiện 1 skill
    protected Dictionary<string, Skill> skillList = new Dictionary<string, Skill>();

    bool isAction = false;
    string skillNameSelected;
    bool isNeedRandomSkill;// trong truong hop cac skill duoc uu tien co cung thoi gian hoi
    float TimeToUseNextSkill;
    bool isPrepareUsingSkill;// giai doan set cac tham so de van hanh action cua skill
    List<Skill> possibleSkill = new List<Skill>();

    public void Mechanics()
    {
        isAction = AttackMechanics();
        if (!isAction)
        {
            FollowMechanics();
        }
    }
    public bool AttackMechanics()
    {
        if (Time.time >= TimeToUseNextSkill)
        {
            checkSkillCooldown();// check xem cac chieu nao cooldown da het;

            if (!isAction)
            {
                possibleSkill = new List<Skill>();
                possibleSkill = checkPossibleSkill();//cac skill dang chua cooldown
                if (possibleSkill.Count == 0) return false;// k co skill de dung`
                possibleSkill = selectSkillBaseRangeAttack(possibleSkill, player);// dong thoi check co the tan cong tren khong khong
                possibleSkill = selectSkillCoolDownBase(possibleSkill);//uu tien lay cac skill co cooldown dai nhat
                if(possibleSkill == null|| possibleSkill.Count == 0) return false;
                isNeedRandomSkill = possibleSkill.Count == 1 ? false : true;
                if (isNeedRandomSkill)
                {
                    skillNameSelected = randomSkill(possibleSkill).name;
                }
                else
                {
                    skillNameSelected = possibleSkill.ElementAt(0).name;
                    isAction = true;
                    isPrepareUsingSkill = true; //-> thuc hien cac buoc can lam truoc khi dung skill
                }
                // thuc hien danh gia

            }
            else
            {

                if (isPrepareUsingSkill)
                {
                    TimeToUseNextSkill = Time.time + skillList[skillNameSelected].SkillDuration;
                    skillList[skillNameSelected].isCooldown = true;
                    skillList[skillNameSelected].timeToNextUse = Time.time + skillList[skillNameSelected].cooldown;
                    isPrepareUsingSkill = false;

                }

                skillList[skillNameSelected].SkillAction();

            }
        }
        return isAction;


    }
    /// /////////////////////////support attackMechanics-start/////////////////////////////////////////////////
    //sap xep skill kha dung 
    public List<Skill> checkPossibleSkill()
    {
        List<Skill> skills = new List<Skill>();
        foreach (var skill in skillList.Keys)
        {
            if (!skillList[skill].isCooldown)
                skills.Add(skillList[skill]);
        }
        return skills;

    }
    //chon skill co cooldown dai nhat
    public List<Skill> selectSkillCoolDownBase(List<Skill> skills)
    {
        if (skills.Count == 0) return null;
        skills.OrderByDescending(p => p.cooldown);
        float max_cooldown = skills[0].cooldown;
        return skills.Where(s => s.cooldown == max_cooldown).ToList();
    }
    public List<Skill> selectSkillBaseRangeAttack(List<Skill> skills, Transform followObject)
    {
        List<Skill> skillList = new List<Skill>();
        foreach (var skill in skills)
        {
            if (Mathf.Abs(transform.position.x - followObject.position.x) <= skill.xRange &&
                Mathf.Abs(transform.position.y - followObject.position.y) <= skill.yRange &&
                isFloating == skill.canFloating)
            {
                skillList.Add(skill);
            }
        }
        return skillList;
    }
    public Skill randomSkill(List<Skill> skills)
    {
        int skillIndex = Random.Range(0, skills.Count - 1);
        return skills.ElementAt(skillIndex);
    }
    public Transform player;

    public void checkSkillCooldown()
    {
        foreach (var s in skillList.Keys)
        {
            if (Time.time >= skillList[s].timeToNextUse)
            {
                skillList[s].isCooldown = false;
            }
        }
    }
    /// /////////////////////////support attackMechanics-end/////////////////////////////////////////////////
    public Transform targetMove;// day la cac ground center 
    public int currentTargetMoveIndex = 0;
    public List<Transform> targetMoves = new List<Transform>();
    private float relaxDuration = 0;
    private float TimeToRelaxTime = 20;//time cua 1 lan update
    int count = 0;

    public void FollowMechanics()
    {

        if (CheckIsInGround(transform) != null && CheckIsInGround(player) != null)
        {
            //if(count == TimeToRelaxTime)
            //{
            //    relaxDuration = Time.time + 1;
            //    animator.Play("stand");
            //    count = 0;
            //}
            //count++;
           
            //cham mat dat se cap nhat path 1 lan
            targetMoves = UpdatePath(targetMoves);
            Debug.Log("targetMoves Updated:" + targetMoves.Count);
        }
        //if (relaxDuration<Time.time)
        //{
            followPath(targetMoves);
        //}
        //else
        //{
        //    count = 0;
        //}
        


    }
    public float meleeAttackRange = 15;
    private float jumpVelocity = 50;
    private float runVelocity = 10;
    public void followPath(List<Transform> targetMoves)
    {
        if (targetMoves.Count == 0)
        {
            Vector3 newPos;
            if (Mathf.Abs(transform.position.x - player.position.x) <= 0.5)
            {
                newPos = new Vector3(transform.position.x, player.position.y, transform.position.z);
            }
            else { 
                animator.Play("run");
                newPos = new Vector3(player.position.x - meleeAttackRange, player.position.y, transform.position.z);
            }
            transform.position = Vector3.MoveTowards(transform.position, newPos, runVelocity * Time.deltaTime);
            return;
        }
        if (CheckIsInGround(transform) != targetMoves[currentTargetMoveIndex])
        {
            //BoxCollider2D collider = targetMoves[currentTargetMoveIndex].GetComponent<BoxCollider2D>();
            float dodaichan = transform.position.y - getEdge(transform, "bottom");
            //Random.Range(getEdge(targetMoves[currentTargetMoveIndex], "left") + 1, getEdge(targetMoves[currentTargetMoveIndex], "right") - 1
            Vector3 newPos = new Vector3(targetMoves[currentTargetMoveIndex].position.x
                , getEdge(targetMoves[currentTargetMoveIndex], "top") + dodaichan + 0.2f, transform.position.z);
            Debug.Log(newPos.x + "," + newPos.y + "," + newPos.z);
            transform.position = Vector3.MoveTowards(transform.position, newPos, jumpVelocity * Time.fixedDeltaTime);
            removeBorderWhenMove();
            animator.Play("jump");
        }
        else
        {
            Debug.LogError("done path point");

            addBorderWhenStand();
            if (currentTargetMoveIndex < targetMoves.Count - 1)
                currentTargetMoveIndex++;
        }
    }
    public void removeBorderWhenMove()
    {
        rb.gravityScale = 0;
        foreach (var targetMove in groundsToJump)
        {
            Physics2D.IgnoreCollision(transform.GetComponent<BoxCollider2D>()
   , targetMove.GetComponent<BoxCollider2D>(), true);
        }

    }
    public void addBorderWhenStand()
    {
        rb.gravityScale = 1;

        foreach (var targetMove in groundsToJump)
        {
            Physics2D.IgnoreCollision(transform.GetComponent<BoxCollider2D>()
   , targetMove.GetComponent<BoxCollider2D>(), false);
        }
    }
    public List<Transform> groundsToJump = new List<Transform>();
    public List<Transform> UpdatePath(List<Transform> oldTargetMoves)
    {
        List<Transform> targets = new List<Transform>();
        try
        {
            targets = findPathGround(groundsToJump, CheckIsInGround(transform), CheckIsInGround(player));
        }
        catch
        {
            return targets;
        }
        bool isPathChange = false;
        if (oldTargetMoves.Count != targets.Count) isPathChange = true;
        else
            for (int i = 0; i < oldTargetMoves.Count; i++)
            {
                if (oldTargetMoves[i] != targets[i])
                {
                    isPathChange = true;
                    break;
                }
            }
        if (isPathChange)
        {
            currentTargetMoveIndex = 0;
            return targets;
        }
        return oldTargetMoves;
    }
    /// /////////////////////////findPathGround-start/////////////////////////////////////////////////
    public List<Transform> findPathGround(List<Transform> grounds, Transform bossGround, Transform playerGround)
    {
        List<Transform> targetGroundPath = new List<Transform>();
        if (bossGround.position == playerGround.position)
        {
            return targetGroundPath;// noi check findPathGround se hieu la khong can di chuyen toi ground khac
        }
        List<float> heightLevels = new List<float>();

        if (bossGround.position.y > playerGround.position.y)
        {
            grounds = sortGroundByHeight(grounds, true);
            grounds = grounds.Where(g => g.position.y < bossGround.position.y
            && playerGround.position.y <= g.position.y).ToList();

        }
        else
        {
            grounds = sortGroundByHeight(grounds, false);
            grounds = grounds.Where(g => g.position.y > bossGround.position.y
            && playerGround.position.y >= g.position.y).ToList();
        }
        for (int i = 0; i < grounds.Count; i++)
        {
            if (!heightLevels.Where(h => h == grounds.ElementAt(i).position.y).Any())
            {
                heightLevels.Add(grounds.ElementAt(i).position.y);
            }
        }
        List<List<Transform>> groundsSortByHeightLevels = new List<List<Transform>>();
        foreach (var height in heightLevels)
        {
            List<Transform> heightLayerGround = new List<Transform>();
            foreach (var g in grounds)
            {
                if (g.position.y == height)
                {
                    heightLayerGround.Add(g);
                }
            }
            groundsSortByHeightLevels.Add(heightLayerGround);
        }
        foreach (var gs in groundsSortByHeightLevels)
        {
            targetGroundPath.Add(evaluateWhichGroundToGo(gs, playerGround, bossGround));
        }
        return targetGroundPath;
    }
    public List<Transform> sortGroundByHeight(List<Transform> grounds, bool desc)
    {
        if (desc)
        {
            return grounds.OrderByDescending(g => g.position.y).ToList();
        }
        else
        {
            return grounds.OrderByDescending(g => -g.position.y).ToList();
        }
    }
    public Transform evaluateWhichGroundToGo(List<Transform> GroundsWithSameHeight, Transform playerGround, Transform bossGround)
    {
        Vector3 averagePostion = new Vector3((playerGround.position.x + bossGround.position.x) / 2,
            (playerGround.position.y + bossGround.position.y) / 2,
            (playerGround.position.z + bossGround.position.z) / 2);
        GroundsWithSameHeight.OrderByDescending(g => Mathf.Abs(Vector3.Distance(averagePostion, g.position))).ToList();
        return GroundsWithSameHeight.Last();
    }

    /// ///////////////////////////findPathGround-end///////////////////////////////////////////
    /// ///////////////////////////CheckIsInGround-start///////////////////////////////////////////
    private float doBatCaoCuaNhanVat = 1;
    //return ground gần nhân vật nhất
    public Transform CheckIsInGround(Transform character)
    {
        //if (Mathf.Abs(LowestGround.position.y - player.position.y) <= doBatCaoCuaNhanVat / 1.5 + dodaichan)
        //{
        //    return LowestGround;
        //}
        float dodaichan = character.position.y - getEdge(character, "bottom");
        //Debug.Log("Dodaichan" + dodaichan);
        foreach (var gr in groundsToJump)
        {

            if ((character.position.y - dodaichan - getEdge(gr, "top")) >= 0 && (character.position.y - getEdge(gr, "top")) <= (doBatCaoCuaNhanVat / 1.5 + dodaichan) &&
                getEdge(gr, "left") <= character.position.x && getEdge(gr, "right") >= character.position.x)
            {
                return gr;
            }
        }
        return null;
    }
    public float getEdge(Transform bc, string direction)
    {
        BoxCollider2D collider = bc.GetComponent<BoxCollider2D>();
        if (direction == "top")
        {
            //get the extents
            var yHalfExtents = collider.bounds.extents.y;
            //get the center
            var yCenter = collider.bounds.center.y;
            return bc.position.y + yHalfExtents;
        }
        if (direction == "bottom")
        {
            //get the extents
            var yHalfExtents = collider.bounds.extents.y;
            //get the center
            var yCenter = collider.bounds.center.y;
            return bc.position.y - yHalfExtents;
        }
        if (direction == "left")
        {
            //get the extents
            var xHalfExtents = collider.bounds.extents.x;
            //get the center
            var xCenter = collider.bounds.center.x;
            return bc.position.x - xHalfExtents;
        }
        if (direction == "right")
        {
            //get the extents
            var xHalfExtents = collider.bounds.extents.x;
            //get the center
            var xCenter = collider.bounds.center.x;
            return bc.position.x + xHalfExtents;
        }
        return 0f;
    }
    /// ///////////////////////////CheckIsInGround-end///////////////////////////////////////////


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
    public void FixedUpdate()
    {
        groundCheck();
        try
        {
            player = GameObject.FindGameObjectWithTag("Player").gameObject.transform;
        }
        catch
        {
            player = null;
        }
        // return neu k tim thay
        if (player != null)
        {
            Vector3 currentCharLocation = player.transform.position;
            Vector3 currentBossLocation = transform.position;
            Physics2D.IgnoreCollision(player.transform.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>(), true);
            ChangeMoveAction("rotate", (currentCharLocation.x < currentBossLocation.x ? "left" : "right"));

        }
        else
        {
            ChangeMoveAction("stand", null);
        }
    }
    private void groundCheck()
    {
        isFloating = true;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(GroundCheckPoint.position, 0.2f, grounds);
        if (colliders.Length > 0)
            isFloating = false;
    }
    void ChangeMoveAction(string actionName, string direction)
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

    }
    bool isFloating = true;
    bool directionRight = true;
    Vector3 lastVelocity;
    float LastTimeInteract;




    //Status: building
    string currentAnimation;
    //animation name:
    //- stand
    //- run
    //-teleport
    float startTimeAnimation;
    float endTimeAnimation;



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

}
