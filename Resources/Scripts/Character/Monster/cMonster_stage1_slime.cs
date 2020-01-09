using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cMonster_stage1_slime : cEnemy_monster
{
    // 분열된 횟수
    private int remainSplitCount;
    public GameObject bullet;
    
    public void reduceSplitCount() { remainSplitCount -= 1; }

    void Start()
    {
        Init(cEnemyTable.SetMonsterInfo(0));
        remainSplitCount = 1;
        curMoveSpeed = maxMoveSpeed;
    }
    
    protected override void Move()
    {
        Debug.Log("maxMoveSpeed : " + maxMoveSpeed);
        Debug.Log("curMoveSpeed : " + curMoveSpeed);
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
        else if (isInNoticeRange && !isInAttackRange)
        {
            playerPos = dp._player.transform.position;

            if (bullet.activeSelf.Equals(false))
            {
                bullet.SetActive(true);
                bullet.GetComponent<cBullet>().SetDir(playerPos);
            }

        }
        // 공격 범위 안에 들어온 경우
        else if (isInAttackRange)
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

    public override void ReduceHp(float pVal)
    {
        curHp -= pVal;

        if (curHp <= 0)
        {
            curHp = 0;
            isDead = true;
            this.GetComponent<BoxCollider2D>().enabled = false;
            
            // 남은 분열 횟수가 0이면
            if (remainSplitCount == 0)
            {
                StartCoroutine(respawnCor);
            }
            else
            {
                // 분열
                GameObject newSlime1 = Instantiate(Resources.Load<GameObject>(cPath.PrefabPath() + "enemy_Slime"),
                    new Vector3(this.transform.position.x - 100f, this.transform.position.y, this.transform.position.z),
                    Quaternion.identity, this.transform.parent);
                newSlime1.GetComponent<cMonster_stage1_slime>().reduceSplitCount();

                GameObject newSlime2 = Instantiate(Resources.Load<GameObject>(cPath.PrefabPath() + "enemy_Slime"),
                    new Vector3(this.transform.position.x + 100f, this.transform.position.y, this.transform.position.z),
                    Quaternion.identity, this.transform.parent);
                this.transform.localPosition = new Vector3(InitPos.x, InitPos.y, -1000f);
                newSlime2.GetComponent<cMonster_stage1_slime>().reduceSplitCount();
            }
        }
        SetHp();
    }
}
