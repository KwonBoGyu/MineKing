using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cBoss_theme1_stage1 : cEnemy_monster
{
    //아이템 랜덤 드랍 확률
    private int per_BossKey;

    private void Start()
    {
        Init(cEnemyTable.SetMonsterInfo(0));
    }

    protected override void Init(enemyInitStruct pEs)
    {
        base.Init(pEs);
        respawnTime = 5.0f;
        per_BossKey = 100;
    }

    protected override void Move()
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


    protected override void RespawnInit()
    {
        if (cUtil._user.GetInventory().isBossKeyExist().Equals(false))
        {
            int percent = Random.Range(0, 101);

            if (percent <= per_BossKey)
            {
                Debug.Log("Boss Key Dropped!");
                cItem pEtc;
                citemTable.GetItemInfo(out pEtc, 3);
                cUtil._user.GetInventory().GetItemEtc().Add((cItem_etc)pEtc);
            }
        }

        this.transform.localPosition = InitPos;
        curHp = maxHp;
    }
}
