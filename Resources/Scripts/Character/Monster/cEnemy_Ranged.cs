using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cEnemy_Ranged : cEnemy_monster
{
    public cBulletManager bulletManager; // 투사체 관련 BulletManager에서 관리

    protected int bulletTypeNum; // 투사체 타입 변수 (cBulletManager의 SetBullet() 인자)
    protected long bulletDamage; // 투사체 데미지

    public float GetBulletDamage() { return bulletDamage; }

    private void Start()
    {
        bulletTypeNum = 0;
        bulletDamage = (long)(damage.value * 0.5f);
    }

    // 공격 쿨타임 관리
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Move()
    {
        // 인식 범위 내 진입
        if (isInNoticeRange)
        {
            // 쿨타임 차면
            if(isAttackReady)
            {
                // 근접 공격 범위 바깥
                if (!isInAttackRange)
                {
                    bulletManager.SetBullet(bulletTypeNum);
                    isAttackReady = false;
                }
                // 근접 공격 범위 안에 들어온 경우
                else
                {
                    playerPos = cUtil._player.gameObject.transform.position;
                    if (playerPos.x >= this.transform.position.x)
                        dir = Vector3.right;
                    else if (playerPos.x < this.transform.position.x)
                        dir = Vector3.left;

                    if (dir == Vector3.right)
                    {
                        attackBox.transform.localPosition = attackBoxPos[0];
                        attackBox.gameObject.SetActive(true);
                    }
                    else if (dir == Vector3.left)
                    {
                        attackBox.transform.localPosition = attackBoxPos[2];
                        attackBox.gameObject.SetActive(true);
                    }
                    isAttackReady = false;
                }
            }
        }
        // idle
        else if (!isInAttackRange)
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
    }
}
