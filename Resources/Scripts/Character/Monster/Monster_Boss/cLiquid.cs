using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cLiquid : MonoBehaviour
{
    private float timer;
    private bool isPlayerIn;

    private void Start()
    {
        timer = 0;
        isPlayerIn = false;
    }

    private void FixedUpdate()
    {
        if(isPlayerIn)
        {
            timer += Time.deltaTime;

            if (timer >= 1.0f)
            {
                cUtil._player.ReduceHp((long)(cUtil._player.GetMaxHp().value * 0.05f));
                timer = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isPlayerIn = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isPlayerIn = false;
    }
}
