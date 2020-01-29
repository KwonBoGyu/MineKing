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
        changingGravity = 200f;
        defaultGravity = 400f;

        splitCount = 0;
        explodeOn = false;
        explodeTime = 3.0f;
    }

    private void OnEnable()
    {
        this.transform.position = originMonster.transform.position;
        this.transform.localScale = new Vector3(1, 1, 1);
        changingGravity = 200f;
    }

    public void SetType(BULLET_TYPE pType)
    {
        type = pType;

        switch (pType)
        {
            case BULLET_TYPE.NORMAL:
                speed = 300f;
                break;
                
            case BULLET_TYPE.SPLIT:
                speed = 300f;
                break;
                
            case BULLET_TYPE.GRENADE:
                speed = 500f;
                break;
        }
    }

    private void FixedUpdate()
    {
        if (dir != null)
        {
            transform.Translate(dir * Time.deltaTime * speed);

            if (isGravityOn)
            {
                SetGravity();
            }
        }

        //distance = new Vector2(this.transform.position.x - originMonster.transform.position.x,
        //    this.transform.position.y - originMonster.transform.position.y).magnitude;
        //// 총알 최대 범위 이상으로 벗어나면 소멸
        //if (distance >= attackRange)
        //{
        //    this.gameObject.SetActive(false);
        //}
    }

    private void SetGravity()
    {
        this.gameObject.transform.Translate(Vector3.down * changingGravity * Time.deltaTime);
        if (changingGravity <= defaultGravity)
            changingGravity *= 1.02f;
        if (changingGravity > defaultGravity)
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

            // 벽 충돌시 분열하는 기능 추가 예정
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
                        if (dir.y <= 0)
                        {
                            dir = new Vector3(dir.x, dir.y * -1, dir.z);
                        }
                        else
                        {
                            dir = new Vector3(dir.x * -1, dir.y, dir.z);
                        }
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

                    }
                }
                break;

            case BULLET_TYPE.GRENADE:
                if (collision.gameObject.tag.Equals("Player"))
                {
                    if (explodeOn)
                    {
                        Debug.Log("explode collide");
                        cUtil._player.ReduceHp(damage);
                        this.gameObject.SetActive(false);
                    }
                }
                else if (collision.gameObject.tag.Equals("Tile_canHit"))
                {
                    Debug.Log("col1");
                    speed = 0f;
                    dir = Vector3.zero;
                    isGravityOn = false;
                    StartCoroutine("SetGrenade");
                }
                else if (collision.gameObject.tag.Equals("Tile_cannotHit"))
                {
                    Debug.Log("col2");
                    speed = 0f;
                    dir = Vector3.zero;
                    isGravityOn = false;
                    StartCoroutine("SetGrenade");
                }
                break;
        }
    }
    
    IEnumerator SetGrenade()
    {
        yield return new WaitForSeconds(explodeTime);
        explodeOn = true;
        rangeCollider.radius = rangeCollider.radius * 3;
        this.transform.localScale = new Vector3(3, 3, 3); // 이펙트 추가 예정

        yield return new WaitForSeconds(0.3f);
        this.gameObject.SetActive(false);
    }
}
