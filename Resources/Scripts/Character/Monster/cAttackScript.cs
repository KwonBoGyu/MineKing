using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cAttackScript : MonoBehaviour
{
    private long damage;
    private Vector3 dir;
    public cEnemy_monster em;

    public void SetAttackParameter(long pDamage, Vector3 pDir)
    {
        damage = pDamage;
        dir = pDir;
    }

    public void SetBullet()
    {
        em.SetBullet1();
    }

    public void Attack()
    {
        if(em.isInAttackRange.Equals(true))
            cUtil._player.ReduceHp(damage, dir);
    }

    public void Dead()
    {
        Debug.Log("dead");
        this.transform.parent.localPosition = new Vector3(-1760, 1000, 1);
    }
}
