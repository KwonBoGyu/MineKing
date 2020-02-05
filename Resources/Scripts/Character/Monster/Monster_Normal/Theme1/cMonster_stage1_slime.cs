using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cMonster_stage1_slime : cEnemy_Ranged
{
    // 분열된 횟수
    public bool isSplited;

    private GameObject clone1;
    private GameObject clone2;

    public bool isClone1Dead;
    public bool isClone2Dead;
    
    public cMonster_stage1_slime()
    {
        isSplited = false;
    }

    void Start()
    {
        bulletTypeNum = 0; // 발사체 타입 : 일반형
        attackCoolTime = 3.0f;
        curCoolTime = attackCoolTime;
        isAttackReady = true;
        bulletDamage = damage / 2;

        Init(cEnemyTable.SetMonsterInfo(1));
        curMoveSpeed = maxMoveSpeed;
        bulletDamage = damage / 2;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public void Split()
    {
        isSplited = true;
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
