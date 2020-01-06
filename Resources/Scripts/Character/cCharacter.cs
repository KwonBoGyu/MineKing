using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CHARACTERSTATUS
{
    NONE,
    MOVE,
    CROUCH,
    ATTACK,
    JUMP,
    JUMP_ATTACK,
    DASH,
    DASH_ATTACK
}

public enum CHARDIRECTION
{
    NONE,
    UP,
    UPRIGHT,
    RIGHT,
    DOWNRIGHT,
    DOWN,
    DOWNLEFT,
    LEFT,
    UPLEFT,
}

public class cCharacter : MonoBehaviour
{
    protected string nickName;
    protected float damage;
    protected float maxMoveSpeed;
    protected float dashMoveSpeed;
    protected const float dashTime = 0.5f;
    protected float curMoveSpeed;
    protected float maxHp;
    protected float curHp;
    protected float jumpHeight;
    protected bool isDoubleJump;
    protected float maxDashCoolDown; // 실제 대쉬 쿨다운
    protected float dashCoolDown; // 쿨다운 계산용 (변화)
    protected bool isJetPackOn;
    protected CHARACTERSTATUS status;
    protected CHARDIRECTION charDir;
    public CHARDIRECTION GetCharDir() { return charDir; }
    protected Vector3 dir;
    public GameObject originObj;
    public BoxCollider2D rt;

    //hp
    public Image img_curHp;

    // 중력 적용에 필요한 변수
    public float changingGravity;
    public float defaultGravity;

    public bool isGrounded;
    public bool isRightBlocked;
    public bool isLeftBlocked;
    public bool isUpBlocked;

    public GameObject attackBox;
    protected Vector3[] attackBoxPos; //오른, 아래, 왼, 위

    private IEnumerator cor_knockBack;
    protected Animator _animator;

    public virtual void Init(string pNickName, float pDamage, float pMaxMoveSpeed, float pMaxHp, float pCurHp)
    {
        nickName = pNickName;
        damage = pDamage;
        maxMoveSpeed = pMaxMoveSpeed;
        dashMoveSpeed = maxMoveSpeed + 300;
        curMoveSpeed = 0;
        maxHp = pMaxHp;
        curHp = pCurHp;
        dir = Vector3.right;
        jumpHeight = 200.0f;
        attackBoxPos = new Vector3[4];
        status = CHARACTERSTATUS.NONE;

        maxDashCoolDown = 4.0f;
        dashCoolDown = maxDashCoolDown;
        isJetPackOn = false;
    }



    public virtual void FixedUpdate()
    {
        //ManageCollision();
    }

    public float GetMaxHp() { return maxHp; }
    public float GetCurHP() { return curHp; }
    public void IncreaseHP(float pAmount)
    {
        if(curHp <= maxHp)
            curHp += pAmount;

        if (curHp > maxHp)
            curHp = maxHp;

        SetHp();
    }

    public void DecreaseHP(float pAmount)
    {
        if (curHp <= 0)
            return;
        curHp -= pAmount;

        if (curHp < 0)
            curHp = 0;

        SetHp();
    }

    public float GetDamage() { return damage; }

    public float GetDashCoolDown() { return dashCoolDown; }
    public float GetMaxDashCoolDown() { return maxDashCoolDown; }

    public CHARACTERSTATUS GetStatus() { return status; }
    public void SetStatus(CHARACTERSTATUS pCs) { status = pCs; }

    public Vector3 GetDirection() { return dir; }
    public void SetDir(Vector3 pDir) { dir = pDir; }
    public void SetDir(Vector3 pDir, CHARDIRECTION pCharDir) { dir = pDir; charDir = pCharDir; }
    
    public float GetMaxMoveSpeed() { return maxMoveSpeed; }
    public void SetMaxMoveSpeed(float pMaxMoveSpeed) { maxMoveSpeed = pMaxMoveSpeed; }

    public float GetCurMoveSpeed() { return curMoveSpeed; }
    public virtual void SetCurMoveSpeed(float pCurMoveSpeed) { curMoveSpeed = pCurMoveSpeed; }

