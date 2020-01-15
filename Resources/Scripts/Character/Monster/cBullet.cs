using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cBullet : MonoBehaviour
{
    public GameObject originMonster;
    private CircleCollider2D rangeCollider;
    private float attackRange;
    public float speed;
    public Vector3 dir;
    private float distance;

    private void Start()
    {
        //originMonster = this.transform.parent.gameObject;
        rangeCollider = originMonster.transform.GetChild(1).GetComponent<CircleCollider2D>();
        attackRange = rangeCollider.radius;
        speed = 500f;
    }

    private void OnEnable()
    {
        this.transform.position = originMonster.transform.position;
    }
    
    // 플레이어와 몬스터 위치 비교해서 총알 발사 방향 설정
    public void SetDir(Vector3 pDir)
    {
        dir = new Vector2(pDir.x - originMonster.transform.position.x,pDir.y - originMonster.transform.position.y).normalized;
    }
    
    private void FixedUpdate()
    {
        if (dir != null)
        {
            transform.Translate(dir * Time.deltaTime * speed);
        }
        distance = new Vector2(this.transform.position.x - originMonster.transform.position.x,
            this.transform.position.y - originMonster.transform.position.y).magnitude;
        // 총알 최대 범위 이상으로 벗어나면 소멸
        if (distance >= attackRange)
        {
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collision");

        if (collision.gameObject.tag.Equals("Player"))
        {
            Debug.Log("### : " + collision.gameObject.name);
            collision.transform.gameObject.GetComponent<cPlayer>().
                ReduceHp(originMonster.GetComponent<cMonster_stage1_slime>().GetDamage(), dir);
        }
        else if (collision.gameObject.tag.Equals("Tile_canHit"))
        {
            Debug.Log("shot tile_can");
            this.gameObject.SetActive(false);
        }
        else if (collision.gameObject.tag.Equals("Tile_cannotHit"))
        {
            Debug.Log("shot tile_cannot");
            this.gameObject.SetActive(false);
        }

    }
}
