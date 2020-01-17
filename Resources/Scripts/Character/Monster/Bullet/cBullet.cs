using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cBullet : MonoBehaviour
{
    public GameObject originMonster;
    private CircleCollider2D rangeCollider;
    private float attackRange;

    public BULLET_TYPE type;
    public bool isGravityOn;
    public float speed;
    public Vector3 dir;
    private float distance;
    public float damage;

    private float changingGravity;
    private float defaultGravity;

    private int splitCount;
    private bool explodeOn;
    private float explodeTime;

    private void Start()
    {
        rangeCollider = originMonster.transform.GetChild(1).GetComponent<CircleCollider2D>();
        attackRange = rangeCollider.radius;
        damage = originMonster.GetComponent<cEnemy_monster>().GetDamage();
        changingGravity = 500f;
        defaultGravity = 1000f;

        splitCount = 0;
        explodeOn = false;
        explodeTime = 3.0f;
    }

    private void OnEnable()
    {
        this.transform.position = originMonster.transform.position;
    }

    public void SetType(BULLET_TYPE pType)
    {
        type = pType;

        switch (pType)
        {
            case BULLET_TYPE.TRIPLE:
                speed = 500f;
                break;
                
            case BULLET_TYPE.SPLIT:
                speed = 300f;
                break;
                
            case BULLET_TYPE.GRENADE:
                speed = 400f;
                break;
        }
    }

    private void FixedUpdate()
    {
        if (dir != null)
        {
            transform.Translate(dir * Time.deltaTime * speed);

            if(isGravityOn)
            {
                SetGravity();
            }
        }

        distance = new Vector2(this.transform.position.x - originMonster.transform.position.x,
            this.transform.position.y - originMonster.transform.position.y).magnitude;
        // 총알 최대 범위 이상으로 벗어나면 소멸
        if (distance >= attackRange)
        {
            this.gameObject.SetActive(false);
        }
    }

    private void SetGravity()
    {
        this.gameObject.transform.Translate(Vector3.down * changingGravity * Time.deltaTime);
        if (changingGravity <= 1000)
            changingGravity *= 1.02f;
        if (changingGravity > 1000)
            changingGravity = defaultGravity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collision");
        switch(type)
        {
            case BULLET_TYPE.NORMAL:
                if (collision.gameObject.tag.Equals("Player"))
                {
                    // 원거리 공격 데미지 나중에 변수 따로 선언해야 함
                    cUtil._player.ReduceHp(damage, dir);
                }
                else if (collision.gameObject.tag.Equals("Tile_canHit"))
                {
                    this.gameObject.SetActive(false);
                }
                else if (collision.gameObject.tag.Equals("Tile_CannotHit"))
                {
                    this.gameObject.SetActive(false);
                }
                break;

            case BULLET_TYPE.TRIPLE:
                if (collision.gameObject.tag.Equals("Player"))
                {
                    // 원거리 공격 데미지 나중에 변수 따로 선언해야 함
                    cUtil._player.ReduceHp(damage, dir);
                }
                else if (collision.gameObject.tag.Equals("Tile_canHit"))
                {
                    this.gameObject.SetActive(false);
                }
                else if (collision.gameObject.tag.Equals("Tile_CannotHit"))
                {
                    this.gameObject.SetActive(false);
                }
                break;

            case BULLET_TYPE.SPLIT:
                if (collision.gameObject.tag.Equals("Player"))
                {
                    cUtil._player.ReduceHp(damage / splitCount + 1);
                }
                else if (collision.gameObject.tag.Equals("Tile_canHit"))
                {
                    if (splitCount >= 3)
                    {
                        this.gameObject.SetActive(false);
                    }
                    else
                    {
                        dir = new Vector3(dir.x * -1, dir.y, dir.z);
                    }
                }
                else if (collision.gameObject.tag.Equals("Tile_cannotHit"))
                {
                    if (splitCount >= 3)
                    {
                        this.gameObject.SetActive(false);
                    }
                    else
                    {
                        dir = new Vector3(dir.x * -1, dir.y, dir.z);
                    }
                }
                break;

            case BULLET_TYPE.GRENADE:
                if (collision.gameObject.tag.Equals("Player"))
                {
                    if (explodeOn)
                    {
                        cUtil._player.ReduceHp(damage);
                    }
                }
                else if (collision.gameObject.tag.Equals("Tile_canHit"))
                {
                    StartCoroutine("SetGrenade");
                }
                else if (collision.gameObject.tag.Equals("Tile_cannotHit"))
                {
                    StartCoroutine("SetGrenade");
                }
                break;
        }
    }

    IEnumerator SetGrenade()
    {
        speed = 0f;
        dir = Vector3.zero;
        yield return new WaitForSeconds(explodeTime);

        explodeOn = true;
        rangeCollider.radius = rangeCollider.radius * 3;
    }
}