    //점프 코루틴
    IEnumerator Jump()
    {
        yield return null;

        bool goBreak = false;

        if (isGrounded == false && isDoubleJump == false)
            isDoubleJump = true;
        else if (isGrounded == false && isDoubleJump == true)
            goBreak = true;

        if (isGrounded == true)
            isDoubleJump = false;
        
        float currentHeight = originObj.transform.position.y;
        float maxHeight = originObj.transform.position.y + jumpHeight;        

        while (true)
        {
            if(goBreak == true)            
                break;

            yield return new WaitForFixedUpdate();

            status = CHARACTERSTATUS.JUMP;
            isGrounded = false;
            currentHeight += 13f;
            originObj.transform.position = new Vector3(originObj.transform.position.x,
                currentHeight, originObj.transform.position.z);
            
            if (currentHeight >= maxHeight)
            {
                originObj.transform.position = new Vector2(originObj.transform.position.x, maxHeight);
                break;
            }
            if (isUpBlocked == true)
            {
                originObj.transform.position = new Vector2(originObj.transform.position.x, currentHeight - 15.0f);
                break;
            }
        }

        status = CHARACTERSTATUS.NONE;
    }

    // 대쉬 코루틴
    IEnumerator Dash()
    {
        yield return null;

        status = CHARACTERSTATUS.DASH;
        float DashTimer = 0;
        float factor = 0;

        while (true)
        {
            yield return new WaitForFixedUpdate();

            factor = Mathf.PI * (DashTimer / dashTime);
            
            if (charDir.Equals(CHARDIRECTION.RIGHT) || charDir.Equals(CHARDIRECTION.LEFT))
                curMoveSpeed = maxMoveSpeed + dashMoveSpeed * Mathf.Sin(factor);
            else
                curMoveSpeed = dashMoveSpeed * Mathf.Sin(factor);

            if (curMoveSpeed > dashMoveSpeed)
                curMoveSpeed = dashMoveSpeed;

            DashTimer += Time.deltaTime;

            if(DashTimer > dashTime)
            {
                if (charDir.Equals(CHARDIRECTION.RIGHT) || charDir.Equals(CHARDIRECTION.LEFT))
                    curMoveSpeed = maxMoveSpeed;
                else
                    curMoveSpeed = 0;
                break;
            }
        }

        status = CHARACTERSTATUS.NONE;
    }

    IEnumerator DashCoolDown()
    {
        yield return null;
        
        while(true)
        {
            yield return new WaitForFixedUpdate();
            dashCoolDown -= Time.deltaTime;
            if (dashCoolDown <= 0)
                break;
        }

        dashCoolDown = maxDashCoolDown;
    }

    IEnumerator JetPack()
    {
        float currentHeight = originObj.transform.position.y;

        while(isJetPackOn)
        {
            yield return new WaitForFixedUpdate();
            currentHeight += 5;
            originObj.transform.position = new Vector2(originObj.transform.position.x, currentHeight);

            if (isUpBlocked)
            {
                originObj.transform.position = new Vector2(originObj.transform.position.x, originObj.transform.position.y - 15.0f);
                break;
            }
        }
    }

    public virtual void SetJetPackOn()
    {
        isJetPackOn = true;
    }

    public virtual void SetJetPackOff()
    {
        isJetPackOn = false;
    }
    
    public virtual void SetGravity()
    {
        if (isGrounded == false)
        {
            originObj.transform.Translate(Vector3.down * changingGravity * Time.deltaTime);

            if (changingGravity <= 1500)
                changingGravity *= 1.03f;
            if (changingGravity > 1500)
                changingGravity = 1500;
        }
        else
            changingGravity = defaultGravity;
    }

