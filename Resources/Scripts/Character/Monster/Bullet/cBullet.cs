using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cBullet : cProjectile
{
    private bool isInit;

    private CircleCollider2D rangeCollider;
    private float attackRange;

    public BULLET_TYPE type;
    private float distance;
    public float damage;
    private bool isCollide;
    // type : split인 경우 몇 번 분열했는지 카운트
    private int splitCount;
    // type : grenade인 경우 폭발 관리를 위한 변수
    private bool explodeOn;
    private float explodeTime;

    public GameObject parentMonster;
    
    public void Init(BULLET_TYPE pType, float pPower, bool pUseGravity, Vector3 pDir, long pDamage, float pGravity = 7.0f)
    {
        this.transform.position = originObj.transform.position;
        //this.transform.localScale = new Vector3(1, 1, 1);

        type = pType;
        //rangeCollider = parentMonster.GetComponent<cEnemy_monster>().notizer.GetComponent<CircleCollider2D>();
        //attackRange = rangeCollider.radius;
        damage = pDamage;

        splitCount = 0;
        explodeOn = false;
        explodeTime = 3.0f;
        isCollide = false;
        upBlockedContinue = false;
        gravityAmount = pGravity;

        flyingTime = 0;
        defaultPower = pPower;
        changingPower = defaultPower;
        isReflectOn = true;
        isGravityOn = pUseGravity;
        SetDir(pDir);

        isInit = true;
    }

    public void SetType(BULLET_TYPE pType)
    {
        type = pType;

        //switch (pType)
        //{
        //    case BULLET_TYPE.NORMAL:
        //        maxSpeed = 300f;
        //        curSpeed = maxSpeed;
        //        break;
                
        //    case BULLET_TYPE.SPLIT:
        //        maxSpeed = 300f;
        //        curSpeed = maxSpeed;
        //        break;
                
        //    case BULLET_TYPE.GRENADE:
        //        maxSpeed = 500f;
        //        curSpeed = maxSpeed;
        //        break;
        //}
    }

    protected override void FixedUpdate()
    {
        if (isInit.Equals(false))
            return;

        if (dir.Equals(Vector3.zero))
            return;

        Move();

        //distance = new Vector2(this.transform.position.x - originObj.transform.position.x,
        //    this.transform.position.y - originObj.transform.position.y).magnitude;

        //// 총알 최대 범위 이상으로 벗어나면 소멸
        //if (distance >= attackRange)
        //{
        //    this.gameObject.SetActive(false);
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isInit.Equals(false))
            return;

        if (collision.gameObject.tag.Equals("Tile_canHit"))
        {
            isCollide = true;
        }
        else if (collision.gameObject.tag.Equals("Tile_CannotHit"))
        {
            isCollide = true;
        }

        switch (type)
        {
            case BULLET_TYPE.NORMAL:
                if (collision.gameObject.tag.Equals("Player"))
                {                    
                    cUtil._player.ReduceHp((long)damage, dir);
                    this.gameObject.SetActive(false);
                }
                else if (collision.gameObject.tag.Equals("Tile_canHit"))
                {
                    this.gameObject.SetActive(false);
                }
                else if (collision.gameObject.tag.Equals("Tile_cannotHit"))
                {
                    this.gameObject.SetActive(false);
                }
                break;

            // 벽 충돌시 분열하는 기능 추가 예정
            case BULLET_TYPE.SPLIT:
                if (collision.gameObject.tag.Equals("Player"))
                {
                    cUtil._player.ReduceHp((long)(damage / splitCount + 1));
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
                    // 폭발한 상태일때 플레이어에게 닿은 경우
                    if (explodeOn)
                    {
                        cUtil._player.ReduceHp((long)damage);
                        this.gameObject.SetActive(false);
                    }
                }
                else if (collision.gameObject.tag.Equals("Tile_canHit"))
                {
                    dir = Vector3.zero;
                    isGravityOn = false;
                    StartCoroutine("SetGrenade");
                }
                else if (collision.gameObject.tag.Equals("Tile_cannotHit"))
                {
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
        rangeCollider.radius = rangeCollider.radius * 3; // 충돌 범위 확장
        this.transform.localScale = new Vector3(3, 3, 3); // 임시적 시각효과

        yield return new WaitForSeconds(0.3f);
        this.gameObject.SetActive(false); // 0.3초 후 삭제
    }
}
