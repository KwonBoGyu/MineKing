using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cEnemy_AttckBox : MonoBehaviour
{
    private cEnemy_monster originMon;
    private bool isInAttackRange;
    public bool GetIsInAttackRange() { return isInAttackRange; }
    private float timer;
    private long damage;
    private float coolTime;
    
    private void Start()
    {
        originMon = this.transform.GetComponentInParent<cEnemy_monster>();
        damage = originMon.GetDamage().value;
        coolTime = originMon.GetAttackCoolTime();
        isInAttackRange = false;
        timer = 0;
    }

    private void FixedUpdate()
    {
        if(isInAttackRange)
        {
            Debug.Log("is in Attack Range");
            timer += Time.deltaTime;
        }
        if(timer >= coolTime)
        {
            Debug.Log("attack");
            cUtil._player.ReduceHp(damage, originMon.GetDirection());
            timer = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 히트박스 안에 플레이어가 있을 경우 
        if (collision.tag.Equals("Player"))
        {
            isInAttackRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            isInAttackRange = false;
            timer = 0;
        }
    }
}