    //충돌 검사
    public virtual void ManageCollision()
    {
        //바닥 검사
        Vector2 rayDown = new Vector2(rt.bounds.center.x - rt.size.x / 2.55f, rt.bounds.center.y);
        Vector2 rayDown2 = new Vector2(rt.bounds.center.x + rt.size.x / 2.55f, rt.bounds.center.y);

        RaycastHit2D[] hitDown = Physics2D.RaycastAll(rayDown, Vector2.down, 200.0f);
        RaycastHit2D[] hitDown2 = Physics2D.RaycastAll(rayDown2, Vector2.down, 200.0f);

        RaycastHit2D hitDownFin = new RaycastHit2D();
        RaycastHit2D hitDownFin2 = new RaycastHit2D();

        for (int i = 0; i < hitDown.Length; i++)
            if (hitDown[i].transform.tag.Equals("Tilemap_rock"))
            {
                hitDownFin = hitDown[i];
                break;
            }
        for (int i = 0; i < hitDown2.Length; i++)
            if (hitDown2[i].transform.tag.Equals("Tilemap_rock"))
            {
                hitDownFin2 = hitDown2[i];
                break;
            }

        Debug.DrawRay(rayDown, Vector2.down * 200.0f, new Color(1.0f, 0.0f, 0.0f));
        Debug.DrawRay(rayDown2, Vector2.down * 200.0f, new Color(1.0f, 0.0f, 0.0f));

        //둘 중 하나가 검출되었을 때
        if ((hitDownFin && !hitDownFin2) || (!hitDownFin && hitDownFin2))
        {
            bool isFirst = hitDownFin ? true : false;
            if (isFirst == false)
            {
                rayDown = rayDown2;
                hitDownFin = hitDownFin2;
            }

            //거리
            float dist = Vector2.Distance(new Vector2(hitDownFin.transform.position.x, rayDown.y - rt.size.y / 2),
                new Vector2(hitDownFin.transform.position.x, hitDownFin.transform.position.y + 90));

            //일정 거리 이상 가까워지면..            
            
            if (hitDownFin.transform.position.y + 90 >= rayDown.y - rt.size.y / 2)
            {
                originObj.transform.position = new Vector2(originObj.transform.position.x, hitDownFin.transform.position.y + 90 + rt.size.y / 2);
                isGrounded = true;
            }
            else
                isGrounded = false;
        }
        //둘 다 검출되었을 때
        else if (hitDownFin && hitDownFin2)
        {
            //거리
            float dist = Vector2.Distance(new Vector2(hitDownFin.transform.position.x, rayDown.y - rt.size.y / 2),
                new Vector2(hitDownFin.transform.position.x, hitDownFin.transform.position.y + 90));
            float dist2 = Vector2.Distance(new Vector2(hitDownFin2.transform.position.x, rayDown2.y - rt.size.y / 2),
                new Vector2(hitDownFin2.transform.position.x, hitDownFin2.transform.position.y + 90));

            if (dist > dist2)
            {
                hitDownFin = hitDownFin2;
                rayDown = rayDown2;
            }

            //충돌했다면..
            if (hitDownFin.transform.position.y + 90 >= rayDown.y - rt.size.y / 2)
            {
                originObj.transform.position = new Vector2(originObj.transform.position.x, hitDownFin.transform.position.y + 90 + rt.size.y / 2);
                isGrounded = true;
            }
            else
                isGrounded = false;
        }
        else
            isGrounded = false;

        //위쪽 검사
        Vector2 rayUp = new Vector2(rt.bounds.center.x - rt.size.x / 2.15f, rt.bounds.center.y);
        Vector2 rayUp2 = new Vector2(rt.bounds.center.x + rt.size.x / 2.15f, rt.bounds.center.y);

        RaycastHit2D[] hitUp = Physics2D.RaycastAll(rayUp, Vector2.up, 200.0f);
        RaycastHit2D[] hitUp2 = Physics2D.RaycastAll(rayUp2, Vector2.up, 200.0f);

        RaycastHit2D hitUpFin = new RaycastHit2D();
        RaycastHit2D hitUpFin2 = new RaycastHit2D();

        for (int i = 0; i < hitUp.Length; i++)
            if (hitUp[i].transform.tag.Equals("Tilemap_rock")) { hitUpFin = hitUp[i]; break; }
        for (int i = 0; i < hitUp2.Length; i++)
            if (hitUp2[i].transform.tag.Equals("Tilemap_rock")) { hitUpFin2 = hitUp2[i]; break; }

        Debug.DrawRay(rayUp, Vector2.up * 200.0f, new Color(1.0f, 0.0f, 0.0f));
        Debug.DrawRay(rayUp2, Vector2.up * 200.0f, new Color(1.0f, 0.0f, 0.0f));

        //둘 중 하나가 검출되었을 때
        if ((hitUpFin && !hitUpFin2) || (!hitUpFin && hitUpFin2))
        {
            bool isFirst = hitUpFin ? true : false;
            if (isFirst == false)
            {
                rayUp = rayUp2;
                hitUpFin = hitUpFin2;
            }

            //거리
            float dist = Vector2.Distance(new Vector2(hitUpFin.transform.position.x, rayUp.y + rt.size.y / 2),
                new Vector2(hitUpFin.transform.position.x, hitUpFin.transform.position.y - 90));

            //일정 거리 이상 가까워지면..
            if (dist <= 10 && dist >= 0)
            {
                originObj.transform.position = new Vector2(originObj.transform.position.x, hitUpFin.transform.position.y - 90 - rt.size.y / 2 - 2);
                isUpBlocked = true;
            }
            else
                isUpBlocked = false;
        }
        //둘 다 검출되었을 때
        else if (hitUpFin && hitUpFin2)
        {
            //거리
            float dist = Vector2.Distance(new Vector2(hitUpFin.transform.position.x, rayUp.y + rt.size.y / 2),
                new Vector2(hitUpFin.transform.position.x, hitUpFin.transform.position.y - 90));
            float dist2 = Vector2.Distance(new Vector2(hitUpFin2.transform.position.x, rayUp2.y + rt.size.y / 2),
                new Vector2(hitUpFin2.transform.position.x, hitUpFin2.transform.position.y - 90));

            if (dist > dist2)
            {
                dist = dist2;
                hitUpFin = hitUpFin2;
            }

            //일정 거리 이상 가까워지면..
            if (dist <= 10 && dist >= 0)
            {
                originObj.transform.position = new Vector2(originObj.transform.position.x, hitUpFin.transform.position.y - 90 - rt.size.y / 2 - 2);
                isUpBlocked = true;
            }
            else
                isUpBlocked = false;
        }
        else
            isUpBlocked = false;


        //오른쪽으로 이동할 때..
        Vector2 rayRight = new Vector2(rt.bounds.center.x, rt.bounds.center.y + rt.size.y / 2.15f);
        Vector2 rayRight2 = new Vector2(rt.bounds.center.x, rt.bounds.center.y - rt.size.y / 2.15f);

        RaycastHit2D[] hitRight = Physics2D.RaycastAll(rayRight, Vector2.right, 200.0f);
        RaycastHit2D[] hitRight2 = Physics2D.RaycastAll(rayRight2, Vector2.right, 200.0f);

        RaycastHit2D hitRightFin = new RaycastHit2D();
        RaycastHit2D hitRightFin2 = new RaycastHit2D();

        for (int i = 0; i < hitRight.Length; i++)
            if (hitRight[i].transform.tag.Equals("Tilemap_rock")) { hitRightFin = hitRight[i]; break; }
        for (int i = 0; i < hitRight2.Length; i++)
            if (hitRight2[i].transform.tag.Equals("Tilemap_rock")) { hitRightFin2 = hitRight2[i]; break; }

        Debug.DrawRay(rayRight, Vector2.right * 200.0f, new Color(1.0f, 0.0f, 0.0f));
        Debug.DrawRay(rayRight2, Vector2.right * 200.0f, new Color(1.0f, 0.0f, 0.0f));

        //둘 중 하나가 검출되었을 때
        if ((hitRightFin && !hitRightFin2) || (!hitRightFin && hitRightFin2))
        {
            bool isFirst = hitRightFin ? true : false;
            if (isFirst == false)
            {
                rayRight = rayRight2;
                hitRightFin = hitRightFin2;
            }

            //거리
            float dist = Vector2.Distance(new Vector2(rayRight.x + rt.size.x / 2, hitRightFin.transform.position.y),
                new Vector2(hitRightFin.transform.position.x - 90, hitRightFin.transform.position.y));

            //일정 거리 이상 가까워지면..
            if (dist <= 10 && dist >= 0 && dir != Vector3.left)
            {
                originObj.transform.position = new Vector2(hitRightFin.transform.position.x - 90 - rt.size.x / 2,
                    originObj.transform.position.y);
                isRightBlocked = true;
            }
            else if (dist <= 10 && dist >= 0)
                isRightBlocked = true;
            else            
                isRightBlocked = false;            
        }
        //둘 다 검출되었을 때
        else if (hitRightFin && hitRightFin2)
        {
            //거리
            float dist = Vector2.Distance(new Vector2(rayRight.x + rt.size.x / 2, hitRightFin.transform.position.y),
                new Vector2(hitRightFin.transform.position.x - 90, hitRightFin.transform.position.y));
            float dist2 = Vector2.Distance(new Vector2(rayRight2.x + rt.size.x / 2, hitRightFin2.transform.position.y),
                new Vector2(hitRightFin2.transform.position.x - 90, hitRightFin2.transform.position.y));

            if (dist > dist2)
            {
                dist = dist2;
                hitRightFin = hitRightFin2;
            }

            //일정 거리 이상 가까워지면..
            if (dist <= 10 && dist >= 0 && dir != Vector3.left)
            {
                originObj.transform.position = new Vector2(hitRightFin.transform.position.x - 90 - rt.size.x / 2,
                    originObj.transform.position.y);
                isRightBlocked = true;
            }
            else if (dist <= 10 && dist >= 0)
                isRightBlocked = true;
            else
                isRightBlocked = false;
        }
        else
            isRightBlocked = false;

        //왼쪽으로 이동할 때..
        Vector2 rayLeft = new Vector2(rt.bounds.center.x, rt.bounds.center.y + rt.size.y / 2.15f);
        Vector2 rayLeft2 = new Vector2(rt.bounds.center.x, rt.bounds.center.y - rt.size.y / 2.15f);

        RaycastHit2D[] hitLeft = Physics2D.RaycastAll(rayLeft, Vector2.left, 200.0f);
        RaycastHit2D[] hitLeft2 = Physics2D.RaycastAll(rayLeft2, Vector2.left, 200.0f);

        RaycastHit2D hitLeftFin = new RaycastHit2D();
        RaycastHit2D hitLeftFin2 = new RaycastHit2D();

        for (int i = 0; i < hitLeft.Length; i++)
            if (hitLeft[i].transform.tag.Equals("Tilemap_rock")) { hitLeftFin = hitLeft[i]; break; }
        for (int i = 0; i < hitLeft2.Length; i++)
            if (hitLeft2[i].transform.tag.Equals("Tilemap_rock")) { hitLeftFin2 = hitLeft2[i]; break; }

        Debug.DrawRay(rayLeft, Vector2.left * 200.0f, new Color(1.0f, 0.0f, 0.0f));
        Debug.DrawRay(rayLeft2, Vector2.left * 200.0f, new Color(1.0f, 0.0f, 0.0f));

        //둘 중 하나가 검출되었을 때
        if ((hitLeftFin && !hitLeftFin2) || (!hitLeftFin && hitLeftFin2))
        {
            bool isFirst = hitLeftFin ? true : false;
            if (isFirst == false)
            {
                rayLeft = rayLeft2;
                hitLeftFin = hitLeftFin2;
            }

            //거리
            float dist = Vector2.Distance(new Vector2(rayLeft.x - rt.size.x / 2, hitLeftFin.transform.position.y),
                new Vector2(hitLeftFin.transform.position.x + 90, hitLeftFin.transform.position.y));

            //일정 거리 이상 가까워지면..
            if (dist <= 10 && dist >= 0 && dir != Vector3.right)
            {
                originObj.transform.position = new Vector2(hitLeftFin.transform.position.x + 90 + rt.size.x / 2,
                    originObj.transform.position.y);
                isLeftBlocked = true;
            }
            else if (dist <= 10 && dist >= 0)
                isLeftBlocked = true;
            else
                isLeftBlocked = false;
        }
        //둘 다 검출되었을 때
        else if (hitLeftFin && hitLeftFin2)
        {
            //거리
            float dist = Vector2.Distance(new Vector2(rayLeft.x - rt.size.x / 2, hitLeftFin.transform.position.y),
                   new Vector2(hitLeftFin.transform.position.x + 90, hitLeftFin.transform.position.y));
            float dist2 = Vector2.Distance(new Vector2(rayLeft2.x - rt.size.x / 2, hitLeftFin2.transform.position.y),
                   new Vector2(hitLeftFin2.transform.position.x + 90, hitLeftFin2.transform.position.y));

            if (dist > dist2)
            {
                dist = dist2;
                hitLeftFin = hitLeftFin2;
            }

            //일정 거리 이상 가까워지면..
            if (dist <= 10 && dist >= 0 && dir != Vector3.right)
            {
                originObj.transform.position = new Vector2(hitLeftFin.transform.position.x + 90 + rt.size.x / 2,
                    originObj.transform.position.y);
                isLeftBlocked = true;
            }
            else if (dist <= 10 && dist >= 0)
                isLeftBlocked = true;
            else
                isLeftBlocked = false;
        }
        else
            isLeftBlocked = false;
    }

