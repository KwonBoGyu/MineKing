using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cEnemy_AttckBox : MonoBehaviour
{
    private cProperty damage;
    
    private void Start()
    {
        damage.value = this.transform.GetComponentInParent<cEnemy_monster>().GetDamage().value;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.transform.gameObject.name);
        // 히트박스 안에 플레이어가 있을 경우 
        if (collision.tag.Equals("Player"))
        {
            cUtil._player.ReduceHp(damage.value, this.transform.GetComponentInParent<cEnemy_monster>().GetDirection());
            Debug.Log("attacked by " + this.transform.GetComponentInParent<cEnemy_monster>().name);

            this.gameObject.SetActive(false);
        }
    }
}
