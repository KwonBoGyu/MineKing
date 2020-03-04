using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cBoss_theme1_stage1 : cEnemy_Boss
{
    //cBulletManager bulletMng;

    //Pattern MELEE;
    //Pattern RANGED1;
    //Pattern RANGED2;
    //Pattern LIQUID;
    //Pattern SUMMON;

    //public GameObject liquid;
    //private IEnumerator liquidCor;

    public void InitBoss()
    {
        skills = this.GetComponent<cBossSkill>();
        skills.Init(this);
        Init(cEnemyTable.SetMonsterInfo(50));
    }

    public override void Init(enemyInitStruct pEs)
    {
        base.Init(pEs);

    }
    //patternTable = new List<Pattern>();

    //    // 패턴 세팅
    //    MELEE = new Pattern(0, 5, 4);
    //    RANGED1 = new Pattern(1, 4, 3);
    //    RANGED2 = new Pattern(2, 10, 2);
    //    LIQUID = new Pattern(3, 30, 1);
    //    SUMMON = new Pattern(4, 30, 1);

    //    patternTable.Add(MELEE);
    //    patternTable.Add(RANGED1);
    //    patternTable.Add(RANGED2);

    //    curPattern = MELEE;
    //    curCoolTime = 0;
    //    timer = 0;
    //    curPatternCount = 0;

    //    curMoveSpeed = maxMoveSpeed; 
    //    curCoolTime = MELEE.patternCount;
    //    timer = curCoolTime;
    //    liquidCor = SetLiquid();

    //    bulletMng = GameObject.Find("Bullets").GetComponent<cBulletManager>();
    //}

    //protected override void FixedUpdate()
    //{
    //    base.FixedUpdate();
    //}

    //protected override void Move()
    //{
    //    switch (curPattern.patternNum)
    //    {
    //        // 근접 공격 및 이동 패턴
    //        case 0:
    //            if (curPatternCount > curPattern.patternCount)
    //            {
    //                ChangePattern();
    //            }

    //            if (timer >= curCoolTime)
    //            {
    //                curPatternCount += 1;
    //            }
    //            base.Move();
    //            break;

    //        // 원거리 공격
    //        case 1:
    //            if (curPatternCount > curPattern.patternCount)
    //            {
    //                Debug.Log("change Pattern");
    //                ChangePattern();
    //            }

    //            if (timer >= curCoolTime)
    //            {
    //                RangeAttack1();
    //                curPatternCount += 1;
    //                timer = 0;
    //            }
    //            else
    //            {
    //                timer += Time.deltaTime;
    //            }
    //            Debug.Log("pattern2");
    //            break;

    //        // 원거리 공격
    //        case 2:
    //            if (curPatternCount > curPattern.patternCount)
    //            {
    //                Debug.Log("change Pattern");
    //                ChangePattern();
    //            }
    //            if (timer >= curCoolTime)
    //            {
    //                RangeAttack2();
    //                curPatternCount += 1;
    //                timer = 0;
    //                break;
    //            }
    //            else
    //            {
    //                timer += Time.deltaTime;
    //            }
    //            Debug.Log("pattern3");
    //            break;

    //        // 액체 상태 공격 (장판)
    //        case 3:
    //            if (curPatternCount > curPattern.patternCount)
    //            {
    //                Debug.Log("change Pattern");
    //                StopCoroutine(liquidCor);
    //                ChangePattern();
    //            }
    //            if (timer >= curCoolTime)
    //            {
    //                StartCoroutine(liquidCor);
    //                curPatternCount += 1;
    //                timer = 0;
    //                break;
    //            }
    //            else
    //            {
    //                timer += Time.deltaTime;
    //            }
    //            Debug.Log("pattern4");
    //            break;

    //        // 슬라임 소환
    //        case 4:
    //            if (curPatternCount > curPattern.patternCount)
    //            {
    //                Debug.Log("change Pattern");
    //                ChangePattern();
    //            }
    //            if (timer >= curCoolTime)
    //            {
    //                SummonSlime();
    //                curPatternCount += 1;
    //                timer = 0;
    //                break;
    //            }
    //            else
    //            {
    //                timer += Time.deltaTime;
    //            }
    //            Debug.Log("pattern5");
    //            break;
    //    }
    //}

    //private void RangeAttack1()
    //{
    //    bulletMng.SetBullet(3, 0.2f, originObj.transform.position, false, playerPos);
    //}

    //private void RangeAttack2()
    //{
    //    bulletMng.SetBullet(5, 0.5f, originObj.transform.position, true, playerPos);
    //}

    //public override void ReduceHp(long pVal)
    //{
    //    base.ReduceHp(pVal);

    //    if (curHp.value <= 0)
    //    {
    //        SummonSlime();
    //    }

    //    if (curHp.value <= maxHp.value * 0.5f)
    //    {
    //        patternTable.Add(LIQUID);
    //    }

    //    if(curHp.value <= maxHp.value * 0.2f)
    //    {
    //        patternTable.Add(SUMMON);
    //    }
    //}

    //public override void ReduceHp(long pVal, Vector3 pDir, float pVelocity = 7.5F)
    //{
    //    base.ReduceHp(pVal, pDir, pVelocity);

    //    if (curHp.value <= 0)
    //    {
    //        SummonSlime();
    //    }

    //    if (curHp.value <= maxHp.value * 0.5f)
    //    {
    //        patternTable.Add(LIQUID);
    //    }

    //    if (curHp.value <= maxHp.value * 0.2f)
    //    {
    //        patternTable.Add(SUMMON);
    //    }
    //}


    //IEnumerator SetLiquid()
    //{
    //    liquid.SetActive(true);

    //    float height = this.gameObject.transform.position.y - 1500f;
    //    this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y,
    //        this.gameObject.transform.position.z - 1000f);
    //    float liquidZPos = liquid.transform.position.z + 1000f;

    //    while(true)
    //    {
    //        liquid.transform.position = new Vector3(liquid.transform.position.x, height, liquidZPos);
    //        height += 1f;

    //        if (liquid.transform.position.y >= this.gameObject.transform.position.y)
    //        {
    //            break;
    //        }

    //        yield return new WaitForFixedUpdate();
    //    }

    //    this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y,
    //        this.gameObject.transform.position.z + 1000f);
    //    liquid.SetActive(false);
    //}

    //private void SummonSlime()
    //{
    //    GameObject slime1 = Instantiate(Resources.Load<GameObject>(cPath.PrefabPath() + "Monster/Monster_Slime"), 
    //        new Vector3(this.gameObject.transform.position.x - 100f, 
    //        this.transform.position.y, this.transform.position.z), Quaternion.identity, this.transform.parent);

    //    GameObject slime2 = Instantiate(Resources.Load<GameObject>(cPath.PrefabPath() + "Monster/Monster_Slime"), 
    //        new Vector3(this.gameObject.transform.position.x + 100f, 
    //        this.transform.position.y, this.transform.position.z), Quaternion.identity, this.transform.parent);
    //}
}