﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cMonster_stage1_mouse : cEnemy_monster
{
    private void Start()
    {
        Init(cEnemyTable.SetMonsterInfo(0));
    }

    protected override void Init(enemyInitStruct pEs)
    {
        base.Init(pEs);
        respawnTime = 5.0f;
    }
}