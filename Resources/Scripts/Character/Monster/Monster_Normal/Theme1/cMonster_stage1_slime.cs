using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cMonster_stage1_slime : cEnemy_Ranged
{
    // 분열된 횟수
    public bool isSplited;

    public GameObject motherObj;
    private GameObject clone1;
    private GameObject clone2;

    public bool isClone1Dead;
    public bool isClone2Dead;
    
    public cMonster_stage1_slime()
    {
        isSplited = false;
    }

    private void Start()
    {
        Init(cEnemyTable.SetMonsterInfo(1));
    }

    public override void Init(enemyInitStruct pEs)
    {
        base.Init(pEs);
        
        respawnTime = 5.0f;
        attackCoolTime = 3.0f;
        bulletCoolTime = 2.0f;
        timer = bulletCoolTime;
        bulletDamage = (long)(damage.value * 0.5f);
        _animator = this.transform.Find("rig_slime").GetComponent<Animator>();
        isClone1Dead = false;
        isClone2Dead = false;
    }

    protected override void Move()
    {
        if (isDead.Equals(true))
            return;

        if (_animator != null)
            _animator.SetFloat("MoveSpeed", curMoveSpeed);

        // 인식 범위 내 진입
        if (isInNoticeRange)
        {   
            //Hp바 on
            img_curHp.transform.parent.gameObject.SetActive(true);

            playerPos = cUtil._player.gameObject.transform.position;
            if (playerPos.x >= this.transform.position.x)
                ChangeDir(Vector3.right);
            else if (playerPos.x < this.transform.position.x)
                ChangeDir(Vector3.left);
            
            // 공격 범위 바깥
            if (isInAttackRange.Equals(false))
            {
                this.transform.Translate(dir * curMoveSpeed * Time.deltaTime);
            }
            // 공격 범위 안에 들어온 경우
            // 공격 자체는 cEnemy_AttackBox에서 처리
            else if (isInAttackRange.Equals(true))
            {
                timer += Time.deltaTime;
                if (timer >= bulletCoolTime)
                {
                    Attack1();
                }                
            }
        }
        // idle
        else if (isInNoticeRange.Equals(false))
        {
            //Hp바 off
            img_curHp.transform.parent.gameObject.SetActive(false);

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

    public override void Attack1()
    {
        base.Attack1();
    }

    public override void SetBullet1()
    {
        // 기본값 : 발사체 1, 발사체 타입 일반형, 중력 적용 x, 타겟 : 유저
        bulletManager.LaunchBullet(this.gameObject.transform.position, true, cUtil._player.originObj.transform.position);
        timer = 0;
    }

    public void Split()
    {
        isSplited = true;
    }
    public void SetMotherObj(GameObject pObj)
    {
        motherObj = pObj;
    }

    public void StartRespawn()
    {
        if(isClone1Dead && isClone2Dead)
        {
            StartCoroutine(respawnCor);
        }
    }

    public override void ReduceHp(long pVal, Vector3 pDir, float pVelocity = 7.5f)
    {
        curHp.value -= pVal;

        if (isDead.Equals(false))
            _animator.SetTrigger("GetHit");

        // 넉백
        if (curHp.value > 0)
            StartKnockBack(pDir, pVelocity);
        
        SetHp();

        if (curHp.value <= 0)
        {
            curHp.value = 0;
            isDead = true;
            _animator.SetTrigger("Dead");
            img_curHp.transform.parent.gameObject.SetActive(false);
            this.GetComponent<BoxCollider2D>().enabled = false;

            // 이미 분열한 슬라임이라면
            if (isSplited)
            {
                if(!motherObj.GetComponent<cMonster_stage1_slime>().isClone1Dead)
                {   
                    motherObj.GetComponent<cMonster_stage1_slime>().isClone1Dead = true;
                    this.gameObject.SetActive(false);
                }
                else
                {
                    motherObj.GetComponent<cMonster_stage1_slime>().isClone2Dead = true;
                    this.gameObject.SetActive(false);

                    motherObj.GetComponent<cMonster_stage1_slime>().Respawn();
                }
            }

            else
            {
                // 분열        
                clone1 = Instantiate(Resources.Load<GameObject>(cPath.PrefabPath() + "Monster/Monster_Slime"), new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z),
                    Quaternion.identity, this.transform.parent);
                clone1.transform.position = new Vector3(clone1.transform.position.x, clone1.transform.position.y, clone1.transform.position.z);
                clone1.GetComponent<cMonster_stage1_slime>().Split();
                clone1.GetComponent<cMonster_stage1_slime>().SetMotherObj(this.gameObject);
                
                clone2 = Instantiate(Resources.Load<GameObject>(cPath.PrefabPath() + "Monster/Monster_Slime"),new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z),
                    Quaternion.identity, this.transform.parent);
                clone2.transform.position = new Vector3(clone2.transform.position.x, clone2.transform.position.y, clone2.transform.position.z);
                clone2.GetComponent<cMonster_stage1_slime>().Split();
                clone2.GetComponent<cMonster_stage1_slime>().SetMotherObj(this.gameObject);
            }
        }
    }
}
