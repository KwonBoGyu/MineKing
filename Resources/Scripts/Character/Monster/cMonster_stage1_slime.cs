using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cMonster_stage1_slime : cEnemy_monster
{
    // 분열된 횟수
    public bool isSplited;
    public GameObject bullet;
    public cBulletManager bulletMng;

    private GameObject clone1;
    private GameObject clone2;

    public bool isClone1Dead;
    public bool isClone2Dead;

    private float maxBulletCoolDown;
    private float curBulletCoolDown;
    
    public cMonster_stage1_slime()
    {
        isSplited = false;
    }

    void Start()
    {
        Init(cEnemyTable.SetMonsterInfo(0));
        curMoveSpeed = maxMoveSpeed;
        maxBulletCoolDown = 4.0f;
        curBulletCoolDown = maxBulletCoolDown;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (curBulletCoolDown < maxBulletCoolDown)
        {
            curBulletCoolDown += Time.deltaTime;
        }
        else if ( curBulletCoolDown >= maxBulletCoolDown)
        {
            curBulletCoolDown = maxBulletCoolDown;
        }
    }

    public void Split()
    {
        isSplited = true;
    }
    
    protected override void Move()
    {
        // idle
        if (!isInNoticeRange)
        {
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
        // 인식 범위 안에 들어왔지만 공격 범위 내에는 없는 경우 ( cRangeNotizer에서 감지 )
        else if (isInNoticeRange && curBulletCoolDown == maxBulletCoolDown)
        {
            if (!isInAttackRange)
            {
                if (bullet.activeSelf.Equals(false))
                {
                    bulletMng.SetBullet(0);
                }
                else
                {
                    curBulletCoolDown = 0;
                }
            }
            // 공격 범위 안에 들어온 경우
            else
            {
                time += Time.deltaTime;
                playerPos = dp._player.transform.position;
                if (playerPos.x >= this.transform.position.x)
                    dir = Vector3.right;
                else if (playerPos.x < this.transform.position.x)
                    dir = Vector3.left;

                //쿨타임이 다 찼을 때 히트박스 활성화
                if (time >= attackDelay)
                {
                    time = 0;
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
                }
            }

        }
        
    }

    public void StartRespawn()
    {
        if(isClone1Dead && isClone2Dead)
        {
            StartCoroutine(respawnCor);
        }
    }

    public override void ReduceHp(float pVal, Vector3 pDir, float pVelocity = 7.5f)
    {
        curHp -= pVal;

        if (curHp <= 0)
        {
            curHp = 0;
            isDead = true;
            this.GetComponent<BoxCollider2D>().enabled = false;
            this.curHp = this.maxHp;
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -1000f);
            
            // 이미 분열한 슬라임이라면
            if (isSplited)
            {
                if(!this.transform.parent.GetComponent<cMonster_stage1_slime>().isClone1Dead)
                {
                    this.transform.parent.GetComponent<cMonster_stage1_slime>().isClone1Dead = true;
                    this.gameObject.SetActive(false);
                }
                else
                {
                    this.transform.parent.GetComponent<cMonster_stage1_slime>().isClone2Dead = true;
                    this.gameObject.SetActive(false);

                    this.transform.parent.GetComponent<cMonster_stage1_slime>().StartRespawn();
                }
            }

            else
            {
                // 분열        
                clone1 = Instantiate(Resources.Load<GameObject>(cPath.PrefabPath() + "enemy_Slime"), new Vector3(this.transform.position.x - 100f, this.transform.position.y, this.transform.position.z),
                    Quaternion.identity, this.transform);
                clone1.transform.localScale = new Vector3(0.7f,0.7f,0.7f);
                clone1.transform.position = new Vector3(clone1.transform.position.x, clone1.transform.position.y, clone1.transform.position.z + 1000f);
                clone1.GetComponent<cMonster_stage1_slime>().Init(this.nickName + " clone1", this.damage/2, 
                    this.maxMoveSpeed, this.maxHp/2, this.maxHp/2);
                clone1.GetComponent<cMonster_stage1_slime>().Split();
                
                clone2 = Instantiate(Resources.Load<GameObject>(cPath.PrefabPath() + "enemy_Slime"),new Vector3(this.transform.position.x + 100f, this.transform.position.y, this.transform.position.z),
                    Quaternion.identity, this.transform);
                clone2.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                clone2.transform.position = new Vector3(clone2.transform.position.x, clone2.transform.position.y, clone2.transform.position.z + 1000f);
                clone2.GetComponent<cMonster_stage1_slime>().Init(this.nickName + " clone2", this.damage / 2, 
                    this.maxMoveSpeed, this.maxHp / 2, this.maxHp / 2);
                clone2.GetComponent<cMonster_stage1_slime>().Split();
            }
        }
        SetHp();
    }
}
