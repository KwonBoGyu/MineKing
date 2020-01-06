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
    protected float curMoveSpeed;
    protected float maxHp;
    protected float curHp;
    protected float jumpHeight;
    protected bool isDoubleJump;
    protected float dashDistance;
    protected float maxDashCoolDown; // 실제 대쉬 쿨다운
    protected float dashCoolDown; // 쿨다운 계산용 (변화)
    protected bool isJetPackOn;
    protected CHARACTERSTATUS status;
    protected CHARDIRECTION charDir;
    public CHARDIRECTION GetCharDir() { return charDir; }
    protected Vector3 dir;
    public GameObject originObj;
    protected BoxCollider2D rt;

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

    public float pheight;
    public float pwidth;
    public float mapHeight;
    public int[] Playerpos = { 1, 1 };
    public int[,] Map = { {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                     {-1,0,0,0,0,0,0,0,0,-1},
                                     {-1,0,0,0,0,0,0,0,0,-1},
                                     {-1,0,0,0,0,0,0,0,0,-1},
                                     {-1,1,0,0,0,0,0,0,0,-1},
                                     {-1,1,0,0,0,0,0,1,0,-1},
                                     {-1,1,1,0,0,0,1,1,1,-1},
                                     {-1,1,1,1,0,1,1,1,1,-1},
                                     {-1,1,1,1,1,1,1,1,1,-1},
                                     {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},};
    public virtual void Init(string pNickName, float pDamage, float pMaxMoveSpeed, float pMaxHp, float pCurHp)
    {
        nickName = pNickName;
        damage = pDamage;
        maxMoveSpeed = pMaxMoveSpeed;
        curMoveSpeed = 0;
        maxHp = pMaxHp;
        curHp = pCurHp;
        dir = Vector3.right;
        jumpHeight = 200.0f;
        attackBoxPos = new Vector3[4];
        status = CHARACTERSTATUS.NONE;

        dashDistance = 300.0f;
        maxDashCoolDown = 5.0f;
        dashCoolDown = maxDashCoolDown;
        isJetPackOn = false;

        pheight = 171.84f;
        pwidth = 61.52f;
        mapHeight = 1800f;
        originObj.transform.position = new Vector2(Playerpos[1] * 180, mapHeight - Playerpos[0] * 180);
        Debug.Log("init done");
    }



    public virtual void FixedUpdate()
    {
        ManageCollision();
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

        float currentPosition = originObj.transform.position.x;
        float maxPosition = originObj.transform.position.x + dashDistance;
        Vector3 fixedDir = dir;
        if (fixedDir == Vector3.right)
        {
            maxPosition = originObj.transform.position.x + dashDistance;
        }
        else
        {
            maxPosition = originObj.transform.position.x - dashDistance;
        }

        while (true)
        {
            yield return new WaitForFixedUpdate();
            dir = fixedDir;
            if (isGrounded == false)
                break;

            if (fixedDir == Vector3.right)
            {
                currentPosition += 15f;
            }
            else
            {
                currentPosition -= 15f;
            }

            originObj.transform.position = new Vector3(currentPosition, originObj.transform.position.y, originObj.transform.position.z);

            if (fixedDir == Vector3.right)
            {
                if (currentPosition >= maxPosition)
                {
                    originObj.transform.position = new Vector2(maxPosition, originObj.transform.position.y);
                    break;
                }
            }
            else
            {
                if (currentPosition <= maxPosition)
                {
                    originObj.transform.position = new Vector2(maxPosition, originObj.transform.position.y);
                    break;
                }
            }

            if (isLeftBlocked || isRightBlocked)
            {
                originObj.transform.position = new Vector2(originObj.transform.position.x - 15.0f, originObj.transform.position.y);
                break;
            }
        }
    }

    IEnumerator DashCoolDown()
    {
        yield return null;
        
        while(true)
        {
            yield return new WaitForFixedUpdate();
            Debug.Log(dashCoolDown);
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
        Vector2 pPos = originObj.transform.position;

        //맵을 타일 하나의 크기인 180으로 가로와 세로를 나누어 int [,] Map의 행렬에 그 칸의 정보가 들어있게 만듬
        //맵 상의 어떤 위치의 x과 y값을 180으로 나눈 값을 X,Y라 하면 그 위치의 칸은 Map[Y,X]이다. 
        //Map[Y,X]에서 Y가 행, X가 열이다
        //주의!)Map[Y,X]에서 Y가 증가하면 실제 좌표값은 감소하고 Y가 감소하면 좌표값은 증가한다
        //플레이어 위치를 TestPlayerPos라 하고, y값에 해당하는 행은 [0]에 x값에 해당하는 열은 [1]에
        Playerpos = new int[] { Mathf.FloorToInt((mapHeight - pPos.y) / 180), Mathf.FloorToInt(pPos.x / 180) };

        Debug.Log("y:"+Playerpos[0]+"x:"+Playerpos[1]);
        Debug.Log(pPos);
        
        ////현재 칸 빈공간
        //if (Map[Playerpos[0], Playerpos[1]].Equals(0))
        //{
            //아래 막힘&땅 닿는 범위에 있음
            if ((pPos.y <= (mapHeight - Playerpos[0] * 180.0f)-(85f-pheight/2)) && (!Map[Playerpos[0] + 1, Playerpos[1]].Equals(0)))
            {
                originObj.transform.position = new Vector2(originObj.transform.position.x, (mapHeight - Playerpos[0] * 180.0f) - 90 + pheight / 2);
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
            //왼쪽 막힘&땅 닿는 범위에 있음
            if ((pPos.x < Playerpos[1] * 180.0f - (85.0f-pwidth)) && (!Map[Playerpos[0], Playerpos[1] - 1].Equals(0)))
            {
                originObj.transform.position = new Vector2(Playerpos[1] * 180.0f - (90.0f - pwidth), originObj.transform.position.y);
                isLeftBlocked = true;
            }
            else
            {
                isLeftBlocked = false;
            }
            //오른쪽 막힘&땅 닿는 범위에 있음
            if ((pPos.x > Playerpos[1] * 180.0f + (85.0f - pwidth)) && (!Map[Playerpos[0], Playerpos[1] + 1].Equals(0)))
            {
                originObj.transform.position = new Vector2(Playerpos[1] * 180.0f + (90.0f - pwidth), originObj.transform.position.y);
                isRightBlocked = true;
            }
            else
            {
                isRightBlocked = false;
            }
            //위쪽 막힘&땅 닿는 범위에 있음
            if ((pPos.y > (mapHeight - Playerpos[0] * 180.0f) + (90.0f - pheight / 2)) && (!Map[Playerpos[0] - 1, Playerpos[1]].Equals(0)))
            {
                originObj.transform.position = new Vector2(originObj.transform.position.x, (mapHeight - Playerpos[0] * 180.0f) + (90f - pheight / 2));
                isUpBlocked = true;
            }
            else
            {
                isUpBlocked = false;
            }
        //}
        ////현재 칸 채워져있음
        //else
        //{
        //    if (pPos.x - Playerpos[1] * 180.0f >= 0)
        //    {

        //    }
        //    else
        //    {

        //    }
        //    if (pPos.y - (mapHeight - Playerpos[0] * 180.0f) >= 0)
        //    {

        //    }
        //    else
        //    {

        //    }
        //}

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
