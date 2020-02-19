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
}
