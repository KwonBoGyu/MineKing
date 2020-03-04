using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public struct Pattern
//{
//    public int patternNum { get; set; } // 패턴 고유 번호
//    public float coolTime { get; set; } // 스킬 쿨타임
//    public int patternCount { get; set; } // 스킬 실행 횟수
//    public delegate void ActivePattern();

//    public Pattern(int pPatternNum, float pCoolTime, int pPatternCount)
//    {
//        patternNum = pPatternNum;
//        coolTime = pCoolTime;
//        patternCount = pPatternCount;

//        cBossSkill bossSkill = new cBossSkill();
//        ActivePattern activePattern;
//    }

//}

public class cEnemy_Boss : cEnemy_monster
{
    protected bool isInit;

    protected delegate void Skill(float pTime);
    protected List<Skill[]> patternTable;
    public cBossSkill skills;

    public bool isSkillActive;

    private byte curPatternIdx;
    private sbyte curSkillIdx;

    public override void Init(string pNickname, cProperty pDamage, float pMaxMoveSpeed, cProperty pMaxHp, cProperty pCurHp,
        int pId, cProperty pRocks)
    {
        base.Init(pNickname, pDamage, pMaxMoveSpeed, pMaxHp, pCurHp);

        rt = originObj.GetComponent<BoxCollider2D>();
        defaultGravity = 300.0f;
        changingGravity = defaultGravity;
        SetIsGrounded(false);
        jumpHeight = 200.0f;
        id = pId;
        rocks.value = pRocks.value;

        isSkillActive = false;

        curPatternIdx = 0;
        curSkillIdx = -1;

        //skills = new cBossSkill(this);
        Skill skill1 = new Skill(skills.MoveAndAttack);
        Skill skill2 = new Skill(skills.Stop);
        Skill skill3 = new Skill(skills.RangeAttack);

        Skill[] pattern1 = { skill1, skill2, skill1 };
        Skill[] pattern2 = { skill2, skill1 };
        Skill[] pattern3 = { skill3 };

        patternTable = new List<Skill[]>();
        patternTable.Add(pattern1);
        patternTable.Add(pattern2);
        patternTable.Add(pattern3);

    }

    public override void Init(enemyInitStruct pEs)
    {
        base.Init(pEs);
        defaultGravity = 300.0f;
        changingGravity = defaultGravity;
        SetIsGrounded(false);
        jumpHeight = 200.0f;
        id = pEs.id;
        rocks.value = pEs.rocks.value;

        isSkillActive = false;

        curPatternIdx = 0;
        curSkillIdx = -1;
        Skill skill1 = new Skill(skills.MoveAndAttack);
        Skill skill2 = new Skill(skills.Stop);
        Skill skill3 = new Skill(skills.RangeAttack);

        Skill[] pattern1 = { skill1, skill2, skill1 };
        Skill[] pattern2 = { skill2, skill1 };
        Skill[] pattern3 = { skill3 };

        patternTable = new List<Skill[]>();
        patternTable.Add(pattern1);
        patternTable.Add(pattern2);
        patternTable.Add(pattern3);
        isInit = true;
        
    }

    protected override void FixedUpdate()
    {
        if (isInit.Equals(true))
        {
            base.FixedUpdate();
            if (isSkillActive.Equals(false))
            {
                Debug.Log("false");
                curSkillIdx++;

                if (curSkillIdx > patternTable[1].Length)
                {
                    curSkillIdx = 0;
                }
                patternTable[curPatternIdx][curSkillIdx](1f);
            }
            else
            {
                Debug.Log("true");
                if (curHp.value <= maxHp.value * 0.7f && curHp.value > maxHp.value * 0.3f)
                {
                    curPatternIdx++;
                }
                else if (curHp.value <= maxHp.value * 0.3f)
                {
                    curPatternIdx++;
                }
            }
        }
        
    }

    protected override void Move()
    {
        if (_animator != null)
            _animator.SetFloat("MoveSpeed", curMoveSpeed);

        // idle 상태
        else if (isInNoticeRange.Equals(false))
        {
            img_curHp.transform.parent.gameObject.SetActive(false);
            coolTimer = 0;
            originObj.transform.Translate(dir * curMoveSpeed * Time.deltaTime);
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
        }

    }
    public override void ChangeDir(Vector3 pDir)
    {
        dir = pDir;

        if (dir.Equals(Vector3.right))
            originObj.transform.localScale = new Vector3(-1, 1, 1);
        else if (dir.Equals(Vector3.left))
            originObj.transform.localScale = new Vector3(1, 1, 1);
    }
    //protected override void FixedUpdate()
    //{
    //    base.FixedUpdate();
    //    Move();
    //}

    //protected void ChangePattern()
    //{
    //    int idx = Random.Range((int)0, patternTable.Count);

    //    for (int i = 0; i < patternTable.Count; i++)
    //    {
    //        if (idx == patternTable[i].patternNum)
    //        {
    //            curPatternCount = 0;
    //            curPattern = patternTable[i];
    //            curCoolTime = curPattern.coolTime;
    //            timer = 0;
    //            break;
    //        }
    //    }
    //}
}