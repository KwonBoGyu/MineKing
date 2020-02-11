using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cMonster_stage2_ghost : cEnemy_Flying
{
    private float hideTime;
    private float time;
    private bool isHideOn;

    private void Start()
    {
        Init(cEnemyTable.SetMonsterInfo(2));
        changingGravity = 0;
        defaultGravity = 0;
        flyingRangeY = 0.5f;
        hideTime = 5.0f;
        time = 0;
        isHideOn = false;
    }

    public override void Init(enemyInitStruct pEs)
    {
        base.Init(pEs);
        respawnTime = 5.0f;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if(isHideOn)
        {
            Debug.Log("hiding");
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -1000f);
        }
        else
        {
            Debug.Log("not hiding");
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0);
        }

        time += Time.deltaTime;
        
        if (time >= hideTime && time < hideTime * 2)
        {
            isHideOn = true;
        }
        else if (time >= hideTime * 2)
        {
            isHideOn = false;
            time = 0;
        }
    }
}
