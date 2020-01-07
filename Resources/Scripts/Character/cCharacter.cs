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

    public cTileMng tileMng;
    public int isUpBlocked_123;
    public int isGrounded_123;

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

    protected virtual void FixedUpdate()
    {
        if (cUtil._tileMng != null && tileMng == null)
            tileMng = cUtil._tileMng;
        
        if(tileMng != null)
            tileMng.CheckCanGroundTile(this);
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

        isGrounded = false;
        
        float currentHeight = originObj.transform.position.y;
        float maxHeight = originObj.transform.position.y + jumpHeight;        

        while (true)
        {
            if(goBreak == true)            
                break;

            yield return new WaitForFixedUpdate();

            if (status.Equals(CHARACTERSTATUS.ATTACK))
                break;

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
                changingGravity *= 1.02f;
            if (changingGravity > 1500)
                changingGravity = 1500;
        }
        else
            changingGravity = defaultGravity;
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
