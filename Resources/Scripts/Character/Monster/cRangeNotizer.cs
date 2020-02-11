using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cRangeNotizer : MonoBehaviour
{
    private bool isInRange;
    private float radius;
    private cEnemy_monster script;
    private cPlayer _player;

    private void Start()
    {
        isInRange = false;
        radius = this.gameObject.GetComponent<CircleCollider2D>().radius;
        script = GetComponentInParent<cEnemy_monster>();
        _player = cUtil._player;
    }

    private void FixedUpdate()
    {
        if (isInRange)
        {
            Debug.Log("플레이어 인식 ");
            Vector3 dir = (cUtil._player.originObj.transform.position - this.gameObject.transform.position).normalized;
            Ray ray = new Ray(this.gameObject.transform.position,
                dir);
            RaycastHit2D[] hit = Physics2D.RaycastAll(ray.origin, ray.direction, radius);
            Debug.DrawRay(ray.origin, ray.direction * radius * 2, Color.red);

            for (int i = 0; i < hit.Length; i++)
            {
                Debug.Log(hit[i].collider.gameObject.name);
                if (hit[i].collider.gameObject.tag.Equals("Tile_canHit"))
                {
                    script.isInNoticeRange = false;
                    break;
                }
                else if (hit[i].collider.gameObject.tag.Equals("Tile_cannotHit"))
                {
                    script.isInNoticeRange = false;
                    break;
                }
                else
                {
                    // 인식 범위 안에 들어온 경우
                    script.isInNoticeRange = true;
                    break;
                }
            }
        }
        else
        {
            script.isInNoticeRange = false;
        }

        Debug.Log("isInNoticeRange : " + script.isInNoticeRange);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isInRange = true;
        }
    }

    // 인식 범위를 벗어난 경우
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isInRange = false;
        }
    }
}

