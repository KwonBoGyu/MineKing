using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct enemyInitStruct
{
    public string nickName;
    public cProperty damage;
    public float maxMoveSpeed;
    public cProperty maxHp;
    public cProperty curHp;
    public int id;
    public cProperty rocks;

    public enemyInitStruct(string pNickName, cProperty pDmg, float pMaxMoveSpeed, cProperty pMaxHp, cProperty pCurHp, int pId, cProperty pRocks)
    {
        nickName = pNickName;
        damage = new cProperty("Damage", pDmg.value);
        maxMoveSpeed = pMaxMoveSpeed;
        maxHp = new cProperty("MaxHp", pMaxHp.value);
        curHp = new cProperty("CurHp", pCurHp.value);
        id = pId;
        rocks = new cProperty("Rocks", pRocks.value);
    }

    public void Init(string pNickName, cProperty pDmg, float pMaxMoveSpeed, cProperty pMaxHp, cProperty pCurHp, int pId, cProperty pRocks)
    {
        nickName = pNickName;
        damage = new cProperty("Damage", pDmg.value);
        maxMoveSpeed = pMaxMoveSpeed;
        maxHp = new cProperty("MaxHp", pMaxHp.value);
        curHp = new cProperty("CurHp", pCurHp.value);
        id = pId;
        rocks = new cProperty("Rocks", pRocks.value);
    }
}

public static class cEnemyTable
{
    public static enemyInitStruct SetMonsterInfo(int pMonsterId)
    {
        enemyInitStruct es = new enemyInitStruct();
        int pRocks = 0;
        switch (pMonsterId)
        {
            //주석에 이름 필수 작성
            case 0:
                pRocks = Random.Range((int)0, (int)4);
                es.Init("똘똘이",
                    new cProperty("Damage", 1),
                    160,
                    new cProperty("MaxHp", 10),
                    new cProperty("CurHp", 10),
                    0,
                    new cProperty("Rocks", pRocks));
                return es;

            // 1-1 : 슬라임
            case 1:
                pRocks = Random.Range((int)0, (int)4);
                es.Init("슬라임",
                    new cProperty("Damage", 10),
                    160,
                    new cProperty("MaxHp", 10),
                    new cProperty("CurHp", 10),
                    1,
                    new cProperty("Rocks", pRocks));
                return es;

            // 1-2 : 유령 
            case 2:
                pRocks = Random.Range((int)0, (int)4);
                es.Init("ghost_normal",
                    new cProperty("Damage", 1),
                    160,
                    new cProperty("MaxHp", 10),
                    new cProperty("CurHp", 10),
                    0,
                    new cProperty("Rocks", pRocks));
                return es;

            // 1-2 : 스켈레톤(일반)
            case 3:
                pRocks = Random.Range((int)0, (int)4);
                es.Init("skeleton_normal",
                    new cProperty("Damage", 1),
                    160,
                    new cProperty("MaxHp", 10),
                    new cProperty("CurHp", 10),
                    0,
                    new cProperty("Rocks", pRocks));
                return es;

            // 1-2 : 스켈레톤(돌격병)
            case 4:
                pRocks = Random.Range((int)0, (int)4);
                es.Init("skeleton_fast",
                    new cProperty("Damage", 1),
                    160,
                    new cProperty("MaxHp", 10),
                    new cProperty("CurHp", 10),
                    0,
                    new cProperty("Rocks", pRocks));
                return es;

            // 1-3 : 박쥐
            case 5:
                pRocks = Random.Range((int)0, (int)4);
                es.Init("bat",
                    new cProperty("Damage", 1),
                    160,
                    new cProperty("MaxHp", 10),
                    new cProperty("CurHp", 10),
                    0,
                    new cProperty("Rocks", pRocks));
                return es;

            // 1-3 : 고블린(일반)
            case 6:
                pRocks = Random.Range((int)0, (int)4);
                es.Init("goblin_normal",
                    new cProperty("Damage", 1),
                    160,
                    new cProperty("MaxHp", 10),
                    new cProperty("CurHp", 10),
                    0,
                    new cProperty("Rocks", pRocks));
                return es;

            // 1-3 : 고블린(투척병)
            case 7:
                pRocks = Random.Range((int)0, (int)4);
                es.Init("goblin_ranged",
                    new cProperty("Damage", 1),
                    160,
                    new cProperty("MaxHp", 10),
                    new cProperty("CurHp", 10),
                    0,
                    new cProperty("Rocks", pRocks));
                return es;

            // 1-4 : 오염된 고블린(투척)
            case 8:
                pRocks = Random.Range((int)0, (int)4);
                es.Init("goblin_ranged_contaminated",
                    new cProperty("Damage", 1),
                    160,
                    new cProperty("MaxHp", 10),
                    new cProperty("CurHp", 10),
                    0,
                    new cProperty("Rocks", pRocks));
                return es;

            // 1-4 : 오염된 거미
            case 9:
                pRocks = Random.Range((int)0, (int)4);
                es.Init("spider_contaminated",
                    new cProperty("Damage", 1),
                    160,
                    new cProperty("MaxHp", 10),
                    new cProperty("CurHp", 10),
                    0,
                    new cProperty("Rocks", pRocks));
                return es;

            // 1-4 : 오염된 스켈레톤 전투병
            case 10:
                pRocks = Random.Range((int)0, (int)4);
                es.Init("skeleton_normal_contaminated",
                    new cProperty("Damage", 1),
                    160,
                    new cProperty("MaxHp", 10),
                    new cProperty("CurHp", 10),
                    0,
                    new cProperty("Rocks", pRocks));
                return es;

            // 1-5 : 곰팡이
            case 11:
                pRocks = Random.Range((int)0, (int)4);
                es.Init("fungus",
                    new cProperty("Damage", 1),
                    160,
                    new cProperty("MaxHp", 10),
                    new cProperty("CurHp", 10),
                    0,
                    new cProperty("Rocks", pRocks));
                return es;

            // 1-5 : 오염된 유령
            case 12:
                pRocks = Random.Range((int)0, (int)4);
                es.Init("ghost_contaminated",
                    new cProperty("Damage", 1),
                    160,
                    new cProperty("MaxHp", 10),
                    new cProperty("CurHp", 10),
                    0,
                    new cProperty("Rocks", pRocks));
                return es;

            // 1-5 : 좀비
            case 13:
                pRocks = Random.Range((int)0, (int)4);
                es.Init("zombie",
                    new cProperty("Damage", 1),
                    160,
                    new cProperty("MaxHp", 10),
                    new cProperty("CurHp", 10),
                    0,
                    new cProperty("Rocks", pRocks));
                return es;

            //보스_슬라임
            case 50:
                pRocks = Random.Range((int)100, (int)150);
                es.Init("Boss_slime",
                    new cProperty("Damage", 2),
                    160,
                    new cProperty("MaxHp", 50),
                    new cProperty("CurHp", 50),
                    0,
                    new cProperty("Rocks", pRocks));
                return es;

            //보스_짜바리슬라임
            case 51:
                pRocks = Random.Range((int)10, (int)20);
                es.Init("Boss_slime",
                    new cProperty("Damage", 1),
                    160,
                    new cProperty("MaxHp", 25),
                    new cProperty("CurHp", 25),
                    0,
                    new cProperty("Rocks", pRocks));
                return es;
        }

        return es;
    }
}