using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cBoss_Theme1_SkeletonMage : cEnemy_monster
{
    private byte curNormalAttackCount;
    private byte curBonePileCount;

    private Transform originRtPos;
    private bool isShieldOn;
    public Transform shieldPos;
    private float shieldTimer;
    private float curShieldHp;
    private float maxShieldHp;
    private const float leastShieldTimeForSummon = 5.0f;

    private const float normalAttackCoolTime = 3.0f;
    private float normalAttackCoolTimer;

    private float randomX;
    private float randomY;
    private long randomPower;
    private float randomGravity;

    public void InitBoss()
    {
        base.Init(cEnemyTable.SetMonsterInfo(51));

        originRtPos = rt.transform;
        bulletDamage = 10;
        maxShieldHp = 3;
    }

    protected override void FixedUpdate()
    {
        if(isDead.Equals(true))
        {
            return;
        }

        if(isInit.Equals(true))
        {
            base.FixedUpdate();
        }
    }

    protected override void Move()
    {
        if (_animator != null)
        {
            _animator.SetFloat("MoveSpeed", curMoveSpeed);            
        }
        
        if(isRightBlocked)
        {
            isRightBlocked = false;
            ChangeDir(Vector3.left);
        }
        else if(isLeftBlocked)
        {
            isLeftBlocked = false;
            ChangeDir(Vector3.right);
        }

        if(curPatternId.Equals(0))
        {
            Pattern1();
        }
        else if (curPatternId.Equals(1))
        {
            Pattern2();
        }
    }
    private void Pattern1()
    {
        if(isInNoticeRange)
        {
            playerPos = cUtil._player.originObj.transform.position;

            if(playerPos.x >= this.transform.position.x)
            {
                ChangeDir(Vector3.right);
            }
            else
            {
                ChangeDir(Vector3.left);
            }

            if (curBonePileCount.Equals(2))
            {
                BoneBoomerang();
                return;
            }

            if (curNormalAttackCount.Equals(5))
            {
                Debug.Log("bonepile");
                BonePile();
                return;
            }

            if(isInAttackRange.Equals(false))
            {
                normalAttackCoolTimer += Time.deltaTime;
                if (normalAttackCoolTimer >= normalAttackCoolTime)
                {
                    NormalAttack();
                    normalAttackCoolTimer = 0;
                }
            }
            else if(isInAttackRange.Equals(true))
            {
                normalAttackCoolTimer += Time.deltaTime;
                if (normalAttackCoolTimer >= normalAttackCoolTime)
                {
                    NormalAttack();
                    normalAttackCoolTimer = 0;
                }
            }
        }
    }
    private void Pattern2()
    {
        if (isInNoticeRange)
        {
            playerPos = cUtil._player.originObj.transform.position;

            if (playerPos.x >= this.transform.position.x)
            {
                ChangeDir(Vector3.right);
            }
            else
            {
                ChangeDir(Vector3.left);
            }

            if (curNormalAttackCount.Equals(5))
            {
                EngageShield();
                return;
            }

            if (isShieldOn)
            {
                _animator.SetBool("ShieldOn", true);
                shieldTimer += Time.deltaTime;
                if (shieldTimer >= leastShieldTimeForSummon)
                {
                    shieldTimer = 0;
                    SummonSkeleton();
                    return;
                }
                return;
            }

            normalAttackCoolTimer += Time.deltaTime;
            if (normalAttackCoolTimer >= normalAttackCoolTime)
            {
                NormalAttack();
                normalAttackCoolTimer = 0;
            }
        }
    }

    private void BonePile()
    {
        _animator.SetTrigger("BonePile");
        Debug.Log("BonePile()");
        curNormalAttackCount = 0;
        curBonePileCount += 1;
    }
    private void BonePile_ani()
    {
        StartCoroutine(BonePileCor());
    }
    IEnumerator BonePileCor()
    {
        byte bulletCount = 0; 

        while(true)
        {
            if(bulletCount > 20)
            {
                break;
            }
            randomPower = Random.Range(10, 20);
            randomGravity = Random.Range(5, 9);
            randomX = Random.Range(shieldPos.position.x - 50, shieldPos.position.x + 50);
            randomY = Random.Range(shieldPos.position.y - 50, shieldPos.position.y + 50);

            bulletManager.LaunchBullet(new Vector3(randomX, randomY, shieldPos.position.z), true,
                cUtil._player.transform.position,
                randomPower, randomGravity);

            bulletCount++;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void BoneBoomerang()
    {
        _animator.SetTrigger("BoneBoomerang");
        curBonePileCount = 0;
    }
    private void BoneBoomerang_ani()
    {
        Debug.Log("booooooomerang");
    }

    private void NormalAttack()
    {
        _animator.SetTrigger("NormalAttack");
        curNormalAttackCount += 1;
    }
    private void NormalAttack_ani()
    {
        bulletManager.LaunchBullet(shieldPos.position, false, 
            cUtil._player.transform.position, bulletDamage, 15.0f);
    }

    private void EngageShield()
    {
        _animator.SetTrigger("ShiledActive");
        isShieldOn = true;
        curShieldHp = maxShieldHp;
        rt.transform.position = shieldPos.position;
        curNormalAttackCount = 0;
    }
    private void DisEngageShield()
    {
        _animator.SetBool("ShieldOn", false);
        isShieldOn = false;
        rt.transform.position = originRtPos.position;
    }

    private void SummonSkeleton()
    {
        _animator.SetTrigger("Summon");
    }
    private void SummonSkeleton_ani()
    {
        Debug.Log("summon");
    }

    public override void ReduceHp(long pVal)
    {
        if (isDead.Equals(true))
        {
            return;
        }

        if(isShieldOn.Equals(false))
        {
            curHp.value -= pVal;

            if (curHp.value <= 0)
            {
                coolTimer = 0;
                curHp.value = 0;
                isDead = true;
                _animator.SetTrigger("Dead");
                img_curHp.transform.parent.gameObject.SetActive(false);
                originObj.GetComponent<BoxCollider2D>().enabled = false;
            }
            if (isDead.Equals(false))
                _animator.SetTrigger("GetHit");
        }
        else if(isShieldOn)
        {
            if(cUtil._player.isCritical.Equals(true))
            {
                curShieldHp -= 1;
            }

            if(curShieldHp <= 0)
            {
                DisEngageShield();
            }
        }

        if (curHp.value <= maxHp.value * 0.3)
        {
            curPatternId = 1;
        }
    }

    public override void ReduceHp(long pVal, Vector3 pDir, float pVelocity = 7.5F)
    {
        if (isDead.Equals(true))
        {
            return;
        }

        if (isShieldOn.Equals(false))
        {
            curHp.value -= pVal;

            if (curHp.value <= 0)
            {
                coolTimer = 0;
                curHp.value = 0;
                isDead = true;
                _animator.SetTrigger("Dead");
                img_curHp.transform.parent.gameObject.SetActive(false);
                originObj.GetComponent<BoxCollider2D>().enabled = false;
            }
            if (isDead.Equals(false))
                _animator.SetTrigger("GetHit");
        }
        else if (isShieldOn)
        {
            if (cUtil._player.isCritical.Equals(true))
            {
                curShieldHp -= 1;
            }

            if (curShieldHp <= 0)
            {
                DisEngageShield();
            }
        }

        if (curHp.value <= maxHp.value * 0.3)
        {
            curPatternId = 1;
        }
    }
}
