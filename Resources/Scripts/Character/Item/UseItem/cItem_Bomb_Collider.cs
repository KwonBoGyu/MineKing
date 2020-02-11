using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cItem_Bomb_Collider : MonoBehaviour
{
    public cItem_Bomb_O _origin;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            Debug.Log("bomb attack player");
            cUtil._player.ReduceHp(_origin.damage.value);
        }

        if (collision.gameObject.tag.Equals("Enemy"))
        {
            Debug.Log("bomb attack enemy");
            collision.gameObject.GetComponent<cEnemy_monster>().ReduceHp(_origin.damage.value);
        }
    }
}
