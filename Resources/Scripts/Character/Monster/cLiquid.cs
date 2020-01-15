using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cLiquid : MonoBehaviour
{
    private float tick;

    private void Start()
    {
        tick = 0;
    }

    private void OnEnable()
    {
        this.transform.position = this.transform.parent.position; // 보스 맵 크기 정해지면 맵 맨 아래 위치로 수정해야함
    }

    private void FixedUpdate()
    {
        this.transform.Translate(Vector3.up * 50f * Time.deltaTime);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Equals("Player"))
        {
            tick += Time.deltaTime;
            if (tick >= 1.0f)
            {
                tick = 0;
                cUtil._user.GetPlayer().ReduceHp(cUtil._user.GetPlayer().GetMaxHp() * 0.05f);
            }
        }
    }
}
