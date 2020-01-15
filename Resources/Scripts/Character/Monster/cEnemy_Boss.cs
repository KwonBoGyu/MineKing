using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cEnemy_Boss : cEnemy_monster
{
    //몬스터 id
    private int id;
    public int GetId() { return id; }
    //몬스터 광물 보유량
    private int rocks;
    public int GetRocks() { return rocks; }
    //강화 아이템 랜덤 드랍 확률 (현재 100%)
    private int per_Upgrade = 100;

    public struct Pattern
    {
        public int patternNum { get; set; }
        public float coolTime { get; set; }
        public float skillTime { get; set; }

        public Pattern(int pPatternNum, float pCoolTime, float pSkillTime)
        {
            patternNum = pPatternNum;
            coolTime = pCoolTime;
            skillTime = pSkillTime;
        }
    }

    public void Init(string pNickname, float pDamage, float pMaxMoveSpeed, float pMaxHp, float pCurHp,
        int pId, int pRocks)
    {
        base.Init(pNickname, pDamage, pMaxMoveSpeed, pMaxHp, pCurHp);

        rt = originObj.GetComponent<BoxCollider2D>();
        defaultGravity = 300.0f;
        changingGravity = defaultGravity;
        isGrounded = false;
        jumpHeight = 200.0f;
        id = pId;
        rocks = pRocks;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        //Move();
    }

    //private void Move()
    //{
    //    this.transform.Translate(dir * curMoveSpeed * Time.deltaTime);
    //    //막히면 방향 바꿔준다.
    //    if (isRightBlocked == true)
    //    {
    //        isRightBlocked = false;
    //        dir = Vector3.left;
    //    }
    //    else if (isLeftBlocked == true)
    //    {
    //        isLeftBlocked = false;
    //        dir = Vector3.right;
    //    }
    //}

    public override void ReduceHp(float pVal)
    {
        curHp -= pVal;

        if (curHp <= 0)
        {
            curHp = 0;

            if (cUtil._user.GetInventory().isBossKeyExist().Equals(false))
            {
                int percent = Random.Range(0, 101);

                if (percent <= per_Upgrade)
                {
                    Debug.Log("Get Upgrade Item");
                    cItem pEtc;
                    citemTable.GetItemInfo(out pEtc, 102);
                    cUtil._user.GetInventory().GetItemEtc().Add((cItem_etc)pEtc);
                }
            }
        }

        SetHp();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.transform.GetChild(0).GetComponent<cPlayer>().ReduceHp(damage);

            Debug.Log("attacked by " + this.nickName);
        }

    }
}
