using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 아직 미완성임!!! */

public class cEnemy_Flying : cEnemy_monster
{
    protected float flyingRangeY;

    private void Start()
    {
        changingGravity = 0;
        defaultGravity = 0;
        flyingRangeY = 50f;
    }

    protected override void Move()
    {
        if (isInNoticeRange)
        {
            dir = cUtil._player.transform.position - this.transform.position;
            this.transform.Translate(dir * curMoveSpeed * Time.deltaTime);
        }
        // idle상태 : 사인파 형태로 부유
        else if(!isInNoticeRange)
        {
            if (InitPos.x + 200.0f >= this.transform.position.x)
            {
                dir = Vector3.left;
            }
            else if (InitPos.x - 200.0f <= this.transform.position.x)
            {
                dir = Vector3.right;
            }
            else if (isRightBlocked)
            {
                isRightBlocked = false;
                dir = Vector3.left;
            }
            else if (isLeftBlocked)
            {
                isLeftBlocked = false;
                dir = Vector3.right;
            }

            dir = new Vector3(dir.x, Mathf.Sin(Time.deltaTime) * flyingRangeY, dir.z).normalized;
            this.transform.Translate(dir * curMoveSpeed * Time.deltaTime);
        }
    }
}
