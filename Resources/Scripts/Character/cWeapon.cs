using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cWeapon : MonoBehaviour
{
    public cProperty damage;
    public cPlayer scr_player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag.Equals("Tilemap_rock"))
        {
            Debug.Log(collision.gameObject.name);
        }

        if (collision.tag.Equals("Enemy"))
        {
            Debug.Log("Atack!");
            if(scr_player.GetStatus() == CHARACTERSTATUS.DASH_ATTACK)
            {
                collision.GetComponent<cEnemy_monster>().ReduceHp(damage.value, GetComponentInParent<cPlayer>().GetDirection(), 10.0f);
            }
            else
                collision.GetComponent<cEnemy_monster>().ReduceHp(damage.value, GetComponentInParent<cPlayer>().GetDirection());
        }

        this.gameObject.SetActive(false);
    }
}
