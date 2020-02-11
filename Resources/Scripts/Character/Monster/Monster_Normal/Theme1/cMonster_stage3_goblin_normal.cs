using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cMonster_stage3_goblin_normal : cEnemy_monster
{
    private bool isInDanger;
    private bool isRestoring;
    private cProperty restoringHP;
    private float distance;
    private float time;

    private void Start()
    {
        Init(cEnemyTable.SetMonsterInfo(6));
        isInDanger = false;
        isRestoring = false;
        distance = 0;
        restoringHP = new cProperty("restore", 1); // 디버깅용 수치이므로 나중에 값 수정해야 함
        time = 0;
    }

    public override void Init(enemyInitStruct pEs)
    {
        base.Init(pEs);
        respawnTime = 5.0f;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (isRestoring)
        {
            time += Time.deltaTime;
            if (time >= 1f)
            {
                Debug.Log("Restoring HP...");
                // hp 초당 회복
                RestoreHp(restoringHP);
                time = 0;
            }

            if (curHp.value == maxHp.value)
            {
                isRestoring = false;
            }
        }

        // 디버깅용
        if(Input.GetKeyDown(KeyCode.B))
        {
            ReduceHp(1);
        }
    }

    protected override void Move()
    {
        // 위험 상태 (체력 10% 이하인 경우)
        if (isInDanger)
        {
            Debug.Log("isInDanger");
            // 플레이어 반대방향으로 일정 거리 이동
            if (playerPos.x >= this.transform.position.x)
            {
                dir = Vector3.left;
            }

            else if (playerPos.x < this.transform.position.x)
            {
                dir = Vector3.right;
            }

            distance = Vector2.Distance(cUtil._player.originObj.transform.position, this.gameObject.transform.position);

            // 직선거리 일정 이상 or 막다른 길이면 휴식상태 돌입, 위험상태 off
            if (distance >= 300.0f)
            {
                isInDanger = false;
                isRestoring = true;
            }
            else if (isRightBlocked ^ isLeftBlocked)
            {
                isInDanger = false;
                isRestoring = true;
            }
            else
            {
                this.transform.Translate(dir * curMoveSpeed * Time.deltaTime);
            }
        }
        else if (isRestoring)
        {
            this.transform.position = this.transform.position;
        }

        // 인식 범위 내 진입
        else if (isInNoticeRange && !isInDanger)
        {
            isInAttackRange = attackBoxMng.GetIsInAttackRange();
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
            // 공격 자체는 cEnemy_AttackBox에서 처리
            else if (isInAttackRange)
            {
                playerPos = cUtil._player.gameObject.transform.position;
                if (playerPos.x >= this.transform.position.x)
                    dir = Vector3.right;
                else if (playerPos.x < this.transform.position.x)
                    dir = Vector3.left;
            }
        }
        // idle 상태
        else if (!isInNoticeRange && !isInDanger)
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
    }

    public override void ReduceHp(long pVal)
    {
        base.ReduceHp(pVal);

        if (curHp.value <= maxHp.value * 0.1)
        {
            isInDanger = true;
        }

        if (isRestoring)
        {
            isRestoring = false;
        }
    }

    public override void ReduceHp(long pVal, Vector3 pDir, float pVelocity = 7.5F)
    {
        base.ReduceHp(pVal, pDir, pVelocity);

        if (curHp.value <= maxHp.value * 0.1)
        {
            isInDanger = true;
        }

        if (isRestoring)
        {
            isRestoring = false;
        }
    }
}
