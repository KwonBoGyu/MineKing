using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Pattern
{
    public int patternNum { get; set; } // 패턴 고유 번호
    public float coolTime { get; set; } // 스킬 쿨타임
    public int patternCount { get; set; } // 스킬 실행 횟수

    public Pattern(int pPatternNum, float pCoolTime, int pPatternCount)
    {
        patternNum = pPatternNum;
        coolTime = pCoolTime;
        patternCount = pPatternCount;
    }
}

public class cEnemy_Boss : cEnemy_monster
{
    //몬스터 광물 보유량
    private cProperty souls;
    public cProperty GetSoulss() { return souls; }

    public override void Init(string pNickname, cProperty pDamage, float pMaxMoveSpeed, cProperty pMaxHp, cProperty pCurHp,
        int pId, cProperty pSouls)
    {
        base.Init(pNickname, pDamage, pMaxMoveSpeed, pMaxHp, pCurHp);

        rt = originObj.GetComponent<BoxCollider2D>();
        defaultGravity = 300.0f;
        changingGravity = defaultGravity;
        SetIsGrounded(false);
        jumpHeight = 200.0f;
        id = pId;
        souls.value = pSouls.value;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        Move();
    }
}
