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

public class cCharacter : cObject
{
    protected string nickName;
    protected cProperty damage;
    protected float maxMoveSpeed;
    protected float dashMoveSpeed;
    protected const float dashTime = 0.7f;
    protected const float jumpTime = 0.5f;
    protected float curMoveSpeed;
    protected cProperty maxHp;
    protected cProperty curHp;
    protected float jumpHeight;
    public byte jumpCount;
    public bool isJumpStart;
    protected float maxDashCoolDown; // 실제 대쉬 쿨다운
    protected float dashCoolDown; // 쿨다운 계산용 (변화)
    protected bool isJetPackOn;
    protected bool isClimbing;
    protected CHARACTERSTATUS status;
    protected CHARDIRECTION charDir;
    public CHARDIRECTION GetCharDir() { return charDir; }
    protected Vector3 dir;

    //크리티컬
    public bool isCritical;

    //hp
    public Image img_curHp;
    public Text t_hp;
    protected IEnumerator hpCor;

    // 중력 적용에 필요한 변수
    public float changingGravity;
    public float defaultGravity;

    private bool horizontalGroundJumpCheck;
    public override void SetIsGrounded(bool pGrounded)
    {
        if (isGrounded.Equals(pGrounded))
            return;

        isGrounded = pGrounded;

        if (GetIsGrounded().Equals(true))
        {
            if (isJumpStart.Equals(false))
                jumpCount = 0;

            isJumpStart = false;
            if (jumpCount > 1)
                jumpCount = 0;
        }
    }
    public GameObject attackBox;
    public Vector3[] attackBoxPos; //오른, 아래, 왼, 위

    private IEnumerator cor_knockBack;
    protected Animator _animator;

    //이펙트
    public ParticleSystem[] effects;

    //사운드
    public cSoundMng sm;

    public virtual void Init(string pNickName, cProperty pDamage, float pMaxMoveSpeed, cProperty pMaxHp, cProperty pCurHp)
    {
        nickName = pNickName;
        damage = new cProperty(pDamage);
        maxMoveSpeed = pMaxMoveSpeed;
        dashMoveSpeed = maxMoveSpeed + 400;
        curMoveSpeed = 0;
        maxHp = new cProperty(pMaxHp);
        curHp = new cProperty(pCurHp);
        SetHp();
        dir = Vector3.right;
        jumpHeight = 200.0f;
        attackBoxPos = new Vector3[4];
        status = CHARACTERSTATUS.NONE;

        maxDashCoolDown = 4.0f;
        dashCoolDown = maxDashCoolDown;
        isJetPackOn = false;
    }

    public cProperty GetMaxHp() { return maxHp; }
    public cProperty GetCurHP() { return curHp; }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (tileMng != null)
        {
            tileMng.CheckCanGroundTile(this);
        }
    }

    public cProperty GetDamage() { return damage; }

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
        bool goBreak = false;
        Debug.Log(jumpCount);
        if (jumpCount > 2)
        {
            jumpCount = 0;
            goBreak = true;
        }
        SetIsClimbing(false);

        float jumpTimer = 0;
        float factor;

        float currentHeight = originObj.transform.position.y;

        sm.playEffect(1);
        while (true)
        {
            if (goBreak || isClimbing)
            {
                break;
            }

            yield return new WaitForFixedUpdate();

            if (status.Equals(CHARACTERSTATUS.ATTACK))
                break;

            status = CHARACTERSTATUS.JUMP;
            isGrounded = false;

            factor = Mathf.PI * (jumpTimer / jumpTime) * 0.5f;

            originObj.transform.position = new Vector3(originObj.transform.position.x,
                currentHeight + jumpHeight * Mathf.Sin(factor), originObj.transform.position.z);

            if (jumpTimer >= jumpTime)
            {
                changingGravity = defaultGravity;
                break;
            }
            if (isUpBlocked == true)
            {
                changingGravity = defaultGravity;
                break;
            }
            jumpTimer += Time.deltaTime;
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

            if (DashTimer > dashTime)
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

        while (true)
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

        while (isJetPackOn)
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
        if (!isGrounded && !status.Equals(CHARACTERSTATUS.JUMP))
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

    public virtual void ReduceHp(long pVal)
    {
        Debug.Log(this.name + " hp : " + this.curHp.value);
        curHp.value -= pVal;

        if (curHp.value < 0)
            curHp.value = 0;

        SetHp();
    }

    public virtual void ReduceHp(long pVal, Vector3 pDir, float pVelocity = 7.5f)
    {
        curHp.value -= pVal;

        if (curHp.value < 0)
            curHp.value = 0;

        SetHp();

        if (curHp.value > 0)
            StartKnockBack(pDir, pVelocity);

        if (originObj.tag.Equals("Player"))
        {
            _animator.SetTrigger("getHit");
            SetStatus(CHARACTERSTATUS.NONE);
        }
    }

    public void RestoreHp(cProperty pVal, bool toFool = false)
    {
        if (toFool)
            curHp.value = maxHp.value;
        else
            curHp.value += pVal.value;

        if (curHp.value > maxHp.value)
            curHp.value = maxHp.value;
        SetHp();
    }

    protected void SetHp()
    {
        hpCor = HpInterpolation();
        StartCoroutine(hpCor);
        if (this.tag.Equals("Player"))
            t_hp.text = curHp.GetValueToString() + " / " + maxHp.GetValueToString();
    }

    protected void StartKnockBack(Vector3 pDir, float pVelocity = 7.5f)
    {
        StartCoroutine(KnockBack(pDir, pVelocity));
    }


    IEnumerator KnockBack(Vector3 pDir, float pVelocity = 7.5f)
    {
        float velocity = pVelocity; // 넉백 속도

        Vector3 attackerDir;
        Vector3 currentPos = originObj.transform.position;

        if (pDir.x <= 0)
        {
            attackerDir = Vector3.left;
        }
        else
        {
            attackerDir = Vector3.right;
        }

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

    public bool GetIsClimbing() { return isClimbing; }
    public void SetIsClimbing(bool pbool)
    {
        if (isClimbing.Equals(pbool))
            return;

        if (isClimbing.Equals(false))
        {
            _animator.SetTrigger("Crawl");
        }

        isClimbing = pbool;
        _animator.SetBool("isCrawl", isClimbing);
    }

    IEnumerator HpInterpolation()
    {
        float prevAmount = img_curHp.fillAmount;
        float curAmount = (float)curHp.value / (float)maxHp.value;
        float tick = (curAmount - prevAmount) / 10f;

        while (true)
        {
            yield return new WaitForFixedUpdate();
            img_curHp.fillAmount = img_curHp.fillAmount + tick;
            if (Mathf.Abs(img_curHp.fillAmount - curAmount) <= Mathf.Abs(tick))
            {
                img_curHp.fillAmount = curAmount;
                break;
            }
        }
    }
}
