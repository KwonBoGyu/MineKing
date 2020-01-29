using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cBoss_theme1_stage1 : cEnemy_Boss
{
    cBulletManager bulletMng;

    List<Pattern> patternTable;
    Pattern curPattern;

    Pattern MELEE;
    Pattern RANGED1;
    Pattern RANGED2;
    Pattern LIQUID;
    Pattern SUMMON;
    
    private float curCoolTime;
    private float timer;
    private int curPatternCount;

    public GameObject liquid;
    private IEnumerator liquidCor;

    private void Start()
    {
        Init(cEnemyTable.SetMonsterInfo(0));

        patternTable = new List<Pattern>();

        // 패턴 세팅
        MELEE = new Pattern(0, 5, 4);
        RANGED1 = new Pattern(1, 4, 3);
        RANGED2 = new Pattern(2, 10, 2);
        LIQUID = new Pattern(3, 30, 1);
        SUMMON = new Pattern(4, 30, 1);

        //patternTable.Add(MELEE);
        //patternTable.Add(RANGED1);
        patternTable.Add(RANGED2);
        //patternTable.Add(LIQUID);
        //patternTable.Add(SUMMON);

        curPattern = RANGED2;
        curCoolTime = 0;
        timer = 0;
        curPatternCount = 0;

        curMoveSpeed = maxMoveSpeed; // 임시
        curCoolTime = MELEE.patternCount; // 임시
        timer = curCoolTime;

        bulletMng = this.gameObject.transform.GetChild(4).gameObject.GetComponent<cBulletManager>();
        liquidCor = SetLiquid();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Move()
    {
        switch (curPattern.patternNum)
        {
            // 근접 공격 및 이동 패턴
            case 0:
                if (curPatternCount > curPattern.patternCount)
                {
                    //Debug.Log("change Pattern");
                    ChangePattern();
                }
                //Debug.Log("pattern1");
                
                if (timer >= curCoolTime)
                {
                    curPatternCount += 1;
                }
                else
                {
                    timer += Time.deltaTime;
                    if (isInNoticeRange)
                    {
                        if (isInAttackRange)
                        {
                            time += Time.deltaTime;
                            //쿨타임이 다 찼을 때 히트박스 활성화
                            if (time >= attackDelay)
                            {
                                time = 0;
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
                            }
                        }
                    }
                    else
                    {
                        this.transform.Translate(dir * curMoveSpeed * Time.deltaTime);

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
                        else
                        {
                            if (this.gameObject.transform.position.x > playerPos.x)
                            {
                                dir = Vector3.left;
                            }
                            else
                            {
                                dir = Vector3.right;
                            }
                        }
                    }
                }

                break;

            // 원거리 공격
            case 1:
                if (curPatternCount > curPattern.patternCount)
                {
                    //Debug.Log("change Pattern");
                    ChangePattern();
                }

                if (timer >= curCoolTime)
                {
                    bulletMng.SetBullet(1);
                    curPatternCount += 1;
                    timer = 0;
                }
                else
                {
                    timer += Time.deltaTime;
                }
                //Debug.Log("pattern2");
                break;

            // 원거리 공격
            case 2:
                if (curPatternCount > curPattern.patternCount)
                {
                    //Debug.Log("change Pattern");
                    ChangePattern();
                }
                if (timer >= curCoolTime)
                {
                    Debug.Log("on");
                    bulletMng.SetBullet(3);
                    curPatternCount += 1;
                    timer = 0;
                    break;
                }
                else
                {
                    Debug.Log("coolDown");
                    timer += Time.deltaTime;
                }
                //Debug.Log("pattern3");
                break;

            // 액체 상태 공격 (장판)
            case 3:
                if (curPatternCount > curPattern.patternCount)
                {
                    Debug.Log("change Pattern");
                    StopCoroutine(liquidCor);
                    ChangePattern();
                }
                if (timer >= curCoolTime)
                {
                    StartCoroutine(liquidCor);
                    curPatternCount += 1;
                    timer = 0;
                    break;
                }
                else
                {
                    timer += Time.deltaTime;
                }
                Debug.Log("pattern4");
                break;

            // 슬라임 소환
            case 4:
                if (curPatternCount > curPattern.patternCount)
                {
                    Debug.Log("change Pattern");
                    ChangePattern();
                }
                if (timer >= curCoolTime)
                {
                    SummonSlime();
                    curPatternCount += 1;
                    timer = 0;
                    break;
                }
                else
                {
                    timer += Time.deltaTime;
                }
                Debug.Log("pattern5");
                break;
        }
    }

    public override void ReduceHp(float pVal)
    {
        curHp -= pVal;

        if (curHp <= 0)
        {
            curHp = 0;
        }

        SetHp();
    }

    private void ChangePattern()
    {
        int idx = Random.Range((int)0, patternTable.Count);
        
        for(int i = 0; i < patternTable.Count; i++)
        {
            if (idx == patternTable[i].patternNum)
            {
                curPatternCount = 0;
                curPattern = patternTable[i];
                curCoolTime = curPattern.coolTime;
                timer = curPattern.coolTime;
                break;
            } 
        }
    } 
    
    IEnumerator SetLiquid()
    {
        liquid.SetActive(true);

        float height = this.gameObject.transform.position.y - 1500f;
        this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y,
            this.gameObject.transform.position.z - 1000f);
        float liquidZPos = liquid.transform.position.z + 1000f;
        
        while(true)
        {
            liquid.transform.position = new Vector3(liquid.transform.position.x, height, liquidZPos);
            height += 1f;

            if (liquid.transform.position.y >= this.gameObject.transform.position.y)
            {
                break;
            }

            yield return new WaitForFixedUpdate();
        }

        this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y,
            this.gameObject.transform.position.z + 1000f);
        liquid.SetActive(false);
    }

    private void SummonSlime()
    {
        GameObject slime1 = Instantiate(Resources.Load<GameObject>(cPath.PrefabPath() + "enemy_Slime"), 
            new Vector3(this.gameObject.transform.position.x - 100f, 
            this.transform.position.y, this.transform.position.z), Quaternion.identity, this.transform.parent);
        slime1.GetComponent<cMonster_stage1_slime>().Init("slime1", 1, 160, 100, 100);

        GameObject slime2 = Instantiate(Resources.Load<GameObject>(cPath.PrefabPath() + "enemy_Slime"), 
            new Vector3(this.gameObject.transform.position.x + 100f, 
            this.transform.position.y, this.transform.position.z), Quaternion.identity, this.transform.parent);
        slime2.GetComponent<cMonster_stage1_slime>().Init("slime2", 1, 160, 100, 100);
    }
}
