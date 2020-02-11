using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cEnemy_Ranged : cEnemy_monster
{
    public cBulletManager bulletManager; // 투사체 관련 BulletManager에서 관리
    protected long bulletDamage; // 투사체 데미지
    protected float bulletCoolTime;
    protected float timer;

    public float GetBulletDamage() { return bulletDamage; }

    private void Start()
    {
        bulletDamage = (long)(damage.value * 0.5f);
        bulletCoolTime = 3.0f;
        timer = bulletCoolTime;
    }

    protected override void Move()
    {
        // 인식 범위 내 진입
        if (isInNoticeRange)
        {
            isInAttackRange = attackBoxMng.GetIsInAttackRange();
            // 근접 공격 범위 바깥
            if (!isInAttackRange)
            {
                timer += Time.deltaTime;
                if (timer >= bulletCoolTime)
                {
                    // 기본값 : 발사체 1, 발사체 타입 일반형, 중력 적용 x, 타겟 : 유저
                    bulletManager.SetBullet(1, BULLET_TYPE.NORMAL, false, cUtil._player.originObj.transform.position);
                    timer = 0;
                }
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
