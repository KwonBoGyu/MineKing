using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cBossSkill : MonoBehaviour
{
    private cEnemy_Boss boss;

    //public cBossSkill(cEnemy_Boss pBoss)
    //{
    //    boss = pBoss;
    //}

    public void Init(cEnemy_Boss pBoss)
    {
        boss = pBoss;
    }

    public void MoveAndAttack(float pTime)
    {
        boss.isSkillActive = true;
        StartCoroutine(MoveAndAttackCor());
    }
    public void Stop(float pTime)
    {
        boss.isSkillActive = true;
        StartCoroutine(StopCor(pTime));
    }
    public void RangeAttack(float pTime)
    {
        boss.isSkillActive = true;
        StartCoroutine(RangeAttackCor(pTime));
    }

    IEnumerator MoveAndAttackCor()
    {
        Debug.Log("MoveAndAttack");
        yield return new WaitForFixedUpdate();
        boss.isSkillActive = false;
    }

    IEnumerator StopCor(float pTime)
    {
        Debug.Log("Stop");
        yield return new WaitForFixedUpdate();
        boss.isSkillActive = false;
    }

    IEnumerator RangeAttackCor(float pTime)
    {
        Debug.Log("RangeAttack");
        yield return new WaitForFixedUpdate();
        boss.isSkillActive = false;
    }
}