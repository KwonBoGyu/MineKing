﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cMonster_stage1_mouse : cEnemy_monster
{
    private void Start()
    {
        Init(cEnemyTable.SetMonsterInfo(0));
    }

    public override void Init(enemyInitStruct pEs)
    {
        base.Init(pEs);
        _animator = this.transform.Find("log_rig").GetComponent<Animator>();
        respawnTime = 5.0f;
        attackCoolTime = 1.0f;
    }

}
