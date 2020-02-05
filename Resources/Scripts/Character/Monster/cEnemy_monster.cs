using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cEnemy_monster : cCharacter
{
    //초기 위치
    protected Vector3 InitPos;
    //몬스터의 생사 상태
    protected bool isDead;
    //리스폰 시간, 타이머
    public float respawnTime;
    //몬스터 id
    protected int id;
    public int GetId() { return id; }
    //몬스터 광물 보유량
    protected cProperty rocks;
    public cProperty GetRocks() { return rocks; }
    //아이템 랜덤 드랍 확률
    private int per_BossKey = 100;
    // 플레이어 인식 여부, 공격 범위 인식 여부
    public bool isInNoticeRange;
    public bool isInAttackRange;
    // (공격 행동시) 플레이어 위치
    protected Vector3 playerPos;
    // 리스폰 코루틴
    protected IEnumerator respawnCor;
    // 쿨타임 관리 변수
    protected float attackCoolTime;
    protected float curCoolTime;
    protected bool isAttackReady;

    protected cDungeonNormal_processor dp;
        
    public virtual void Init(string pNickname, cProperty pDamage, float pMaxMoveSpeed, cProperty pMaxHp, cProperty pCurHp,
        int pId, cProperty pRocks)
    {
        base.Init(pNickname, pDamage, pMaxMoveSpeed, pMaxHp, pCurHp);

        originObj = this.gameObject;
        rt = originObj.GetComponent<BoxCollider2D>();
        defaultGravity = 300.0f;
        changingGravity = defaultGravity;
        isGrounded = false;
        jumpHeight = 200.0f;
        id = pId;
        rocks.value = pRocks.value;
        isDead = false;
        respawnTime = 5.0f;
        InitPos = this.transform.localPosition;
        isInNoticeRange = false;
        isInAttackRange = false;
        playerPos = Vector3.zero;
        attackBoxPos[0] = new Vector3(1.5f, 0f, 0f);
        attackBoxPos[2] = new Vector3(-1.5f, 0f, 0f);
        respawnCor = RespawnTimer();
        curMoveSpeed = maxMoveSpeed;
        attackCoolTime = 3.0f;
        curCoolTime = attackCoolTime;
        isAttackReady = true;
    }

    public virtual void Init(enemyInitStruct pEs)
    {
        base.Init(pEs.nickName, pEs.damage, pEs.maxMoveSpeed, pEs.maxHp, pEs.curHp);

        originObj = this.gameObject;
        rt = originObj.GetComponent<BoxCollider2D>();
        defaultGravity = 300.0f;
        changingGravity = defaultGravity;
        isGrounded = false;
        jumpHeight = 200.0f;
        id = pEs.id;
        rocks = pEs.rocks;
        isDead = false;
        respawnTime = 5.0f;
        InitPos = this.transform.localPosition;
        isInNoticeRange = false;
        isInAttackRange = false;
        playerPos = Vector3.zero;
        attackBoxPos[0] = new Vector3(1.5f, 0f, 0f);
        attackBoxPos[2] = new Vector3(-1.5f, 0f, 0f);
        respawnCor = RespawnTimer();
        curMoveSpeed = maxMoveSpeed;
        attackCoolTime = 3.0f;
        curCoolTime = attackCoolTime;
        isAttackReady = true;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        
        if(isDead.Equals(false))
        {
            SetGravity();
            Move();
        }

        // 쿨타임 관리
        // 공격이 준비되지 않았을 경우
        if (isAttackReady.Equals(false))
        {
            curCoolTime += Time.deltaTime;
        }
        // 공격이 준비된 경우
        else
        {
            curCoolTime = 0;
        }

        // 쿨타임 다 차면 공격 가능
        if (curCoolTime >= attackCoolTime)
        {
            isAttackReady = true;
            curCoolTime = 0;
        }
    }
    
    protected virtual void Move()
    {
        // 인식 범위 내 진입
        if (isInNoticeRange)
        {
            // 쿨타임이 다 차면 공격
            if (isAttackReady)
            {
                // 인식 범위 안에 들어왔지만 공격 범위 내에는 없는 경우 ( cRangeNotizer에서 감지 )
                if (!isInAttackRange)
                {
                    playerPos = cUtil._player.gameObject.transform.position;
                    this.transform.Translate(dir * curMoveSpeed * Time.deltaTime);

                    if (playerPos.x >= this.transform.position.x)
                        dir = Vector3.right;

                    else if (playerPos.x < this.transform.position.x)
                        dir = Vector3.left;
                }
                // 공격 범위 안에 들어온 경우
                else if (isInAttackRange)
                {
                    playerPos = cUtil._player.gameObject.transform.position;
                    if (playerPos.x >= this.transform.position.x)
                        dir = Vector3.right;
                    else if (playerPos.x < this.transform.position.x)
                        dir = Vector3.left;

                    //쿨타임이 다 찼을 때 히트박스 활성화
                    if (dir == Vector3.right)
                    {
                        attackBox.transform.localPosition = attackBoxPos[0];
                        attackBox.gameObject.SetActive(true);
                    }
                    else if (dir == Vector3.left)
                    {
                        attackBox.transform.localPosition = attackBoxPos[2];
                        attackBox.gameObject.SetActive(true);
                    }
                    isAttackReady = false;
                }
            }
        }
        // idle 상태
        else if (!isInNoticeRange)
        {
            Debug.Log("dir : " + dir);
            Debug.Log("curMoveSpeed : " + curMoveSpeed);
            this.transform.Translate(dir * curMoveSpeed * Time.deltaTime);
            //막히면 방향 바꿔준다.
            if (isRightBlocked == true)
            {
                isRightBlocked = false;
                dir = Vector3.left;
            }
            else if (isLeftBlocked == true)
            {
                isLeftBlocked = false;
                dir = Vector3.right;
            }
        }
    }

    public override void ReduceHp(long pVal)
    {
        curHp.value -= pVal;
        
        if (curHp.value <= 0)
        {
            curHp.value = 0;
            isDead = true;
            this.GetComponent<BoxCollider2D>().enabled = false;
            // 리스폰 타이머 활성화
            StartCoroutine(RespawnTimer());
        }
        SetHp();
    }

    // 공격자가 있는 경우 공격자가 바라보는 방향을 pDir로 받음
    public override void ReduceHp(long pVal, Vector3 pDir, float pVelocity = 7.5f)
    {
        curHp.value -= pVal;
        
        // 체력이 0 이하로 떨어질 시 코루틴 중지, 리스폰 타이머 활성화
        if (curHp.value <= 0)
        {
            curHp.value = 0;
            isDead = true;
            this.GetComponent<BoxCollider2D>().enabled = false;
            // 리스폰 타이머 활성화
            StartCoroutine(RespawnTimer());
        }

        SetHp();

        // 넉백
        if (curHp.value > 0)
            StartKnockBack(pDir, pVelocity);
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.tag == "Player")
    //    {
    //        collision.transform.GetChild(0).GetComponent<cPlayer>().ReduceHp(damage);
    //        Debug.Log("attacked by " + this.nickName);
    //    }
    //}

    // 리스폰 관리
    IEnumerator RespawnTimer()
    {
        float time = 0;
        this.transform.localPosition = new Vector3(InitPos.x, InitPos.y, -1000.0f);

        while (true)
        {
            yield return new WaitForFixedUpdate();

            if (time >= respawnTime)
            {
                RespawnInit();
                break;
            }

            time += Time.deltaTime;
        }
    }

    protected virtual void RespawnInit()
    {
        this.GetComponent<BoxCollider2D>().enabled = true;
        this.transform.localPosition = InitPos;
        isDead = false;
        curHp = maxHp;
        curMoveSpeed = maxMoveSpeed;
        SetHp();
        Debug.Log("respawn");
    }
}
