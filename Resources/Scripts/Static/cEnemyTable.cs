using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct enemyInitStruct
{
    public string nickName;
    public int damage;
    public float maxMoveSpeed;
    public float maxHp;
    public float curHp;
    public int id;
    public int rocks;

    public enemyInitStruct(string pNickName, int pDmg, float pMaxMoveSpeed, float pMaxHp, float pCurHp, int pId, int pRocks)
    {
        nickName = pNickName;
        damage = pDmg;
        maxMoveSpeed = pMaxMoveSpeed;
        maxHp = pMaxHp;
        curHp = pCurHp;
        id = pId;
        rocks = pRocks;
    }

    public void Init(string pNickName, int pDmg, float pMaxMoveSpeed, float pMaxHp, float pCurHp, int pId, int pRocks)
    {
        nickName = pNickName;
        damage = pDmg;
        maxMoveSpeed = pMaxMoveSpeed;
        maxHp = pMaxHp;
        curHp = pCurHp;
        id = pId;
        rocks = pRocks;
    }
}

public static class cEnemyTable
{
    public static enemyInitStruct SetMonsterInfo(int pMonsterId)
    {
        enemyInitStruct es = new enemyInitStruct();

        switch (pMonsterId)
        {
            //주석에 이름 필수 작성
            case 0:
                int pRocks = Random.Range((int)0, (int)4);
                es.Init("똘똘이", 1, 160, 100, 100, 0, pRocks);
                return es;

            // 슬라임
            case 1:
                pRocks = Random.RandomRange((int)0, (int)4);
                es.Init("슬라임", 1, 160, 100, 100, 1, pRocks);
                return es;
        }
        
        return es;
    }
}
