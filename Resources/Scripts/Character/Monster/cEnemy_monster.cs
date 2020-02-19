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
    public int id;
    public int GetId() { return id; }
    //몬스터 광물 보유량
    protected cProperty rocks;
    public cProperty GetRocks() { return rocks; }
    //아이템 랜덤 드랍 확률
    private int per_BossKey = 100;
    // 플레이어 인식 여부, 공격 범위 위치 여부
    protected cEnemy_AttckBox attackBoxMng; // 공격 범위는 attackBox에서 관리함
    public cRangeNotizer notizer;
    public bool isInAttackRange;
    public bool isInNoticeRange;
    // (공격 행동시) 플레이어 위치
    protected Vector3 playerPos;
    // 리스폰 코루틴
    protected IEnumerator respawnCor;
    // 쿨타임 관리 변수
    protected float attackCoolTime;
    public float GetAttackCoolTime() { return attackCoolTime; }
    protected float coolTimer;

    public cAttackScript attackScript;

    protected cDungeonNormal_processor dp;
        
    public virtual void Init(string pNickname, cProperty pDamage, float pMaxMoveSpeed, cProperty pMaxHp, cProperty pCurHp,
        int pId, cProperty pRocks)
    {
        base.Init(pNickname, pDamage, pMaxMoveSpeed, pMaxHp, pCurHp);
        originObj = this.gameObject;
        rt = originObj.GetComponent<BoxCollider2D>();
        defaultGravity = 300.0f;
        changingGravity = defaultGravity;
        SetIsGrounded(false);
        jumpHeight = 200.0f;
        id = pId;
        rocks = new cProperty(pRocks);
        isDead = false;
        respawnTime = 5.0f;
        InitPos = this.transform.localPosition;
        isInNoticeRange = false;
        playerPos = Vector3.zero;
        attackBoxPos[0] = new Vector3(1.5f, 0f, 0f);
        attackBoxPos[2] = new Vector3(-1.5f, 0f, 0f);
        respawnCor = RespawnTimer();
        curMoveSpeed = maxMoveSpeed;
        attackCoolTime = 3.0f;
        attackBoxMng = attackBox.GetComponent<cEnemy_AttckBox>();
        isInAttackRange = false;

        attackBoxMng.Init();
        notizer.Init();
    }

    public virtual void Init(enemyInitStruct pEs)
    {
        base.Init(pEs.nickName, pEs.damage, pEs.maxMoveSpeed, pEs.maxHp, pEs.curHp);

        originObj = this.gameObject;
        rt = originObj.GetComponent<BoxCollider2D>();
        defaultGravity = 300.0f;
        changingGravity = defaultGravity;
        jumpHeight = 200.0f;
        id = pEs.id;
        rocks = new cProperty(pEs.rocks);
        isDead = false;
        respawnTime = 5.0f;
        InitPos = this.transform.localPosition;
        isInNoticeRange = false;
        playerPos = Vector3.zero;
        attackBoxPos[0] = new Vector3(1.5f, 0f, 0f);
        attackBoxPos[2] = new Vector3(-1.5f, 0f, 0f);
        respawnCor = RespawnTimer();
        curMoveSpeed = maxMoveSpeed;
        attackCoolTime = 3.0f;
        attackBoxMng = attackBox.GetComponent<cEnemy_AttckBox>();
        isInAttackRange = false;
        ChangeDir(Vector3.right);

        attackBoxMng.Init();
        notizer.Init();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        
        if(isDead.Equals(false))
        {
            SetGravity();
            Move();
        }
    }
    
    //Defalut Move
    protected virtual void Move()
    {
        if (isDead.Equals(true))
            return;

        if(_animator != null)
             _animator.SetFloat("MoveSpeed", curMoveSpeed);

        // 인식 범위 내 진입
        if (isInNoticeRange.Equals(true))
        {
            //Hp바 on
            img_curHp.transform.parent.gameObject.SetActive(true);

            // 인식 범위 안에 들어왔지만 공격 범위 내에는 없는 경우 ( cRangeNotizer에서 감지 )
            if (isInAttackRange.Equals(false))
            {
                playerPos = cUtil._player.gameObject.transform.position;
                this.transform.Translate(dir * curMoveSpeed * Time.deltaTime);

                if (playerPos.x >= this.transform.position.x)
                    ChangeDir(Vector3.right);

                else if (playerPos.x < this.transform.position.x)
                    ChangeDir(Vector3.left);
            }
            // 공격 범위 안에 들어온 경우
            // 공격 자체는 cEnemy_AttackBox에서 처리
            else if (isInAttackRange.Equals(true))
            {
                playerPos = cUtil._player.gameObject.transform.position;
                if (playerPos.x >= this.transform.position.x)
                    ChangeDir(Vector3.right);
                else if (playerPos.x < this.transform.position.x)
                    ChangeDir(Vector3.left);

                //공격 쿨타임 돌린다.
                coolTimer += Time.deltaTime;
                if (coolTimer >= attackCoolTime)
                {
                    Attack1();
                }
            }
        }
        // idle 상태
        else if (isInNoticeRange.Equals(false))
        {
            img_curHp.transform.parent.gameObject.SetActive(false);
            coolTimer = 0;
            this.transform.Translate(dir * curMoveSpeed * Time.deltaTime);
            //막히면 방향 바꿔준다.
            if (isRightBlocked == true)
            {
                isRightBlocked = false;
                ChangeDir(Vector3.left);
            }
            else if (isLeftBlocked == true)
            {
                isLeftBlocked = false;
                ChangeDir(Vector3.right);
            }
        }
    }

    public virtual void Attack1()
    {
        attackScript.SetAttackParameter(damage.value, GetDirection());
        coolTimer = 0;
        _animator.SetTrigger("Attack");
    }

    public virtual void SetBullet1()
    {

    }


    public virtual void ChangeDir(Vector3 pDir)
    {
        dir = pDir;

        if(dir.Equals(Vector3.right))
            originObj.transform.localScale = new Vector3(-1, 1, 1);
        else if(dir.Equals(Vector3.left))
            originObj.transform.localScale = new Vector3(1, 1, 1);
    }

    public override void ReduceHp(long pVal)
    {
        curHp.value -= pVal;
        
        if (curHp.value <= 0)
        {
            coolTimer = 0;
            curHp.value = 0;
            isDead = true;
            _animator.SetTrigger("Dead");
            img_curHp.transform.parent.gameObject.SetActive(false);
            this.GetComponent<BoxCollider2D>().enabled = false;
            // 리스폰 타이머 활성화
            StartCoroutine(RespawnTimer());
        }
        if (isDead.Equals(false))
            _animator.SetTrigger("GetHit");

        
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
            _animator.SetTrigger("Dead");
            img_curHp.transform.parent.gameObject.SetActive(false);
            this.GetComponent<BoxCollider2D>().enabled = false;
            // 리스폰 타이머 활성화
            StartCoroutine(RespawnTimer());
        }

        if (isDead.Equals(false))
            _animator.SetTrigger("GetHit");
        SetHp();

        // 넉백
        if (curHp.value > 0)
            StartKnockBack(pDir, pVelocity);
    }

    // 리스폰 관리
    IEnumerator RespawnTimer()
    {
        float time = 0;

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
        _animator.SetTrigger("Init");
        this.GetComponent<BoxCollider2D>().enabled = true;
        this.transform.localPosition = InitPos;
        isDead = false;
        curHp.value = maxHp.value;
        curMoveSpeed = maxMoveSpeed;
        SetHp();
        Debug.Log("respawn");
        ChangeDir(Vector3.right);
    }
}
