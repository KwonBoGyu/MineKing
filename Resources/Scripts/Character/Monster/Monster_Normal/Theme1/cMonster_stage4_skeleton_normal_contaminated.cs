using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cMonster_stage4_skeleton_normal_contaminated : cEnemy_monster
{
    private void Start()
    {
        Init(cEnemyTable.SetMonsterInfo(10));
        this.SetMaxMoveSpeed(this.maxMoveSpeed * 1.5f);
        this.SetCurMoveSpeed(this.curMoveSpeed * 1.5f);
    }

    public override void Init(enemyInitStruct pEs)
    {
        base.Init(pEs);
        respawnTime = 5.0f;
    }
}
