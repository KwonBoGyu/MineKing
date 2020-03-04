using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cRangeNotizer : MonoBehaviour
{
    private float radius;
    private cEnemy_monster script;
    private IEnumerator cor_CalculateRange;

    public void Init()
    {
        radius = this.gameObject.GetComponent<CircleCollider2D>().radius;
        script = this.transform.parent.GetChild(0).GetComponent<cEnemy_monster>();
        cor_CalculateRange = CalculateRange();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            StartCoroutine(cor_CalculateRange);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            StopCoroutine(cor_CalculateRange);
            script.isInNoticeRange = false;
        }
    }

    private IEnumerator CalculateRange()
    {
        while(true)
        {
            yield return new WaitForFixedUpdate();

            Vector3 dir = (cUtil._player.originObj.transform.position - this.gameObject.transform.position).normalized;
            Ray ray = new Ray(this.gameObject.transform.position,
                dir);
            RaycastHit2D[] hit = Physics2D.RaycastAll(ray.origin, ray.direction, radius);
            Debug.DrawRay(ray.origin, ray.direction * radius * 2, Color.red);

            for (int i = 0; i < hit.Length; i++)
            {
                if (hit[i].collider.gameObject.tag.Equals("Enemy"))
                    continue;

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
    }
}

