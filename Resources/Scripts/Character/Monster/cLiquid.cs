using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cLiquid : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        cUtil._player.ReduceHp(cUtil._player.GetMaxHp() * 0.05f);
    }
}
