using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cRangeNotizer : MonoBehaviour
{
    private float radius;

    private void Start()
    {
        radius = this.gameObject.GetComponent<CircleCollider2D>().radius;    
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("플레이어 인식 ");
            Ray2D ray = new Ray2D(this.gameObject.transform.position,
                cUtil._user.GetPlayer().originObj.transform.position - this.gameObject.transform.position);
            RaycastHit2D[] hit = Physics2D.RaycastAll(ray.origin, ray.direction, radius);
            Debug.DrawRay(ray.origin, ray.direction * radius, new Color(255, 0, 0));

            for (int i = 0; i < hit.Length; i++)
            {
                Debug.Log(hit[i].collider.gameObject.name);
                if (hit[i].collider.gameObject.tag.Equals("Tile_canHit"))
                {
                    Debug.Log("벽1");
                    GetComponentInParent<cEnemy_monster>().isInNoticeRange = false;
                    break;
                }
                else if (hit[i].collider.gameObject.tag.Equals("Tile_cannotHit"))
                {
                    Debug.Log("벽2");
                    GetComponentInParent<cEnemy_monster>().isInNoticeRange = false;
                    break;
                }
                else
                {
                    Debug.Log("인식함");
                    // 인식 범위 안에 들어온 경우
                    GetComponentInParent<cEnemy_monster>().isInNoticeRange = true;
                    break;
                }
            }

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
