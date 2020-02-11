using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cMonster_stage5_zombie : cEnemy_monster
{
    bool isAttacked;
    bool isRestoring;
    float lastAttackTime;
    float waitTimeRestore;
    float time;
    cProperty healAmount;

    private void Start()
    {
        Init(cEnemyTable.SetMonsterInfo(13));
        curMoveSpeed = curMoveSpeed * 0.7f;
        maxMoveSpeed = maxMoveSpeed * 0.7f;

        isRestoring = false;
        lastAttackTime = 0;
        waitTimeRestore = 7.0f; // 7초 후 회복 시작
        time = 0;
        healAmount = new cProperty("temp", 1); // 힐량 임시
    }

    public override void Init(enemyInitStruct pEs)
    {
        base.Init(pEs);
        respawnTime = 5.0f;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        // 공격당한 상태인 경우 시간 재기 시작
        if (isAttacked)
        {
            lastAttackTime += Time.deltaTime;
            // 회복 대기 시간이 지나면 피격상태 해제, 회복상태 돌입
            if (lastAttackTime >= waitTimeRestore)
            {
                isAttacked = false;
                isRestoring = true;
                lastAttackTime = 0;
            }
        }
        // 회복상태 1초마다 체력 회복
        if (isRestoring)
        {
            time += Time.deltaTime;
            if (time >= 1.0f)
            {
                time = 0;
                this.RestoreHp(healAmount);
            }
        }
    }

    public override void ReduceHp(long pVal)
    {
        base.ReduceHp(pVal);

        // 공격당하지 않은 상태인 경우 피격상태로 전환
        if (lastAttackTime == 0)
        {
            isAttacked = true;
        }
        // 피격상태지만 회복상태에 돌입하지 않은 경우에 타이머 초기화
        else if (lastAttackTime <= waitTimeRestore && lastAttackTime > 0)
        {
            lastAttackTime = 0;
        }
        
        // 회복 중 공격받았을 경우 회복상태 취소
        if (isRestoring)
        {
            isRestoring = false;
        }
    }

    public override void ReduceHp(long pVal, Vector3 pDir, float pVelocity = 7.5F)
    {
        base.ReduceHp(pVal, pDir, pVelocity);

        if (lastAttackTime == 0)
        {
            isAttacked = true;
        }
        else if (lastAttackTime <= waitTimeRestore && lastAttackTime > 0)
        {
            lastAttackTime = 0;
        }

        if (isRestoring)
        {
            isRestoring = false;
        }
    }
}