    public virtual void ReduceHp(float pVal)
    {
        curHp -= pVal;

        if (curHp < 0)
            curHp = 0;

        SetHp();
    }

    public virtual void ReduceHp(float pVal, Vector3 pDir, float pVelocity = 7.5f)
    {
        curHp -= pVal;

        if (curHp < 0)
            curHp = 0;

        SetHp();

        if (curHp > 0)
            StartKnockBack(pDir, pVelocity);

        if (this.tag.Equals("Player"))
            _animator.SetTrigger("getHit");
    }

    public void RestoreHp(float pVal, bool toFool)
    {
        if (toFool)
            curHp = maxHp;
        else
            curHp += pVal;

        if (curHp > maxHp)
            curHp = maxHp;
    }

    protected void SetHp()
    {
        img_curHp.fillAmount = curHp / maxHp;
    }

    protected void StartKnockBack(Vector3 pDir, float pVelocity = 7.5f)
    {
        StartCoroutine(KnockBack(pDir, pVelocity));
    }
    

    IEnumerator KnockBack(Vector3 pDir, float pVelocity = 7.5f)
    {
        float velocity = pVelocity; // 넉백 속도

        Vector3 attackerDir = pDir;
        Vector3 currentPos = originObj.transform.position;

        while (true)
        {
            yield return new WaitForFixedUpdate();

            float curPosY = originObj.transform.position.y;

            if (velocity <= 0)
                break;
            
            // 옆이 막힌 경우 넉백 종료
            if (isLeftBlocked)
            {
                originObj.transform.position = new Vector3(currentPos.x + 1f, curPosY, currentPos.z);
                break;
            }
            else if (isRightBlocked)
            {
                originObj.transform.position = new Vector3(currentPos.x - 1f, curPosY, currentPos.z);
                break;
            }

            // 피격자 기준 등 뒤에서 맞은 경우
            if (attackerDir == dir)
            {
                originObj.transform.position = new Vector3(currentPos.x + dir.x * velocity, curPosY, currentPos.z);
            }
            // 피격자 기준 정면에서 맞은 경우
            else
            {
                originObj.transform.position = new Vector3(currentPos.x + (-dir.x) * velocity, curPosY, currentPos.z);
            }

            currentPos = originObj.transform.position;
            velocity -= 0.5f;
        }

        Debug.Log("넉백 종료");
    }
}
