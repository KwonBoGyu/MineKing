using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cWeapon : MonoBehaviour
{
    public cProperty damage;
    public cPlayer scr_player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("flag"))
        {
            Debug.Log(collision.gameObject.name);
            collision.transform.parent.GetComponent<cFlag>().FlagOn(0);
        }
        
        if (collision.tag.Equals("Enemy"))
        {
            Debug.Log("Attack!");
            if(scr_player.GetStatus() == CHARACTERSTATUS.DASH_ATTACK)
            {
                collision.transform.GetChild(0).GetComponent<cEnemy_monster>().ReduceHp(damage.value, cUtil._player.GetDirection(), 10.0f);
            }
            else
                collision.transform.GetChild(0).GetComponent<cEnemy_monster>().ReduceHp(damage.value, cUtil._player.GetDirection());
        }

        this.gameObject.SetActive(false);
    }
}
