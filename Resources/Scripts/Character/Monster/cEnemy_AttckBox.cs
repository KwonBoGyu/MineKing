using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cEnemy_AttckBox : MonoBehaviour
{
    private cEnemy_monster script;
    
    public void Init()
    {
        script = this.transform.parent.GetChild(0).GetComponent<cEnemy_monster>();
        script.isInAttackRange = false;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            script.isInAttackRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            script.isInAttackRange = false;
        }
    }
}
