﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cMonster_stage2_skeleton_normal : cEnemy_monster
{
    private void Start()
    {
        Init(cEnemyTable.SetMonsterInfo(3));
    }

    public override void Init(enemyInitStruct pEs)
    {
        base.Init(pEs);
        respawnTime = 5.0f;
    }
}
