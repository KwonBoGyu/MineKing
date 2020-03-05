using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cBoss_theme1_stage1_sub : cEnemy_monster
{
    //Pattern1 변수
    private byte curNormalAttackCount; //n번 일반 공격 시 총알 난사
    private byte curBulletSpoilCount; //n번 총알 난사 시 궁극기
    public Transform smashGroundPos;

    private const float normalAttackCoolTime = 3.0f;
    private float normalAttackCoolTimer;

    //Pattern2 변수
    public cBoss_theme1_stage1 prevBoss;

    private Vector3 NormalNearAttackBoxPos;//local

    float randomX;
    float randomY;
    float randomX2;
    float randomY2;

    public void InitBoss()
    {
        base.Init(cEnemyTable.SetMonsterInfo(51));

        bulletDamage = 10;
        NormalNearAttackBoxPos = new Vector3(-54, 39, -1);
    }

    protected override void FixedUpdate()
    {
        if (isDead.Equals(true))
            return;

        if (isInit.Equals(true))
        {
            base.FixedUpdate();
        }
    }

    protected override void Move()
    {
        if (_animator != null)
            _animator.SetFloat("MoveSpeed", curMoveSpeed);

        //막히면 방향 바꿔준다.
        if (isRightBlocked == true)
        {
            isRightBlocked = false;
            ChangeDir(Vector3.left);
        }
        else if (isLeftBlocked == true)
        {
            isLeftBlocked = false;
            ChangeDir(Vector3.right);
        }

        //패턴
        if (curPatternId.Equals(0))
            Pattern1();
        else if (curPatternId.Equals(1))
            Pattern2();
    }

    private void Pattern1()
    {
        // 인식 범위 내 진입
        //isInAttackRange : 근접공격범위
        if (isInNoticeRange.Equals(true))
        {
            playerPos = cUtil._player.gameObject.transform.position;

            if (playerPos.x >= this.transform.position.x)
                ChangeDir(Vector3.right);

            else if (playerPos.x < this.transform.position.x)
                ChangeDir(Vector3.left);

            //총알 난사 공격을 2회 했을 시 스매쉬
            if (curBulletSpoilCount.Equals(2))
            {
                SmashGround();
                return;
            }

            //일반 공격을 5회 했을 시 총알 난사
            if (curNormalAttackCount.Equals(3))
            {
                SpoilBullet();
                return;
            }

            //일반 원거리 공격
            if (isInAttackRange.Equals(false))
            {
                //공격 쿨타임 돌린다.
                normalAttackCoolTimer += Time.deltaTime;
                if (normalAttackCoolTimer >= normalAttackCoolTime)
                {
                    NormalAttack(false);
                    normalAttackCoolTimer = 0;
                }
            }
            //일반 근접 공격
            else if (isInAttackRange.Equals(true))
            {
                //공격 쿨타임 돌린다.
                normalAttackCoolTimer += Time.deltaTime;
                if (normalAttackCoolTimer >= normalAttackCoolTime)
                {
                    NormalAttack(true);
                    normalAttackCoolTimer = 0;
                }
            }
        }
    }

    private void Pattern2()
    {

    }

    private void SmashGround()
    {
        _animator.SetTrigger("SmashGround");
        curBulletSpoilCount = 0;
    }

    public void SmashGround_ani()
    {
        for (byte i = 0; i < 20; i++)
        {
            randomX = Random.Range(-100, 100);
            randomY = Random.Range(50, 100);
            randomX2 = Random.Range(-200, 200);
            randomY2 = Random.Range(100, 150);

            bulletManager.LaunchBullet(
                new Vector3(smashGroundPos.position.x + randomX,
                smashGroundPos.position.y + 10,
                smashGroundPos.position.z),
                true,
                new Vector3(smashGroundPos.position.x + randomX2,
                smashGroundPos.position.y + randomY2,
                smashGroundPos.position.z),
                bulletDamage, 15.0f, 15.0f);
        }

    }

    private void SpoilBullet()
    {
        _animator.SetTrigger("SpoilBullet");
        curNormalAttackCount = 0;
        curBulletSpoilCount += 1;
    }

    public void SpoilBullet_ani()
    {
        randomY = Random.Range(-30, 30);
        bulletManager.LaunchBullet(originObj.transform.position, true,
            new Vector3(cUtil._player.originObj.transform.position.x,
            cUtil._player.originObj.transform.position.y + randomY,
            cUtil._player.originObj.transform.position.z),
            bulletDamage, 20.0f);
    }

    private void NormalAttack(bool isNear)
    {
        if (isNear.Equals(true))
        {
            _animator.SetTrigger("NormalNearAttack");
        }
        else
        {
            _animator.SetTrigger("NormalFarAttack");
        }

        curNormalAttackCount += 1;
    }

    public void NormalNearAttack_ani()
    {
        if (isInAttackRange.Equals(true))
        {
            cUtil._player.ReduceHp(damage.value, dir);
        }
    }

    public void NormalFarAttack_ani()
    {
        bulletManager.LaunchBullet(originObj.transform.position, true, cUtil._player.originObj.transform.position, bulletDamage, 15.0f);
    }

    public override void ReduceHp(long pVal)
    {
        curHp.value -= pVal;

        if (curHp.value <= 0)
        {
            coolTimer = 0;
            curHp.value = 0;
            isDead = true;
            _animator.SetTrigger("Dead");
            originObj.GetComponent<BoxCollider2D>().enabled = false;
            originObj.transform.position = new Vector3(-10000, -10000, 0);
        }

        prevBoss.SlimeKingSetHp();
    }

    // 공격자가 있는 경우 공격자가 바라보는 방향을 pDir로 받음
    public override void ReduceHp(long pVal, Vector3 pDir, float pVelocity = 7.5f)
    {
        curHp.value -= pVal;

        if (curHp.value <= 0)
        {
            coolTimer = 0;
            curHp.value = 0;
            isDead = true;
            _animator.SetTrigger("Dead");
            originObj.GetComponent<BoxCollider2D>().enabled = false;
            originObj.transform.position = new Vector3(-10000, -10000, 0);
        }

        prevBoss.SlimeKingSetHp();
    }

    protected override void SetHp()
    {
        
    }
}