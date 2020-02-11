using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cMonster_stage4_spider_contaminated : cEnemy_Ranged
{
    private void Start()
    {
        Init(cEnemyTable.SetMonsterInfo(9));
        attackCoolTime = 3.0f;
        bulletDamage = this.damage.value / 2;
    }

    public override void Init(enemyInitStruct pEs)
    {
        base.Init(pEs);
        respawnTime = 5.0f;
    }

    protected override void Move()
    {
        base.Move();
    }
}
