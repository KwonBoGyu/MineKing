using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cWeapon : MonoBehaviour
{
    public float damage;
    public cPlayer scr_player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag.Equals("Tilemap_rock"))
        {
            Destroy(collision.gameObject);
        }

        if (collision.tag.Equals("Enemy"))
        {
            if(scr_player.GetStatus() == CHARACTERSTATUS.DASH_ATTACK)
            {
                collision.GetComponent<cEnemy_monster>().ReduceHp(damage, GetComponentInParent<cPlayer>().GetDirection(), 10.0f);
            }
            else
                collision.GetComponent<cEnemy_monster>().ReduceHp(damage, GetComponentInParent<cPlayer>().GetDirection());
        }

        this.gameObject.SetActive(false);
    }
}
