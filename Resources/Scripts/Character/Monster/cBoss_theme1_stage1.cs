using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cBoss_theme1_stage1 : cEnemy_Boss
{
    List<Pattern> patternTable;
    Pattern curPattern;

    Pattern MELEE;
    Pattern RANGED1;
    Pattern RANGED2;
    Pattern LIQUID;
    Pattern SUMMON;

    private float curCoolTime;
    private float patternCoolTime;

    private float curSkillTime;

    private GameObject[] bullets_Multi;
    public GameObject bullet_Single;

    private void Start()
    {
        Init(cEnemyTable.SetMonsterInfo(0));

        patternTable = new List<Pattern>();
        MELEE = new Pattern(0, 0, 3);
        RANGED1 = new Pattern(1, 4, 3);
        RANGED2 = new Pattern(2, 4, 3);
        LIQUID = new Pattern(3, 15, 1);
        SUMMON = new Pattern(4, 10, 1);

        patternTable.Add(MELEE);
        patternTable.Add(RANGED1);
        patternTable.Add(RANGED2);

        curPattern = MELEE;
        curSkillTime = 0;

        curMoveSpeed = maxMoveSpeed; // 임시

        bullets_Multi = new GameObject[16];
        for(int i = 0; i < bullets_Multi.Length; i++)
        {
            bullets_Multi[i] = this.gameObject.transform.GetChild(i + 3).gameObject;
            Debug.Log(bullets_Multi[i]);
        }
        for(int i = 0; i < bullets_Multi.Length; i++)
        {
            float degree = i * 25f * Mathf.Deg2Rad;
            bullets_Multi[i].GetComponent<cBullet>().dir = new Vector3(Mathf.Sin(degree), Mathf.Cos(degree), 0);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void MoveAttack()
    {

    }

    private void RangeAttack()
    {

    }

    private void LiquidAttack()
    {

    }

    protected override void Move()
    {
        curSkillTime += Time.deltaTime;
        playerPos = dp._player.transform.position;

        if (this.gameObject.transform.position.x > playerPos.x)
        {
            dir = Vector3.left;
        }
        else
        {
            dir = Vector3.right;
        }

        switch (curPattern.patternNum)
        {
            // 근접 공격 및 이동 패턴
            case 0:
                if (curSkillTime >= curPattern.skillTime)
                {
                    Debug.Log("change Pattern");
                    curSkillTime = 0;
                    ChangePattern();
                }
                Debug.Log("pattern1");

                if (isInNoticeRange)
                {
                    if (isInAttackRange)
                    {
                        time += Time.deltaTime;
                        //쿨타임이 다 찼을 때 히트박스 활성화
                        if (time >= attackDelay)
                        {
                            Debug.Log("ADFADAD");
                            Debug.Log(dir);

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
                }
                break;

            // 원거리 공격 (방사형)
            case 1:
                if (curSkillTime >= curPattern.skillTime)
                {
                    Debug.Log("change Pattern");
                    curSkillTime = 0;
                    ChangePattern();
                }
                Debug.Log("pattern2");

                for (int i = 0; i < bullets_Multi.Length; i++)
                {
                    bullets_Multi[i].SetActive(true);
                }
                break;

            // 원거리 공격 (단일형)
            case 2:
                if (curSkillTime >= curPattern.skillTime)
                {
                    Debug.Log("change Pattern");
                    curSkillTime = 0;
                    ChangePattern();
                }
                Debug.Log("pattern3");
                break;

            // 액체 상태 공격 (장판)
            case 3:
                if (curSkillTime >= curPattern.skillTime)
                {
                    Debug.Log("change Pattern");
                    curSkillTime = 0;
                    ChangePattern();
                }
                Debug.Log("pattern4");

                break;

            // 슬라임 소환
            case 4:
                if (curSkillTime >= curPattern.skillTime)
                {
                    Debug.Log("change Pattern");
                    curSkillTime = 0;
                    ChangePattern();
                }
                Debug.Log("pattern5");
                break;
        }
    }

    private void ChangePattern()
    {
        int idx = Random.Range((int)0, patternTable.Count);
        
        for(int i = 0; i < patternTable.Count; i++)
        {
            if (idx == patternTable[i].patternNum)
            {
                curPattern = patternTable[i];
                break;
            }
        }
    }
}
