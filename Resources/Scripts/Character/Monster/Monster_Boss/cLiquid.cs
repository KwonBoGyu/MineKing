using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cLiquid : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        cUtil._player.ReduceHp((long)(cUtil._player.GetMaxHp().value * 0.05f));
    }
}
