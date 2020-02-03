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

            // 슬라임
            case 1:
                pRocks = Random.Range((int)0, (int)4);
                es.Init("슬라임",
                    new cProperty("Damage", 1),
                    160,
                    new cProperty("MaxHp", 10),
                    new cProperty("CurHp", 10),
                    0,
                    new cProperty("Rocks", pRocks));
                return es;
        }
        
        return es;
    }
}
