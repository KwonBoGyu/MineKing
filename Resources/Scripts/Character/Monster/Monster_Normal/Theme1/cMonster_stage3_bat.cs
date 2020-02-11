using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cMonster_stage3_bat : cEnemy_Flying
{
    private void Start()
    {
        Init(cEnemyTable.SetMonsterInfo(5));
        changingGravity = 0;
        defaultGravity = 0;
        flyingRangeY = 0.5f;
        this.SetMaxMoveSpeed(this.maxMoveSpeed * 1.5f);
        this.SetCurMoveSpeed(this.curMoveSpeed * 1.5f);
    }

    public override void Init(enemyInitStruct pEs)
    {
        base.Init(pEs);
        respawnTime = 5.0f;
    }
}
