using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cRangeNotizer : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // 인식 범위 안에 들어온 경우
            GetComponentInParent<cEnemy_monster>().isInNoticeRange = true;

            // 공격 범위 안에 들어왔는지 계산
            if (Vector3.Distance(collision.transform.position, this.transform.position) < 70.0f)
            {
                GetComponentInParent<cEnemy_monster>().isInAttackRange = true;
            }
            else
            {
                GetComponentInParent<cEnemy_monster>().isInAttackRange = false;
            }
        }
    }

    // 인식 범위를 벗어난 경우
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GetComponentInParent<cEnemy_monster>().isInNoticeRange = false;
        }
    }
}
