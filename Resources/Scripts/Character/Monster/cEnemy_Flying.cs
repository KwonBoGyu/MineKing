using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cEnemy_Flying : cEnemy_monster
{
    protected float flyingRangeY;
    protected float floatTime;

    private void Start()
    {
        changingGravity = 0;
        defaultGravity = 0;
        flyingRangeY = 0.5f;
        floatTime = 0;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Move()
    {
        // 인식 범위 내 진입
        if (isInNoticeRange)
        {
            isInAttackRange = attackBoxMng.GetIsInAttackRange();
            // 인식 범위 안에 들어왔지만 공격 범위 내에는 없는 경우 ( cRangeNotizer에서 감지 )
            if (!isInAttackRange)
            {
                playerPos = cUtil._player.gameObject.transform.position;
                this.transform.Translate(dir * curMoveSpeed * Time.deltaTime);

                if (playerPos.x >= this.transform.position.x)
                    dir = Vector3.right;

                else if (playerPos.x < this.transform.position.x)
                    dir = Vector3.left;
            }
            // 공격 범위 안에 들어온 경우
            // 공격 자체는 cEnemy_AttackBox에서 처리
            else if (isInAttackRange)
            {
                playerPos = cUtil._player.gameObject.transform.position;
                if (playerPos.x >= this.transform.position.x)
                    dir = Vector3.right;
                else if (playerPos.x < this.transform.position.x)
                    dir = Vector3.left;
            }
        }

        // idle상태 : 사인파 형태로 부유
        else // (!isInNoticeRange)
        {
            Debug.Log("else");
            if (isRightBlocked)
            {
                isRightBlocked = false;
                dir = Vector3.left;
            }
            else if (isLeftBlocked)
            {
                isLeftBlocked = false;
                dir = Vector3.right;
            }

            floatTime += Time.deltaTime;
            dir = new Vector3(dir.x, Mathf.Sin(floatTime) * flyingRangeY, dir.z);

            if (isUpBlocked)
            {
                dir.y = dir.y * -1;
            }
            else if (GetIsGrounded())
            {
                dir.y = dir.y * -1;
            }

            this.transform.Translate(dir * curMoveSpeed * Time.deltaTime);
        }

        // 오버플로우 방지
        if (floatTime >= float.MaxValue - 100.0f)
        {
            floatTime = 0;
        }
    }
}
