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

    public override void Init(string pNickname, cProperty pDamage, float pMaxMoveSpeed, cProperty pMaxHp, cProperty pCurHp, int pId, cProperty pRocks)
    {
        base.Init(pNickname, pDamage, pMaxMoveSpeed, pMaxHp, pCurHp, pId, pRocks);
        bulletManager = GameObject.Find("Bullets").GetComponent<cBulletManager>();
    }

    public override void Init(enemyInitStruct pEs)
    {
        base.Init(pEs);
        bulletManager = GameObject.Find("Bullets").GetComponent<cBulletManager>();
    }
}
